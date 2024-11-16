using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainScript : MonoBehaviour
{
    public Transform[] stations;//station location
    public float speed;         //train speed
    public int currStation;    //current target station
    public float stopTime;    //time at station
    Vector2 nextStop;           
    Vector2 lastStop;
    Collider2D collider;
    [SerializeField] LightField lightField;
    [SerializeField] Interactable interact;
    [SerializeField] GameObject player;
    [SerializeField] GameObject fov;
    private float rotation;
    private bool playerRiding;
    private bool atStation;


    // Start is called before the first frame update
    void Start()
    {
        speed = 15f;
        currStation = 0;
        nextStop = new Vector2(stations[currStation].position.x, stations[currStation].position.y);
        collider = GetComponent<BoxCollider2D>();
        playerRiding = false;
        atStation = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (atStation)
            {
                if(playerRiding)
                {
                    player.SetActive(true);
                    fov.SetActive(true);
                }
            }
        }
        if (currStation > stations.Length-1)
        {
            currStation = 0;
        }
        else if (Vector2.Distance(transform.position, stations[currStation].position)<1)
        {
            if(stopTime >= 0)
            {
                stationArrival();
            }
            else
            {
                leavingStation();
            }
                
        }
        if (!atStation)
        {
            nextStop = new Vector2(stations[currStation].position.x, stations[currStation].position.y);
            lastStop = new Vector2(stations[currStation].transform.position.x - transform.position.x, stations[currStation].transform.position.y - transform.position.y);
            rotation = Mathf.Atan2(lastStop.y, lastStop.x) * Mathf.Rad2Deg;
            lightField.SetAimDirection(rotation);
        }
        moveTrain();
    }

    void moveTrain()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextStop, speed*Time.deltaTime);
        if (playerRiding)
        {
            player.transform.position = transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !atStation)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void stationArrival()
    {
        atStation = true;
        stopTime -= Time.deltaTime;
    }

    void leavingStation()
    {
        currStation++;
        atStation = false;
        stopTime = 2;
    }

    void OnPlayerInteract()
    {
        if (atStation)
        {
            playerRiding = !playerRiding;

            if (playerRiding)
            {
                player.SetActive(false);
                fov.SetActive(false);
            }
            else
            {
                player.SetActive(true);
                fov.SetActive(true);
            }
        }
    }

    
}
