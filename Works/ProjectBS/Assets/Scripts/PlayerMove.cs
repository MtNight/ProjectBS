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
    MissionUI missionUI;
    Vector3 endPoint;
    public bool isCompleteMision=false;

    bool isIdle = true;
    public bool isStartGame = false;
    Rigidbody rigidbody;
    Animator animator;

    public KeyCode FrontKey;
    public KeyCode BackKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode AccelKey;

    void Start()
    {
        moveVec = new Vector3(0, 0, 0);
        moveSpeed = 10.0f;

        playerForward = Vector3.forward;

        rigidbody = GetComponent<Rigidbody>();
        animator = transform.GetChild(1).GetComponent<Animator>();

        missionUI = MissionPanel.GetComponent<MissionUI>();
        StopOrResumeMove(true);
    }

    private void FixedUpdate()
    {
        if (isStartGame)
        {
            theta = -transform.localEulerAngles.y * Mathf.Deg2Rad;
            playerForward = new Vector3(-Mathf.Sin(theta), 0, Mathf.Cos(theta));
            moveVec = new Vector3(0, 0, 0);

            bool currentState = true;

            if (Input.GetKey(FrontKey))
            {
                moveVec += playerForward;
                currentState = false;
            }
            if (Input.GetKey(LeftKey))
            {
                moveVec += new Vector3(-playerForward.z, 0, playerForward.x);
                currentState = false;
            }
            if (Input.GetKey(BackKey))
            {
                moveVec -= playerForward;
                currentState = false;
            }
            if (Input.GetKey(RightKey))
            {
                moveVec -= new Vector3(-playerForward.z, 0, playerForward.x);
                currentState = false;
            }
            moveVec = moveVec.normalized;


            if (Input.GetKey(AccelKey))
            {
                moveVec = moveVec * 2;
            }

            if (isIdle != currentState)
            {
                StopOrResumeMove(currentState);
            }

            //transform.position += moveVec * moveSpeed * Time.deltaTime;
            rigidbody.AddForce(moveVec * moveSpeed, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (isCompleteMision)
        {
            //Debug.Log(Vector3.Distance(transform.position, endPoint));
            if (Vector3.Distance(transform.position, endPoint) < 2)
            {
                missionUI.ArriveAtEndPoint();
                isCompleteMision = false;
            }
        }
        if (transform.position.y < 0)
        {
            Vector3 resetPos = transform.position;
            resetPos.y = 3;
            transform.position = resetPos;
        }
    }

    public void CompleteMission(Vector3 ep)
    {
        endPoint = ep;
        isCompleteMision = true;
    }

    void StopOrResumeMove(bool stop)
    {
        if (stop)
        {
            animator.SetFloat("Speed_f", 0.2f);
            animator.SetInteger("Animation_int", 0);
        }
        else
        {
            animator.SetFloat("Speed_f", 0.3f);
            animator.SetInteger("Animation_int", 0);
        }
        isIdle = stop;
    }

    public void InitStartState()
    {
        isStartGame = true;
        transform.position = missionUI.mission.GetStartPosition();
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
