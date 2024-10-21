using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneFader sceneFader;

    public void Play()
    {
        FindObjectOfType<SceneFader>().FadeTo("Level");
    }
    public void Game()
    {
        FindObjectOfType<SceneFader>().FadeTo("MainGame");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
