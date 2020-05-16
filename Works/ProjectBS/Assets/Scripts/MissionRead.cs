using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Missions {
    int index;
    int route;
    int missionNumber;
    int[][] missionObjects;
    int[][] objectScore;
    int[][] hiddenObjects;
    bool bLisActive;

    public Missions(Dictionary<string, object> par)
    {
        index = (int)par["index"];
        route = (int)par["route"];
        missionNumber = (int)par["mission_number"];
        bLisActive = true;

        //Save Mission Objects
        saveMissionData(par, missionObjects, "mission_object");
        //saveMissionData(par, objectScore, "object_score");
        saveMissionData(par, hiddenObjects, "hidden_object");
    }

    private void saveMissionData(Dictionary<string, object> par, int[][] directory, string type)
    {
        int cntMission = 0;
        for (; cntMission < 5 && par[type + (cntMission + 1)] != null; cntMission++) { }
        directory = new int[cntMission][];
        for (int i = 0; i < cntMission; i++)
        {
            string[] str = (Convert.ToString((int)par[type + (i + 1)])).Split('/');
            directory[i] = new int[str.Length];
            for (int j = 0; j < str.Length; j++)
            {
                directory[i][j] = Convert.ToInt32(str[j]);
            }
        }
    }
}

public class MissionRead : MonoBehaviour {

	public int _id = 0;
    private int indexOfMission = 1; //test
    public Missions mission;

    void Start()
    {
        List<Dictionary<string, object>> objectData = CSVReader.Read("TBL_OBJECT");
        List<Dictionary<string, object>> missionData = CSVReader.Read("TBL_MISSION");

        for (int i = 0; i < objectData.Count; i++)
        {
            //Debug.Log((i).ToString() + " : index=" + objectData[i]["index"] + " prefeb name=" + objectData[i]["prefab_name"] + " english nickname=" + objectData[i]["nickname_eng"]);
        }
        Debug.Log(missionData.Count);

        indexOfMission = UnityEngine.Random.Range(0, missionData.Count - 1);
        mission = new Missions(missionData[indexOfMission]);
    }
}
