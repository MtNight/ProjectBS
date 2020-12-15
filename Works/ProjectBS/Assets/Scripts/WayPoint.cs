using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoint previousWaypoint;
    public WayPoint nextWaypoint;

    [Range(0f, 30f)]
    public float width = 1;
    public bool isCross = false;
    public bool isNSDirection = false;
    public bool isEWDirection = false;
    public bool shouldForward = false;
    public bool shouldReverse = false;

    public List<WayPoint> branchs;

    [Range(0f, 1f)]
    public float[] branchPercent = { 0.5f };

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2.0f;
        Vector3 maxBound = transform.position - transform.right * width / 2.0f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
