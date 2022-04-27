using System.Collections;
using TMPro;
using UnityEngine;

public class EC_GameController : MonoBehaviour
{
    public static EC_GameController instance;

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
    public int ballsLeft, gameTime, player1Score, player2Score;
    private int timeLeft;

    [SerializeField] private TMP_Text timeText, player1ScoreText, player2ScoreText;
    [SerializeField] GameObject gamePanel, blurEffect;

    public bool isNearlyOver;
    [SerializeField] GameObject flashingTimeText, timerBG;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "eggcatcher";
        Time.timeScale = 1;
        gameState = GameStates.notStarted;
        //PH_AudioManager.instance.PlayAudio("Start");
        StartCoroutine(StartGame());

        isNearlyOver = false;
        player1Score = 0;
        player2Score = 0;

        //timeText.text = ((int)(gameTime / 60)).ToString("0") + ":" + ((int)(gameTime % 60)).ToString("00");
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }
    public IEnumerator GameTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
        timeLeft--;
        timeText.text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
        flashingTimeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
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
            timeText.gameObject.SetActive(false);
            timerBG.SetActive(true);
            flashingTimeText.SetActive(true);
        }
    }


    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        EC_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(4.5f);

        Time.timeScale = 1;
        timeLeft = gameTime;
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.eggcatcher;
        YipliHelper.SetGameClusterId(7,7);
        StartCoroutine(GameTimer());
        EC_AudioManager.instance.PlayAudio("Soundtrack");
        EC_AudioManager.instance.PlayAudio("ChickenSound");
        StartCoroutine(EC_InputController.instance.PlayerOneIdling());
        StartCoroutine(EC_InputController.instance.PlayerTwoIdling());
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            gameState = GameStates.gameOver;
            EC_AudioManager.instance.PlayAudio("Victory");
            EC_AudioManager.instance.StopAudio("Soundtrack");
            EC_AudioManager.instance.StopAudio("ChickenSound");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        gamePanel.SetActive(false);
        PlayerSession.Instance.StoreMPSession(player1Score, player2Score);
        if (player1Score > player2Score)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
        }
        else if (player1Score < player2Score)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
        }
        else
        {
            MM_GameUIManager.instance.winnerNumber = 3;
        }
        yield return new WaitForSeconds(2f);
        blurEffect.SetActive(true);
        EC_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }

    public void changeScore(int playerNumber, int points)
    {
        if (gameState == GameStates.playing)
        {
            if (playerNumber == 1)
            {
                player1Score += points;
                if (player1Score < 0)
                {
                    player1Score = 0;
                }
                player1ScoreText.text = player1Score.ToString();
            }
            else if (playerNumber == 2)
            {
                player2Score += points;
                if (player2Score < 0)
                {
                    player2Score = 0;
                }
                player2ScoreText.text = player2Score.ToString();
            }
        }
    }
}

