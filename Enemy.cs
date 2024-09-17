using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Vector3 target;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    public GameObject player;

    public Transform[] waypoints;

    public float maxAngle;
    public float maxRadius;

    public float walk = 2f;
    public float run = 5f;

    public int currentIndex = 0;

    public bool seesPlayer = false;

    void Start()
    {
        target = waypoints[currentIndex].transform.position;
    }

    void Update()
    {
        seesPlayer = SpotPlayer();
        if (!seesPlayer)
        {
            SetTarget(waypoints[currentIndex].transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target, walk * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 1)
            {
                currentIndex = (currentIndex + 1) % waypoints.Length;
                target = waypoints[currentIndex].transform.position;
            }
        }

        //else chase player
        else
        {
            SetTarget(player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target, run * Time.deltaTime);
        }

    }

    void SetTarget(Vector3 t)
    {
        target = t;
    }

    bool SpotPlayer()
    {
        //test if it can switch by using distance
        if(Vector3.Distance(transform.position, player.transform.position) < 2.5f) 
        {
            return true;
        }
        return false;
    }

}
