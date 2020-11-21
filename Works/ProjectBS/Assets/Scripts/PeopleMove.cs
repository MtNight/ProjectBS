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

    WayPointNavigator wayPointNavi;
    float cool = 0;

    void Start()
    {
        wayPointNavi = GetComponent<WayPointNavigator>();

        moveSpeed = 30 + Random.Range(-2.5f, 2.5f);

        StartCoroutine(BehaveChange());
    }
    
    void Update()
    {
        if (isIdle)
        {
            transform.forward = prevVec;
        }
        else
        {
            MovePeople();

            if (Vector3.Dot(moveVec, prevVec) * Mathf.Rad2Deg >= 90)
            {
                wayPointNavi.NextDestination();
            }
            else
            {
                GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
            }


            if (Vector3.Magnitude(prevPos - transform.position) < 0.001f && cool <= 0)
            {
                wayPointNavi.ResetDestination();
                cool = 1;
            }
            prevPos = transform.position;
            if (moveVec != Vector3.zero) prevVec = moveVec;
            cool -= Time.deltaTime;
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
                StartCoroutine(StopMove());
            }

            if (Random.Range(0.0f, 1.0f) < directionChangePercent)
            {
                wayPointNavi.ChangeDirection();
            }
        }
    }
    IEnumerator StopMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.0f, 10.0f));

            if (!isInCross)
            {
                isIdle = !isIdle;
                if (isIdle)
                {
                    GetComponent<Animator>().SetFloat("Speed_f", 0.2f);
                    GetComponent<Animator>().SetInteger("Animation_int", Random.Range(0, 3));
                }
                else
                {
                    GetComponent<Animator>().SetFloat("Speed_f", 0.3f);
                    GetComponent<Animator>().SetInteger("Animation_int", 0);
                }

            }
            yield break;
        }
    }
}
