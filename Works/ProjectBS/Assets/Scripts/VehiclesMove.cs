using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclesMove : MonoBehaviour
{
    public bool isMove = false;
    public float vehicleSpeed = 30.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            transform.position -= transform.forward.normalized * vehicleSpeed * Time.deltaTime;
        }
    }
}
