using UnityEngine;
using System.Collections.Generic;
using System;

/*
 - Select Mission &
 */

//Mission Class
public class Missions
{
    int index;
    int route;
    Vector3 startPosition, endPosition;
    int missionNumber;
    int[][] objectsTableIdx;
    int[][] objectScore;
    int[][] hiddenObjects;
    bool isActive;
    bool[] isCleared;

    Vector3[] mainMissionPosition;

    public Missions(Dictionary<string, string> tuple)
    {
        index = Convert.ToInt32(tuple["index"]);
        route = Convert.ToInt32(tuple["route"]);
        RouteSetting();
        missionNumber = Convert.ToInt32(tuple["mission_number"]);
        isActive = true;

        //Save Mission Objects
        objectsTableIdx = SaveMissionData(tuple, "mission_object");
        isCleared = new bool[objectsTableIdx.Length];
        mainMissionPosition = new Vector3[objectsTableIdx.Length];
        //saveMissionData(par, objectScore, "object_score");
        //SaveMissionData(tuple, hiddenObjects, "hidden_object");
        //Debug.Log(objects[1][0] + ", "+objects.Length);
    }

    private void RouteSetting()
    {
        switch (route)
        {
            case 1:
                {
                    startPosition = new Vector3(40, 3, 240);
                    endPosition = new Vector3(130, 3, 485);
                    break;
                } 
            case 2:
                {
                    startPosition = new Vector3(75, 3, 630);
                    break;
                }
            case 3:
                {
                    startPosition = new Vector3(285, 3, 620);
                    break;
                }
            case 4:
                {
                    startPosition = new Vector3(270, 3, 200);
                    break;
                }
        }
    }

    private int[][] SaveMissionData(Dictionary<string, string> tuple, string type)
    {
        int[][] directory;
        int cntMission = 0;
        for (; cntMission < 5 && tuple[type + (cntMission + 1)] != ""; cntMission++) { }
        directory = new int[cntMission][];
        for (int i = 0; i < cntMission; i++)
        {
            string[] str = tuple[type + (i + 1)].Split('/');
            directory[i] = new int[str.Length];
            for (int j = 0; j < str.Length; j++)
            {
                directory[i][j] = Convert.ToInt32(str[j]);
            }
        }

        return directory;
    }

    public int GetMissionCount()
    {
        return objectsTableIdx.Length;
    }
    public int GetDuplicateObjectsCount(int idx)
    {
        return objectsTableIdx[idx].Length;
    }
    public int GetObjectIndex(int idx1, int idx2)
    {
        return objectsTableIdx[idx1][idx2];
    }
    public bool GetClearCheck(int idx1)
    {
        return isCleared[idx1];
    }
    public void SetClearOrNot(int idx1, bool stat)
    {
        isCleared[idx1] = stat;
    }
    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
    public Vector3 GetEndPosition()
    {
        return endPosition;
    }
    public void SetMissionPosition(MissionObject[][] mObject)
    {
        for (int i = 0; i < GetMissionCount(); i++)
        {
            float min = 1000;
            Vector3 minVec = Vector3.zero;
            for (int j = 0; j < GetDuplicateObjectsCount(i); j++)
            {
                Vector3 tmpVec = mObject[i][j].GetSD();
                float tmp = Vector3.Distance(startPosition, tmpVec);
                if (min > tmp)
                {
                    min = tmp;
                    minVec = tmpVec;
                }
            }
            mainMissionPosition[i] = minVec;
        }
    }
    public Vector3 GetMissionPosition(int idx)
    {
        return mainMissionPosition[idx];
    }
}

//Mission Object Class
public class MissionObject
{
    int index;
    int objectType;
    int objectSpecies;
    int objectNumber;
    bool isActive;
    string prefabName;
    string name;

    Vector3 shortestDistance;

    public MissionObject(List<Dictionary<string, string>> objectData, int idx, Vector3 playerPosition)
    {
        Dictionary<string, string> tuple = null;
        for (int i = 0; i < objectData.Count; i++)
        {
            if (Convert.ToInt32(objectData[i]["index"]) == idx)
            {
                tuple = objectData[i];
                break;
            }
        }
        if (tuple == null) { Debug.Log("오브젝트를 찾을 수 없습니다."); return; }

        index = Convert.ToInt32(tuple["index"]);
        objectType = Convert.ToInt32(tuple["object_type"]);
        objectSpecies = Convert.ToInt32(tuple["object_species"]);
        objectNumber = Convert.ToInt32(tuple["object_number"]);
        isActive = true;
        prefabName = Convert.ToString(tuple["prefab_name"]);
        name = Convert.ToString(tuple["nickname_eng"]);

        Debug.Log(prefabName);

        int objectIdx = 0;
        string[] tmpNames = new string[100];
        GameObject[] tmpObjects = new GameObject[100];
        float tmpDistance, minDistance = 1000;
        int minIdx = 0;
        GameObject prefab = GameObject.Find(prefabName);
        while (prefab != null)
        {
            tmpNames[objectIdx] = prefab.name;
            tmpObjects[objectIdx] = prefab;
            rChangeTagWithChilds(prefab, "Capturable", prefab.name);
            prefab.name = "searching";

            tmpDistance = Vector3.Distance(prefab.transform.position, playerPosition);
            if (minDistance > tmpDistance)
            {
                minDistance = tmpDistance;
                minIdx = objectIdx;
            }
            objectIdx++;

            prefab = GameObject.Find(prefabName);
        }
        for (int i = 0; i < objectIdx; i++)
        {
            tmpObjects[i].name = tmpNames[i];
        }

        shortestDistance = tmpObjects[minIdx].transform.position;

        if (objectIdx == 0)
        {
            Debug.Log("[" + prefabName + "]오브젝트를 찾을 수 없습니다.");
        }
    }

    void rChangeTagWithChilds(GameObject root, string tagInput, string name)
    {
        for (int i = 0; i < root.transform.childCount; i++)
        {
            rChangeTagWithChilds(root.transform.GetChild(i).gameObject, tagInput, name);
        }
        root.tag = tagInput;
        root.name = name + "_1";
    }

    public string GetName()
    {
        return name;
    }
    public string GetPrefabName()
    {
        return prefabName;
    }
    public Vector3 GetSD()
    {
        return shortestDistance;
    }
}

public class MissionRead : MonoBehaviour {

	public int _id = 0;
    private int indexOfMission = 1; //test
    public Missions mission;
    public MissionObject[][] missionObjects;

    void Awake()
    {
        List<Dictionary<string, string>> missionData = CSVReader.Read("TBL_MISSION");
        List<Dictionary<string, string>> objectData = CSVReader.Read("TBL_OBJECT");
        
        //Select Mission
        //indexOfMission = UnityEngine.Random.Range(0, missionData.Count - 1);
        indexOfMission = 1;
        mission = new Missions(missionData[indexOfMission]);

        //Find MissionObject & Initiallize
        int cnt1 = mission.GetMissionCount();
        missionObjects = new MissionObject[cnt1][];
        for (int i = 0; i < cnt1; i++)
        {
            int cnt2 = mission.GetDuplicateObjectsCount(i);
            missionObjects[i] = new MissionObject[cnt2];
            for (int j = 0; j < cnt2; j++)
            {
                missionObjects[i][j] = new MissionObject(objectData, mission.GetObjectIndex(i, j), mission.GetStartPosition());
            }
        }

        mission.SetMissionPosition(missionObjects);
        for (int i = 0; i < missionObjects.Length; i++)
        {
            //Debug.Log(missionObjects[i].GetName());
        }
    }
}
