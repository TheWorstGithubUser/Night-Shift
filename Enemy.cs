using System;
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

    private AIPather pather;
    public AIPather Pather => pather;

    public float increment = 1/3f;

    public float maxAngle;
    public float maxRadius;

    public float walk = 2f;
    public float run = 5f;

    public int currentIndex = 0;

    public bool seesPlayer = false;

    private Vector2 facingDirection;
    private float time;
    public bool RemembersPlayer => time <= 1;

    public float pushRay = 0.5f;
    public float pushRayDistance = 1;

    public int pathDepth = 3;

    private float waitTime = 0;
    public bool IsWaiting => waitTime > 0;

    private bool autoTarget = true;


    private Vector2 moveDir = Vector2.zero;
    private float aiPathTime = 0;

    public const float PATH_CHECK_INTERVOL = 0.3f;

    Vector2 currentMovementVelocity = Vector2.zero;
    public Vector2 CurrentMovementVelocity => currentMovementVelocity;

    void Start()
    {
        target = waypoints[currentIndex].transform.position;
        pather = FindObjectOfType<AIPather> ();
        aiPathTime = UnityEngine.Random.Range (0.0f, PATH_CHECK_INTERVOL);
    }

    void Update()
    {
        float movementSpeed = run;
        seesPlayer = SpotPlayer();
        if (waitTime <= 0) {
            if (!seesPlayer) {
                if (!RemembersPlayer) {
                    if (autoTarget)
                        SetTarget (waypoints[currentIndex].transform.position);
                    //transform.position = Vector2.MoveTowards (transform.position, target, walk * Time.deltaTime);
                    movementSpeed = walk;

                    if (Vector2.Distance (transform.position, waypoints[currentIndex].position) < 1) {
                        currentIndex = (currentIndex + 1) % waypoints.Length;
                        target = waypoints[currentIndex].transform.position;
                    }
                }
                else {
                    if (autoTarget)
                        SetTarget (player.transform.position);
                    //transform.position = Vector2.MoveTowards (transform.position, target, run * Time.deltaTime);
                }
                time += Time.deltaTime * increment;
                //Debug.Log(time);
            }


            //else chase player
            else {
                //autoTarget = true;
                if (autoTarget)
                    SetTarget (player.transform.position);
                //transform.position = Vector2.MoveTowards (transform.position, target, run * Time.deltaTime);
                time = 0;
            }

            if (aiPathTime < 0) {
                if (pather != null) {
                    Vector2 dir = pather.FindPath (transform.position, target, pathDepth);
                    Vector2 sideDir = new Vector2 (-dir.y, dir.x);
                    Vector2 walkDir = dir;
                    for (int d = -1; d <= 1; d+=2) {
                        if (Physics2D.Raycast (transform.position, dir + sideDir * pushRay * d, pushRayDistance, pather.GetObstacleMask ()).collider != null) {
                            walkDir -= sideDir * pushRay * d;
                        }
                    }
                    walkDir.Normalize ();
                    currentMovementVelocity = walkDir * movementSpeed;
                    //transform.position += (Vector3)CurrentMovementVelocity * Time.deltaTime;
                }
                else {
                    currentMovementVelocity = (Vector2)(target - transform.position).normalized * movementSpeed;
                    //transform.position = Vector2.MoveTowards (transform.position, target, movementSpeed * Time.deltaTime);
                }
                aiPathTime = PATH_CHECK_INTERVOL;
            }

            aiPathTime -= Time.deltaTime;

            transform.position += (Vector3)currentMovementVelocity * Time.deltaTime;
        }

        

        facingDirection = (target - transform.position).normalized;
        waitTime = MathF.Max(waitTime - Time.deltaTime, 0);
    }

    void SetTarget(Vector3 t)
    {
        target = t;
    }

    public Vector3 GetCurrentWaypoint () {
        return waypoints[currentIndex].position;
    }

    public void ForceTarget (Vector3 t) {
        SetTarget (t);
        autoTarget = false;
    }

    public void MakeWait (float time) {
        waitTime = time;
    }

    public void EnableAutoTarget () { autoTarget = true; }

    bool SpotPlayer()
    {
        //test if it can switch by using distance
        bool facingPlayer = false;
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        if (seesPlayer) {
            facingPlayer = true;
        }
        else {
            facingPlayer = Vector2.Dot (playerDirection, facingDirection) > Mathf.Cos(maxAngle * Mathf.Deg2Rad);
        }

        var res = Physics2D.Raycast (transform.position, playerDirection, Vector2.Distance(player.transform.position, transform.position), obstacleMask);

        return Vector3.Distance (transform.position, player.transform.position) < maxRadius && facingPlayer && res.collider == null;
        
    }

}
