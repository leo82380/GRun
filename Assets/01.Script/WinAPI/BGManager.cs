using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    public static BGManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SystemParametersInfo(uint uAction, int uParam, string lpvParam, uint fuWinIni); //WINAPI 윈도우 시스템 값 변경 함수

    [DllImport("libwebp")]
    public static extern IntPtr WebPDecodeRGBA(IntPtr data, int dataSize, out int width, out int height);

    public const uint SPI_GETDESKWALLPAPER = 0x0073;
    public const uint SPI_SETDESKWALLPAPER = 0x0014;
    public const uint SPIF_UPDATEINIFILE = 0x0001;
    public const uint SPIF_SENDWININICHANGE = 0x0002;


    private byte[] defaultBGbyte;

    private void OnApplicationQuit()
    {
        SetDefaultBG();
    }


    public Texture2D OverlayImages(Texture2D baseImage, Texture2D overlayImage)
    {
        Texture2D result = new Texture2D(2, 2);
        Vector2 pos = new Vector2();
        int width = baseImage.width;
        int height = baseImage.height;

        result = new Texture2D(width, height, TextureFormat.ARGB32, false);

        pos.x = (baseImage.width / 2) - (Screen.width / 2);
        pos.y = (baseImage.height / 2) - (Screen.height / 2);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result.SetPixel(x, y, baseImage.GetPixel(x, y));
            }
        }

        for (int y = 0; y < overlayImage.height; y++)
        {
            for (int x = 0; x < overlayImage.width; x++)
            {
                if (x + pos.x < width && y + pos.y < height)
                {
                    Color baseColor = result.GetPixel((int)(x + pos.x), (int)(y + pos.y));
                    Color overlayColor = overlayImage.GetPixel(x, y);
                    Color finalColor = Color.Lerp(baseColor, overlayColor, overlayColor.a / 1f);
                    result.SetPixel((int)(x + pos.x), (int)(y + pos.y), finalColor);
                }
            }
        }



        return result;
    }


    public void SetModifiedBG(Texture2D origin, Texture2D modified)
    {
        File.WriteAllBytes(GetCurrentDesktopWallPaperPath(), modified.EncodeToPNG());

        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, GetCurrentDesktopWallPaperPath(), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

        //배경화면 2개를 겹쳐서 최종 적용해주는 기능
    }

    public Texture2D LoadTextureFromPath(string path)
    {

        //배경 화면 경로에서 텍스쳐 가져오는 기능

        Texture2D texture = new Texture2D(2, 2);

        defaultBGbyte = File.ReadAllBytes(path);

        if (ImageConversion.LoadImage(texture, defaultBGbyte)) //jpg , png , jpeg 
        {
            Color[] bgColors = texture.GetPixels();

            texture.Reinitialize(texture.width, texture.height, TextureFormat.RGBA32, false);

            texture.SetPixels(bgColors);

            texture.Apply();
        }
        else // webp
        {
            Debug.Log("이거 Webp임");
            texture = LoadWebpImage(defaultBGbyte);
            // webp 확장자만 읽을 수 있게 해주는 기능
        }

        return texture;
    }

    public byte[] FlipImageVertically(byte[] rawimage, int width, int height)
    {
        int stride = width * 4;
        byte[] flippedImage = new byte[rawimage.Length];

        for (int y = 0; y < height; y++)
        {
            Array.Copy(rawimage, y * stride, flippedImage, (height - 1 - y) * stride, stride);
        }

        return flippedImage;
    }

    public Texture2D LoadWebpImage(byte[] webpData)
    {
        int width, height;
        IntPtr dataPtr = Marshal.AllocHGlobal(webpData.Length);
        Marshal.Copy(webpData, 0, dataPtr, webpData.Length);

        IntPtr resultIntPtr = WebPDecodeRGBA(dataPtr, webpData.Length, out width, out height);
        if (resultIntPtr == IntPtr.Zero)
        {
            Debug.LogError("Failed to decode Webp Image");
            Marshal.FreeHGlobal(dataPtr);
            return null;
        }

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        byte[] rgbaImage = new byte[width * height * 4];
        Marshal.Copy(resultIntPtr, rgbaImage, 0, rgbaImage.Length);

        Marshal.FreeHGlobal(resultIntPtr);
        Marshal.FreeHGlobal(dataPtr);


        byte[] flippedImage = FlipImageVertically(rgbaImage, width, height);

        texture.LoadRawTextureData(flippedImage);
        texture.Apply();



        return texture;
    }

    public Texture2D GetPcWallpaper()
    {
        return LoadTextureFromPath(GetCurrentDesktopWallPaperPath());
    }

    public string GetCurrentDesktopWallPaperPath()
    {
        string curWallpaper = new string('\0', 260);
        SystemParametersInfo(SPI_GETDESKWALLPAPER, curWallpaper.Length, curWallpaper, 0);
        Debug.Log(curWallpaper);


        return curWallpaper.Substring(0, curWallpaper.IndexOf('\0'));
    }


    public void SetDefaultBG()
    {
        if (defaultBGbyte.Length <= 0)
        {
            return;
        }

        File.WriteAllBytes(GetCurrentDesktopWallPaperPath(), defaultBGbyte);
        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, GetCurrentDesktopWallPaperPath(), SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
    }
}
