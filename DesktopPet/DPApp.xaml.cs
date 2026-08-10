

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
                        // Create main application window, starting minimized if specified
            PetBasics mainWindow = new PetBasics();
            if (startMinimized)
            {
                mainWindow.WindowState = WindowState.Minimized;
            }
            mainWindow.Show();
        }

    }
}