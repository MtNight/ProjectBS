using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneGallary : MonoBehaviour {

    private GameObject /*phoneCam, phoneGallay,*/ camCanvas, phoneCanvas;
    private GameObject gallarySeleted, gallaryScroll;
    //public List<string> gallaryPath = new List<string>();
    public List<Texture2D> gallaryImages = new List<Texture2D>();
    public List<GameObject> gallaryScrolls = new List<GameObject>();
    private GameObject scrollImagePrefeb;
    private GameObject mainCamera;

    int selectNum = 0;
    public bool isGallary;

	void Start () {
        //phoneCam = transform.GetChild(0).gameObject;
        //phoneGallay = transform.GetChild(1).gameObject;
        camCanvas = transform.GetChild(2).gameObject;
        phoneCanvas = transform.GetChild(3).gameObject;
        gallarySeleted = phoneCanvas.transform.GetChild(0).gameObject;
        scrollImagePrefeb = Resources.Load("Prefebs/ScrolledImage") as GameObject;
        mainCamera = transform.parent.gameObject;

        isGallary = false;

    }

    void Update ()
    {
        if (Input.GetMouseButton(1))    //right button
        {
            Vector3 tmpRot = transform.rotation.eulerAngles;
            tmpRot.z = Mathf.Lerp(tmpRot.z, 270, Time.deltaTime * 5);
            transform.rotation = Quaternion.Euler(tmpRot);
        }
        else
        {
            Vector3 tmp = transform.rotation.eulerAngles;
            tmp.z = Mathf.Lerp(tmp.z, 180, Time.deltaTime * 5);
            transform.rotation = Quaternion.Euler(tmp);
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isGallary = !isGallary;
            ToggleGallary();
        }

        if (isGallary == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                DeletePicture(selectNum);
            }

            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (wheelInput > 0)
            {
                if (selectNum < gallaryImages.Count - 1)
                {
                    RefreshSelectedImage(selectNum + 1); SetScrollImages();
                    //Debug.Log("Up");
                }
            }
            else if (wheelInput < 0)
            {
                if (selectNum > 0)
                {
                    RefreshSelectedImage(selectNum - 1);SetScrollImages();
                    //Debug.Log("Down");
                }
            }
        }
    }

    private void ToggleGallary()
    {
        if (isGallary == true)
        {
            camCanvas.SetActive(false);
            phoneCanvas.SetActive(true);
            ImageLoad(gallaryImages.Count - 1);
        }
        else
        {
            camCanvas.SetActive(true);
            phoneCanvas.SetActive(false);
        }
    }

    public void ImageSave(string name, Texture2D SS)
    {
        //string path = "ScreenShot/" + name;
        //gallaryPath.Add(path);
        gallaryImages.Add(SS);
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
        gallaryScrolls.Clear();

        for (int i = gallaryImages.Count - 1; i >= 0; i--)
        {
            GameObject g = Instantiate(scrollImagePrefeb, new Vector3(0, 0, 0), Quaternion.identity);
            g.transform.SetParent(phoneCanvas.gameObject.transform);
            g.GetComponent<RectTransform>().localPosition = new Vector3(0, -580, 0);
            g.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
            g.GetComponent<RectTransform>().localScale = new Vector3(3.5f, 3.5f, 1);
            Texture2D tmp = gallaryImages[i];
            g.GetComponent<Image>().sprite = Sprite.Create(tmp, new Rect(0, 0, tmp.width, tmp.height), new Vector2(0.5f, 0.5f));
            gallaryScrolls.Add(g);
        }
        SetScrollImages();
    }
    void RefreshSelectedImage(int idx)
    {
        selectNum = idx;
        if (selectNum >= gallaryImages.Count) selectNum = gallaryImages.Count - 1;
        if (selectNum < 0) selectNum = 0;
        Texture2D t = gallaryImages[selectNum];
        gallarySeleted.GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
   
    }
    void SetScrollImages()
    {
        for (int i = gallaryScrolls.Count - 1; i >= 0; i--)
        {
            gallaryScrolls[i].GetComponent<RectTransform>().localPosition = new Vector3((i - (gallaryScrolls.Count - selectNum - 1)) * 400, -580, 0);
        }
    }
    void ConfirmDeletion(int idx)
    {

    }
    void DeletePicture(int idx)
    {
        if (gallaryImages.Count <= 0) { return; }
        Destroy(gallaryImages[idx]);
        gallaryImages.RemoveAt(idx);
        ImageLoad(idx);
    }
}
