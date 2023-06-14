using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WayPoint))]

public class NewBehaviourScript : Editor
{
    WayPoint WayPoint => target as WayPoint;

    //g?i m?i khi Unity c?p nh?t scene view
    private void OnSceneGUI()
    {
        for(int i = 0; i < WayPoint.Points.Length; i++)
        {
            //Create Handle (tao va chinh sua các handle trong scene view)
            Vector3 currentWaypointPoint = WayPoint.CurrentPosition + WayPoint.Points[i];
            Vector3 newWaypointPoint = Handles.FreeMoveHandle(currentWaypointPoint,
                Quaternion.identity, 0.7f, new Vector3(0.3f, 0.3f, 0.3f),
                Handles.SphereHandleCap);

            //Create Text cho moi waypoint
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.yellow;
            Vector3 textAlliment = Vector3.down * 0.35f + Vector3.right * 0.35f;
            Handles.Label(WayPoint.CurrentPosition + WayPoint.Points[i] + textAlliment,
                $"{i+1}", textStyle);
            EditorGUI.EndChangeCheck();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move Handle");
                WayPoint.Points[i] = newWaypointPoint - WayPoint.CurrentPosition; 
            }


 
        }
    }

}
    