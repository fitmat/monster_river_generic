using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FTv2_GameController : MonoBehaviour
{
    public static FTv2_GameController instance;

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
    }

    public enum GameStates { notStarted, playing, paused, gameOver }
    public GameStates gameState;

    [SerializeField] FTv2_PlayerController rightPlayer, leftPlayer;
    [SerializeField] GameObject timeObject, playerOneScoreObject, playerTwoScoreObject, playerOneScoreBackground, playerTwoScoreBackground, gamePanel, blurEffect;
    TMP_Text timeText, playerOneScoreText, playerTwoScoreText;
    public int gameTime;
    public int playerOneScore, playerTwoScore;
    private int timeLeft;

    public bool isNearlyOver;
    [SerializeField] GameObject flashingTimeText, timerBG, countdown, blueWinCamera, redWinCamera;
    public int currentState;

    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "fishtrap";
        Time.timeScale = 1;
        gameState = GameStates.notStarted;

        StartCoroutine(StartGame());

        timeText = timeObject.GetComponent<TMP_Text>();
        playerOneScoreText = playerOneScoreObject.GetComponent<TMP_Text>();
        playerTwoScoreText = playerTwoScoreObject.GetComponent<TMP_Text>();

        playerOneScore = 0;
        playerTwoScore = 0;
        isNearlyOver = false;

        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();
    }


    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        FT_AudioManager.instance.PlayAudio("Soundtrack");
        //FT_AudioManager.instance.PlayAudio("Ambient");
        FT_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        //FT_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSeconds(5f);
        countdown.SetActive(true);
        Time.timeScale = 1;
        yield return new WaitForSeconds(4.5f);
        gamePanel.SetActive(true);

        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.fishtrap;
        YipliHelper.SetGameClusterId(7, 7);

        timeLeft = gameTime;
        timeObject.SetActive(true);

        playerOneScoreBackground.SetActive(true);
        playerTwoScoreBackground.SetActive(true);

        StartCoroutine(FTv2_InputController.instance.PlayerOneIdling());
        StartCoroutine(FTv2_InputController.instance.PlayerTwoIdling());
        FTv2_FishSpawnController.instance.StartSpawningFish();

        timeObject.GetComponent<Animator>().SetTrigger("Flash");
        StartCoroutine(GameTimer());

        currentState = 1;

        FT_AudioManager.instance.SetTrackVolume("Soundtrack", 1f);
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            gameState = GameStates.gameOver;
            FT_AudioManager.instance.PlayAudio("End");
            FT_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        timeObject.SetActive(false);

        gameState = GameStates.gameOver;
        yield return new WaitForSeconds(0.5f);
        gamePanel.SetActive(false);
        MM_AudioManager.instance.PlayAudio("ResultCheer");

        if (playerOneScore > playerTwoScore)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
            redWinCamera.SetActive(true);
            leftPlayer.WinAnimation();
            rightPlayer.LoseAnimation();
        }
        else if (playerOneScore < playerTwoScore)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            blueWinCamera.SetActive(true);
            rightPlayer.WinAnimation();
            leftPlayer.LoseAnimation();
        }
        else
        {
            MM_GameUIManager.instance.winnerNumber = 3;
        }

        yield return new WaitForSecondsRealtime(2f);
        PlayerSession.Instance.StoreMPSession(playerOneScore, playerTwoScore);
        yield return new WaitForSecondsRealtime(2f);
        blurEffect.SetActive(true);
        FT_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }


    public IEnumerator GameTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
        timeLeft--;
        timeText.text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
        flashingTimeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
        if (timeLeft % 60 == 0)
        {
            timeObject.GetComponent<Animator>().SetTrigger("Flash");
        }
        if (timeLeft == 0)
        {
            MM_AudioManager.instance.StopAudio("Ticking");
            GameOver();
        }
        else
        {
            // Play ticking clock sound when time is nearly over
            if (timeLeft == 10)
            {
                NearlyOver();
            }
            StartCoroutine(GameTimer());
        }
    }

    public void NearlyOver()
    {
        if (!isNearlyOver)
        {
            isNearlyOver = true;
            MM_AudioManager.instance.PlayAudio("Ticking");
            timerBG.SetActive(true);
            flashingTimeText.SetActive(true);
        }
    }

    public void ChangeScore(int playerNumber,int scoreChange)
    {
        if (playerNumber == 1)
        {
            playerOneScore += scoreChange;
            if (playerOneScore < 0)
            {
                playerOneScore = 0;
            }
            playerOneScoreText.text = playerOneScore.ToString();
            playerOneScoreBackground.GetComponent<Animator>().SetTrigger("Flash");
        }
        if (playerNumber == 2)
        {
            playerTwoScore += scoreChange;
            if (playerTwoScore < 0)
            {
                playerTwoScore = 0;
            }
            playerTwoScoreText.text = playerTwoScore.ToString();
            playerTwoScoreBackground.GetComponent<Animator>().SetTrigger("Flash");
        }
    }

}
