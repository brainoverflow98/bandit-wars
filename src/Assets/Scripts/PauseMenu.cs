using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameController gameController;
    public Slider volumeSlider;
    public GameObject exitButton, menuText;

    public void CloseMenu()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("CustomizationMenu");
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeVolume(float vol)
    {
        gameController.GetComponent<AudioSource>().volume = vol;
    }
}
