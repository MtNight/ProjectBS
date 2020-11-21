using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WayPointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(WayPoint wp, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else 
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(wp.transform.position, 0.1f);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(wp.transform.position + (wp.transform.right * wp.width / 2.0f), wp.transform.position - (wp.transform.right * wp.width / 2f));

        if (wp.previousWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = wp.transform.right * wp.width / 2.0f;
            Vector3 offsetTo = wp.previousWaypoint.transform.right * wp.previousWaypoint.width / 2.0f;

            Gizmos.DrawLine(wp.transform.position + offset, wp.previousWaypoint.transform.position + offsetTo);
        }
        if (wp.nextWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = wp.transform.right * wp.width / 2.0f;
            Vector3 offsetTo = wp.nextWaypoint.transform.right * wp.nextWaypoint.width / 2.0f;

            Gizmos.DrawLine(wp.transform.position - offset, wp.nextWaypoint.transform.position - offsetTo);
        }

        if (wp.branchs != null && wp.branchs.Count > 0)
        {
            foreach (WayPoint branch in wp.branchs)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(wp.transform.position, branch.transform.position);
            }
        }
    }
}
