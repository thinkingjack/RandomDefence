using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;

    public SceneFader sceneFader;

    public void Pause()
    {
        ui.SetActive(!ui.activeSelf);

        if(ui.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Pause();
        FindObjectOfType<SceneFader>().FadeTo("MainGame");
    }
    public void Continue()
    {
        Pause();
    }
    public void Menu()
    {
        Pause();
        SceneManager.LoadScene("Menu");
    }
}
