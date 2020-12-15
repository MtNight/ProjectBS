using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointNavigator : MonoBehaviour
{

    PeopleMove moveScript;
    public WayPoint currentWayPoint;
    public bool wasCross = false;
    int dir;
    bool isBranched = false;

    void Awake()
    {
        moveScript = GetComponent<PeopleMove>();
        dir = Random.Range(0, 2);
    }

    void Start()
    {
        moveScript.SetDestination(currentWayPoint.GetPosition());
    }
    
    void Update()
    {
        if (moveScript.isReachedDestination)
        {
            bool shouldbranch = false;
            int chosenBranch = 0;
            if (!isBranched)
            {
                if (currentWayPoint.branchs != null && currentWayPoint.branchs.Count > 0)
                {
                    while (chosenBranch < currentWayPoint.branchPercent.Length)
                    {
                        shouldbranch = Random.Range(0f, 1f) < currentWayPoint.branchPercent[chosenBranch] ? true : false;
                        if (shouldbranch) break;
                        chosenBranch++;
                    }
                }
            }

            if (shouldbranch)
            {
                currentWayPoint = currentWayPoint.branchs[chosenBranch];
                isBranched = true;
            }
            else
            {
                if (isBranched)
                {
                    if (currentWayPoint.shouldForward)
                    {
                        if (dir != 0)
                        {
                            ChangeDirection();
                        }
                    }
                    else if (currentWayPoint.shouldReverse)
                    {
                        if (dir != 1)
                        {
                            ChangeDirection();
                        }
                    }
                    else
                    {
                        float cdRand = Random.Range(0.0f, 1.0f);
                        if (cdRand < 0.5f)
                        {
                            ChangeDirection();
                        }
                    }
                }
                if (dir == 0)
                {
                    NextDestination();
                }
                else if (dir == 1)
                {
                    PrevDestination();
                }
                isBranched = false;
            }

            CheckCross();
            ResetDestination();
        }
    }

    public void NextDestination()
    {
        if (currentWayPoint.nextWaypoint != null)
        {
            currentWayPoint = currentWayPoint.nextWaypoint;
        }
        else
        {
            PrevDestination();
            ChangeDirection();
        }
    }
    public void PrevDestination()
    {
        if (currentWayPoint.previousWaypoint != null)
        {
            currentWayPoint = currentWayPoint.previousWaypoint;
        }
        else
        {
            NextDestination();
            ChangeDirection();
        }
    }
    public void ResetDestination()
    {
        moveScript.SetDestination(currentWayPoint.GetPosition());
    }
    public void ChangeDirection()
    {
        if (dir == 0) dir = 1;
        else dir = 0;
    }
    void CheckCross()
    {
        bool inCrossTmp = false;
        if (currentWayPoint.isCross)
        {
            if (wasCross == true)
            {
                inCrossTmp = true;
            }
            wasCross = true;
        }
        else
        {
            wasCross = false;
        }
        moveScript.isInCross = inCrossTmp;
    }
}
