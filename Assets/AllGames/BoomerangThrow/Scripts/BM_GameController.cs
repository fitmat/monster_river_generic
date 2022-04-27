using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BM_GameController : MonoBehaviour
{
    public static BM_GameController instance;

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
    public int topBallsLeft, smallBallsLeft, gameTime, player1Score, player2Score;
    public bool isBonusTimeOn;

    [SerializeField] private GameObject bonusRound, movingStacks, gamePanel, countdown, blueWinCamera, redWinCamera, blurEffect;
    [SerializeField] private TMP_Text timeText, player1ScoreText, player2ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "boomerang";
        Time.timeScale = 1.5f;
        gameState = GameStates.notStarted;
        StartCoroutine(StartGame());

        topBallsLeft = 6;
        smallBallsLeft = 25;
        gameTime = 0;
        player1Score = 0;
        player2Score = 0;

        isBonusTimeOn = false;

        //timeText.text = ((int)(gameTime / 60)).ToString("0") + ":" + ((int)(gameTime % 60)).ToString("00");
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }

    public IEnumerator StartGame()
    {
        BM_AudioManager.instance.PlayAudio("Soundtrack");
        BM_AudioManager.instance.PlayAudio("Ambient");
        BM_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        //BM_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSeconds(8f);
        Time.timeScale = 1;
        countdown.SetActive(true);
        yield return new WaitForSeconds(4.5f);

        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.boomerang;
        YipliHelper.SetGameClusterId(205,205);
        StartCoroutine(GameTimer());
        BM_AudioManager.instance.SetTrackVolume("Soundtrack", 1f);
        BM_AudioManager.instance.SetTrackVolume("Ambient", 0.5f);
        StartCoroutine(BM_InputController.instance.PlayerOneIdling());
        StartCoroutine(BM_InputController.instance.PlayerTwoIdling());
    }

    public void GameOver()
    {
        MM_GameUIManager.instance.FlashBlackScreen();
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame()
    {
        BM_AudioManager.instance.PauseAudio("Soundtrack");
        BM_AudioManager.instance.PlayAudio("End");
        gamePanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        MM_AudioManager.instance.PlayAudio("ResultCheer");
        if (player1Score > player2Score)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
            BM_LeftPlayerController.instance.Win();
            redWinCamera.SetActive(true);
            BM_RightPlayerController.instance.Lose();
        }
        else if (player1Score < player2Score)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            BM_LeftPlayerController.instance.Lose();
            blueWinCamera.SetActive(true);
            BM_RightPlayerController.instance.Win();
        }
        else
        {
            MM_GameUIManager.instance.winnerNumber = 3;
            BM_LeftPlayerController.instance.Lose();
            BM_RightPlayerController.instance.Lose();
        }
        yield return new WaitForSeconds(2f);
        //gamePanel.SetActive(false);
        PlayerSession.Instance.StoreMPSession(player1Score, player2Score);
        yield return new WaitForSeconds(2f);
        blurEffect.SetActive(true);
        MM_GameUIManager.instance.ShowResultsScreen();
        BM_AudioManager.instance.StopAllAudio();
    }

    public IEnumerator GameTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
        gameTime++;
        //timeText.text = ((int)(gameTime / 60)).ToString("00") + ":" + ((int)(gameTime % 60)).ToString("00");
        if (gameState == GameStates.playing)
        {
            StartCoroutine(GameTimer());
        }
    }

    public void changeScore(int playerNumber, int points)
    {
        if (playerNumber == 1)
        {
            player1Score += points;
            if (player1Score < 0)
            {
                player1Score = 0;
            }
            player1ScoreText.text = player1Score.ToString();
            //player1ScoreText.gameObject.GetComponent<Animator>().SetTrigger("flash");
        }
        else if (playerNumber == 2)
        {
            player2Score += points;
            if (player2Score < 0)
            {
                player2Score = 0;
            }
            player2ScoreText.text = player2Score.ToString();
            //player2ScoreText.gameObject.GetComponent<Animator>().SetTrigger("flash");
        }
    }
}

