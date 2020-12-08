using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNavigate : MonoBehaviour
{
    public GameObject player;
    public GameObject MissionManager;

    GameObject naviSign, nearSign;
    bool naviActive = true;

    public Vector3 targetPos;
    public Vector3 naviDir;
    float naviSpeed;
    public int currentIdx = 0;
    void Start()
    {
        //player = transform.parent.gameObject;
        targetPos = Vector3.zero;
        naviDir = player.transform.forward;

        naviSign = transform.GetChild(0).gameObject;
        nearSign = transform.GetChild(1).gameObject;
        naviSpeed = Time.deltaTime * 4;

        UpdateTarget();
    }
    
    void Update()
    {
        if (targetPos != Vector3.zero)
        {

            float dist = Vector3.Distance(player.transform.position, targetPos);
            if (dist < 30)
            {
                naviActive = false;
                naviDir = player.transform.forward;
                naviDir.y = 0;
                naviDir.Normalize();
                transform.forward = Vector3.Lerp(transform.forward, naviDir, naviSpeed);

                //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, naviSpeed / 2);
                //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, Vector3.zero, naviSpeed);
            }
            else
            {
                naviActive = true;

                naviDir = targetPos - player.transform.position;
                naviDir.y = 0;
                naviDir.Normalize();
                transform.forward = Vector3.Lerp(transform.forward, naviDir, naviSpeed);
            }
            naviSign.SetActive(naviActive);
            nearSign.SetActive(!naviActive);
        }
    }

    void SetTarget(Vector3 target)
    {
        targetPos = target;
    }
    public void UpdateTarget()
    {
        Missions missions = MissionManager.GetComponent<MissionRead>().mission;
        if (player.GetComponent<PlayerMove>().isCompleteMision)
        {
            SetTarget(missions.GetEndPosition());
        }
        else
        {
            for (int i = 0; i < missions.GetMissionCount(); i++)
            {
                if (!MissionManager.GetComponent<MissionRead>().mission.GetClearCheck(i))
                {
                    currentIdx = i;
                    break;
                }
            }

            SetTarget(missions.GetMissionPosition(currentIdx));
        }
    }
}
