using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HB_GameController : MonoBehaviour
{
    public static HB_GameController instance;

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

    [SerializeField] private Animator playerOneAnimator, playerTwoAnimator;
    [SerializeField] private HB_LightingController lighting;
    [SerializeField] private TMP_Text playerOneScoreText, playerTwoScoreText;
    [SerializeField] private GameObject missText;
    [SerializeField] private HB_PlayerController playerOne, playerTwo;

    public enum GameStates { notStarted, playing, paused, gameOver }
    private GameStates gameState;
    public int winningPlayer;


    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "headball";
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
        //PB_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.headball;
        YipliHelper.SetGameClusterId(6, 6);
        StartGameplay();
        yield return new WaitForSecondsRealtime(2.5f);
        //PB_AudioManager.instance.PlayAudio("Soundtrack");
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            //PB_AudioManager.instance.PlayAudio("GameOver");
            //PB_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        PlayerSession.Instance.StoreMPSession(playerOneScore, playerTwoScore);
        yield return new WaitForSecondsRealtime(2f);
        //PB_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }


    // Start is called before the first frame update
    private void StartGameplay()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
        StartCoroutine(lighting.DayCycle());
    }

    public IEnumerator ScoreGoal(int scoringPlayer)
    {
        if (scoringPlayer == 1)
        {
            playerOneScore++;
            playerOneScoreText.text = playerOneScore.ToString();
            playerOneAnimator.SetTrigger("Goal");
            playerTwoAnimator.SetTrigger("Sad");

            if (playerOneScore == 10)
            {
                MM_GameUIManager.instance.winnerNumber = 1;
                GameOver();
                yield return new WaitForSeconds(1.5f);
                playerOneAnimator.SetTrigger("Win");
                playerTwoAnimator.SetTrigger("Lose");
            }

        }
        else if (scoringPlayer == 2)
        {
            playerTwoScore++;
            playerTwoScoreText.text = playerTwoScore.ToString();
            playerTwoAnimator.SetTrigger("Goal");
            playerOneAnimator.SetTrigger("Sad");
        }

        if (playerTwoScore == 10)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            GameOver();
            yield return new WaitForSeconds(1.5f);
            playerTwoAnimator.SetTrigger("Win");
            playerOneAnimator.SetTrigger("Lose");
        }


        StartCoroutine(playerOne.ResetPosition());
        StartCoroutine(playerTwo.ResetPosition());
        StartCoroutine(HB_BallController.instance.ResetBall());
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator MissGoal()
    {
        StartCoroutine(playerOne.ResetPosition());
        StartCoroutine(playerTwo.ResetPosition());
        StartCoroutine(HB_BallController.instance.ResetBall());
        missText.SetActive(true);
        yield return new WaitForSeconds(2f);
        missText.SetActive(false);

    }

}