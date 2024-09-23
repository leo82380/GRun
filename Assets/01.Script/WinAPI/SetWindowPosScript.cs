using System.Runtime.InteropServices;

public static class SetWindowPosScript
{
    [DllImport("user32.dll")] static extern int GetForegroundWindow();

    [DllImport("user32.dll", EntryPoint="SetWindowPos")]
    public static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    
    private static int handle;

    public static int GetHandle() => GetForegroundWindow();
}
