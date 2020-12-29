using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 - Player's head movement & camera movement.
 - Phone Mode Change ZoomIn-Out.
 */

public class CameraMove : MonoBehaviour {

    public float sensitivity;

    private GameObject sightHeight;
    private GameObject mainCamera;
    private PhoneGallary phoneGallary;
    private Camera cam;

    public bool isStartGame = false;

    void Start()
    {
        Cursor.visible = false;

        sensitivity = 100.0f;

        sightHeight = transform.GetChild(0).gameObject;
        mainCamera = sightHeight.transform.GetChild(0).gameObject;
        phoneGallary = sightHeight.transform.GetChild(1).gameObject.GetComponent<PhoneGallary>();
        cam = mainCamera.GetComponent<Camera>();
    }
    
    void Update()
    {
        if (isStartGame)
        {
            //Change Phone Mod
            int changeSpeed = 5;
            if (phoneGallary.isLandscapeMode == true)    //right button
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
            float yRot = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float xRot = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
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
            bool isOverDownLimit = ((int)tmpAngle.eulerAngles.x / 90 == 0 && tmpAngle.eulerAngles.x > 50);
            bool isOverUpLimit = ((int)tmpAngle.eulerAngles.x / 90 == 3 && tmpAngle.eulerAngles.x < 360 - 50);
            if (!isOverUpLimit && !isOverDownLimit)
            {
                sightHeight.transform.localRotation *= Quaternion.Euler(-xRot, 0, 0);
            }
        }
    }
}