using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepScript : MonoBehaviour
{
    [SerializeField] TaskScript task;
    private bool sweeping;
    public float timeDown;

    void Start()
    {
        timeDown = 2;
    }

    void Update()
    {
        if(timeDown <= 0)
        {
            task.complete = true;
        }
    }

    void OnPlayerInteract()
    {
        //Debug.Log(timeDown);
        //timeDown -= Time.deltaTime;
        if (timeDown >= 0)
        {
            Debug.Log(timeDown);
            timeDown -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Done");
            task.complete = true;
            task.IsComplete();
        }
    }
}
