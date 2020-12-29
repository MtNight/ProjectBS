using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 - Gallay Function (Create Photo, Set Gallary, Delete Photo)
 - Turn On / Off gallary
 - Turn On / Off landscape mode
 - Phone Movement
 */

//Photo Class
public class Photo
{
    Texture2D image;
    string imageName;
    bool isLandscapeMode;
    bool[] missionCheck = new bool[10];

    public Photo(Texture2D SS, string name, bool mode, int num)
    {
        image = SS;
        imageName = name;
        isLandscapeMode = mode;
        for (int i = 0; i < 10; i++)
        {
            missionCheck[i] = false;
        }
        if (num >= 0)
        {
            missionCheck[num] = true;
        }
    }

    public Texture2D GetImage() { return image; }
    public bool GetMode() { return isLandscapeMode; }
    public void DestroyImage() { /*Object.Destroy(image);*/ }
}

public class PhoneGallary : MonoBehaviour {

    public GameObject mainCamera;   //Camera
    public GameObject camCanvas, phoneCanvas;   //Canvases
    public GameObject gallarySeleted, gallaryScroll, deleteWindow;  //Gallary Component
    
    public List<Photo> gallaryImages = new List<Photo>();           //List of Photo Class
    public List<GameObject> gallaryScrolls = new List<GameObject>();//List of Photo's Scroll GameObject
    private List<float> gallaryScrollXpos = new List<float>();      //List of Scroll Photo's x position
    public GameObject scrollImagePrefeb;   //Instance for scroll

    int selectNum = 0;
    public bool isGallary;
    private bool isCheckDeletion;
    public bool isLandscapeMode;

    public KeyCode GallaryKey;
    public KeyCode DeleteKey;

    void Start ()
    {
        camCanvas = transform.GetChild(2).gameObject;
        phoneCanvas = transform.GetChild(3).gameObject;
        gallarySeleted = phoneCanvas.transform.GetChild(0).gameObject;
        deleteWindow = phoneCanvas.transform.GetChild(1).gameObject;
        scrollImagePrefeb = Resources.Load("Prefabs/Scrolled Image") as GameObject;
        mainCamera = transform.parent.gameObject;

        isGallary = false;
        isCheckDeletion = false;
        isLandscapeMode = false;
    }

    void Update ()
    {
        //Gallary Key input
        if (Input.GetKeyDown(GallaryKey))
        {
            isGallary = !isGallary;
            ToggleGallary();
        }

        //In gallary
        if (isGallary == true)
        {
            if (gallaryImages.Count > 0)
            {
                //Delete Photo
                if (Input.GetKeyDown(DeleteKey))
                {
                    isCheckDeletion = true;
                    deleteWindow.SetActive(true);
                }

                if (isCheckDeletion == true)
                {
                    //create deletion checking window
                    ConfirmDeletion(selectNum);
                }
                if (isCheckDeletion != true)
                {
                    //Select other image by mouse wheel (scrolling)
                    float wheelInput = Input.GetAxis("Mouse ScrollWheel");
                    if (wheelInput > 0)         //WheelDown = PhotoRight
                    {
                        if (selectNum < gallaryImages.Count - 1)
                        {
                            selectNum++;
                            RefreshSelectedImage();
                            RefreshScrollImage();
                            //Debug.Log("WheelDown");
                        }
                    }
                    else if (wheelInput < 0)    //WheelUp = PhotoLeft
                    {
                        if (selectNum > 0)
                        {
                            selectNum--;
                            RefreshSelectedImage();
                            RefreshScrollImage();
                            //Debug.Log("WheelUp");
                        }
                    }
                }

                //Selected image setting
                if (gallaryImages.Count > 0)
                {
                    float rotationSetting = 0;
                    float scaleSettingX = 11;
                    float scaleSettingY = 11;
                    if (gallaryImages[selectNum].GetMode() == true) //Landscape Mode Setting
                    {
                        rotationSetting = 90.0f;
                        scaleSettingX *= 0.75f / 2;
                        scaleSettingY *= 0.75f * 2;
                    }
                    Vector3 tmpRot = gallarySeleted.transform.localEulerAngles;
                    tmpRot.z = rotationSetting;
                    gallarySeleted.transform.localRotation = Quaternion.Euler(tmpRot);

                    Vector3 tmpScale = gallarySeleted.transform.localScale;
                    tmpScale.x = scaleSettingX;
                    tmpScale.y = scaleSettingY;
                    gallarySeleted.transform.localScale = tmpScale;
                }

                //scrolling image
                for (int i = 0; i < gallaryScrolls.Count; i++)
                {
                    int scrollSpeed = 5;
                    float lerpX = Mathf.Lerp(gallaryScrolls[i].transform.localPosition.x, gallaryScrollXpos[i], Time.deltaTime * scrollSpeed);
                    gallaryScrolls[i].transform.localPosition = new Vector3(lerpX, -520, 0);
                }
            }
        }
        else
        {
            //Change camera mode (Normal - Landscape)
            if (Input.GetMouseButton(1))    //right button
            {
                isLandscapeMode = true;
            }
            else
            {
                isLandscapeMode = false;
            }
        }

        //Phone Movement
        int changeSpeed = 5;
        if (isLandscapeMode == true)    //right button
        {
            Vector3 tmpRot = transform.rotation.eulerAngles;
            tmpRot.z = Mathf.Lerp(tmpRot.z, 270, Time.deltaTime * changeSpeed);
            transform.rotation = Quaternion.Euler(tmpRot);
        }
        else
        {
            Vector3 tmpRot = transform.rotation.eulerAngles;
            tmpRot.z = Mathf.Lerp(tmpRot.z, 180, Time.deltaTime * changeSpeed);
            transform.rotation = Quaternion.Euler(tmpRot);
        }
    }

    //Turn on/off gallary
    private void ToggleGallary()
    {
        if (isGallary == true)
        {
            camCanvas.SetActive(false);
            phoneCanvas.SetActive(true);

            isCheckDeletion = false;
            isLandscapeMode = false;
            deleteWindow.SetActive(false);
            ImageLoad();
        }
        else
        {
            camCanvas.SetActive(true);
            phoneCanvas.SetActive(false);
        }
    }

    //Create new photo (from 'ScreenShot' script)
    public void ImageSave(Texture2D SS, string name, int num)
    {
        //string path = "ScreenShot/" + name;
        //gallaryPath.Add(path);
        Photo tmpPhoto = new Photo(SS, name, isLandscapeMode, num);
        gallaryImages.Add(tmpPhoto);
        selectNum = gallaryImages.Count - 1;
    }

    //Initiallize Gallary
    public void ImageLoad()
    {
        if (gallaryImages.Count <= 0) { return; }
        //Set Selected image
        RefreshSelectedImage();

        //delete every scroll photos
        for (int i = 0; i < gallaryScrolls.Count; i++)
        {
            Destroy(gallaryScrolls[i]);
        }

        //reset contents
        gallaryScrolls.Clear();
        gallaryScrollXpos.Clear();

        //create images
        CreateImage();

        //locate Scroll images
        RefreshScrollImage();
    }

    //create images
    void CreateImage()
    {
        for (int i = gallaryImages.Count - 1; i >= 0; i--)
        {
            GameObject g = Instantiate(scrollImagePrefeb, new Vector3(0, 0, 0), Quaternion.identity);
            g.transform.SetParent(phoneCanvas.gameObject.transform);

            Texture2D tmp = gallaryImages[i].GetImage();
            float rotationSetting = 0;
            float scaleSettingX = 3.5f;
            float scaleSettingY = 3.5f * 0.75f;
            float w = tmp.width;
            float h = tmp.height;
            if (gallaryImages[i].GetMode() == true) //Landscape Mode Setting
            {
                rotationSetting = 90.0f;
                scaleSettingX *= 0.75f;
                scaleSettingY /= 0.75f;
            }

            g.transform.localPosition = new Vector3(((gallaryImages.Count - 1) - i) * 400, -520, 0);
            g.transform.localEulerAngles = new Vector3(0, 0, rotationSetting);
            g.transform.localScale = new Vector3(scaleSettingX, scaleSettingY, 1);
            g.GetComponent<Image>().sprite = Sprite.Create(tmp, new Rect(0, h / 4, w, h / 2), new Vector2(0.5f, 0.5f));

            gallaryScrolls.Add(g);
            gallaryScrollXpos.Add(g.transform.position.x);
        }

    }

    //Set Seleted image
    void RefreshSelectedImage()
    {
        Texture2D t;
        if (gallaryImages.Count == 0)
        {
            t = new Texture2D(128 / 2, 128, TextureFormat.RGB24, false);
        }
        else
        {
            if (selectNum < 0)                      { selectNum = 0; }
            if (selectNum >= gallaryImages.Count)   { selectNum = gallaryImages.Count - 1; }
            t = gallaryImages[selectNum].GetImage();
        }

        Rect tmpRect = new Rect(0, 0, t.width, t.height);
        gallarySeleted.GetComponent<Image>().sprite = Sprite.Create(t, tmpRect, new Vector2(0.5f, 0.5f));
    }

    //Set scroll image's location
    void RefreshScrollImage()
    {
        if (gallaryImages.Count == 0) { return; }

        if (selectNum >= gallaryImages.Count) { selectNum = gallaryImages.Count - 1; }
        if (selectNum < 0) { selectNum = 0; }
        for (int i = 0; i < gallaryScrolls.Count; i++)
        {
            //gallaryScrollXpos[i] = (i - (gallaryScrolls.Count - selectNum - 1)) * 400;
            gallaryScrollXpos[i] = (i + selectNum - (gallaryImages.Count - 1)) * 400;
        }
    }

    //Check Photo Deletion
    void ConfirmDeletion(int idx)
    {
        if (Input.GetMouseButtonDown(1))        //right button == Yes
        {
            DeletePicture(idx);
            isCheckDeletion = false;
            deleteWindow.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(0))   //left button == No
        {
            isCheckDeletion = false;
            deleteWindow.SetActive(false);
        }
    }

    //Delete Photo
    void DeletePicture(int idx)
    {
        if (gallaryImages.Count <= 0) { return; }

        gallaryImages[idx].DestroyImage();
        gallaryImages.RemoveAt(idx);
        GameObject g = gallaryScrolls[(gallaryScrolls.Count - 1) - idx];
        gallaryScrolls.RemoveAt((gallaryScrolls.Count - 1) - idx);
        Destroy(g);
        gallaryScrollXpos.RemoveAt((gallaryScrollXpos.Count - 1) - idx);

        //relocate scroll images
        if (idx != 0)
        {
            selectNum--;
        }
        RefreshSelectedImage();
        RefreshScrollImage();
    }
}
