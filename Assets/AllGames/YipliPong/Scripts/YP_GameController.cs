using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YP_GameController : MonoBehaviour
{
    public static YP_GameController instance;

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

    public int playerOneScore, playerTwoScore;
    private float direction;
    [SerializeField] private TMP_Text playerOneScoreText, playerTwoScoreText;

    [SerializeField] private GameObject ball, playerOnePaddle, playerTwoPaddle, countdown, gamePanel, blurEffect;

    [SerializeField] private ParticleSystem leftGoalParticles, rightGoalParticles, gameOverParticles;

    private Vector3 ballStartPosition, playerOnePaddleStartPosition, playerTwoPaddleStartPosition;


    public enum GameStates { notStarted, playing, paused, gameOver }
    public GameStates gameState;
    public int winningPlayer;


    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "pingpong";
        Time.timeScale = 1;
        gameState = GameStates.notStarted;

        

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        yield return new WaitForSeconds(1f);
        YP_AudioManager.instance.PlayAudio("Start");
        countdown.SetActive(true);
        yield return new WaitForSecondsRealtime(4.5f);
        Time.timeScale = 1;
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.yiplipong;
        YipliHelper.SetGameClusterId(7, 7);
        SetBoard();
        StartCoroutine(YP_InputController.instance.PlayerOneIdling());
        StartCoroutine(YP_InputController.instance.PlayerTwoIdling());
        YP_BallController.instance.ThrowBall();
        YP_AudioManager.instance.PlayAudio("Soundtrack");
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            gameState = GameStates.gameOver;
            rightGoalParticles.Play();
            leftGoalParticles.Play();
            gameOverParticles.Play();
            YP_AudioManager.instance.PlayAudio("GameOver");
            YP_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        gamePanel.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);
        PlayerSession.Instance.StoreMPSession(playerOneScore, playerTwoScore);
        yield return new WaitForSecondsRealtime(2f);
        YP_AudioManager.instance.StopAllAudio();
        blurEffect.SetActive(true);
        MM_GameUIManager.instance.ShowResultsScreen();
    }


    public void IncreasePlayerOneScore()
    {
        playerOneScore++;
        playerOneScoreText.text = playerOneScore.ToString();
        if (playerOneScore == 10)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
            GameOver();
            return;
        }
        YP_BallController.instance.direction = -1;
        StartCoroutine(ResetBoard());
    }
    public void IncreasePlayerTwoScore()
    {
        playerTwoScore++;
        playerTwoScoreText.text = playerTwoScore.ToString();
        if (playerTwoScore == 10)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            GameOver();
            return;
        }
        YP_BallController.instance.direction = 1;
        StartCoroutine(ResetBoard());
    }

    public void SetBoard()
    {
        playerOneScore = 0;
        playerTwoScore = 0;

        ballStartPosition = ball.transform.position;
        playerOnePaddleStartPosition = playerOnePaddle.transform.position;
        playerTwoPaddleStartPosition = playerTwoPaddle.transform.position;


        direction = Random.Range(-1f, 1f);
        if (direction < 0)
        {
            YP_BallController.instance.direction = -1;
        }
        else
        {
            YP_BallController.instance.direction = 1;
        }
    }

    public IEnumerator ResetBoard()
    {
        ball.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        ball.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        ball.transform.position = ballStartPosition;
        playerOnePaddle.transform.position = playerOnePaddleStartPosition;
        playerTwoPaddle.transform.position = playerTwoPaddleStartPosition;
        yield return new WaitForSeconds(0.5f);
        ball.transform.GetChild(0).gameObject.SetActive(true);
        YP_BallController.instance.ThrowBall();
    }

}
