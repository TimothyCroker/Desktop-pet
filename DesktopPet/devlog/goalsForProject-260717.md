

# OVERALL PROJECT GOALS
-To create a simple desktop pet and develop my c# skills.
-At a minimum, program will keep track of cursor location, pet location and clicks and be able to drag and drop pet to various locations.
-Pet will have idle animations along with dragging and dropping animations and will be it's own frame, created using WPF.

---


#### Brief overview on the parts:
> C# - Game logic + physics calcs
> WPF - Pet rendering using XAML + Transparent window
> Win32 API (P/Invoke) - Interactions with desktop and windows.

## C Sharp
-Counter keeps track of time spent.
-Tracks pane sizes and positions
-Tracks cursor

Decides to what to show and when, using controllers.

Will include:
Idle behaviours
Walking cycle on bottom bar and panes.
Menu panel for settings

Calls wpf commands whenever something needs changing, using a 


Fun logic to include:
Running into things + colliding
Falling.
Chasing cursor/Looking at cursor.
Sleeping
Menu panel for settings
## WPF
Transparent windows+Always on top+borderless
Animations!
Only rendering. Requires win32 API, which will need **P/Invoke**.

Will be using simple pixel art which will be changed every so often to appear animated (Multiple animation frames)

Uses XAML (similar to html and XML) to show what will be on screen.

#### P Invoke
Is required as C# cant directly call win32 API functions like findWindow.