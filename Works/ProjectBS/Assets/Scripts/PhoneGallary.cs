using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneGallary : MonoBehaviour {

    private GameObject phoneCam, phoneGallay, camCanvas, phoneCanvas;
    private GameObject gallarySeleted, gallaryScroll;
    //public List<string> gallaryPath = new List<string>();
    public List<Texture2D> gallaryImages = new List<Texture2D>();
    public List<GameObject> gallaryScrolls = new List<GameObject>();
    private GameObject scrollImagePrefeb;

    public bool isGallary;

	void Start () {
        phoneCam = transform.GetChild(0).gameObject;
        phoneGallay = transform.GetChild(1).gameObject;
        camCanvas = transform.GetChild(2).gameObject;
        phoneCanvas = transform.GetChild(3).gameObject;
        gallarySeleted = phoneCanvas.transform.GetChild(0).gameObject;
        gallaryScrolls.Add(phoneCanvas.transform.GetChild(1).gameObject);
        scrollImagePrefeb = Resources.Load("Prefebs/ScrolledImage") as GameObject;

        isGallary = false;
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isGallary = !isGallary;
            ToggleGallary();
        }
	}

    private void ToggleGallary()
    {
        if (isGallary == true)
        {
            camCanvas.SetActive(false);
            phoneCanvas.SetActive(true);
            ImageLoad();
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
    public void ImageLoad()
    {
        if (gallaryImages.Count <= 0) { return; }

        Texture2D t = gallaryImages[gallaryImages.Count - 1];
        gallarySeleted.GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));

        if (gallaryImages.Count != gallaryScrolls.Count)
        {
            gallaryScrolls.Clear();
            for (int i = gallaryImages.Count - 1; i >= 0; i--)
            {
                GameObject g = Instantiate(scrollImagePrefeb, new Vector3(0, 0, 0), Quaternion.identity);
                g.transform.SetParent(phoneCanvas.gameObject.transform);
                g.GetComponent<RectTransform>().localPosition = new Vector3(-400 + (gallaryImages.Count - i - 1) * 400, -580, 0);
                g.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                g.GetComponent<RectTransform>().localScale = new Vector3(3.5f, 3.5f, 1);
                Texture2D tmp = gallaryImages[i];
                g.GetComponent<Image>().sprite = Sprite.Create(tmp, new Rect(0, 0, tmp.width, tmp.height), new Vector2(0.5f, 0.5f));
                
            }
        }

    }
}
