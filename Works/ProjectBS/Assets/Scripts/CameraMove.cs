using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    private float xSensitivity;
    private float ySensitivity;

    private GameObject sightHeight;
    private GameObject mainCamera;
    private GameObject phone;
    private Camera cam;

    void Start()
    {
        Cursor.visible = false;

        xSensitivity = 90.0f;
        ySensitivity = 90.0f;

        sightHeight = transform.GetChild(0).gameObject;
        mainCamera = sightHeight.transform.GetChild(0).gameObject;
        phone = sightHeight.transform.GetChild(1).gameObject;
        cam = mainCamera.GetComponent<Camera>();
    }
    
    void Update()
    {
        if (phone.GetComponent<PhoneGallary>().isGallary == false)
        {
            if (Input.GetMouseButton(1))    //right button
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 32, Time.deltaTime * 5);

                Vector3 tmpPos = mainCamera.transform.localPosition;
                tmpPos.y = Mathf.Lerp(tmpPos.y, -0.3f, Time.deltaTime * 5);
                mainCamera.transform.localPosition = tmpPos;
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 64, Time.deltaTime * 5);

                Vector3 tmpPos = mainCamera.transform.localPosition;
                tmpPos.y = Mathf.Lerp(tmpPos.y, 0, Time.deltaTime * 5);
                mainCamera.transform.localPosition = tmpPos;
            }
        }


        float yRot = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        float xRot = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
        if (Mathf.Abs(yRot) > 30)
        {
            yRot *= 30 / Mathf.Abs(yRot);
        }
        if (Mathf.Abs(xRot) > 30)
        {
            xRot *= 30 / Mathf.Abs(xRot);
        }

        this.transform.localRotation *= Quaternion.Euler(0, yRot, 0);
        
        Quaternion tmpAngle = sightHeight.transform.rotation;
        tmpAngle *= Quaternion.Euler(-xRot, 0, 0);
        bool isOverDownLimit = ((int)tmpAngle.eulerAngles.x / 90 == 0 && tmpAngle.eulerAngles.x > 70);
        bool isOverUpLimit = ((int) tmpAngle.eulerAngles.x / 90 == 3 && tmpAngle.eulerAngles.x < 290);
        if (!isOverUpLimit && !isOverDownLimit)
        {
            sightHeight.transform.localRotation *= Quaternion.Euler(-xRot, 0, 0);//부호 주의
        }
    }
}