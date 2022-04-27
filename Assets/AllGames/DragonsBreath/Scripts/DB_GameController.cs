using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_GameController : MonoBehaviour
{
    public static DB_GameController instance;

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

    [SerializeField] DB_PlayerController playerOneController, playerTwoController;

    public enum GameStates { notStarted, playing, paused, gameOver }
    public GameStates gameState;
    public int winningPlayer;
    [SerializeField] GameObject gamePanel, countdown, cinematicArea, gameArea, blurEffect;


    private void Start()
    {
        Time.timeScale = 1.5f;
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "dragonbreath";
        gameState = GameStates.notStarted;
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }

    //public IEnumerator GameTimer()
    //{
    //    if (gameState == GameStates.playing)
    //    {
    //        timeText.text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
    //        yield return new WaitForSecondsRealtime(1f);
    //        timeLeft--;
    //        if (timeLeft == 0)
    //        {
    //            MM_GameUIManager.instance.winnerNumber = 3;
    //            GameOver();
    //        }
    //        else
    //        {
    //            StartCoroutine(GameTimer());
    //        }
    //    }
    //}

    public IEnumerator IncreaseGameSpeed()
    {
        yield return new WaitForSecondsRealtime(45f);
        Time.timeScale += 0.015f;
        StartCoroutine(IncreaseGameSpeed());
    }


    public IEnumerator StartGame()
    {
        DB_AudioManager.instance.PlayAudio("Soundtrack");
        //DB_AudioManager.instance.PlayAudio("Ambient");
        DB_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        //DB_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSeconds(8f);
        Time.timeScale = 1;
        countdown.SetActive(true);
        cinematicArea.SetActive(false);
        gameArea.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        gamePanel.SetActive(true);
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.dragonbreath;
        YipliHelper.SetGameClusterId(205, 205);
        //timeLeft = gameTime;
        //StartCoroutine(GameTimer());
        StartCoroutine(IncreaseGameSpeed());
        DB_AudioManager.instance.SetTrackVolume("Soundtrack", 1f);
        //DB_AudioManager.instance.SetTrackVolume("Ambient", 0.5f);
        StartCoroutine(DB_InputController.instance.PlayerOneIdling());
        StartCoroutine(DB_InputController.instance.PlayerTwoIdling());
        StartCoroutine(DB_DragonController.instance.StartDragons());
    }

    public IEnumerator GameOver()
    {
        MM_GameUIManager.instance.FlashBlackScreen();
        gameState = GameStates.gameOver;
        gamePanel.SetActive(false);
        DB_AudioManager.instance.PlayAudio("GameOver");
        DB_AudioManager.instance.StopAudio("Soundtrack");
        yield return new WaitForSeconds(0.5f);
        playerOneController.Win();
        playerTwoController.Win();
        yield return new WaitForSeconds(0.25f);
        {
            if (!playerOneController.hasLost && playerTwoController.hasLost)
            {
                MM_GameUIManager.instance.winnerNumber = 1;
            }
            else if (playerOneController.hasLost && !playerTwoController.hasLost)
            {
                MM_GameUIManager.instance.winnerNumber = 2;
            }
            else
            {
                MM_GameUIManager.instance.winnerNumber = 3;
            }
        }
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        PlayerSession.Instance.StoreMPSession(1, 1);
        yield return new WaitForSecondsRealtime(2f);
        DB_AudioManager.instance.StopAllAudio();
        blurEffect.SetActive(true);
        MM_GameUIManager.instance.ShowResultsScreen();
    }

    public void PlayerJumpSuccess(int playerNumber)
    {
        if (playerNumber == 1)
        {
            playerOneController.successfulJumps++;
        }
        else if (playerNumber == 2)
        {
            playerTwoController.successfulJumps++;
        }
    }
    public void PlayerJumpFail(int playerNumber)
    {
        if (playerNumber == 1)
        {
            StartCoroutine(playerOneController.GetPushed());
        }
        else if (playerNumber == 2)
        {
            StartCoroutine(playerTwoController.GetPushed());
        }
    }
}
