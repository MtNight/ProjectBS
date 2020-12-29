using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMove : MonoBehaviour
{
    public Vector3 destination;
    public bool isReachedDestination = false;
    public float moveSpeed = 30.0f;
    public float angleSpeed = 15.0f;
    public Vector3 moveVec;
    Vector3 prevPos;
    Vector3 prevVec;
    bool isIdle = false;
    public bool isInCross = false;
    bool checkedCross = false;

    WayPointNavigator wayPointNavi;
    public GameObject trafficSystem;
    float cool = 0;

    public GameObject player;
    Renderer childRenderer;
    Rigidbody rigidbody;
    Animator animator;


    void Start()
    {
        wayPointNavi = GetComponent<WayPointNavigator>();

        moveSpeed = 30 + Random.Range(-5f, 5f);

        childRenderer = transform.GetChild(0).GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        StartCoroutine(BehaveChange());
    }
    
    void Update()
    {
        CheckInCross();

        if (isIdle)
        {
            transform.forward = prevVec;
        }
        else
        {
            //set forward
            MovePeople();

            //direction reposition or straight move
            if (Vector3.Dot(moveVec, prevVec) * Mathf.Rad2Deg >= 90)
            {
                wayPointNavi.NextDestination();
            }
            else
            {
                if (childRenderer.isVisible || (player != null && Vector3.Distance(transform.position, player.transform.position) < 256)) 
                {
                    rigidbody.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
                    animator.enabled = true;
                }
                else
                {
                    animator.enabled = false;
                }
            }

            //stop reposition
            if (Vector3.Magnitude(prevPos - transform.position) < 0.001f && cool <= 0)
            {
                wayPointNavi.ResetDestination();
                cool = 1;
            }
            prevPos = transform.position;
            if (moveVec != Vector3.zero) prevVec = moveVec;
            cool -= Time.deltaTime;
        }

        if (transform.position.y < 0)
        {
            Vector3 resetPos = transform.position;
            resetPos.y = 3;
            transform.position = resetPos;
        }
    }

    void MovePeople()
    {
        moveVec = Vector3.zero;
        isReachedDestination = IsCloseToDestination();
        if (!isReachedDestination)
        {
            moveVec = destination - transform.position;
        }

        moveVec.y = 0;
        moveVec = moveVec.normalized;

        if (moveVec != Vector3.zero)
        {
            //transform.forward = moveVec;
            transform.forward = Vector3.Lerp(transform.forward, moveVec, Time.deltaTime * angleSpeed);
        }
    }

    bool IsCloseToDestination()
    {
        int cnt = 0;
        float thredhold = 0.5f;
        if (Mathf.Abs(destination.x - transform.position.x) <= thredhold)
        {
            cnt++;
        }
        if (Mathf.Abs(destination.z - transform.position.z) <= thredhold)
        {
            cnt++;
        }
        if (cnt == 2)
        {
            return true;
        }
        return false;
    }

    public void SetDestination(Vector3 dst)
    {
        destination = dst;
        isReachedDestination = false;
    }

    IEnumerator BehaveChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f + Random.Range(0.0f, 10.0f));

            float modeChangePercent = 0.01f;
            float directionChangePercent = 0.1f;
            if (Random.Range(0.0f, 1.0f) < modeChangePercent)
            {
                StartCoroutine(StopOrResumeMoving());
            }

            if (Random.Range(0.0f, 1.0f) < directionChangePercent)
            {
                wayPointNavi.ChangeDirection();
            }
        }
    }
    IEnumerator StopOrResumeMoving()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 10.0f));
            if (!isInCross)
            {
                StopOrResumeMove(!isIdle);
            }
            yield break;
        }
    }
    void StopOrResumeMove(bool stop)
    {
        if (isIdle != stop)
        {
            if (stop)
            {
                animator.SetFloat("Speed_f", 0.2f);
                animator.SetInteger("Animation_int", Random.Range(0, 3));
            }
            else
            {
                animator.SetFloat("Speed_f", 0.3f);
                animator.SetInteger("Animation_int", 0);
            }
        }
        isIdle = stop;
    }
    public void CheckInCross()
    {
        if (isInCross)
        {
            if (checkedCross == false)
            {
                bool checkCross = false;
                if (trafficSystem.GetComponent<TrafficSystem>().cross == true)
                {
                    bool ns = (wayPointNavi.currentWayPoint.isNSDirection == true && trafficSystem.GetComponent<TrafficSystem>().NSLight != 0);
                    bool ew = (wayPointNavi.currentWayPoint.isEWDirection == true && trafficSystem.GetComponent<TrafficSystem>().EWLight != 0);
                    if (ns || ew)
                    {
                        checkCross = true;
                    }
                }
                StopOrResumeMove(!checkCross);
                checkedCross = checkCross;
            }
        }
        else
        {
            checkedCross = false;
            StopOrResumeMove(false);
        }
    }
}
