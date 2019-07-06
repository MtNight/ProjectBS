using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    private Vector3 moveVec;
    private float moveSpeed;

	void Start () {
        moveVec = new Vector3(0, 0, 0);
        moveSpeed = 2.0f;
	}
	
	void Update ()
    {
        moveVec = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            moveVec += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVec += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec += Vector3.right;
        }

        transform.position += moveVec * moveSpeed * Time.deltaTime;
    }
}
