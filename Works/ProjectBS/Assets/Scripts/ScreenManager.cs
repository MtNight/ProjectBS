using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ScreenManager : MonoBehaviour
{
    public GameObject player;
    public GameObject missionUI;
    public GameObject mainCam;
    public GameObject option;

    bool isStarting = false;
    public bool isInTitle = true;
    GameObject TitleCamera;
    GameObject TitleCanvas;
    int titleNumber;
    Vector3[] TitlePosition =
        {
        new Vector3(6, 24, 24),
        new Vector3(-50, 18, 775),
        new Vector3(465, 30, 320),
        new Vector3(284, 4, 50)
    };
    Vector3[] TitleRotation =
        {
        new Vector3(45, 30, 0),
        new Vector3(45, 105, 0),
        new Vector3(45, -90, 0),
        new Vector3(0, 0, 0)
    };

    float camFar = 0;

    public PostProcessVolume titlePPVol;
    public PostProcessVolume gamePPVol;
    AutoExposure titleExposure;
    AutoExposure gameExposure;

    GameObject TitleUI;
    GameObject OptionUI;

    void Awake()
    {
        TitleCamera = transform.GetChild(0).gameObject;
        TitleCanvas = transform.GetChild(1).gameObject;
        TitleUI = TitleCanvas.transform.GetChild(1).gameObject;
        OptionUI = TitleCanvas.transform.GetChild(2).gameObject;

        titleNumber = Random.Range(0, 4);
        camFar = mainCam.GetComponent<Camera>().farClipPlane;
        mainCam.GetComponent<Camera>().farClipPlane = 1;
        CamSetting();
        //missionUI.SetActive(false);

        titlePPVol.profile.TryGetSettings(out titleExposure);
        gamePPVol.profile.TryGetSettings(out gameExposure);

        titleExposure.minLuminance.value = 4;
        gameExposure.minLuminance.value = 0;
    }
    void Start()
    {
    }

    void Update()
    {
        if (isInTitle)
        {
            int changeNum = titleNumber;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKey(KeyCode.F1))
                {
                    changeNum = 0;
                }
                if (Input.GetKey(KeyCode.F2))
                {
                    changeNum = 1;
                }
                if (Input.GetKey(KeyCode.F3))
                {
                    changeNum = 2;
                }
                if (Input.GetKey(KeyCode.F4))
                {
                    changeNum = 3;
                }
            }
            if (changeNum != titleNumber)
            {
                titleNumber = changeNum;
                CamSetting();
            }
        }
    }

    void CamSetting()
    {
        player.transform.position = TitlePosition[titleNumber];
        TitleCamera.transform.position = TitlePosition[titleNumber];
        TitleCamera.transform.eulerAngles = TitleRotation[titleNumber];
    }
    IEnumerator StartGame()
    {
        while (true)
        {
            titleExposure.minLuminance.value = -9;
            gameExposure.minLuminance.value = -9;
            yield return new WaitForSeconds(0.8f);
            isInTitle = false;
            mainCam.GetComponent<ScreenShot>().isStartGame = true;
            mainCam.GetComponent<Camera>().farClipPlane = camFar;
            missionUI.GetComponent<MissionUI>().InitStartState();
            missionUI.SetActive(true);
            player.GetComponent<PlayerMove>().InitStartState();
            player.GetComponent<CameraMove>().isStartGame = true;
            player.SetActive(true);
            TitleCamera.SetActive(false);
            TitleCanvas.SetActive(false);

            titleExposure.minLuminance.value = 3;
            gameExposure.minLuminance.value = 0;
            yield break;
        }
    }

    public void ClickStartButton()
    {
        if (isInTitle)
        {
            if (!isStarting)
            {
                isStarting = true;
                StartCoroutine(StartGame());
            }
        }
    }
    public void ClickOptionButton()
    {
        if (isInTitle)
        {
            option.GetComponent<OptionManager>().isOnOption = true;
            isInTitle = false;
        }
    }
    public void ClickExitButton()
    {
        if (isInTitle)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit() // 어플리케이션 종료
        }
#endif
        }
    }
}
