//Man-in-middle class that holds all the screens and handles switching between them


using System.Configuration;


namespace DesktopPet;
public class SpriteManager
{
    private  List<Sprite> SpritesAvailable = new List<Sprite>();

    public SpriteManager()
    {
        //load the sprite type and 
    }


    public void LoadAllSprites()
    {
        foreach (Sprite sprite in SpritesAvailable)
        {
            loadSprite(sprite.spriteName);
        }
    }

    public void loadSprite(String x) //Reads image + stores
    {
        Sprite generatedSprite = new Sprite(x);
        SpritesAvailable.Add(generatedSprite);
        //might be used more later for feed system
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






