using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;


    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }


    public void Restart()
    {
        Time.timeScale = 1;
        GameManager.instance.RestartGame();
    }

    private void Update()
    {
        Pause();
    }

}