using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private Vector3[] points;
    private Vector3 currentPosition;
    private bool gameStarted;

    public Vector3[] Points => points;
    public Vector3 CurrentPosition => currentPosition;  

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = true;
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public Vector3 GetWaypointPosition(int index)
    {
        return CurrentPosition + Points[index];
    }

    //s? d?ng ?? v? các ??i t??ng và hi?n th? thông tin g? l?i trong Scene View c?a Unity Editor
    private void OnDrawGizmos()
    {
        if(!gameStarted && transform.hasChanged)
        {
            currentPosition = transform.position;
        }

        for(int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(points[i] + currentPosition, 0.5f);

            if (i < points.Length - 1)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(points[i] + currentPosition, points[i + 1] + currentPosition);
            }
        }  
    }
}
