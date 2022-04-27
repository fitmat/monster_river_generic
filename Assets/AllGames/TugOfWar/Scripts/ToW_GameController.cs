using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToW_GameController : MonoBehaviour
{

    public static ToW_GameController instance;

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
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "tugofwar";
    }

    [SerializeField] private GameObject startPanel, gamePanel, gamePausePanel;
    [SerializeField] private GameObject nextRoundPanel;


    [SerializeField] private TMP_Text advantageText;
    [SerializeField] private TMP_Text timerText;


    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text nextInstructionText, roundInstructionText, roundActionText;
    [SerializeField] private Image[] playerOneStars;
    [SerializeField] private Image[] playerTwoStars;


    [SerializeField] private TMP_Text playerOneName, playerTwoName;
    [SerializeField] GameObject counterText;

    public bool isTimeComplete, isGamePaused, isGameOver, isGameRunning;
    public int winner;
    public int gameTime, roundDuration, remainingTime;

    public int[] roundWinner = { 0, 0, 0 };
    public int playerOneWins, playerTwoWins;

    public string playerOne, playerTwo;
    public bool isSinglePlayer;

    public int currentRound;

    public enum actions { Jump, Running, NinjaKick, HighKnee, SkierJack};
    public actions expectedAction;


    private void Start()
    {
        Screen.SetResolution(1280, 720, true);

        playerOne = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne;
        playerTwo = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo;

        isSinglePlayer = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.isSinglePlayer;

        playerOneName.text = playerOne;
        playerTwoName.text = playerTwo;

        isGamePaused = false;
        isTimeComplete = false;
        isGameRunning = false;

        currentRound = 1;
        nextInstructionText.text = "Next Round- " + "High Knee";


        ToW_AudioManager.instance.PlayAudio("MenuMusic");

        PlayerSession.Instance.StartMPSession();

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
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.tugofwar;
        StartCoroutine(DelayRoundStart());
        ToW_AudioManager.instance.PlayAudio("GameStart");
        yield return new WaitForSecondsRealtime(1.5f);
        //nextRoundPanel.SetActive(true);
        ToW_AudioManager.instance.PlayAudio("HighKneeRound");
        yield return new WaitForSecondsRealtime(2f);
        //nextRoundPanel.SetActive(false);
    }

    public IEnumerator StartRound()
    {
        StartCoroutine(DelayRoundStart());
        counterText.SetActive(true);
        counterText.GetComponent<TMP_Text>().text = "3";
        yield return new WaitForSecondsRealtime(1f);
        counterText.GetComponent<TMP_Text>().text = "2";
        yield return new WaitForSecondsRealtime(1f);
        counterText.GetComponent<TMP_Text>().text = "1";
        yield return new WaitForSecondsRealtime(1f);
        counterText.SetActive(false);
    }

    private IEnumerator DelayRoundStart()
    {
        ToW_AudioManager.instance.PauseAudio("MenuMusic");
        Time.timeScale = 1f;
        startPanel.SetActive(false);
        roundText.text = "Round " + currentRound;
        gamePanel.SetActive(true);
        ToW_GamePlayObjectManager.instance.SetGameObject();

        if (currentRound == 1)
        {
            expectedAction = actions.HighKnee;
            nextInstructionText.text = "Next Round- " + "Jump";
            roundInstructionText.text = "High Knee";
        }
        else if (currentRound == 2)
        {
            expectedAction = actions.Jump;
            nextInstructionText.text = "Next Round- " + "Run";
            roundInstructionText.text = "Jump";
        }
        else if (currentRound == 3)
        {
            expectedAction = actions.Running;
            roundInstructionText.text = "Run";
        }

        if (expectedAction == actions.Jump)
        {
            ToW_AudioManager.instance.PlayAudio("JumpingRound");
            roundActionText.text = "Jump";
            YipliHelper.SetGameClusterId(205, 205);
        }
        else if (expectedAction == actions.Running)
        {
            ToW_AudioManager.instance.PlayAudio("RunningRound");
            roundActionText.text = "Run";
            YipliHelper.SetGameClusterId(202, 202);
        }
        else if (expectedAction == actions.NinjaKick)
        {
            roundActionText.text = "Ninja Kick";
            YipliHelper.SetGameClusterId(207, 207);
        }
        else if (expectedAction == actions.HighKnee)
        {
            roundActionText.text = "High Knee";
            YipliHelper.SetGameClusterId(203, 203);
        }
        else if (expectedAction == actions.SkierJack)
        {
            roundActionText.text = "Skier Jack";
            YipliHelper.SetGameClusterId(204, 204);
        }

        yield return new WaitForSeconds(3f);
        ToW_GameEndController.instance.FindPlayers();

        gamePanel.SetActive(true);

        gameTime = 0;
        remainingTime = roundDuration;

        

        isGameRunning = true;
        ToW_AudioManager.instance.PlayAudio("Soundtrack");
        StartCoroutine(GameTimer());
    }

    public void UpdateStars()
    {
        if (winner == 1)
        {
            playerOneStars[currentRound - 1].color = Color.yellow;
            playerTwoStars[currentRound - 1].color = Color.black;
        }
        else if (winner == 2)
        {
            playerOneStars[currentRound - 1].color = Color.black;
            playerTwoStars[currentRound - 1].color = Color.yellow;
        }
        else
        {
            playerOneStars[currentRound - 1].color = Color.black;
            playerTwoStars[currentRound - 1].color = Color.black;
        }
    }

    public void UpdateAdvantageText()
    {
        switch (winner)
        {
            case 1:
                // Handle player one victory
                advantageText.text = "Advantage- " + playerOne;
                break;
            case 2:
                // Handle player two victory
                advantageText.text = "Advantage- " + playerTwo;
                break;
            default:
                // Handle tie
                advantageText.text = "Players Even";
                break;
        }
    }

    private IEnumerator GameTimer()
    {
        timerText.text = ((int)(remainingTime / 60)).ToString("00") + ":" + ((int)(remainingTime % 60)).ToString("00");
        yield return new WaitForSecondsRealtime(1f);
        gameTime++;
        remainingTime = roundDuration - gameTime;

        if (remainingTime == 0)
        {
            isTimeComplete = true;
            isGameRunning = false;
            StartCoroutine(EndRound());
        }

        if (!isGamePaused && isGameRunning)
        {
            StartCoroutine(GameTimer());
        }

    }



    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        //DestroyImmediate(chainObject,true);
    }

    public IEnumerator EndRound()
    {
        FindObjectOfType<ToW_GameplayController>().DropChain();

        if (isTimeComplete)
        {
            // Handle time over
        }

        switch (winner)
        {
            case 1:
                // Handle player one victory
                playerOneWins++;

                roundWinner[currentRound - 1] = 1;

                break;
            case 2:
                // Handle player two victory
                playerTwoWins++;

                roundWinner[currentRound - 1] = 2;

                break;
            default:
                // Handle tie
                roundWinner[currentRound - 1] = 0;

                break;
        }
        ToW_AudioManager.instance.PauseAudio("Soundtrack");

        ToW_AudioManager.instance.PlayAudio("Cheer");
        Time.timeScale = 0.5f;
        isGameOver = true;
        isGameRunning = false;
        yield return new WaitForSecondsRealtime(1f);
        ToW_AudioManager.instance.PlayAudio("Gong");
        gamePanel.SetActive(false);
        UpdateStars();
        currentRound++;

        if (currentRound < 4)
        {
            //nextRoundPanel.SetActive(true);
            StartCoroutine(StartRound());
        }

        if (currentRound == 4)
        {
            if (playerOneWins > playerTwoWins)
            {
                MM_GameUIManager.instance.winnerNumber = 1;

                // Handle Game Over
            }
            else if (playerOneWins < playerTwoWins)
            {
                MM_GameUIManager.instance.winnerNumber = 2;
            }
            else
            {
                MM_GameUIManager.instance.winnerNumber = 3;
            }

            ToW_AudioManager.instance.StopAudio("Soundtrack");
            //nextRoundPanel.SetActive(false);


            ToW_AudioManager.instance.PlayAudio("End");
            PlayerSession.Instance.StoreMPSession(playerOneWins, playerTwoWins);
            yield return new WaitForSecondsRealtime(2f);
            MM_GameUIManager.instance.ShowResultsScreen();
            ToW_AudioManager.instance.StopAllAudio();

        }
    }


    
}
