
using System.Drawing;
using System.Windows;

using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Configuration;
using System.Windows.Interop;


namespace DesktopPet{

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator System.Windows.Point(POINT point)
            {
                return new System.Windows.Point(point.X, point.Y);
            }
    }

    public enum SpriteState //Can use this later
{
    Idle,
    Held,
    Walking,
    Falling,
    Sleeping
}


public struct RECT
{
    public int Left; //x-coordinate of the upper-left corner
    public int Top; //y-coordinate of the upper-left corner
    public int Right; //x-coordinate of the lower-right corner
    public int Bottom; //y-coordinate of the lower-right corner
} 


public partial class PetBasics : Window
{
    private DispatcherTimer _timer; //Timer for wpf
    private DateTime _myDateTime;
    //private List<Sprite> SpritesAvailable = new List<Sprite>(); //Unnessesary but could be useful for food pellets
    private Random rand = new Random();
    int idleCountdown;
    int idleCooldown = 0;
    int walkCountdown;
    int walkCooldown = 0;
    private Sprite mainSprite;
    private SpriteManager spriteManager;

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT point);
    public static System.Windows.Point GetCursorPosition()
    {
        POINT lpPoint;
        GetCursorPos(out lpPoint);
        
        return lpPoint;
    }


    private void Timer_Tick(object? sender, EventArgs e)
    {

        if (mainSprite.currentAnimationType != "Held") //So a fun thing about this is that updatewindowposition is setting the window position from mainSprite.spriteLocation...
        {
            UpdateWindowPosition();
        }

        spriteManager.UpdateSpritePos(mainSprite,this); //...and this sets mainSprite.spriteLocation FROM actual window position and when the sprite is being moved it slingshots back. Creates a jittery effect.

        if (!mainSprite.currentAnimationType.StartsWith("IdleAnimation") && !mainSprite.currentAnimationType.StartsWith("Walk") && mainSprite.currentAnimationType != "Held" && mainSprite.currentAnimationType != "Falling")
        {
            idleCooldown++;//So it doesnt start an idle in another one.
            walkCooldown++;

            if (walkCountdown < walkCooldown)
                {
                    walkCooldown = 0;
                    walkCountdown = rand.Next(40,200); //Same value as in the constructor value. Just make sure to change them so theyr're consistent
                    mainSprite.ChangeState("Walk"); //Will then choose a random idle animation available.
                }

            if (idleCountdown < idleCooldown)
                {
                    idleCooldown = 0;
                    idleCountdown = rand.Next(10,61);
                    mainSprite.ChangeState("IdleAnimation"); //Will then choose a random idle animation available.
                }
            

        } 
        
        


        //Console.WriteLine(this.Left);

        var diff = DateTime.Now - _myDateTime;
        CharacterImage.Source = mainSprite.currentFrame;
    }

    
    public PetBasics(Sprite character, SpriteManager spriteManger)
    {
        this.spriteManager = spriteManger;
        mainSprite = character;
        InitializeComponent();
        //SpriteManager spriteManager = new SpriteManager();
        //Here is where we would check the save folder for an existing character, to load or otherwise open the menu first.
        //Maybe a quick popup telling users what characters they can use and about the program.
        //Sprite pet = new Sprite("Dyr");
        CharacterImage.Source = character.currentFrame;
        idleCountdown = rand.Next(10,61); //So it doesnt just idleanimation immediately
        walkCountdown = rand.Next(40,200);
        spriteManager.Tick += Timer_Tick; //+= is really cool and means whenever Tick changes, run Timer_Tick
    }

/*
    private void LoadCharacter(string path)
    {
        SpritesAvailable.Add(new Sprite(path));

        //CharacterImage.Source = new BitmapImage(new Uri(path, UriKind.Relative)); //Questionmark after currentAnimation makes it skip the call to the field if it is null. Essentially superflous

    }
*/
    private void CharacterClick(object sender, MouseButtonEventArgs e)
    {
            
        mainSprite.ChangeState("Held"); //main sprite will always be first in array. Clunky but it works.
        this.DragMove();
        mainSprite.ChangeState("Idle");

    }

    private void CharacterRightCLick(object sender, MouseButtonEventArgs e)
        {
                    if (e.ChangedButton == MouseButton.Left){ //Just confirming again
            
            mainSprite.ChangeState("Held"); //main sprite will always be first in array. Clunky but it works.
            
            this.DragMove();
            mainSprite.ChangeState("Idle");
        }
        //if (e.ChangedButton == MouseButton.Right)
        }

    private void HelloClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Hello");
    }


    private void UpdateWindowPosition()
    {

        RECT rect = mainSprite.spriteLocation;
        IntPtr hwnd = new WindowInteropHelper(this).Handle;

        MonitorHelper.SetPhysicalPosition(hwnd, rect.Left, rect.Top);
    }

    
    
}
}