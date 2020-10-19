using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGame : MonoBehaviour
{
    public PlayerObjects playerOptions;
    public Text text1, text2;

    public void GameScene()
    {
        playerOptions.P1Name = String.IsNullOrWhiteSpace(text1.text) ? "First Player" : text1.text;
        playerOptions.P2Name = String.IsNullOrWhiteSpace(text2.text) ? "Second Player" : text2.text;
        SceneManager.LoadScene("GameScene");
    }

    public void CustomizeMenu()
    {
        SceneManager.LoadScene("CustomizationMenu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
