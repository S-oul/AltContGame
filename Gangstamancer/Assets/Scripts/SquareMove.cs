using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMove : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int currentWaypointIndex = 0;
    public int CurrentWaypointIndex { get => currentWaypointIndex; }

    [SerializeField] Animator animator;

    void OnEnable()
    {
        RythmTimeLine.OnBeat += Move;
    }

    void Move()
    {
        animator.SetTrigger("OnBeat");
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
    public Transform CurrentSquare()
    {
        return waypoints[currentWaypointIndex];
    }   
}
