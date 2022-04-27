using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TW_GameController : MonoBehaviour
{
    public static TW_GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        Screen.SetResolution(1280, 720, true);
        //PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "treewarrior";
    }

    [SerializeField] private GameObject gameOverPanel;

    public bool isGameRunning, isSinglePlayer, isPaused;

    [SerializeField] private Text gameTimeText, countdown;
    [SerializeField] private GameObject pausedPanel, instructionText, startButton, startPanel, startObjects;
    [SerializeField] private Animator fadeAnimation;
    [SerializeField] private TMP_Text victoryText;
    public int gameTime;
    private int ambientDelay;

    public int winningPlayer;
    public string playerOneName, playerTwoName;


    private void Start()
    {
        Screen.SetResolution(1280, 720, true);

        winningPlayer = 0;
        playerOneName = "Abrar";
        playerTwoName = "Anonymous";

        isSinglePlayer = false;//PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.isSinglePlayer;

        StartCoroutine(StartGame());
    }


    private void Update()
    {
        //PlayerSession.Instance.UpdateDuration();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        //StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        TW_AudioManager.instance.PlayAudio("StartTimer");
        yield return new WaitForSecondsRealtime(2.5f);
        startObjects.SetActive(false);
        countdown.gameObject.SetActive(false);
        startPanel.SetActive(false);
        //MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.treewarrior;
        //PlayerSession.Instance.StartMPSession();
        //YipliHelper.SetGameClusterId(6,6);

        isGameRunning = true;

        isPaused = false;
        gameTime = 0;



        TW_PlayerOneController.instance.StartInstruction();
        TW_PlayerTwoController.instance.StartInstruction();
        StartCoroutine(TW_PlayerOneController.instance.TreeWindup());
        StartCoroutine(TW_PlayerTwoController.instance.TreeWindup());


        ambientDelay = Random.Range(2, 5);
        StartCoroutine(GameTimer());
        yield return new WaitForSecondsRealtime(2.5f);
        TW_AudioManager.instance.PlayAudio("Soundtrack");
    }

    public IEnumerator GameTimer()
    {
        gameTimeText.text="Time: "+ ((int)(gameTime / 60)).ToString("00") + ":" + ((int)(gameTime % 60)).ToString("00");
        gameTime++;
        yield return new WaitForSecondsRealtime(1f);
        if (ambientDelay == 0)
        {
            PlayAmbientSound();
        }
        else
        {
            ambientDelay--;
        }
        if (isGameRunning && !isPaused)
        {
            StartCoroutine(GameTimer());
        }
    }

    private void PlayAmbientSound()
    {
        int randomPicker = Random.Range(1, 8);
        switch (randomPicker)
        {
            case 1:
                TW_AudioManager.instance.PlayAudio("Ambient1");
                break;
            case 2:
                TW_AudioManager.instance.PlayAudio("Ambient2");
                break;
            case 3:
                TW_AudioManager.instance.PlayAudio("Ambient3");
                break;
            case 4:
                TW_AudioManager.instance.PlayAudio("Ambient4");
                break;
            case 5:
                TW_AudioManager.instance.PlayAudio("Ambient5");
                break;
            case 6:
                TW_AudioManager.instance.PlayAudio("Ambient6");
                break;
            case 7:
                TW_AudioManager.instance.PlayAudio("Ambient7");
                break;
        }
        ambientDelay = Random.Range(5, 10);
    }

    public IEnumerator GameOver()
    {
        isGameRunning = false;

        TW_AudioManager.instance.PlayAudio("GameOver");
        TW_AudioManager.instance.StopAudio("Soundtrack");
        TW_AudioManager.instance.PlayAudio("MenuTheme"); 
        //Time.timeScale = 0;

        switch (winningPlayer)
        {
            case 1:
                //MM_GameUIManager.instance.winnerNumber = 1;
                break;
            case 2:
                //MM_GameUIManager.instance.winnerNumber = 2;
                break;
        }


        //PlayerSession.Instance.StoreMPSession(TW_ScoreManager.instance.playerOneTime, TW_ScoreManager.instance.playerTwoTime);
        yield return new WaitForSecondsRealtime(2f);
        TW_AudioManager.instance.StopAllAudio();
        //MM_GameUIManager.instance.ShowResultsScreen();
    }

}
