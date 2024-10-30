using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour{
    // This is a script that tracks how many tasks have been completed
    
    int uncompletedTasks = 0;
    TaskScript[] tasks; // These are the tasks
    public float completed;
    // Start is called before the first frame update
    void Start(){
        tasks = FindObjectsByType<TaskScript> (FindObjectsSortMode.None);
    }

    // this counts the number of tasks that have been completed
    public int GetCompletedTasks () {
        int num = 0;
        for (int i = 0; i < tasks.Length; i++) {
            if (tasks[i].Complete) num++;
        }
        return num;
    }

    // this counts the number of tasks completed, and returns the % (0 = 0%, 1 = 100%)
    public float GetCompletedPercent () {
        return GetCompletedTasks () / ((float)tasks.Length);
    }

    // Update is called once per frame
    void Update(){
        completed = GetCompletedPercent ();
    }
}
