using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_GameController : MonoBehaviour
{
    public static HS_GameController instance;

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
    [SerializeField] private GameObject playerOneCelebration, playerTwoCelebration;

    private void Start()
    {
        //PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "hungrysnake";
        Time.timeScale = 0;
        gameState = GameStates.notStarted;


        StartCoroutine(StartGame());
    }

    private void Update()
    {
        //PlayerSession.Instance.UpdateDuration();
    }
    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        //StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        HS_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(1.5f);
        gameState = GameStates.playing;
        Time.timeScale = 1;
        //PlayerSession.Instance.StartMPSession();
        //MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.hungrysnake;
        //YipliHelper.SetGameClusterId(7, 7);


        StartCoroutine(HS_PlayerOneController.instance.MoveAhead());
        StartCoroutine(HS_PlayerTwoController.instance.MoveAhead());

        yield return new WaitForSecondsRealtime(2.5f);
        HS_AudioManager.instance.PlayAudio("Soundtrack");
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            Time.timeScale = 0;
            if (winningPlayer == 1)
            {
                
            }
            else if (winningPlayer == 2)
            {
                
            }

            gameState = GameStates.gameOver;
            HS_AudioManager.instance.PlayAudio("End");
            HS_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }


    public IEnumerator EndGame()
    {
        Debug.Log("Hungry Snake game over");
        yield return new WaitForSecondsRealtime(3f);
        //PlayerSession.Instance.StoreMPSession(HS_PlayerOneController.instance.snakeLength, HS_PlayerTwoController.instance.snakeLength);
        yield return new WaitForSecondsRealtime(2f);
        HS_AudioManager.instance.StopAllAudio();
        //MM_GameUIManager.instance.ShowResultsScreen();
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

}
