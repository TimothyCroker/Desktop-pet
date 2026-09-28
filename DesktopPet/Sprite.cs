
using System.Configuration;
using System.Runtime.ConstrainedExecution;
using System.IO;
using System.Data;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Automation;

using System.Windows;

using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.VisualBasic;

using System.Windows.Input;
using System.Windows.Controls;

using System.Windows.Media.Animation;
using System.Windows.Threading;

using System.Windows.Interop;
using System.Windows.Automation.Text;
using System.Windows.Markup;

namespace DesktopPet;
public class Sprite
{
    //private readonly string spriteFilePath = "recources/CharacterSprites"; //Readonly is same as final in java
    //private int currentAnimationLength;
    public String currentAnimationType{ get; set; }
    //private String? spriteType; //for multiple sprites that could use different emotions or colours?? might be a lot of extra work
    public String spriteName { get; set; } //Easy getters and setters. thanks stackoverflow/questions/11159438
    //private int spriteHappiness; //Unused ...for now
    public RECT spriteLocation { get; set; }
    private BitmapImage[] currentAnimation;
    private int animationTick;
    public BitmapImage currentFrame { get; private set; }
    private Random rand = new Random();
    //private Window window;
    private double gravity = 0.22;
    private double velocityX;
    private double velocityY;
    private int walkCycleDistance;
    private int walkDuration;
    private int selectedWalkDirection;
    private const int WalkSpeed = 3;
    private int walkTimeCount;

    public Sprite(String name)
    {

        spriteName = name;
        //this.window = window;
        try{
            int currentAnimationLength = Directory.GetFiles(Path.Combine("recources","CharacterSprites",name,"Idle")).Length;
            currentAnimationType = "Idle";
            currentAnimation = GetFrames();
            animationTick = 0;
            currentFrame = currentAnimation[animationTick];
        } catch (DirectoryNotFoundException){
            throw new FileLoadException("Invalid File Path");
        }
        


    }

    public void ChangeState(String state) //Currently only have Held & Idle
    {
        if (state == "IdleAnimation") //For choosing an idle animation
        {
            
        string folderPath = Path.Combine("recources", "CharacterSprites", spriteName);
        string[] animations = Directory.GetDirectories(folderPath, "IdleAnimation*");

        if (animations.Length == 0)
        {
            ChangeState("Idle");
            return;
        }


        

        string selectedAnimation = animations[rand.Next(animations.Length)];

        currentAnimationType = Path.GetFileName(selectedAnimation);
        
            
        } else if (state == "Walk"){
                selectedWalkDirection = rand.Next(0,2) == 0 ? -1 : 1;
                walkDuration = rand.Next(200,1000);
                walkCycleDistance = walkDuration;

                   int[] sidesx = MonitorHelper.GetWorkAreaSides(spriteLocation);
            if (selectedWalkDirection > 0 && spriteLocation.Right == sidesx[1]) currentAnimationType = "WalkL";           
            if (selectedWalkDirection < 0 && spriteLocation.Left == sidesx[0]) currentAnimationType = "WalkR";
            currentAnimationType = selectedWalkDirection < 0 ? "WalkL" : "WalkR";
        }
        
        else //For choosing any other state 
        {
           currentAnimationType = state; 
        }
        animationTick = 0; //Reset frame animation starts at
        currentAnimation = GetFrames(); 
    }

    public void UpdateFrame()
    {
        if (currentAnimation.Length -1 > animationTick)
        {
            animationTick++;
        } else
        {
            animationTick = 0;
            if (currentAnimationType.StartsWith("IdleAnimation"))
            {
                ChangeState("Idle");
            }
        }
        currentFrame = currentAnimation[animationTick];
    }


    

    private BitmapImage[] GetFrames() //Runs through set file path to retrieve all animation frames in a given folder
    {
        string path = Path.Combine("recources","CharacterSprites", spriteName, currentAnimationType); //Path.Combine is like string concatonation for file systems but changes / and \ depending on system!
        string[] files = Directory.GetFiles(path, "*.png");
        BitmapImage[] animFrames = new BitmapImage[files.Length];
        
        for (int x = 0; x < files.Length; x++)
    {
        //Console.WriteLine(files[x]);
        animFrames[x] = new BitmapImage(new Uri(files[x], UriKind.Relative));
    }
        return animFrames;
    }


    public void SpriteGravity()
    {
        
        {
        if (currentAnimationType == "Held"){ //If held then stop looking to fall and also reset falling velocity
                velocityY = 0;
                return;
            }
        if (GravityCheck()){

        
            if (velocityY < 15){
                if (velocityY < 5){
                        velocityY = 5; //So it doesnt start at a snail's pace
                    }
                velocityY += gravity;} //Lets make it 6!

            var newLocation = spriteLocation;
            newLocation.Top += (int)velocityY;  //Implicit casting. Get rid of that decimal. Why not always not use a decimal if we're just converting it now??? Precision + decimals are cool.
            newLocation.Bottom += (int)velocityY;

            int groundY = MonitorHelper.GetWorkAreaBottom(newLocation);
            if (newLocation.Bottom >= groundY)
            {
                int overshoot = (int)(newLocation.Bottom - groundY);
                newLocation.Bottom -= overshoot;
                newLocation.Top -= overshoot;
                velocityY = 0;
                ChangeState("Idle");
            }
            

            this.spriteLocation = newLocation;
        }
        }
    }
            

        public bool GravityCheck()
        {   
            double groundY = MonitorHelper.GetWorkAreaBottom(spriteLocation);
        if (spriteLocation.Bottom >= groundY)
            {
                velocityY = 0;
                return false;
            } else {if (currentAnimationType != "Falling"){ChangeState("Falling");}
        return true;} //Continues to fall
    }

    public void SpriteWalk()
    {
        if (!WalkCheck()) return;

        var newLocation = spriteLocation;
        int steps;
        if (walkCycleDistance >= WalkSpeed){
            steps = WalkSpeed*selectedWalkDirection; 
            walkCycleDistance -= WalkSpeed;}
        else {steps = walkCycleDistance*selectedWalkDirection; walkCycleDistance = 0;}

        newLocation.Left += steps;
        newLocation.Right += steps;
        this.spriteLocation = newLocation;

        walkTimeCount++;
    }

    public bool WalkCheck()
    {
    
        if (currentAnimationType != "WalkL" && currentAnimationType != "WalkR"){
            Console.WriteLine(currentAnimationType);
            return false;}

        int[] sidesX = MonitorHelper.GetWorkAreaSides(spriteLocation);
        bool hitEdge = (selectedWalkDirection < 0 && spriteLocation.Left <= sidesX[0])
                    || (selectedWalkDirection > 0 && spriteLocation.Right >= sidesX[1]);

        bool doneWalking = walkCycleDistance <= 0 || hitEdge;

        if (doneWalking)
        {

            ChangeState("Idle");
            return false;
        }
        return true;
    }



 //currentFrame = (currentFrame + 1) % animation.Frames.Length;
}

