using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour{

    
    int uncompletedTasks = 0;
    TaskScript[] tasks;
    public float completed;
    // Start is called before the first frame update
    void Start(){
        tasks = FindObjectsByType<TaskScript> (FindObjectsSortMode.None);
    }

    public int GetCompletedTasks () {
        int num = 0;
        for (int i = 0; i < tasks.Length; i++) {
            if (tasks[i].Complete) num++;
        }
        return num;
    }

    public float GetCompletedPercent () {
        return GetCompletedTasks () / ((float)tasks.Length);
    }

    // Update is called once per frame
    void Update(){
        completed = GetCompletedPercent ();
    }
}
