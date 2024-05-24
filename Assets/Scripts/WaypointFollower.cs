using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f;


    private Transform waypointTransform;
    private int currentWaypointIndex = 0;


 
   

    private void Update()
    {
        waypointTransform = waypoints[currentWaypointIndex].transform;


        if (Vector2.Distance(waypointTransform.position, transform.position) < .1f)
        {

            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;

            }
        }
               
           
            

        transform.position = Vector2.MoveTowards(transform.position, waypointTransform.position, Time.deltaTime * speed);
    }
}
   
