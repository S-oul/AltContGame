using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMove : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int currentWaypointIndex = 0;

    void OnEnable()
    {
        RhythmManager.OnBeat += Move;
    }

    void Move()
    {
        if (waypoints.Count > 0)
        {
            if(currentWaypointIndex < waypoints.Count - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                currentWaypointIndex = 0;
            }

            transform.position = waypoints[currentWaypointIndex].position;
        }
    }
}
