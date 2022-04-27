using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_GameController : MonoBehaviour
{
    public static TB_GameController instance;

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

    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private GameObject playerOneCelebration, playerTwoCelebration, playerOneLose, playerTwoLose;
    [SerializeField] private GameObject gamePlayObject;
    private Vector3 startPosition,endPosition;
    public int oldBlockCount, currentBlockCount, cameraRises;
    public bool isMovingUp;

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
        yield return new WaitForEndOfFrame();
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        TB_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(4.5f);
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

            if (MM_GameUIManager.instance.winnerNumber == 1)
            {
                //playerOneCelebration.SetActive(true);
                //playerTwoLose.SetActive(true);
                GameObject[] blocks;
                blocks = GameObject.FindGameObjectsWithTag("TB_PlayerTwoBlock");
                foreach (GameObject block in blocks)
                {
                    StartCoroutine(block.GetComponent<TB_BlockController>().BurnDown());
                }
            }
            else if (MM_GameUIManager.instance.winnerNumber == 2)
            {
                //playerTwoCelebration.SetActive(true);
                //playerOneLose.SetActive(true);
                GameObject[] blocks;
                blocks = GameObject.FindGameObjectsWithTag("TB_PlayerOneBlock");
                foreach (GameObject block in blocks)
                {
                    StartCoroutine(block.GetComponent<TB_BlockController>().BurnDown());
                }
            }

            gamePlayObject.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(CameraPan());
            gameState = GameStates.gameOver;
            TB_AudioManager.instance.PlayAudio("End");
            TB_AudioManager.instance.StopAudio("Soundtrack");
            TB_AudioManager.instance.StopAudio("Construction");
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
        PlayerSession.Instance.StoreMPSession(TB_PlayerOneController.instance.blocksStacked, TB_PlayerTwoController.instance.blocksStacked);
        yield return new WaitForSecondsRealtime(2f);
        TB_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
    }
    public IEnumerator AddNewBlock()
    {
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
            else if ((TB_PlayerOneController.instance.blocksStacked == TB_PlayerTwoController.instance.blocksStacked + 1) && currentBlockCount >= 2)
            {
                oldBlockCount += 2;
                StartCoroutine(MoveCraneUp());
            }
            else if ((TB_PlayerTwoController.instance.blocksStacked == TB_PlayerOneController.instance.blocksStacked + 1) && currentBlockCount >= 2)
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
        yield return new WaitForSeconds(0.5f);
        cameraAnimator.SetTrigger("CameraUp");
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
