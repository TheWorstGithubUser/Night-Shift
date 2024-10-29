using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeRemaining = 300;
    public bool timerOn = true;
    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerOn = true;
    }


    // Update is called once per frame
     private void Update()
    {
        if(timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime (float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
