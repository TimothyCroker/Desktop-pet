
using System.Configuration;
using System.Runtime.ConstrainedExecution;
using System.IO;
using System.Data;
using System.Windows.Media.Imaging;

namespace DesktopPet;
public class Sprite
{
    private readonly string spriteFilePath = "recources/CharacterSprites"; //Readonly is same as final in java
    private int currentAnimationLength;
    private String currentAnimationType;
    private String? spriteType; //for multiple sprites that could use different emotions or colours?? might be a lot of extra work
    public String spriteName { get; set; } //Easy getters and setters. thanks stackoverflow/questions/11159438
    private int spriteHappiness; //Unused ...for now
    private POINT? spriteLocation;

    public Sprite(String name)
    {

        spriteName = name;
        try{
            int currentAnimationLength = Directory.GetFiles(Path.Combine(spriteFilePath,"idle",name)).Length;
        } catch (DirectoryNotFoundException){
            spriteName = "MissingNo.";
        }
        currentAnimationType = "idle";
    }

    public BitmapImage[] getFrames() //Runs through set file path to retrieve all animation frames in a given folder
    {
        string path = Path.Combine(spriteFilePath, spriteName, currentAnimationType); //Path.Combine is like string concatonation for file systems but changes / and \ depending on system!
        string[] files = Directory.GetFiles(path);
        BitmapImage[] animFrames = new BitmapImage[files.Length];

        for (int x = 0; x < files.Length; x++)
    {
        animFrames[x] = new BitmapImage(new Uri(files[x], UriKind.Absolute));
    }
        return animFrames;
    }

 //currentFrame = (currentFrame + 1) % animation.Frames.Length;
}

