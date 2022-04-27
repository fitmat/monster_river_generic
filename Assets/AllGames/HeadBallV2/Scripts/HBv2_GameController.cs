using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HBv2_GameController : MonoBehaviour
{
    public static HBv2_GameController instance;

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

    public int playerOneScore, playerTwoScore, gameTime;
    private int timeLeft;

    [SerializeField] GameObject timeObject, playerOneScoreObject, playerTwoScoreObject, playerOneScoreBackground, playerTwoScoreBackground, gamePanel, blurEffect;
    private TMP_Text playerOneScoreText, playerTwoScoreText, timeText;
    [SerializeField] private GameObject missText, goalText;
    [SerializeField] private HBv2_PlayerController playerOne, playerTwo;
    [SerializeField] Animator pointsAnimation;

    [SerializeField] GameObject countdown, playerOneStart, playerTwoStart, playerOneActual, playerTwoActual, closingCinematic, redWon, blueWon;
    [SerializeField] Transform redEndTransform, blueEndTransform;

    public enum GameStates { notStarted, playing, paused, gameOver }
    public GameStates gameState;
    public int winningPlayer;

    public bool isNearlyOver;
    [SerializeField] GameObject flashingTimeText, timerBG;


    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "beachball";
        Time.timeScale = 1.25f;
        gameState = GameStates.notStarted;

        StartCoroutine(StartGame());

        timeText = timeObject.GetComponent<TMP_Text>();
        playerOneScoreText = playerOneScoreObject.GetComponent<TMP_Text>();
        playerTwoScoreText = playerTwoScoreObject.GetComponent<TMP_Text>();

        isNearlyOver = false;
        playerOneScore = 0;
        playerTwoScore = 0;

        playerOneScoreText.text = playerOneScore.ToString();
        playerTwoScoreText.text = playerTwoScore.ToString();
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }


    public IEnumerator StartGame()
    {
        HB_AudioManager.instance.PlayAudio("Soundtrack");
        HB_AudioManager.instance.PlayAudio("Ambient");
        HB_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        yield return new WaitForSeconds(0.5f);
        
        yield return new WaitForSeconds(10f);
        Time.timeScale = 1;
        countdown.SetActive(true);
        playerOneStart.SetActive(false);
        playerTwoStart.SetActive(false);
        playerOneActual.SetActive(true);
        playerTwoActual.SetActive(true);
        StartCoroutine(HBv2_BallController.instance.ResetBall(0));
        //HB_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSeconds(4.5f);

        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.headball;
        YipliHelper.SetGameClusterId(6, 6);
        timeLeft = gameTime;
        timeObject.SetActive(true);

        playerOneScoreBackground.SetActive(true);
        playerTwoScoreBackground.SetActive(true);

        timeObject.GetComponent<Animator>().SetTrigger("Flash");
        StartCoroutine(GameTimer());
        StartGameplay();
        HB_AudioManager.instance.PlayAudio("StartWhistle");
        HB_AudioManager.instance.SetTrackVolume("Soundtrack", 1f);
        HB_AudioManager.instance.SetTrackVolume("Ambient", 0.5f);
        StartCoroutine(HBv2_InputController.instance.PlayerOneIdling());
        StartCoroutine(HBv2_InputController.instance.PlayerTwoIdling());
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            HB_AudioManager.instance.PlayAudio("GameOver");
            HB_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        timeObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        playerOneActual.transform.position = redEndTransform.position;
        playerOneActual.transform.rotation = redEndTransform.rotation;
        playerTwoActual.transform.position = blueEndTransform.position;
        playerTwoActual.transform.rotation = blueEndTransform.rotation;
        closingCinematic.SetActive(true);
        MM_AudioManager.instance.PlayAudio("ResultCheer");

        gameState = GameStates.gameOver;

        if (playerOneScore > playerTwoScore)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
            redWon.SetActive(true);
            playerOne.Win();
            playerTwo.Lose();
        }
        else if (playerOneScore < playerTwoScore)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            blueWon.SetActive(true);
            playerOne.Lose();
            playerTwo.Win();
        }
        else
        {
            MM_GameUIManager.instance.winnerNumber = 3;
        }
        yield return new WaitForSecondsRealtime(2f);
        gamePanel.SetActive(false);
        PlayerSession.Instance.StoreMPSession(playerOneScore, playerTwoScore);
        yield return new WaitForSecondsRealtime(2f);
        HB_AudioManager.instance.StopAllAudio();
        blurEffect.SetActive(true);
        MM_GameUIManager.instance.ShowResultsScreen();
    }


    // Start is called before the first frame update
    private void StartGameplay()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
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
            timeText.gameObject.SetActive(false);
        }
    }

    public IEnumerator ScoreGoal(int scoringPlayer)
    {
        HB_AudioManager.instance.PlayAudio("GoalWhistle");
        if (scoringPlayer == 1)
        {
            playerOneScore++;
            playerOneScoreText.text = playerOneScore.ToString();
            playerOneScoreBackground.GetComponent<Animator>().SetTrigger("Flash");
            playerOne.Happy();
            pointsAnimation.SetTrigger("Left");
            playerTwo.Sad();
        }
        else if (scoringPlayer == 2)
        {
            playerTwoScore++;
            playerTwoScoreText.text = playerTwoScore.ToString();
            playerTwoScoreBackground.GetComponent<Animator>().SetTrigger("Flash");
            playerTwo.Happy();
            pointsAnimation.SetTrigger("Right");
            playerOne.Sad();
        }



        StartCoroutine(playerOne.ResetPosition());
        StartCoroutine(playerTwo.ResetPosition());
        goalText.SetActive(true);
        yield return new WaitForSeconds(3f);
        goalText.SetActive(false);
    }

    public IEnumerator MissGoal()
    {
        StartCoroutine(playerOne.ResetPosition());
        StartCoroutine(playerTwo.ResetPosition());
        StartCoroutine(HBv2_BallController.instance.ResetBall(0));
        missText.SetActive(true);
        yield return new WaitForSeconds(3f);
        missText.SetActive(false);

    }

}