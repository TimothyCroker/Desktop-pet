//Man-in-middle class that holds all the screens and handles switching between them


using System.Configuration;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DesktopPet;
public class SpriteManager
{
    
    private List<Sprite> SpritesAvailable = new List<Sprite>();
    private DispatcherTimer _timer; //Timer for wpf
    public event EventHandler? Tick;

    List<RECT> windows;

    private int debugcounterupdatecounter = 0;

    public SpriteManager()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(50);
        _timer.Tick += Update;
        _timer.Start();

    }


    /*
    public List<Sprite> LoadAllSprites()
    {
        foreach (Sprite sprite in SpritesAvailable)
        {
            loadSprite(sprite.spriteName);
        }
        return SpritesAvailable;
    }
    */

    public Sprite LoadSprite(String x) //Reads image + stores
    {
        Sprite generatedSprite = new Sprite(x);
        SpritesAvailable.Add(generatedSprite);


        return generatedSprite;
        
        //might be used more later for feed system
    }

    private void Update(object? sender, EventArgs e)
    {
        foreach (Sprite sprite in SpritesAvailable)
        {

            if (sprite.currentAnimationType != "Held"){
                //Check the borders and if its on a surface, then apply gravity if not.
                sprite.SpriteGravity(); 
            }

            if (sprite.currentAnimationType.Contains("Walk"))
            {
                sprite.SpriteWalk();
            }
            
            sprite.UpdateFrame();
            
        }
        Tick?.Invoke(this, EventArgs.Empty);
        
    }

    public List<Sprite> GetSprites()
    {
        return SpritesAvailable;
    }

    public Sprite MainSprite
    {
        get { return SpritesAvailable[0]; }
    }

    
    [DllImport("user32.dll")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

    public void UpdateSpritePos(Sprite sprite,Window window){ //Works if there is only one sprite will have to change logic for multiple.
        

        
        
        RECT rect;
        
    bool success = GetWindowRect(
        new HandleRef(window, new WindowInteropHelper(window).Handle),
        out rect
    );

    if (!success)
    {
        // Handle not valid yet (e.g. window not shown/loaded), or call failed.
        // Don't overwrite sprite.spriteLocation with garbage data.
        return;
    }

    sprite.spriteLocation = rect;


        //lets find out where this is! (and everything else on screen)
        debugcounterupdatecounter++;
        List<RECT> windows = GetVisibleWindows();
        if (debugcounterupdatecounter > 500){
            
                //Get the pos of the sprites. can be removed
                foreach (RECT rect2 in windows)
{
    Console.WriteLine(
        $"L:{rect2.Left} " +
        $"T:{rect2.Top} " +
        $"R:{rect2.Right} " +
        $"B:{rect2.Bottom}"
    );
}debugcounterupdatecounter = 0;
    }}


    public delegate bool EnumWindowsCallback(IntPtr hwnd, IntPtr lParam);
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsCallback lpEnumFunc,IntPtr lParamm);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd,out RECT lpRect);
    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);
    

    public List<RECT> GetVisibleWindows()
    {
        List<RECT> windows = new();

        EnumWindows((hwnd, lParam) =>
        {
            if (IsWindowVisible(hwnd) &&
                GetWindowRect(hwnd, out RECT rect))
            {
                windows.Add(rect);
            }

            return true;
        }, IntPtr.Zero);

        return windows;
    }

/*
    public class SpriteAnimation
{
    public string Name { get; }
    public Sprite[] Frames { get; }

    public SpriteAnimation(string name, Sprite[] frames) //read sequence of images and stores them
    {
        Name = name;
        Frames = frames;
    }
}
*/
 //currentFrame = (currentFrame + 1) % animation.Frames.Length;
}






