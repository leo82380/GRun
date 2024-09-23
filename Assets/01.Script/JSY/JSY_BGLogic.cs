using UnityEngine;
using UnityEngine.UI;

public class JSY_BGLogic : MonoBehaviour
{
    [SerializeField]
    private Camera uiCam;

    private void Awake()
    {
        BGModification();
    }
    private void BGModification()
    {
        GetPassWordUIImage();
    }

    private void GetPassWordUIImage()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 32);
        uiCam.targetTexture = renderTexture;
        uiCam.Render();
        RenderTexture.active = renderTexture;

        Texture2D passwordUI = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        passwordUI.ReadPixels(rect, 0, 0);
        passwordUI.Apply();

        RenderTexture.active = null;

        BGManager.Instance.SetModifiedBG(BGManager.Instance.GetPcWallpaper(), passwordUI);
    }
}
