using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_GameController : MonoBehaviour
{
    public static PB_GameController instance;

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

    public int direction, activeBalls, totalBalls, playerOneDrops, playerTwoDrops;

    [SerializeField] private Animator playerOneAnimation, playerTwoAnimation;

    public enum GameStates { notStarted, playing, paused, gameOver }
    private GameStates gameState;
    public int winningPlayer;


    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "pinball";
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
        PB_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.pinball;
        YipliHelper.SetGameClusterId(7, 7);
        StartGameplay();
        yield return new WaitForSecondsRealtime(2.5f);
        PB_AudioManager.instance.PlayAudio("Soundtrack");
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            gameState = GameStates.gameOver;

            if (playerOneDrops < playerTwoDrops)
            {
                MM_GameUIManager.instance.winnerNumber = 1;
            }
            else if (playerOneDrops > playerTwoDrops)
            {
                MM_GameUIManager.instance.winnerNumber = 2;
            }
            else
            {
                MM_GameUIManager.instance.winnerNumber = 3;
            }


            if (MM_GameUIManager.instance.winnerNumber == 1)
            {
                playerOneAnimation.SetTrigger("Win");
            }
            else if (MM_GameUIManager.instance.winnerNumber == 2)
            {
                playerTwoAnimation.SetTrigger("Win");
            }

            PB_AudioManager.instance.PlayAudio("GameOver");
            PB_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        PlayerSession.Instance.StoreMPSession(playerTwoDrops, playerOneDrops);
        yield return new WaitForSecondsRealtime(2f);
        PB_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }


    // Start is called before the first frame update
    private void StartGameplay()
    {
        totalBalls = 10;
        activeBalls = 0;
        playerOneDrops = 0;
        playerTwoDrops = 0;

        direction = Random.Range(0, 2);
        if (direction == 0)
        {
            direction = -1;
        }
        ReleaseBall();
        StartCoroutine(DropAdditionalBall());
    }


    public void ReleaseBall()
    {
        if (direction == 1)
        {
            StartCoroutine(PB_GateController.instance.OpenRightGate());
        }
        else if (direction == -1)
        {
            StartCoroutine(PB_GateController.instance.OpenLeftGate());
        }
        direction = -direction;
        activeBalls++;
    }

    private IEnumerator DropAdditionalBall()
    {
        yield return new WaitForSeconds(Random.Range(5f,10f));
        StartCoroutine(DropAdditionalBall());
        ReleaseBall();
    }
    public void IncreasePlayerOneDrops()
    {
        playerOneDrops++;
        totalBalls--;
        activeBalls--;

        if (activeBalls == 0)
        {
            ReleaseBall();
        }

        if (totalBalls == 0)
        {
            GameOver();
            return;
        }
    }
    public void IncreasePlayerTwoDrops()
    {
        playerTwoDrops++;
        totalBalls--;
        activeBalls--;

        if (activeBalls == 0)
        {
            ReleaseBall();
        }

        if (totalBalls == 0)
        {
            GameOver();
            return;
        }
    }


}
