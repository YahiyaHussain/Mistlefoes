using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    public GameObject OptionsMenu;

    public void Pause()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
    }

    public void Resume ()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Options()
    {
        //this.gameObject.SetActive(false);
        OptionsMenu.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
