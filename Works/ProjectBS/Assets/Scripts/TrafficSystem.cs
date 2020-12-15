using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    public GameObject[] vehiclePrefabs = new GameObject[20];
    Vector3[][] spawnPoint = new Vector3[4][];

    public int NSLight = 0;
    public int EWLight = 3;    //0=blue 1=yellow 2=red
    public bool cross = true;
    GameObject[] CrosswalkNS;
    GameObject[] CrosswalkEW;

    public int cnt = 0;
    float trafficCycle = 120.0f;
    float lightInterval = 5.0f;

    float[] vehicleSpawnRate =
        {
        0.2f,
        0.5f, 0.5f, 0.5f,
        5.0f, 5.0f, 5.0f,
        0.5f,
        0.2f,
        0.5f,
        2.0f,
        3.0f, 3.0f, 3.0f,
        1.0f, 1.0f, 1.0f,
        2.0f, 2.0f, 2.0f

        };
    float rateSum = 0;

    void Start()
    {
        for (int i = 0; i < vehicleSpawnRate.Length; i++)
        {
            rateSum += vehicleSpawnRate[i];
        }

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            spawnPoint[i] = new Vector3[6];
        }
        spawnPoint[0][0] = new Vector3(-26, 1, 1200);
        spawnPoint[0][1] = new Vector3(-22, 1, 1200);
        spawnPoint[0][2] = new Vector3(-18, 1, 1200);
        spawnPoint[0][3] = new Vector3(388, 1, 1200);
        spawnPoint[0][4] = new Vector3(392, 1, 1200);
        spawnPoint[0][5] = new Vector3(396, 1, 1200);

        spawnPoint[1][0] = new Vector3(900, 1, 79.65f);
        spawnPoint[1][1] = new Vector3(900, 1, 83.65f);
        spawnPoint[1][2] = new Vector3(900, 1, 87.65f);
        spawnPoint[1][3] = new Vector3(900, 1, 781.62f);
        spawnPoint[1][4] = new Vector3(900, 1, 785.62f);
        spawnPoint[1][5] = new Vector3(900, 1, 789.62f);

        spawnPoint[2][0] = new Vector3(-12.75f, 1, -300);
        spawnPoint[2][1] = new Vector3(-8.75f, 1, -300);
        spawnPoint[2][2] = new Vector3(-4.75f, 1, -300);
        spawnPoint[2][3] = new Vector3(401.25f, 1, -300);
        spawnPoint[2][4] = new Vector3(405.25f, 1, -300);
        spawnPoint[2][5] = new Vector3(409.25f, 1, -300);

        spawnPoint[3][0] = new Vector3(-500, 1, 66.4f);
        spawnPoint[3][1] = new Vector3(-500, 1, 70.4f);
        spawnPoint[3][2] = new Vector3(-500, 1, 74.4f);
        spawnPoint[3][3] = new Vector3(-500, 1, 768.37f);
        spawnPoint[3][4] = new Vector3(-500, 1, 772.37f);
        spawnPoint[3][5] = new Vector3(-500, 1, 776.37f);

        CrosswalkNS = new GameObject[transform.GetChild(0).GetChild(0).childCount];
        CrosswalkEW = new GameObject[transform.GetChild(0).GetChild(1).childCount];
        for (int i = 0; i < CrosswalkNS.Length; i++)
        {
            CrosswalkNS[i] = transform.GetChild(0).GetChild(0).GetChild(i).gameObject;
        }
        for (int i = 0; i < CrosswalkEW.Length; i++)
        {
            CrosswalkEW[i] = transform.GetChild(0).GetChild(1).GetChild(i).gameObject;
        }
        changeLight();

        StartCoroutine(SpawnVehicle(0));
        StartCoroutine(SpawnVehicle(1));
        StartCoroutine(SpawnVehicle(2));
        StartCoroutine(SpawnVehicle(3));
        StartCoroutine(TrafficSchedule());
    }
    
    void Update()
    {
        cnt = transform.childCount;
    }

    int chooseVehiclePrefab()
    {
        float random = Random.Range(0, rateSum);
        float randomCheck = 0;
        for (int i = 0; i < vehiclePrefabs.Length; i++)
        {
            randomCheck += vehicleSpawnRate[i];
            if (random < randomCheck)
            {
                return i;
            }
        }
        return 1;
    }

    IEnumerator SpawnVehicle(int dir)
    {
        while (true)
        {
            if (transform.childCount < 400)
            {
                GameObject prefab = Instantiate(vehiclePrefabs[chooseVehiclePrefab()]);

                prefab.transform.eulerAngles = new Vector3(0, dir * 90, 0);
                prefab.transform.position = spawnPoint[dir][Random.Range(0, spawnPoint[dir].Length)];
                prefab.transform.SetParent(this.transform);
                prefab.GetComponent<VehiclesMove>().direction = dir;
            }
            yield return new WaitForSeconds(1.0f + Random.Range(-0.2f, 0.8f));
        }
    }


    IEnumerator TrafficSchedule()
    {
        while (true)
        {
            NSLight = 0;
            EWLight = 3;
            changeLight();
            cross = true;
            yield return new WaitForSeconds(trafficCycle / 4);
            cross = false;
            yield return new WaitForSeconds(trafficCycle / 4 - lightInterval);
            NSLight = 1;
            EWLight = 1;
            changeLight();
            yield return new WaitForSeconds(lightInterval);
            NSLight = 3;
            EWLight = 0;
            changeLight();
            cross = true;
            yield return new WaitForSeconds(trafficCycle / 4);
            cross = false;
            yield return new WaitForSeconds(trafficCycle / 4 - lightInterval);
            NSLight = 1;
            EWLight = 1;
            changeLight();
            yield return new WaitForSeconds(lightInterval);
        }
    }

    void changeLight()
    {
        int ns = -3;
        int ew = -3;
        if (NSLight != 0)
        {
            ns = 3;
        }
        else
        {
            ns = -3;
        }

        if (EWLight != 0)
        {
            ew = 3;
        }
        else
        {
            ew = -3;
        }

        for (int i = 0; i < CrosswalkNS.Length; i++)
        {
            Vector3 tmp = CrosswalkNS[i].transform.position;
            tmp.y = ns;
            CrosswalkNS[i].transform.position = tmp;
        }
        for (int i = 0; i < CrosswalkEW.Length; i++)
        {
            Vector3 tmp = CrosswalkEW[i].transform.position;
            tmp.y = ew;
            CrosswalkEW[i].transform.position = tmp;
        }
    }
}
