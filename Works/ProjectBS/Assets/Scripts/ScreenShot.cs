using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public GameObject cam;       //보여지는 카메라.
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
        path = Application.dataPath + "/Resources/ScreenShot/";
        Debug.Log(path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0))
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
        string name = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        RenderTexture r = new RenderTexture(resWidth, resHeight, 24);
        cam.GetComponent<Camera>().targetTexture = r;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.GetComponent<Camera>().Render();
        RenderTexture.active = r;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        //byte[] bytes = screenShot.EncodeToJPG();
        //File.WriteAllBytes(path + name + ".jpg", bytes);
        cam.GetComponent<Camera>().targetTexture = rt;
        Debug.Log("shot!");
        phone.GetComponent<PhoneGallary>().ImageSave(name, screenShot);
    }
}
