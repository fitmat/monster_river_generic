using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FT_GameController : MonoBehaviour
{
    public static FT_GameController instance;

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
    public int player1Score, player2Score, targetScore;

    [SerializeField] private TMP_Text player1ScoreText, player2ScoreText, targetText;
    [SerializeField] private GameObject playerOneCelebration, playerTwoCelebration;
    [SerializeField] private GameObject playerOne, playerTwo;

    void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "fishtrap";
        Time.timeScale = 1;
        gameState = GameStates.notStarted;
        StartCoroutine(StartGame());

        player1Score = 0;
        player2Score = 0;

        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }


    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        FT_AudioManager.instance.PlayAudio("Start");
        targetText.text = "Target:" + targetScore.ToString();
        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1;
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.fishtrap;
        YipliHelper.SetGameClusterId(7, 7);
        yield return new WaitForSecondsRealtime(2.5f);
        FT_AudioManager.instance.PlayAudio("Soundtrack");
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            playerOne.transform.GetChild(0).gameObject.SetActive(false);
            playerOne.transform.GetChild(1).gameObject.SetActive(true);
            playerTwo.transform.GetChild(0).gameObject.SetActive(false);
            playerTwo.transform.GetChild(1).gameObject.SetActive(true);

            if (player1Score > player2Score)
            {
                MM_GameUIManager.instance.winnerNumber = 1;
                playerOneCelebration.SetActive(true);

                playerOne.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Won");
                playerTwo.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Lost");
            }
            else if (player1Score < player2Score)
            {
                MM_GameUIManager.instance.winnerNumber = 2;
                playerTwoCelebration.SetActive(true);

                playerOne.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Lost");
                playerTwo.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Won");
            }
            else
            {
                playerOneCelebration.SetActive(true);
                playerTwoCelebration.SetActive(true);
                playerOne.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Won");
                playerTwo.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Won");
                MM_GameUIManager.instance.winnerNumber = 3;
            }

            gameState = GameStates.gameOver;
            FT_AudioManager.instance.PlayAudio("End");
            FT_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        gameState = GameStates.gameOver;
        yield return new WaitForSecondsRealtime(2f);
        PlayerSession.Instance.StoreMPSession(player1Score, player2Score);
        yield return new WaitForSecondsRealtime(2f);
        FT_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }

    public void ChangeScore(int playerNumber, int points)
    {
        if (playerNumber == 1)
        {
            player1Score += points;
            if (player1Score < 0)
            {
                player1Score = 0;
            }
            player1ScoreText.gameObject.GetComponent<Animator>().SetTrigger("flash");
            player1ScoreText.text = player1Score.ToString();
        }
        else if (playerNumber == 2)
        {
            player2Score += points;
            if (player2Score < 0)
            {
                player2Score = 0;
            }
            player2ScoreText.gameObject.GetComponent<Animator>().SetTrigger("flash");
            player2ScoreText.text = player2Score.ToString();
        }
        if (player1Score >= targetScore || player2Score >= targetScore)
        {
            GameOver();
        }
    }
}
