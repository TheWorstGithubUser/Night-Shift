using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //[SerializeField] Button newGame;
    //[SerializeField] Button continueGame;
    //[SerializeField] Button options;
    //[SerializeField] Button Quit;


    public void StartNewGame()
    {
        Debug.Log("You have clicked the button!");
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //LoadScene(1);
    }

    public void Continue()
    {
        Debug.Log("You have clicked the button!");

    }

    

    public void QuitGame()
    {
        Application.Quit();

        //to close play mode for debugging
        // NOTE: Make sure you comment it out when you build and run the project
        UnityEditor.EditorApplication.isPlaying = false;

        Debug.Log("You have clicked the button!");
    }

    void LoadScene(int sceneIndex)
    {
        Debug.Log("Loading...");
        SceneManager.LoadScene(sceneIndex);
    }
}
