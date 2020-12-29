using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public bool isOnOption = false;
    public GameObject title;
    public GameObject player;

    Vector3 activePosition;
    Vector3 hidePosition;
    float UIspeed = 5.0f;

    public GameObject MouseSenseSlider;
    public GameObject MouseSenseSliderText;
    float sensitivity = 100;

    bool isChangingKey = false;
    int currentChange = -1;
    KeyCode[] UsedKey = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Q, KeyCode.Tab, KeyCode.R, KeyCode.LeftShift };
    KeyCode[] DontUse = { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6 };
    public GameObject gallary;
    public GameObject UI;
    public GameObject[] KeyButtons = new GameObject[8];
    string[] keyNames = { "앞", "뒤", "좌", "우", "UI", "갤러리", "삭제", "가속" };

    void Awake()
    {
        activePosition = GetComponent<RectTransform>().localPosition;
        hidePosition = activePosition;
        hidePosition.y -= 600;
        GetComponent<RectTransform>().localPosition = hidePosition;

        ChangeKey();
    }
    
    void Update()
    {
        Vector3 targetPos;
        if (isOnOption)
        {
            targetPos = activePosition;
        }
        else
        {
            targetPos = hidePosition;
        }

        GetComponent<RectTransform>().localPosition = Vector3.Slerp(GetComponent<RectTransform>().localPosition, targetPos, Time.deltaTime * UIspeed);

        if (isChangingKey)
        {
            if (Input.anyKey)
            {
                bool check = false;
                foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(vKey))
                    {
                        for (int i = 0; i < DontUse.Length; i++)
                        {
                            if (vKey == DontUse[i])
                            {
                                check = true;
                                break;
                            }
                        }
                        if (check) break;

                        if (currentChange != -1)
                        {
                            UsedKey[currentChange] = vKey;
                            ChangeKey();
                        }
                    }
                }
            }
        }
    }

    public void ClickBackButton()
    {
        if (isOnOption)
        {
            isOnOption = false;
            title.GetComponent<ScreenManager>().isInTitle = true;

            isChangingKey = false;
            ResetKeyButtonText();
            currentChange = -1;
        }
    }

    public void ClickKeyChangeButton(int num)
    {
        ResetKeyButtonText();
        if (isChangingKey)
        {
            if (currentChange == num)
            {
                isChangingKey = false;
                currentChange = -1;
                return;
            }
        }
        isChangingKey = true;
        currentChange = num;
        KeyButtons[num].transform.GetChild(0).GetComponent<Text>().text = "press any key";
    }
    void ChangeKey()
    {
        isChangingKey = false;
        player.GetComponent<PlayerMove>().FrontKey = UsedKey[0];
        player.GetComponent<PlayerMove>().BackKey = UsedKey[1];
        player.GetComponent<PlayerMove>().LeftKey = UsedKey[2];
        player.GetComponent<PlayerMove>().RightKey = UsedKey[3];
        UI.GetComponent<MissionUI>().UIOnOffKey = UsedKey[4];
        gallary.GetComponent<PhoneGallary>().GallaryKey = UsedKey[5];
        gallary.GetComponent<PhoneGallary>().DeleteKey = UsedKey[6];
        player.GetComponent<PlayerMove>().AccelKey = UsedKey[7];
        ResetKeyButtonText();
    }
    void ResetKeyButtonText()
    {
        for (int i = 0; i < UsedKey.Length; i++)
        {
            KeyButtons[i].transform.GetChild(0).GetComponent<Text>().text = keyNames[i] + ": " + UsedKey[i].ToString();
        }
    }

    public void ChangeMouseSensitivity()
    {
        sensitivity = MouseSenseSlider.GetComponent<Slider>().value * 200 + 50;
        MouseSenseSliderText.GetComponent<Text>().text = ((int)(MouseSenseSlider.GetComponent<Slider>().value * 100)).ToString() + "%";
        player.GetComponent<CameraMove>().sensitivity = sensitivity;
    }
}
