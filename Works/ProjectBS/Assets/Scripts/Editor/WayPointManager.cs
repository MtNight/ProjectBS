using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WayPointManager : EditorWindow
{
    public Transform wayPointRoot;

    [MenuItem("Tool/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WayPointManager>();
    }

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("wayPointRoot"));

        if (wayPointRoot == null)
        {
            //EditorGUILayout.HelpBox("Please assign a root transform", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("Box");
            DrawButton();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    void DrawButton()
    {
        if (GUILayout.Button("Create WayPoint"))
        {
            CreateWayPoint();
        }
        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<WayPoint>())
        {
            if (GUILayout.Button("Add Branch"))
            {
                CreateBranch();
            }
            if (GUILayout.Button("Create WayPoint Before"))
            {
                CreateWayPointBefore();
            }
            if (GUILayout.Button("Create WayPoint After"))
            {
                CreateWayPointAfter();
            }
            if (GUILayout.Button("Remove WayPoint"))
            {
                RemoveWayPoint();
            }
        }
    }

    void CreateBranch()
    {
        GameObject wayPointObj = new GameObject("WayPoint" + wayPointRoot.childCount, typeof(WayPoint));
        wayPointObj.transform.SetParent(wayPointRoot, false);

        WayPoint wp = wayPointObj.GetComponent<WayPoint>();
        WayPoint branchRoot = Selection.activeGameObject.GetComponent<WayPoint>();
        branchRoot.branchs.Add(wp);

        wp.transform.position = branchRoot.transform.position;
        wp.transform.forward = branchRoot.transform.forward;


        Selection.activeGameObject = wp.gameObject;
    }

    void CreateWayPoint()
    {
        GameObject wayPointObj = new GameObject("WayPoint" + wayPointRoot.childCount, typeof(WayPoint));
        wayPointObj.transform.SetParent(wayPointRoot, false);

        WayPoint wp = wayPointObj.GetComponent<WayPoint>();
        if (wayPointRoot.childCount > 1)
        {
            wp.previousWaypoint = wayPointRoot.GetChild(wayPointRoot.childCount - 2).GetComponent<WayPoint>();
            wp.previousWaypoint.nextWaypoint = wp;

            wp.transform.position = wp.previousWaypoint.transform.position;
            wp.transform.forward = wp.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = wp.gameObject;
        Debug.Log("Create!");
    }
    void CreateWayPointBefore()
    {
        GameObject wayPointObj = new GameObject("WayPoint" + wayPointRoot.childCount, typeof(WayPoint));
        wayPointObj.transform.SetParent(wayPointRoot, false);

        WayPoint wp = wayPointObj.GetComponent<WayPoint>();

        WayPoint newWayPoint = wayPointObj.GetComponent<WayPoint>();
        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();

        wayPointObj.transform.position = selectedWayPoint.transform.position;
        wayPointObj.transform.forward = selectedWayPoint.transform.forward;

        if(selectedWayPoint.previousWaypoint != null)
        {
            newWayPoint.previousWaypoint = selectedWayPoint.previousWaypoint;
            selectedWayPoint.previousWaypoint.nextWaypoint = newWayPoint;
        }
        newWayPoint.nextWaypoint = selectedWayPoint;
        selectedWayPoint.previousWaypoint = newWayPoint;
        newWayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWayPoint.gameObject;
        Debug.Log("CreateB!");
    }
    void CreateWayPointAfter()
    {
        GameObject wayPointObj = new GameObject("WayPoint" + wayPointRoot.childCount, typeof(WayPoint));
        wayPointObj.transform.SetParent(wayPointRoot, false);

        WayPoint wp = wayPointObj.GetComponent<WayPoint>();

        WayPoint newWayPoint = wayPointObj.GetComponent<WayPoint>();
        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();

        wayPointObj.transform.position = selectedWayPoint.transform.position;
        wayPointObj.transform.forward = selectedWayPoint.transform.forward;

        if (selectedWayPoint.nextWaypoint != null)
        {
            newWayPoint.nextWaypoint = selectedWayPoint.nextWaypoint;
            selectedWayPoint.nextWaypoint.previousWaypoint = newWayPoint;
        }
        newWayPoint.previousWaypoint = selectedWayPoint;
        selectedWayPoint.nextWaypoint = newWayPoint;
        newWayPoint.transform.SetSiblingIndex(selectedWayPoint.transform.GetSiblingIndex() + 1);

        Selection.activeGameObject = newWayPoint.gameObject;
        Debug.Log("CreateA!");
    }
    void RemoveWayPoint()
    {
        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();

        if (selectedWayPoint.nextWaypoint != null)
        {
            selectedWayPoint.nextWaypoint.previousWaypoint = selectedWayPoint.previousWaypoint;
        }
        if (selectedWayPoint.previousWaypoint != null)
        {
            selectedWayPoint.previousWaypoint.nextWaypoint = selectedWayPoint.nextWaypoint;
        }

        DestroyImmediate(selectedWayPoint.gameObject);
        Debug.Log("Remove!");
    }


}

//https://youtu.be/MXCZ-n5VyJc