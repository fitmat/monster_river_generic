using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SB_GameController : MonoBehaviour
{
    public static SB_GameController instance;

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
    public int winningPlayer;

    //[SerializeField] private Animator cameraAnimator;
    //[SerializeField] private GameObject playerOneCelebration, playerTwoCelebration, playerOneLose, playerTwoLose;
    [SerializeField] private GameObject gamePlayObject, countdown, blurEffect;
    private Vector3 startPosition, endPosition;
    public int oldBlockCount, currentBlockCount, cameraRises;
    public bool isMovingUp;

    public int playerOneBlocks, playerTwoBlocks;

    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "spacetower";
        Time.timeScale = 1;
        gameState = GameStates.notStarted;

        oldBlockCount = 0;
        currentBlockCount = 0;
        isMovingUp = false;

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }
    public IEnumerator StartGame()
    {
        TB_AudioManager.instance.PlayAudio("Soundtrack");
        //TB_AudioManager.instance.PlayAudio("Ambient");
        TB_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        //TB_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSeconds(4f);
        Time.timeScale = 1;
        countdown.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.towerbuilder;
        YipliHelper.SetGameClusterId(205, 205);
        startPosition = gamePlayObject.transform.position;
        TB_AudioManager.instance.PlayAudio("Soundtrack");
        TB_AudioManager.instance.PlayAudio("Construction");
        StartCoroutine(TB_InputController.instance.PlayerOneIdling());
        StartCoroutine(TB_InputController.instance.PlayerTwoIdling());
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            TB_AudioManager.instance.PlayAudio("BuildingCollapse");
            if (MM_GameUIManager.instance.winnerNumber == 1)
            {
            }
            else if (MM_GameUIManager.instance.winnerNumber == 2)
            {
            }
            //StartCoroutine(CameraPan());
            gameState = GameStates.gameOver;
            TB_AudioManager.instance.PlayAudio("End");
            TB_AudioManager.instance.StopAudio("Soundtrack");
            TB_AudioManager.instance.StopAudio("Construction");
            blurEffect.SetActive(true);
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(1f);
        GameOver();
    }

    public IEnumerator EndGame()
    {
        Debug.Log("Tower Builder game over");
        yield return new WaitForSecondsRealtime(3f);
        PlayerSession.Instance.StoreMPSession(1,1);
        yield return new WaitForSecondsRealtime(2f);
        TB_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }
    public IEnumerator AddNewBlock(int playerNumber)
    {
        if (playerNumber == 1)
        {
            playerOneBlocks++;
        }
        else if (playerNumber == 2)
        {
            playerTwoBlocks++;
        }
        if (playerOneBlocks > playerTwoBlocks + 2)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
            GameOver();
        }
        else if (playerTwoBlocks > playerOneBlocks + 2)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            GameOver();
        }
        currentBlockCount++;
        if (!isMovingUp)
        {
            isMovingUp = true;
            yield return new WaitForSeconds(2f);
            if (currentBlockCount >= oldBlockCount + 2)
            {
                oldBlockCount += 2;
                StartCoroutine(MoveCraneUp());
            }
            else if ((playerOneBlocks == playerTwoBlocks + 2) && currentBlockCount >= 2)
            {
                oldBlockCount += 2;
                StartCoroutine(MoveCraneUp());
            }
            else if ((playerTwoBlocks == playerOneBlocks + 2) && currentBlockCount >= 2)
            {
                oldBlockCount += 2;
                StartCoroutine(MoveCraneUp());
            }
            isMovingUp = false;
        }
    }
    public IEnumerator CameraShake()
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0.0f;
        float duration = 0.25f, magnitude = 0.15f;
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

    public IEnumerator MoveCraneUp()
    {
        yield return null;
        Debug.Log("CameraRaise");
        gamePlayObject.GetComponent<Animator>().SetTrigger("Rise");
    }

    public IEnumerator CameraPan()
    {
        endPosition = new Vector3(gamePlayObject.transform.position.x, gamePlayObject.transform.position.y - 3f, gamePlayObject.transform.position.z);
        gamePlayObject.transform.position = startPosition;
        float totalTime, currentTime;
        totalTime = 2f;
        currentTime = 0;
        yield return new WaitForSeconds(2f);
        while (currentTime <= totalTime)
        {
            gamePlayObject.transform.position = Vector3.Lerp(startPosition, endPosition, currentTime / totalTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }
}
