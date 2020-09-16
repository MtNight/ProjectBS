using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 - Player's movement (WASD)
 - Check EndPoint
 */

public class PlayerMove : MonoBehaviour {

    private Vector3 moveVec;
    private float moveSpeed;

    public Vector3 playerForward;
    public float theta;

    public GameObject MissionPanel;
    Vector3 endPoint;
    bool isCompleteMision=false;

    void Start()
    {
        moveVec = new Vector3(0, 0, 0);
        moveSpeed = 10.0f;

        playerForward = Vector3.forward;
    }

    private void FixedUpdate()
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
        moveVec = moveVec.normalized;


        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVec = moveVec * 2;
        }
        //transform.position += moveVec * moveSpeed * Time.deltaTime;
        this.GetComponent<Rigidbody>().AddForce(moveVec * moveSpeed, ForceMode.Impulse);
    }

    void Update()
    {
        if (isCompleteMision)
        {
            //Debug.Log(Vector3.Distance(transform.position, endPoint));
            if (Vector3.Distance(transform.position, endPoint) < 2)
            {
                MissionPanel.GetComponent<MissionUI>().ArriveAtEndPoint();
                isCompleteMision = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Building")
        //{
        //    Debug.Log("Col");
        //}
    }

    public void CompleteMission(Vector3 ep)
    {
        endPoint = ep;
        isCompleteMision = true;
    }
}
