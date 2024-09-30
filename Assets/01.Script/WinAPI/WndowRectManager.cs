using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WndowRectManager : MonoSingleton<WndowRectManager>
{

    private IntPtr windowHandle;

    public WinRect destRect;


    private void Start()
    {
        windowHandle = GetActiveWindow();
    }

    private void Update()
    {
        GetWindowRect();
    }

    public void GetWindowRect()
    {
        WinRect winRect;
        WinRect frameRect = new WinRect();
        WinRect borderRect;

        GetWindowRect(windowHandle, out winRect);
        DwmGetWindowAttribute(windowHandle, (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out frameRect, Marshal.SizeOf(frameRect));
        
        borderRect.left = frameRect.left - winRect.left;
        borderRect.top = frameRect.top - winRect.top;
        borderRect.right = winRect.right - frameRect.right;
        borderRect.bottom = winRect.bottom - frameRect.bottom;

        destRect.left = winRect.left + borderRect.left + 1;
        destRect.top = winRect.top + borderRect.top + 1;
        destRect.right = winRect.right - borderRect.right - 1;
        destRect.bottom = winRect.bottom - borderRect.bottom - 1;

        int screenWidth = destRect.right - destRect.left;
        int screenHeight = destRect.bottom - destRect.top;

        int captionY = screenHeight - Screen.height;
        int captionX = screenWidth - Screen.width;

        destRect.top += captionY;
        destRect.left += captionX * 2;
    }


    public WinRect GetWindowOutsideRect()
    {
        return new WinRect()
        {
            left = Mathf.Max(0, -destRect.left),
            top = Mathf.Max(0, -destRect.top),
            right = Mathf.Max(destRect.right - Screen.currentResolution.width, 0),
            bottom = Mathf.Max(destRect.bottom - Screen.currentResolution.height, 0)
        };
    }

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hwnd, out WinRect lpRect);

    [DllImport("dwmapi.dll")]
    static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out WinRect pvAttribute, int cbAttribute);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
}

[Serializable]
public struct WinRect
{
    public int left;
    public int top;
    public int right;
    public int bottom;
}

[Flags]
public enum DwmWindowAttribute : int
{
    DWMWA_NCRENDERING_ENABLED = 1,
    DWMWA_NCRENDERING_POLICY,
    DWMWA_TRANSITIONS_FORCEDISABLED,
    DWMWA_ALLOW_NCPAINT,
    DWMWA_CAPTION_BUTTON_BOUNDS,
    DWMWA_NONCLIENT_RTL_LAYOUT,
    DWMWA_FORCE_ICONIC_REPRESENTATION,
    DWMWA_FLIP3D_POLICY,
    DWMWA_EXTENDED_FRAME_BOUNDS,
    DWMWA_HAS_ICONIC_BITMAP,
    DWMWA_DISALLOW_PEEK,
    DWMWA_EXCLUDED_FROM_PEEK,
    DWMWA_CLOAK,
    DWMWA_CLOAKED,
    DWMWA_FREEZE_REPRESENTATION,
    DWMWA_LAST
}