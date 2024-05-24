using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaypointFollower2 : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f;

    private enum MovementState { idle }

    private MovementState state = MovementState.idle;


    private Transform waypointTransform;
    private int currentWaypointIndex = 0;

    private Animator anim;
    private SpriteRenderer sprite;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.flipX = true;


    }


    private void Update()
    {
        waypointTransform = waypoints[currentWaypointIndex].transform;

        
        if (Vector2.Distance(waypointTransform.position, transform.position) < .1f)
        {
            anim.SetBool("Hit", true);
            state= MovementState.idle;
            sprite.flipX = false;
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                sprite.flipX = true;



            }
        }




        transform.position = Vector2.MoveTowards(transform.position, waypointTransform.position, Time.deltaTime * speed);
    }

    public void ResetAnim()
    {
        anim.SetBool("Hit", false);
    }
}

