using System;
using System.Runtime.InteropServices;

public static class WindowHandle
{
    private static IntPtr windowHandle;
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    public static IntPtr GetWindowHandle()
    {
        if (windowHandle == null)
        {
            windowHandle = GetActiveWindow();
        }
        return windowHandle;
    }
}