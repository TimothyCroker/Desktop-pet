
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

public partial class PetBasics : Window
{

    private BitmapImage[]? currentAnimation;
    private int currentFrame;

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT point);
    public static System.Windows.Point GetCursorPosition()
    {
        POINT lpPoint;
        GetCursorPos(out lpPoint);
        
        return lpPoint;
    }


    
    public PetBasics()
    {
        InitializeComponent();
        //SpriteManager spriteManager = new SpriteManager();
        //Here is where we would check the save folder for an existing character, to load or otherwise open the menu first.
        //Maybe a quick popup telling users what characters they can use and about the program.
        LoadCharacter("recources/CharacterSprites/laughingDyr.png");
        //Sprite pet = new Sprite("Dyr");

    }

    private void LoadCharacter(string path)
    {
        CharacterImage.Source = new BitmapImage(new Uri(path, UriKind.Relative)); //Questionmark after currentAnimation makes it skip the call to the field if it is null. Essentially superflous
        currentFrame = 0;
    }

    private void CharacterClick(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left){
            
            LoadCharacter("recources/CharacterSprites/shockedDyr.png");
            this.DragMove();
            LoadCharacter("recources/CharacterSprites/laughingDyr.png");
        }
        //if (e.ChangedButton == MouseButton.Right)
            
    }

    private void HelloClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Hello");
    }
}
}