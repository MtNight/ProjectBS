using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

/* 
 - Camera capture function
 - Check Mission Object & Send information to gallary
 */

//ClickScreenShot 함수 조기종료 필요할지도 모름

public class ScreenShot : MonoBehaviour
{
    public GameObject cam;       //보여지는 카메라.
    public RenderTexture rt;
    private GameObject phone;
    private int resWidth;
    private int resHeight;
    string path;

    public LayerMask capturable;
    public GameObject MissionPanel;

    public bool isStartGame = false;

    void Start()
    {
        phone = transform.parent.GetChild(1).gameObject;
        resWidth = rt.width;
        resHeight = rt.height;
        path = Application.dataPath + "/Resources/ScreenShot/";
        Debug.Log(path);
    }

    private void Update()
    {
        //ScreenShot Key Input
        if (Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0)) //left button
        {
            if (!phone.GetComponent<PhoneGallary>().isGallary && isStartGame)
            {
                ClickScreenShot();
            }
        }
    }

    //ScreenShot functions
    public void ClickScreenShot()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        //Scanning Screen
        RenderTexture r = new RenderTexture(resWidth, resHeight, 24);
        cam.GetComponent<Camera>().targetTexture = r;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cam.GetComponent<Camera>().Render();
        RenderTexture.active = r;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        //Create ScreenShot (RenderTexture)
        //byte[] bytes = screenShot.EncodeToJPG();
        //File.WriteAllBytes(path + name + ".jpg", bytes);
        cam.GetComponent<Camera>().targetTexture = rt;
        Debug.Log("shot!");

        //Find Mission Object in screen
        Ray Ray;
        RaycastHit hit;
        int w = cam.GetComponent<Camera>().pixelWidth;
        int h = cam.GetComponent<Camera>().pixelHeight;

        int cnt = 0;
        int threshold = 25;

        bool isClearObjective = false;
        int numberOfObjective = -1;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(w / 20.0f + x * w / 10.0f, h / 20.0f + y * h / 10.0f, 0));    //shoot ray

                //ray meets mission object
                if (Physics.Raycast(Ray, out hit, 128) && hit.transform.tag == "Capturable")
                {
                    cnt++;
                    if (cnt >= threshold)   //get credit for capture mission object
                    {
                        MissionObject[][] mObjects = MissionPanel.GetComponent<MissionUI>().mObjects;   //mission obejct list
                        for (int i = 0; i < mObjects.Length; i++)
                        {
                            for (int j = 0; j < mObjects[i].Length; j++)
                            {
                                Debug.Log(hit.transform.name + ", " + mObjects[i][j].GetPrefabName());
                                bool isMissionObject1 = hit.transform.name.Equals(mObjects[i][j].GetPrefabName());
                                bool isMissionObject2 = hit.transform.name.Equals(mObjects[i][j].GetPrefabName() + "_1");   //자식오브젝트
                                                                                                                            //find captured mission object's tuple
                                if (isMissionObject1 || isMissionObject2)
                                {
                                    //check mission clear
                                    MissionPanel.GetComponent<MissionUI>().ClearObjective(i, true);
                                    isClearObjective = true;
                                    numberOfObjective = i;
                                    break;
                                }
                            }
                        }
                    }
                    if (isClearObjective == true)   //early end
                    {
                        y = 10;
                        break;
                    }
                }
                Debug.DrawRay(Ray.origin, Ray.direction * 64, Color.red, 0.3f);
            }
        }
        
        //Send information of screenshot to gallary
        phone.GetComponent<PhoneGallary>().ImageSave(screenShot, name, numberOfObjective);
        Debug.Log(cnt);
    }
}
