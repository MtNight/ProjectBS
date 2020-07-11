using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

/* ClickScreenShot 함수 조기종료 필요
 */

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

        Ray Ray;
        RaycastHit hit;
        int w = cam.GetComponent<Camera>().pixelWidth;
        int h = cam.GetComponent<Camera>().pixelHeight;
        int cnt = 0;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Ray = cam.GetComponent<Camera>().ScreenPointToRay(new Vector3(w / 20.0f + x * w / 10.0f, h / 20.0f + y * h / 10.0f, 0));
                if (Physics.Raycast(Ray, out hit, 64, capturable) && hit.transform.tag == "Capturable")
                {

                    cnt++;
                    if (cnt >= 25)
                    {
                        MissionObject[][] mObjects = MissionPanel.GetComponent<MissionUI>().mObjects;
                        for (int i = 0; i < mObjects.Length; i++)
                        {
                            Debug.Log(hit.transform.name + ", " + mObjects[i][0].GetPrefabName());
                            bool isMissionObject1 = hit.transform.name.Equals(mObjects[i][0].GetPrefabName());
                            bool isMissionObject2 = hit.transform.name.Equals(mObjects[i][0].GetPrefabName() + "_1");   //나중에 확인해보고 필요 없다면 지울 수 있음.
                            if (isMissionObject1 || isMissionObject2)
                            {
                                MissionPanel.GetComponent<MissionUI>().ClearObjective(i, true);
                                break;
                            }
                        }
                    }
                    //return;
                }
                Debug.DrawRay(Ray.origin, Ray.direction * 64, Color.red, 0.3f);
            }
        }
        Debug.Log(cnt);
    }
}
