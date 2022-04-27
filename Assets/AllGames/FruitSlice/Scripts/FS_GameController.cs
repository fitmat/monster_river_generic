using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FS_GameController : MonoBehaviour
{
    public static FS_GameController instance;

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
    public int gameLength, timeLeft, player1Score, player2Score;

    public string player1Name, player2Name;
    [SerializeField] GameObject timeObject, playerOneScoreObject, playerTwoScoreObject, playerOneScoreBackground, playerTwoScoreBackground;
    TMP_Text timeText, playerOneScoreText, playerTwoScoreText;
    [SerializeField] private GameObject p1FloatPoints, p2FloatPoints, gamePanel, plateObject, rightPlayer, leftPlayer;

    public bool isNearlyOver;
    [SerializeField] GameObject flashingTimeText, timerBG,blurEffect;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "fruitblast";
        Time.timeScale = 1;
        gameState = GameStates.notStarted;
        StartCoroutine(StartGame());

        timeLeft = gameLength;
        isNearlyOver = false;


        timeText = timeObject.GetComponent<TMP_Text>();
        playerOneScoreText = playerOneScoreObject.GetComponent<TMP_Text>();
        playerTwoScoreText = playerTwoScoreObject.GetComponent<TMP_Text>();

        player1Score = 0;
        player2Score = 0;
        timeText.text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
        flashingTimeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");

        playerOneScoreText.text = player1Score.ToString();
        playerTwoScoreText.text = player2Score.ToString();
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }


    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        FS_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(4.5f);

        Time.timeScale = 1;
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.fruitslice;
        YipliHelper.SetGameClusterId(205,205);
        StartCoroutine(GameTimer());
        FS_AudioManager.instance.PlayAudio("Soundtrack");
        StartCoroutine(FS_InputController.instance.PlayerOneIdling());
        StartCoroutine(FS_InputController.instance.PlayerTwoIdling());
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            gameState = GameStates.gameOver;
            FS_AudioManager.instance.PlayAudio("End");
            FS_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        gameState = GameStates.gameOver;
        gamePanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        plateObject.AddComponent<Rigidbody>();
        rightPlayer.GetComponent<Rigidbody>().isKinematic = false;
        leftPlayer.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSecondsRealtime(2f);
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
        yield return new WaitForSecondsRealtime(2f);
        blurEffect.SetActive(true);
        FS_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }

    public IEnumerator GameTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
        timeLeft--;
        timeText.text = ((int)(timeLeft / 60)).ToString("00") + ":" + ((int)(timeLeft % 60)).ToString("00");
        flashingTimeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
        if (timeLeft % 30 == 0)
        {
            Time.timeScale += 0.2f;
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

    public IEnumerator ChangeScore(int playerNumber,int points)
    {
        if (playerNumber == 1)
        {
            player1Score += points;
            if (player1Score < 0)
            {
                player1Score = 0;
            }
            playerOneScoreText.text = player1Score.ToString();
            playerOneScoreBackground.GetComponent<Animator>().SetTrigger("Flash");
        }
        else if (playerNumber == 2)
        {
            player2Score += points;
            if (player2Score < 0)
            {
                player2Score = 0;
            }
            playerTwoScoreText.text = player2Score.ToString();
            playerTwoScoreBackground.GetComponent<Animator>().SetTrigger("Flash");
        }
        yield return null;
    }

    public IEnumerator CameraShake()
    {
        Vector3 originalPosition = Camera.main.transform.position;
        float elapsed = 0.0f;
        float duration = 0.25f, magnitude = 0.1f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            Camera.main.transform.localPosition = new Vector3(x, originalPosition.y - y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.localPosition = originalPosition;
    }

}
