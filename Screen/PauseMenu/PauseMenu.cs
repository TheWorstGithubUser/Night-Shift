using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject mainPauseUI;

    public GameObject optionsPauseUI;

    [SerializeField] private GameInput gameInput;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        mainPauseUI.SetActive(true);
        optionsPauseUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        GameIsPaused = false;
        gameInput.OnDisable();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
