using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpCamera : MonoBehaviour
{
    //WeaponCamera
    public Camera WeaponCamera;
    public RawImage GunInGameView;
    private RenderTexture _gunTexture;
    //MiniMapCamera
    public Camera MiniMapCamera;
    public RawImage MiniMapImg;
    private RenderTexture _miniMapTexture;
    //LargeMapCamera
    public Camera LargeMapCamera;
    private RenderTexture _largeMapTexture;
    //NavCamera
    public Camera NavCamera;
    public RawImage NavImg;
    private RenderTexture _navMapTexture;
    private void Awake()
    {
        SetUpGunCamera();
        SetUpLargeMapCanera();
        //SetUpMiniMapCamera();
        SetUpNavCamera();
    }
    private void Start()
    {
        //StartCoroutine(WaitForLargeMapRenderTexture());
    }
    void SetUpGunCamera()
    {
        _gunTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 24);
        _gunTexture.Create();
        _gunTexture.Release();
        WeaponCamera.targetTexture = _gunTexture;
        GunInGameView.texture = _gunTexture;
        GunInGameView.enabled = true;
    }
    void SetUpMiniMapCamera()
    {
        _miniMapTexture = new RenderTexture(300, 300, 24);
        _miniMapTexture.Create();
        _miniMapTexture.Release();
        MiniMapCamera.targetTexture = _miniMapTexture;
        MiniMapImg.texture = _miniMapTexture;
        MiniMapImg.enabled = true;
    }
    void SetUpNavCamera()
    {
        _navMapTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 24);
        _navMapTexture.Create();
        _navMapTexture.Release();
        NavCamera.targetTexture = _navMapTexture;
        NavImg.texture = _navMapTexture;
        NavImg.enabled = true;
    }
    void SetUpLargeMapCanera()
    {
        LargeMapCamera.enabled = false;
    }
    IEnumerator WaitForLargeMapRenderTexture()
    {
        _largeMapTexture = new RenderTexture(Screen.width, Screen.height, 24);
        LargeMapCamera.targetTexture = _largeMapTexture;
        _largeMapTexture.Release();
        yield return new WaitForEndOfFrame();
        //Save Render Texture as Png file
        string path = "Assets/Resources/Largemap.png";
        SaveRenderTextureToFile(_largeMapTexture, path, SaveTextureFileFormat.PNG, 95);
        yield return new WaitForEndOfFrame();
        Texture tex = Resources.Load<Texture>("Largemap");
        Debug.Log(tex.GetType());
        LargeMapCamera.gameObject.SetActive(false);
    }

    public void SaveRenderTextureToFile(RenderTexture renderTexture, string filePath, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG, int jpgQuality = 95)
    {
        Texture2D tex;
        if (fileFormat != SaveTextureFileFormat.EXR)
            tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, false);
        else
            tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false, true);
        var oldRt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = oldRt;
        SaveTexture2DToFile(tex, filePath, fileFormat, jpgQuality);
        if (Application.isPlaying)
            Object.Destroy(tex);
        else
            Object.DestroyImmediate(tex);

    }

    public void SaveTexture2DToFile(Texture2D tex, string filePath, SaveTextureFileFormat fileFormat, int jpgQuality = 95)
    {
        switch (fileFormat)
        {
            case SaveTextureFileFormat.EXR:
                System.IO.File.WriteAllBytes(filePath + ".exr", tex.EncodeToEXR());
                break;
            case SaveTextureFileFormat.JPG:
                System.IO.File.WriteAllBytes(filePath + ".jpg", tex.EncodeToJPG(jpgQuality));
                break;
            case SaveTextureFileFormat.PNG:
                System.IO.File.WriteAllBytes(filePath + ".png", tex.EncodeToPNG());
                break;
            case SaveTextureFileFormat.TGA:
                System.IO.File.WriteAllBytes(filePath + ".tga", tex.EncodeToTGA());
                break;
        }
    }
}
public enum SaveTextureFileFormat
{
    EXR, JPG, PNG, TGA
};
