using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public void DestroyImage() { Object.Destroy(image); }
}

public class PhoneGallary : MonoBehaviour {

    public GameObject camCanvas, phoneCanvas;
    public GameObject gallarySeleted, gallaryScroll, deleteWindow;
    public List<Photo> gallaryImages = new List<Photo>();
    public List<GameObject> gallaryScrolls = new List<GameObject>();
    private List<float> gallaryScrollTarget = new List<float>();
    public GameObject scrollImagePrefeb;
    public GameObject mainCamera;

    int selectNum = 0;
    public bool isGallary;
    private bool isCheckDeletion;
    public bool isLandscapeMode;

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
        //input key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isGallary = !isGallary;
            ToggleGallary();
        }

        //in gallary
        if (isGallary == true)
        {
            if (gallaryImages.Count > 0)
            {//delete
                if (Input.GetKeyDown(KeyCode.R))
                {
                    isCheckDeletion = true;
                    deleteWindow.SetActive(true);
                }


                if (isCheckDeletion == true)
                {
                    //delete check
                    ConfirmDeletion(selectNum);
                }
                if (isCheckDeletion != true)
                {
                    //select other image by mouse wheel
                    float wheelInput = Input.GetAxis("Mouse ScrollWheel");
                    if (wheelInput > 0)
                    {
                        if (selectNum < gallaryImages.Count - 1)
                        {
                            RefreshSelectedImage(selectNum + 1);
                            SetScrollImages();
                            //Debug.Log("WheelDown");
                        }
                    }
                    else if (wheelInput < 0)
                    {
                        if (selectNum > 0)
                        {
                            RefreshSelectedImage(selectNum - 1);
                            SetScrollImages();
                            //Debug.Log("WheelUp");
                        }
                    }
                }

                //selected image rotation setting
                float rotationSetting = 0;
                float scaleSettingX = 11;
                float scaleSettingY = 11;
                if (gallaryImages[selectNum].GetMode() == true)
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

                //scrolling image
                for (int i = gallaryScrolls.Count - 1; i >= 0; i--)
                {
                    int scrollSpeed = 5;
                    float lerpX = Mathf.Lerp(gallaryScrolls[i].transform.localPosition.x, gallaryScrollTarget[i], Time.deltaTime * scrollSpeed);
                    gallaryScrolls[i].transform.localPosition = new Vector3(lerpX, -520, 0);
                }
            }
            
        }
        else
        {
            //change mode in camera
            if (Input.GetMouseButton(1))    //right button
            {
                isLandscapeMode = true;
            }
            else
            {
                isLandscapeMode = false;
            }
        }

        //change direction of phone
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

    private void ToggleGallary()
    {

        if (isGallary == true)
        {
            camCanvas.SetActive(false);
            phoneCanvas.SetActive(true);

            isCheckDeletion = false;
            isLandscapeMode = false;
            deleteWindow.SetActive(false);
            ImageLoad(gallaryImages.Count - 1);
        }
        else
        {
            camCanvas.SetActive(true);
            phoneCanvas.SetActive(false);
        }

    }

    public void ImageSave(Texture2D SS, string name, int num)
    {

        //string path = "ScreenShot/" + name;
        //gallaryPath.Add(path);
        Photo tmpPhoto = new Photo(SS, name, isLandscapeMode, num);
        gallaryImages.Add(tmpPhoto);

    }
    public void ImageLoad(int idx)
    {

        if (gallaryImages.Count <= 0) { return; }
        RefreshSelectedImage(idx);

        //delete every scroll picture
        for (int i = gallaryScrolls.Count - 1; i >= 0; i--)
        {
            Destroy(gallaryScrolls[i]);
        }

        //reset contents
        gallaryScrolls.Clear();
        gallaryScrollTarget.Clear();

        //create images
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
            if (gallaryImages[i].GetMode() == true)
            {
                rotationSetting = 90.0f;
                scaleSettingX *= 0.75f;
                scaleSettingY /= 0.75f;
                //w /= 2;
                //h *= 2;
            }
            g.transform.localPosition = new Vector3(((gallaryImages.Count - 1) - i) * 400, -520, 0);
            g.transform.localEulerAngles = new Vector3(0, 0, rotationSetting);
            g.transform.localScale = new Vector3(scaleSettingX, scaleSettingY, 1);
            g.GetComponent<Image>().sprite = Sprite.Create(tmp, new Rect(0, h / 4, w, h / 2), new Vector2(0.5f, 0.5f));

            gallaryScrolls.Add(g);
            gallaryScrollTarget.Add(g.transform.position.x);
        }
        SetScrollImages();

    }
    void RefreshSelectedImage(int idx)
    {

        selectNum = idx;
        if (selectNum >= gallaryImages.Count) selectNum = gallaryImages.Count - 1;
        if (selectNum < 0) selectNum = 0;
        Texture2D t = gallaryImages[selectNum].GetImage();

        Rect tmpRect = new Rect(0, 0, t.width, t.height);
        gallarySeleted.GetComponent<Image>().sprite = Sprite.Create(t, tmpRect, new Vector2(0.5f, 0.5f));
   
    }
    void SetScrollImages()
    {

        for (int i = gallaryScrolls.Count - 1; i >= 0; i--)
        {
            gallaryScrollTarget[i] = (i - (gallaryScrolls.Count - selectNum - 1)) * 400;
        }

    }
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
    void DeletePicture(int idx)
    {

        if (gallaryImages.Count <= 0) { return; }
        //Destroy(gallaryImages[idx]);
        gallaryImages[idx].DestroyImage();
        gallaryImages.RemoveAt(idx);
        ImageLoad(idx);

    }
}
