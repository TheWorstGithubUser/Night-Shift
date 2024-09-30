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

    private AIPather pather;

    public Transform[] waypoints;

    public float maxAngle;
    public float maxRadius;

    public float walk = 2f;
    public float run = 5f;

    private float time;
    public float increment = 1/3f;

    public int currentIndex = 0;

    public bool seesPlayer = false;

    Vector2 facingDirection;

    public float pushRay = 0.5f;
    public float pushRayDistance = 1;

    void Start()
    {
        target = waypoints[currentIndex].transform.position;
        time = 3f;
        //increment = 1 / 60f;
        
        pather = FindObjectOfType<AIPather> ();
    }

    void Update()
    {
        seesPlayer = SpotPlayer();
        float movementSpeed = run;
        if (!seesPlayer)
        {
            if (time > 1f)
            {
                SetTarget(waypoints[currentIndex].transform.position);
                //transform.position = Vector2.MoveTowards(transform.position, target, walk * Time.deltaTime);
                movementSpeed = walk;

                if (Vector2.Distance(transform.position, target) < 1)
                {
                    currentIndex = (currentIndex + 1) % waypoints.Length;
                    target = waypoints[currentIndex].transform.position;
                }
            }
            else
            {
                SetTarget(player.transform.position);
                //transform.position = Vector2.MoveTowards(transform.position, target, run * Time.deltaTime);
            }
            time += Time.deltaTime * increment;
            //Debug.Log(time);
        }

        //else chase player
        else
        {
            SetTarget(player.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, target, run * Time.deltaTime);
            time = 0;
        }

        if (pather != null) {
            Vector2 dir = pather.FindPathFromTo (transform.position, target, 4);
            Vector2 sideDir = new Vector2 (-dir.y, dir.x);
            Vector2 walkDir = dir;
            for (int d = -1; d <= 1; d+=2) {
                if (Physics2D.Raycast (transform.position, dir + sideDir * pushRay * d, pushRayDistance, pather.GetObstacleMask ()).collider != null) {
                    walkDir -= sideDir * pushRay * d;
                }
            }
            walkDir.Normalize ();
            transform.position += (Vector3)walkDir * movementSpeed * Time.deltaTime;
        }
        else {
            transform.position = Vector2.MoveTowards (transform.position, target, movementSpeed * Time.deltaTime);
        
        }

        facingDirection = (target - transform.position).normalized;
    }

    void SetTarget(Vector3 t)
    {
        target = t;
    }

    bool SpotPlayer()
    {
        
        bool facingPlayer = false;
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        if (seesPlayer)
        {
            facingPlayer = true;
        }
        else
        {
            facingPlayer = Vector2.Dot(playerDirection, facingDirection) > Mathf.Cos(maxAngle * Mathf.Deg2Rad);
        }

        var res = Physics2D.Raycast(transform.position, playerDirection, Vector2.Distance(player.transform.position, transform.position), obstacleMask);

        return Vector3.Distance(transform.position, player.transform.position) < maxRadius && facingPlayer && res.collider == null;
    }

}
