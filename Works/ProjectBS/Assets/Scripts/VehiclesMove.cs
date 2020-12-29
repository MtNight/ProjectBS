using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclesMove : MonoBehaviour
{
    public int direction;
    float  currentSpeed, currentAccel;
    public float vehicleAccel = 120.0f, vehicleSpeed;
    bool isCloseToFront = false;
    float stopLimit = 0;

    void Start()
    {
        vehicleSpeed = Random.Range(25.0f, 35.0f);
        currentSpeed = vehicleSpeed - 10.0f;
        currentAccel = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCloseToFront)
        {
            currentAccel = -vehicleAccel;
        }
        else if (currentSpeed > vehicleSpeed)
        {
            currentAccel = -vehicleAccel / 2;
        }
        else if (currentSpeed < vehicleSpeed)
        {
            currentAccel = vehicleAccel / 2;
        }
        else
        {
            currentAccel = 0;
        }

        currentSpeed += currentAccel * Time.deltaTime;
        if (currentSpeed < 0) { currentSpeed = 0; }
        transform.position -= transform.forward.normalized * currentSpeed * Time.deltaTime;

        if (transform.position.x < -510 || transform.position.x > 910 || transform.position.z < -310 || transform.position.z > 1210)
        {
            Destroy(this.gameObject);
        }

        if (isCloseToFront)
        {
            stopLimit += Time.deltaTime;
        }
        else
        {
            stopLimit = 0;
        }
        if (stopLimit >= 90.0f)
        {
            isCloseToFront = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.transform.forward == transform.forward)
        {
            float otherSpeed = 0;
            if (other.tag != "Crosswalk")
            {
                otherSpeed = other.GetComponent<VehiclesMove>().vehicleSpeed;
            }
            bool isCloseDir0 = (direction == 0 && transform.position.z >= other.transform.position.z);
            bool isCloseDir1 = (direction == 1 && transform.position.x >= other.transform.position.x);
            bool isCloseDir2 = (direction == 2 && transform.position.z <= other.transform.position.z);
            bool isCloseDir3 = (direction == 3 && transform.position.x <= other.transform.position.x);
            if (isCloseDir0 || isCloseDir1 || isCloseDir2 || isCloseDir3 || other.tag == "Crosswalk") 
            {
                isCloseToFront = true;
                if (otherSpeed == 0)
                {
                    vehicleSpeed = 0;
                }
            }

            vehicleSpeed = (vehicleSpeed + otherSpeed) / 2.0f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        bool isOverDir0 = false;
        bool isOverDir1 = false;
        bool isOverDir2 = false;
        bool isOverDir3 = false;
        float th = 8.0f;
        if (other != null && other.tag == "Crosswalk" && other.transform.forward == transform.forward)
        {
            isOverDir0 = (direction == 0 && transform.position.z <= other.transform.position.z + th);
            isOverDir1 = (direction == 1 && transform.position.x <= other.transform.position.x + th);
            isOverDir2 = (direction == 2 && transform.position.z >= other.transform.position.z - th);
            isOverDir3 = (direction == 3 && transform.position.x >= other.transform.position.x - th);
        }

        if (other == null || isOverDir0 || isOverDir1 || isOverDir2 || isOverDir3)
        {
            isCloseToFront = false;
            vehicleSpeed = Random.Range(25.0f, 35.0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        bool isCloseDir0 = false;
        bool isCloseDir1 = false;
        bool isCloseDir2 = false;
        bool isCloseDir3 = false;
        if (other != null && other.transform.forward == transform.forward)
        {
            isCloseDir0 = (direction == 0 && transform.position.z >= other.transform.position.z);
            isCloseDir1 = (direction == 1 && transform.position.x >= other.transform.position.x);
            isCloseDir2 = (direction == 2 && transform.position.z <= other.transform.position.z);
            isCloseDir3 = (direction == 3 && transform.position.x <= other.transform.position.x);
        }

        if (other == null || isCloseDir0 || isCloseDir1 || isCloseDir2 || isCloseDir3 || other.tag == "Crosswalk")
        {
            isCloseToFront = false;
            vehicleSpeed = Random.Range(25.0f, 35.0f);
        }
    }
}
