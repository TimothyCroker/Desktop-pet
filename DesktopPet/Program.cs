using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using System.Windows;
/*
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
*/


namespace xyz //Container for classes and other namespaces
{
	class Program
		{

			static void Main()
			{
			Console.WriteLine("Initial Test");
			int[] a = new int[20];

			for(int x = 0; x < 10; x++)
			{
				a[x] = x;
			}
			Console.WriteLine("[{0}]", string.Join(", ", a));
			WinApiTest ab = new WinApiTest();
			WinApiTest.Silly(ab);
			}
		}
	public partial class WinApiTest
		{
			[LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
			private static partial int MessageBoxW(IntPtr hWnd, string lpText, string lpCaption, uint uType);

		public static void Silly(WinApiTest a)
		{
			//Calls function as a regular managed method. Dont need to import library again
			MessageBoxW(IntPtr.Zero, "woah!", "a box", 0);
			
			LocalMethods.POINT cursorPos;
			
			TimeSpan dtLast = DateTime.Now.TimeOfDay;
			TimeSpan repeatCursorValue = TimeSpan.FromSeconds(0.25);

			while (true)
			{		

				TimeSpan dt = DateTime.Now.TimeOfDay;
				if (TimeSpan.Compare(repeatCursorValue,dt-dtLast) == -1)
				{
					dtLast = DateTime.Now.TimeOfDay;
					LocalMethods.GetCursorPos(out cursorPos);
					Console.WriteLine($"X = {cursorPos.X}, Y = {cursorPos.Y}");
				};

			};

			
		}
    }

	public partial class LocalMethods{


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
	
		[DllImport("user32.dll")]
    	public static extern bool GetCursorPos(out POINT point);
		public static System.Windows.Point GetCursorPosition()
		{
			POINT lpPoint;
			GetCursorPos(out lpPoint);
            
			return lpPoint;
		}
		
	}	




	/*
	public partial class basicSpriteMove() //Not using this as will be using WPF
	{
		[LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
		private static partial int CreateWindowEx(
			"Sprite", 
			MAKEINTRESOURCE,
			SS_ICON | WS_VISIBLE,
			xParam, yParam,
			sWith, sHeight
		)

		
	}
	
	*/
}

