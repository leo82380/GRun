using System;
using System.Runtime.InteropServices;

public static class WindowHandle
{
    private static int windowHandle;
    
    [DllImport("user32.dll")]
    private static extern int GetActiveWindow();
    
    public static int GetWindowHandle()
    {
        if (windowHandle == null)
        {
            windowHandle = GetActiveWindow();
        }
        return windowHandle;
    }
}