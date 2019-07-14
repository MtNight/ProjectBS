using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private Vector3 moveVec;
    private float moveSpeed;

    public Vector3 playerForward;
    public float theta;

    void Start()
    {
        moveVec = new Vector3(0, 0, 0);
        moveSpeed = 3.0f;

        playerForward = Vector3.forward;
    }

    void Update()
    {
        theta = -transform.localEulerAngles.y * Mathf.Deg2Rad;
        playerForward = new Vector3(-Mathf.Sin(theta), 0, Mathf.Cos(theta));
        moveVec = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            moveVec += playerForward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVec += new Vector3(-playerForward.z, 0, playerForward.x);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec -= playerForward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec -= new Vector3(-playerForward.z, 0, playerForward.x);
        }

        transform.position += moveVec * moveSpeed * Time.deltaTime;
    }
}
