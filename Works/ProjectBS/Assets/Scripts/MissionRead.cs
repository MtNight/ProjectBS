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
    int missionNumber;
    int[][] objects;
    int[][] objectScore;
    int[][] hiddenObjects;
    bool isActive;

    public Missions(Dictionary<string, string> tuple)
    {
        index = Convert.ToInt32(tuple["index"]);
        route = Convert.ToInt32(tuple["route"]);
        missionNumber = Convert.ToInt32(tuple["mission_number"]);
        isActive = true;

        //Save Mission Objects
        objects = SaveMissionData(tuple, "mission_object");
        //saveMissionData(par, objectScore, "object_score");
        //SaveMissionData(tuple, hiddenObjects, "hidden_object");
        //Debug.Log(objects[1][0] + ", "+objects.Length);
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

    public int GetObject1DCount()
    {
        return objects.Length;
    }
    public int GetObject2DCount(int idx)
    {
        return objects[idx].Length;
    }
    public int GetObjectIndex(int idx1, int idx2)
    {
        return objects[idx1][idx2];
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

    public MissionObject(List<Dictionary<string, string>> objectData, int idx)
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
        GameObject prefab = GameObject.Find(prefabName);
        while (prefab != null)
        {
            tmpNames[objectIdx] = prefab.name;
            tmpObjects[objectIdx] = prefab;
            prefab.name = "searching";
            prefab.tag = "Capturable";
            objectIdx++;

            prefab = GameObject.Find(prefabName);
        }
        for (int i = 0; i < objectIdx; i++)
        {
            tmpObjects[i].name = tmpNames[i];
        }
        if (objectIdx == 0)
        {
            Debug.Log("[" + prefabName + "]오브젝트를 찾을 수 없습니다.");
        }
    }

    public string GetName()
    {
        return name;
    }
    public string GetPrefabName()
    {
        return prefabName;
    }
}

public class MissionRead : MonoBehaviour {

	public int _id = 0;
    private int indexOfMission = 1; //test
    public Missions mission;
    public MissionObject[][] missionObjects;

    void Start()
    {
        List<Dictionary<string, string>> missionData = CSVReader.Read("TBL_MISSION");
        List<Dictionary<string, string>> objectData = CSVReader.Read("TBL_OBJECT");
        
        //Select Mission
        //indexOfMission = UnityEngine.Random.Range(0, missionData.Count - 1);
        indexOfMission = 1;
        mission = new Missions(missionData[indexOfMission]);

        //Find MissionObject & Initiallize
        int cnt1 = mission.GetObject1DCount();
        missionObjects = new MissionObject[cnt1][];
        for (int i = 0; i < cnt1; i++)
        {
            int cnt2 = mission.GetObject2DCount(i);
            missionObjects[i] = new MissionObject[cnt2];
            for (int j = 0; j < cnt2; j++)
            {
                missionObjects[i][j] = new MissionObject(objectData, mission.GetObjectIndex(i, j));
            }
        }

        for (int i = 0; i < missionObjects.Length; i++)
        {
            //Debug.Log(missionObjects[i].GetName());
        }
    }
}
