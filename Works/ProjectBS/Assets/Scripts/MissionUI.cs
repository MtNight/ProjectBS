using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 - Mission UI Display and Change Status
 - Check Mission Clear
 */

public class MissionUI : MonoBehaviour {

    public GameObject Player;
    public GameObject MissionManager;
    public Missions mission;
    public MissionObject[][] mObjects;
    public GameObject Navi;
    public bool isStartGame = false;

    //mission check
    GameObject[] uiTexts = new GameObject[10];
    string[] missionObjectName = new string[10];
    
    //count for check clear mission
    int missionCnt = 0;
    
    //UI On/Off
    float UIspeed = 4;
    bool isUIOpen = true;
    float UIX;
    float UIXposition;

    public KeyCode UIOnOffKey;

	void Awake ()
    {
        MissionManager = transform.parent.parent.gameObject;
        UIXposition = GetComponent<RectTransform>().localPosition.x;
        SetMission();
    }
    private void Start()
    {
        UIXposition = GetComponent<RectTransform>().localPosition.x;
        Vector3 tmp = GetComponent<RectTransform>().localPosition;
        tmp.x = UIXposition - GetComponent<RectTransform>().rect.width - 10;
        GetComponent<RectTransform>().localPosition = tmp;
    }

    private void Update()
    {
        if (isStartGame)
        {
            //UI On/Off key input
            if (Input.GetKeyDown(UIOnOffKey))
            {
                isUIOpen = !isUIOpen;
                Debug.Log("Mission UI " + isUIOpen);
            }

            //UI On/Off movement
            UIX = GetComponent<RectTransform>().localPosition.x;
            if (isUIOpen)
            {
                UIX = Mathf.Lerp(UIX, UIXposition, Time.deltaTime * UIspeed);
            }
            else
            {
                UIX = Mathf.Lerp(UIX, UIXposition - GetComponent<RectTransform>().rect.width - 10, Time.deltaTime * UIspeed);
            }
            Vector3 tmp = GetComponent<RectTransform>().localPosition;
            tmp.x = UIX;
            GetComponent<RectTransform>().localPosition = tmp;
        }
    }

    //initialize mission's information
    public void SetMission()
    {
        mission = MissionManager.GetComponent<MissionRead>().mission;
        mObjects = MissionManager.GetComponent<MissionRead>().missionObjects;
        for (int i = 0; i < mObjects.Length; i++)
        {
            missionObjectName[i] = mObjects[i][0].GetName();
            uiTexts[i] = transform.GetChild(i + 1).gameObject;
            uiTexts[i].GetComponent<Text>().text = "Take a picture with a " + missionObjectName[i];
        }
        uiTexts[mObjects.Length] = transform.GetChild(mObjects.Length + 1).gameObject;
    }

    //clear mission by 'ScreenShot' script OR cancel?
    public void ClearObjective(int objNum, bool stat)
    {
        if (mission.GetClearCheck(objNum) != stat)
        {
            mission.SetClearOrNot(objNum, stat);
            Navi.GetComponent<MissionNavigate>().UpdateTarget();
        }
        else { return; }

        //Text
        if (stat == true)
        {
            uiTexts[objNum].GetComponent<Text>().color = Color.red;
            missionCnt++;
        }
        else
        {
            uiTexts[objNum].GetComponent<Text>().color = Color.black;
            missionCnt--;
        }

        //Checkbox
        uiTexts[objNum].transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = stat;

        //Check mission clear
        if (missionCnt == mObjects.Length)
        {
            ClearMisson();
        }
    }

    //Mission Clear, Send sign to 'PlayerMove' script for leading endPoint
    void ClearMisson()
    {
        uiTexts[mObjects.Length].SetActive(true);
        Player.GetComponent<PlayerMove>().CompleteMission(mission.GetEndPosition());
        Navi.GetComponent<MissionNavigate>().UpdateTarget();
    }

    //Arrive At endPoint by 'PlayerMove' script
    public void ArriveAtEndPoint()
    {
        uiTexts[mObjects.Length].GetComponent<Text>().color = Color.red;
        uiTexts[mObjects.Length].transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = true;
    }

    public void InitStartState()
    {
        isStartGame = true;
    }
}
