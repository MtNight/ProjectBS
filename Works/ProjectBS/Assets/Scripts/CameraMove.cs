using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    private float xSensitivity;
    private float ySensitivity;

    private Camera cam;
    //private GameObject phone;
    
    void Start()
    {
        xSensitivity = 90.0f;
        ySensitivity = 90.0f;

        cam = transform.GetChild(0).GetComponent<Camera>();
    }
    
    void Update()
    {
        float yRot = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        float xRot = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
        
        this.transform.localRotation *= Quaternion.Euler(0, yRot, 0);

        Quaternion tmpAngle = cam.transform.rotation;
        tmpAngle *= Quaternion.Euler(-xRot, 0, 0);
        bool bDownLimit = ((int)tmpAngle.eulerAngles.x / 90 == 0 && tmpAngle.eulerAngles.x > 70);
        bool bUpLimit = ((int) tmpAngle.eulerAngles.x / 90 == 3 && tmpAngle.eulerAngles.x < 290);
        if (!bUpLimit && !bDownLimit)
        {
            cam.transform.localRotation *= Quaternion.Euler(-xRot, 0, 0);//부호 주의
        }
    }
}