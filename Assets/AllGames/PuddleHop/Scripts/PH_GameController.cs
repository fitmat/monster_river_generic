using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PH_GameController : MonoBehaviour
{

    public static PH_GameController instance;

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
    public float speedMultiplier;
    public int winningPlayer;

    [SerializeField] private GameObject startPanel, gamePanel, pausePanel, gameOverPanel;
    [SerializeField] PH_PlayerController playerOne, playerTwo;

    public int gameTime;
    private int timeLeft;

    [SerializeField]
    private TMP_Text timeText;
    public bool isNearlyOver;
    [SerializeField] GameObject flashingTimeText, timerBG, countdown, playerOneActual, playerTwoActual, closingCinematic, redWon, blueWon, blurEffect;
    [SerializeField] Transform redEndTransform, blueEndTransform;




    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "icehopper";
        Time.timeScale = 2f;
        gameState = GameStates.notStarted;
        isNearlyOver = false;
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }
    public IEnumerator GameTimer()
    {
        if (gameState == GameStates.playing)
        {
            yield return new WaitForSecondsRealtime(1f);
            timeLeft--;
            timeText.text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
            flashingTimeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
            if (timeLeft == 0 && gameState == GameStates.playing)
            {
                MM_GameUIManager.instance.winnerNumber = 3;
                MM_AudioManager.instance.StopAudio("Ticking");
                GameOver();
            }
            else
            {
                if (timeLeft == 10)
                {
                    NearlyOver();
                }
                StartCoroutine(GameTimer());
            }
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
            timeText.gameObject.SetActive(false);
        }
    }


    public IEnumerator StartGame()
    {
        PH_AudioManager.instance.PlayAudio("Soundtrack");
        //PH_AudioManager.instance.PlayAudio("Ambient");
        PH_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        //PH_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSeconds(4f);
        countdown.SetActive(true);
        Time.timeScale = 1;
        yield return new WaitForSeconds(4.5f);
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.puddlehop;
        YipliHelper.SetGameClusterId(205,205);
        timeLeft = gameTime;
        StartCoroutine(GameTimer());
        PH_AudioManager.instance.SetTrackVolume("Soundtrack", 1f);
        StartCoroutine(PH_InputManager.instance.PlayerOneIdling());
        StartCoroutine(PH_InputManager.instance.PlayerTwoIdling());
    }

    public void GameOver()
    {
        MM_GameUIManager.instance.FlashBlackScreen();
        PH_AudioManager.instance.PlayAudio("GameOver");
        PH_AudioManager.instance.StopAudio("Soundtrack");
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame()
    {
        gamePanel.SetActive(false);
        gameState = GameStates.gameOver;
        yield return new WaitForSeconds(0.5f);
        playerOneActual.transform.position = redEndTransform.position;
        playerTwoActual.transform.position = blueEndTransform.position;
        closingCinematic.SetActive(true);
        MM_AudioManager.instance.PlayAudio("ResultCheer");
        gameState = GameStates.gameOver;
        if (MM_GameUIManager.instance.winnerNumber == 1)
        {
            redWon.SetActive(true);
            playerTwo.Lose();
        }
        if (MM_GameUIManager.instance.winnerNumber == 2)
        {
            blueWon.SetActive(true);
            playerOne.Lose();
        }
        if (MM_GameUIManager.instance.winnerNumber == 3)
        {
            playerTwo.Lose();
            playerOne.Lose();
        }
        yield return new WaitForSecondsRealtime(1f);
        gamePanel.SetActive(false);
        Camera.main.orthographic = false;
        yield return new WaitForSecondsRealtime(1f);
        PlayerSession.Instance.StoreMPSession(1, 1);
        yield return new WaitForSecondsRealtime(3f);
        blurEffect.SetActive(true);
        PH_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }

}
