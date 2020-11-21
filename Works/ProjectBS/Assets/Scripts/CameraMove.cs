using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 - Player's head movement & camera movement.
 - Phone Mode Change ZoomIn-Out.
 */

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
        //Change Phone Mod
        int changeSpeed = 5;
        if (phone.GetComponent<PhoneGallary>().isLandscapeMode == true)    //right button
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 32, Time.deltaTime * changeSpeed);

            Vector3 tmpPos = mainCamera.transform.localPosition;
            tmpPos.y = Mathf.Lerp(tmpPos.y, -0.3f, Time.deltaTime * changeSpeed);
            mainCamera.transform.localPosition = tmpPos;
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 64, Time.deltaTime * changeSpeed);

            Vector3 tmpPos = mainCamera.transform.localPosition;
            tmpPos.y = Mathf.Lerp(tmpPos.y, 0, Time.deltaTime * changeSpeed);
            mainCamera.transform.localPosition = tmpPos;
        }

        //Camera(player's head) Movement
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
            sightHeight.transform.localRotation *= Quaternion.Euler(-xRot, 0, 0);
        }
    }
}