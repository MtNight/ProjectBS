using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNavigate : MonoBehaviour
{
    public GameObject player;
    public GameObject MissionManager;

    public Vector3 targetPos;
    public Vector3 naviDir;
    public int currentIdx = 0;
    void Start()
    {
        //player = transform.parent.gameObject;
        targetPos = Vector3.zero;
        naviDir = player.transform.forward;

        UpdateTarget();
    }
    
    void Update()
    {
        if (targetPos != Vector3.zero)
        {
            naviDir = targetPos - player.transform.position;
            naviDir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, naviDir, Time.deltaTime / 2);
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
