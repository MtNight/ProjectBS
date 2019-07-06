using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour {

    float angle;

	void Start () {
        angle = 0;
    }
	
	void Update () {
        if (angle < 10)
        {
            angle += Time.deltaTime;
        }
        Vector3 rot = new Vector3(0, angle, 0);
        transform.Rotate(rot);
	}
}
