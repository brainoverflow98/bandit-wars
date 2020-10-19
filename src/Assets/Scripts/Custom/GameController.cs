using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public float phase = 0;

    public static Globals.Player whoseTurn;
    private bool battleTurn;
    private float battleTime, calculateTime;

    public double time;
    public int turnNo = 1;
    

    [HideInInspector]
    public Globals.Player offensive, defensive;

    private ControlPanel controlPanel;
    public MouseManager mouseManager;
    public PauseMenu pauseMenu;

    private GameObject[] unitList;

    private AudioSource audioSource;
    public AudioClip themeMusicClip, battleMarchClip, celebrationClip;

    // Start is called before the first frame update
    void Start()
    {
        controlPanel = GameObject.Find("ControlPanel").GetComponent<ControlPanel>();
        offensive = Globals.Player.One;
        defensive = Globals.Player.Two;
        whoseTurn = offensive;

        calculateTime = Time.time + 90.0f;
        time = calculateTime - Time.time;

        battleTime = Time.time;

        unitList = GameObject.FindGameObjectsWithTag("Unit");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (battleTurn)
        {
            if (phase < 9.8)
            {
                var dif = Time.time - battleTime;
                if (dif > 0.5)
                {
                    phase += 0.5f;
                    battleTime = Time.time;
                }
            }
            else
            {
                phase = 0;
                turnNo += 1;
                battleTurn = false;
                var temp = defensive;
                defensive = offensive;
                offensive = temp;
                whoseTurn = offensive;

                calculateTime = Time.time + 90.0f;
                time = calculateTime - Time.time;

                foreach (var unit in unitList)
                {
                    unit.GetComponent<Unit>().ResetTurnVariables();
                }

                controlPanel.ShowScrollPanel();

                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = themeMusicClip;
                audioSource.volume = pauseMenu.volumeSlider.value;
                audioSource.Play();
            }
        }
        else
        {
            time = calculateTime - Time.time;
            if (time <= 0.0)
            {
                EndPlayersTurn();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
        }
    }

    public void EndPlayersTurn()
    {
        mouseManager.UnselectUnits();
        mouseManager.UnmarkTiles();
        if (whoseTurn == offensive)
        {
            calculateTime = Time.time + 90.0f;
            time = calculateTime - Time.time;
            whoseTurn = defensive;

            controlPanel.ShowScrollPanel();

        }            
        else
        {
            battleTurn = true;
            controlPanel.BattleTurnPanel();

            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = battleMarchClip;
            audioSource.volume = pauseMenu.volumeSlider.value;
            audioSource.Play();
        }
            
    }

    public void GameEnd(Globals.Player loser)
    {
        string winnerName = "";
        if (loser == Globals.Player.One)
            winnerName = controlPanel.P2Name;
        else
            winnerName = controlPanel.P1Name;

        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        pauseMenu.exitButton.SetActive(false);
        pauseMenu.menuText.GetComponent<TextMeshProUGUI>().text = winnerName + " WON THE GAME";

        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = celebrationClip;
        audioSource.volume = pauseMenu.volumeSlider.value;
        audioSource.Play();
    }

}
