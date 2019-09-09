using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneGallary : MonoBehaviour {

    private GameObject phoneCam, phoneCanvas;

    public bool isGallary;

	void Start () {
        phoneCam = transform.GetChild(0).gameObject;
        phoneCanvas = transform.GetChild(1).gameObject;

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
            phoneCam.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            phoneCam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("PhoneImage");
        }
        else
        {
            phoneCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            phoneCam.GetComponent<Camera>().cullingMask = LayerMask.GetMask("Structure");
        }
    }
}
