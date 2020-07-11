using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour {

    public GameObject Player;
    public GameObject MissionManager;
    public MissionObject[][] mObjects;

    GameObject[] uiTexts = new GameObject[10];
    string[] missionObjectName = new string[10];
    bool[] isClearMission = new bool[10];
    Vector3 EndPoint;
    
    int missionCnt = 0;

    float UIspeed = 400;
    bool isUIOpen = true;
    float UIX;
    float UIXposition;

	void Start ()
    {
        MissionManager = transform.parent.parent.gameObject;
        UIXposition = GetComponent<RectTransform>().localPosition.x;
        SetMission();

        EndPoint = new Vector3(7, 3, 755);
    }
    private void FixedUpdate()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isUIOpen = !isUIOpen;
            Debug.Log("test" + isUIOpen);
        }
        if (isUIOpen)
        {
            UIX = GetComponent<RectTransform>().localPosition.x;
            if (UIX < UIXposition)
            {
                UIX += UIspeed * Time.deltaTime;
            }
            if (UIX >= UIXposition)
            {
                UIX = UIXposition;
            }
        }
        else
        {
            UIX = GetComponent<RectTransform>().localPosition.x;
            if (UIX > UIXposition - GetComponent<RectTransform>().rect.width - 10)
            {
                UIX -= UIspeed*Time.deltaTime;
            }
            if (UIX <= UIXposition - GetComponent<RectTransform>().rect.width - 10)
            {
                UIX = UIXposition - GetComponent<RectTransform>().rect.width - 10;
            }
        }
        Vector3 tmp = GetComponent<RectTransform>().localPosition;
        tmp.x = UIX;
        GetComponent<RectTransform>().localPosition = tmp;
    }
    public void SetMission()
    {
        mObjects = MissionManager.GetComponent<MissionRead>().missionObjects;
        for (int i = 0; i < mObjects.Length; i++)
        {
            missionObjectName[i] = mObjects[i][0].GetName();
            uiTexts[i] = transform.GetChild(i + 1).gameObject;
            uiTexts[i].GetComponent<Text>().text = "Take a picture with a " + missionObjectName[i];
            isClearMission[i] = false;
        }
        uiTexts[mObjects.Length] = transform.GetChild(mObjects.Length + 1).gameObject;
    }

    public void ClearObjective(int objNum, bool stat)
    {
        if (isClearMission[objNum] != stat)
        {
            isClearMission[objNum] = stat;
        }
        else { return; }
        if (stat)
        {
            uiTexts[objNum].GetComponent<Text>().color = Color.red;
            missionCnt++;
        }
        else
        {
            uiTexts[objNum].GetComponent<Text>().color = Color.red;
            missionCnt--;
        }
        uiTexts[objNum].transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = stat;

        if (missionCnt == mObjects.Length)
        {
            ClearMisson();
        }
    }

    public void ArriveAtEndPoint()
    {
        uiTexts[mObjects.Length].GetComponent<Text>().color = Color.red;
        uiTexts[mObjects.Length].transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = true;
    } 
    void ClearMisson()
    {
        uiTexts[mObjects.Length].SetActive(true);
        Player.GetComponent<PlayerMove>().CompleteMission(EndPoint);
    }
}
