using System;
using System.Runtime.InteropServices;

public static class NativeWindowAlert
{
    [DllImport("user32.dll")]
    private static extern int MessageBoxW(int hWnd, IntPtr lpText, IntPtr lpCaption, uint flag);
    
    public static int ShowMessageBox(string message, string title, uint flag)
    {
        IntPtr msg = Marshal.StringToHGlobalUni(message);
        IntPtr titlePtr = Marshal.StringToHGlobalUni(title);
        int result = MessageBoxW(WindowHandle.GetWindowHandle(), msg, titlePtr, flag);
        Marshal.FreeHGlobal(msg);
        Marshal.FreeHGlobal(titlePtr);
        return result;
    }
}

[System.Serializable]
public enum NativeTag : long
{
    Error = 0x00000010L,
    Question = 0x00000020L,
    Warning = 0x00000030L,
    Info = 0x00000040L,
    Ok = 0x00000000L,
    OkCancel = 0x00000001L,
    ReCancel = 0x00000005L,
    Yesno = 0x00000004L,
    YesnoCancel = 0x00000003L
}

public enum ButtonType
{
    OK = 0,
    Cancel = 1,
    Yes = 6,
    No = 7
}