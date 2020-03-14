using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour {

    public GameObject Player;

    GameObject[] texts = new GameObject[10];
    string[] str = new string[10];
    bool[] clear = new bool[10];
    Vector3 EndPoint;

    int num = 3;
    int missionCnt = 0;

    float UIspeed = 400;
    bool bLisUIOpen = true;
    float UIX;
    float UIXposition;

	void Start ()
    {
        UIXposition = GetComponent<RectTransform>().localPosition.x;
        str[0] = "a hamburger shop";
        str[1] = "a trash bag";
        str[2] = "a street seller";
        for (int i = 0; i < num; i++)
        {
            texts[i] = this.transform.GetChild(i+1).gameObject;
            texts[i].GetComponent<Text>().text = "Take a picture with " + str[i];
            clear[i] = false;
        }
        texts[num] = this.transform.GetChild(num+1).gameObject;

        EndPoint = new Vector3(7, 3, 755);
    }
    private void FixedUpdate()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bLisUIOpen = !bLisUIOpen;
            Debug.Log("test" + bLisUIOpen);
        }
        if (bLisUIOpen)
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

    public void ClearObjective(int objNum, bool stat)
    {
        if (clear[objNum] != stat)
        {
            clear[objNum] = stat;
        }
        else { return; }
        if (stat)
        {
            texts[objNum].GetComponent<Text>().color = Color.red;
            missionCnt++;
        }
        else
        {
            texts[objNum].GetComponent<Text>().color = Color.red;
            missionCnt--;
        }
        texts[objNum].transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = stat;

        if (missionCnt == num)
        {
            ClearMisson();
        }
    }

    public void ArriveAtEndPoint()
    {
        texts[num].GetComponent<Text>().color = Color.red;
        texts[num].transform.GetChild(0).gameObject.GetComponent<Toggle>().isOn = true;
    } 
    void ClearMisson()
    {
        texts[num].SetActive(true);
        Player.GetComponent<PlayerMove>().CompleteMission(EndPoint);
    }
}
