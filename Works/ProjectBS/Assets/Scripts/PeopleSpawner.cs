using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawner : MonoBehaviour
{
    public GameObject peoplePrefab;
    public GameObject WayPointManager;
    public GameObject VehicleManager;
    public int spawnCount = 20;
    public bool isSpawnedCompletely = false;

    public GameObject[] peoplePrefabs;
    float[] peopleSpawnRate =
        { 10.0f, 10.0f, 10.0f,
          0.1f, 0.1f, 0.1f,
          0.1f, 0.1f, 0.1f,
          5.0f, 5.0f, 5.0f,
          1.0f, 1.0f, 1.0f,
          0.7f, 0.7f, 0.7f,
          0.3f, 0.3f, 0.3f,
          0.2f, 0.2f, 0.2f,
          0.9f, 0.9f, 0.9f,
          0.0f, 0.0f, 0.0f,
          0.5f, 0.5f, 0.5f,
          1.0f, 1.0f, 1.0f,
          1.0f, 1.0f, 1.0f,
          1.0f, 1.0f, 1.0f,
          0.1f, 0.1f, 0.1f,
        };
    float rateSum = 0;

    void Awake()

    {
        //for test
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 30;

    }

    void Start()
    {
        for (int i = 0; i < peoplePrefabs.Length; i++)
        {
            rateSum += peopleSpawnRate[i];
        }

        if (peoplePrefab != null && WayPointManager != null) 
        {
            StartCoroutine(Spawn());
        }
    }

    int choosePeoplePrefab()
    {
        float random = Random.Range(0, rateSum);
        float randomCheck = 0;
        for (int i = 0; i < peoplePrefabs.Length; i++)
        {
            randomCheck += peopleSpawnRate[i];
            if (random < randomCheck)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator Spawn()
    {
        int cnt = 0;
        while (true)
        {
            if (spawnCount > cnt)
            {
                int prefabIdx = choosePeoplePrefab();
                if (prefabIdx == -1) { Debug.Log("Fail to choose people prefab"); }
                GameObject people = Instantiate(peoplePrefabs[prefabIdx]);
                Transform position = WayPointManager.transform.GetChild(Random.Range(0, WayPointManager.transform.childCount - 1));
                people.GetComponent<WayPointNavigator>().currentWayPoint = position.GetComponent<WayPoint>();
                people.GetComponent<PeopleMove>().trafficSystem = VehicleManager;
                people.transform.position = position.position;
                people.transform.SetParent(this.transform);

                yield return new WaitForSeconds(0.1f);
                cnt++;
            }
            else
            {
                isSpawnedCompletely = true;
                Debug.Log("spawn end");
                yield break;
            }
        }
    }
}
