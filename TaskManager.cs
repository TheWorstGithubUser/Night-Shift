using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour{
    // This is a script that tracks how many tasks have been completed
    
    int uncompletedTasks = 0;
    TaskScript[] tasks; // These are the tasks
    public float completed;

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] string taskListLabel = "TaskList:";
    [SerializeField] string allTasksDoneMessage = "All Tasks Completed";
    // Start is called before the first frame update
    void Start(){
        tasks = FindObjectsByType<TaskScript> (FindObjectsSortMode.None);
        for (int i = tasks.Length-1; i >= 0; i--) { // sorts the list (this is important to get them to display properly)
            bool swap = false;
            for (int j = 0; j < i; j++) {
                if (string.Compare (tasks[j].displayName, tasks[j+1].displayName) < 0) {
                    var temp = tasks[j];
                    tasks[j] = tasks[j+1];
                    tasks[j+1] = temp;
                    swap = true;
                }
            }
            if (!swap) break;
        }
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
        if (completed < 1f) {
            // draws the list of tasks (skipping tasks that are already completed)
            string taskList = $"{taskListLabel}\n";
            string lastTaskName = tasks[0].displayName;
            int taskCount = 0;
            for (int i = 0; i < tasks.Length; i++) {
                if (tasks[i].Complete) continue; // skips completed tasks
                if (lastTaskName == tasks[i].displayName) {
                    taskCount++;
                }
                else {
                    taskList += FormatTask (lastTaskName, taskCount);
        
                    taskCount = 1;
                    lastTaskName = tasks[i].displayName;
                }
            }
            taskList += FormatTask (lastTaskName, taskCount);
            text.text = taskList;
        }
        else {
            text.text = allTasksDoneMessage;
        }
    }

    public static string FormatTask (string taskName, int taskCount) {
        switch (taskCount) {
            case 0:
                return string.Empty;
            case 1:
                return $"{taskName}\n";
            default:
                return $"{taskName}: {taskCount}\n";
        }
    }
}
