using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public SceneFader sceneFader;

    public void Retry()
    {
        GameManager.gameEnded = false; // 게임 상태 초기화
        PlayerStats.Lives = 10;       // 목숨 초기화
        Time.timeScale = 1;
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Quit1()
    {
        Application.Quit();
    }
}
