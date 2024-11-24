
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour{

    
    //int uncompletedTasks = 0;
    TaskScript[] tasks;
    public int completed;


    [SerializeField] TextMeshProUGUI text;

    [SerializeField] string taskListLabel = "TaskList:";
    [SerializeField] string allTasksDoneMessage = "All Tasks Completed";
    [SerializeField] Animator taskCompletionMessage;
    [SerializeField] string taskCompletionMessageAnimationName;
    // Start is called before the first frame update
    void Start(){
        tasks = FindObjectsByType<TaskScript> (FindObjectsSortMode.None);

        for (int i = tasks.Length-1; i >= 0; i--) {
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

        if (taskCompletionMessage != null) taskCompletionMessage.Play (taskCompletionMessageAnimationName, -1, 1);
    }

    

    public int GetCompletedTasks () {
        int num = 0;

        

        for (int i = 0; i < tasks.Length; i++) {
            if (tasks[i].Complete)
                num++;
            
                
        }
        return num;
    }

    public float GetCompletedPercent () {
        return GetCompletedTasks () / ((float)tasks.Length);
    }

    // Update is called once per frame
    void Update(){
        int nowCompleted = GetCompletedTasks ();
        if (taskCompletionMessage != null && nowCompleted > completed) taskCompletionMessage.Play (taskCompletionMessageAnimationName, -1, 0);
        completed = nowCompleted;

        if (completed < tasks.Length) {
            string taskList = $"{taskListLabel}\n";
            string lastTaskName = tasks[0].displayName;
            int taskCount = 0;
            for (int i = 0; i < tasks.Length; i++) {
                if (tasks[i].Complete) continue;
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
