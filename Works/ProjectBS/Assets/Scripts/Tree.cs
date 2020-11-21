using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    GameObject[] Trees = new GameObject[3];

    void Start()
    {
        for (int i = 0; i < Trees.Length; i++)
        {
            Trees[i] = transform.GetChild(i + 1).gameObject;
            Trees[i].SetActive(false);
        }

        int idx = Random.Range(0, 3);
        Trees[idx].SetActive(true);

        int r = Random.Range(0, 4);
        Vector3 rot = new Vector3(0, r * 90, 0);
        Trees[idx].transform.rotation = Quaternion.Euler(rot);
    }
}
