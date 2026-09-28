

using System.Windows;


namespace DesktopPet
{
    public partial class DPApp : Application
    {
        
    public void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running
            // Process command line args
            bool startMinimized = false;
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "/StartMinimized")
                {
                    startMinimized = true;
                }
            }

            SpriteManager spriteManager = new SpriteManager();

                        // Create main application window, starting minimized if specified

            Sprite A = spriteManager.LoadSprite("kris");
            //Sprite B = spriteManager.LoadSprite("jevil");
            PetBasics kris = new PetBasics(A,spriteManager);
            //PetBasics jevil = new PetBasics(B,spriteManager);

            if (startMinimized)
            {
                kris.WindowState = WindowState.Minimized;
                //jevil.WindowState = WindowState.Minimized;
            }
            kris.Show();
            //jevil.Show();
            

            //Character sprites to add:
            //Overwatch
            //Echo
        }

        

    }
}