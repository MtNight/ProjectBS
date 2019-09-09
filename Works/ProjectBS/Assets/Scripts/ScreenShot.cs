using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    public new Camera camera;       //보여지는 카메라.
    public RenderTexture rt;
    private GameObject phone;

    private int resWidth;
    private int resHeight;
    string path;
    
    void Start()
    {
        phone = transform.GetChild(0).gameObject;
        resWidth = rt.width;
        resHeight = rt.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!phone.GetComponent<PhoneGallary>().isGallary)
            {
                ClickScreenShot();
            }
        }
    }

    public void ClickScreenShot()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        //name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        name = path + "Test.png";
        RenderTexture r = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = r;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = r;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
        camera.targetTexture = rt;
        Debug.Log("shot!");
    }
}
