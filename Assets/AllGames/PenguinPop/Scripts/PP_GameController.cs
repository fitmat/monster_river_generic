using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PP_GameController : MonoBehaviour
{
    public static PP_GameController instance;

    public GameObject player1, player2, startCountdown;
    public TMP_Text startCountdownText;
    public Transform startPoint, spawnPoint;
    public IEnumerator gameplay;
    [SerializeField] GameObject gamePanel;


    private int enemyPicker;
    private float delay;
    public float timeScale;
    public bool isGameRunning;

    public string[] enemyList;

    private void Awake()
    {
        // Declare class as Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        isGameRunning = false;
        gameplay = Gameplay();
        //PlayerSession.Instance.SetGameId("penguinpop");
        //PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "penguinpop";
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        //PlayerSession.Instance.UpdateDuration();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        PP_ScoreManager.instance.ResetScoreValues();
        Debug.Log("Starting game");
        timeScale = 1;
        Time.timeScale = timeScale;

        //StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        PP_AudioManager.instance.PauseAudio("MainTheme");
        PP_AudioManager.instance.PlayAudio("StartMusic");
        //MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.penguinpop;
        yield return new WaitForSecondsRealtime(1.5f);

        //PlayerSession.Instance.StartMPSession();
        //YipliHelper.SetGameClusterId(205, 205);
        Debug.Log("Starting gameplay with gamemode: " + InitBLE.getGameMode());
        isGameRunning = true;

        StartCoroutine(PP_ScoreManager.instance.GameTimer());
        StartCoroutine(Gameplay());
        StartCoroutine(ControlGameSpeed());

        yield return new WaitForSecondsRealtime(2.5f);
        PP_AudioManager.instance.PlayAudio("Soundtrack");

    }

    public IEnumerator Gameplay()
    {
        enemyPicker = Random.Range(0, enemyList.Length);
        delay = Random.Range(1.3f, 2);
        yield return new WaitForSeconds(1f);

        PP_ObjectPooler.instance.SpawnFromPool(enemyList[enemyPicker], spawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(delay);

        if (isGameRunning)
        {
            StartCoroutine(Gameplay());
        }
        else
        {
            yield break;
        }
    }

    public IEnumerator ControlGameSpeed()
    {
        if (isGameRunning)
        {
            yield return new WaitForSeconds((20f * timeScale));
            timeScale += 0.2f;
            Time.timeScale = timeScale;

            PP_AudioManager.instance.IncreaseTrackPitch("Soundtrack", 0.1f);
        }
    }

    public IEnumerator CameraShake()
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0.0f;
        float duration = 0.25f, magnitude = 0.1f;

        while (elapsed < duration)
        {
            if (PP_GameUIController.instance.isPaused)
            {
                yield return 0;
                continue;
            }

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(x, originalPosition.y - y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPosition;
    }

    public void EndGame()
    {
        if (PP_GameUIController.instance.isGameOver)
        {
            return;
        }
        //PlayerSession.Instance.StoreMPSession(PP_ScoreManager.instance.player1Score, PP_ScoreManager.instance.player2Score);

        PP_GameUIController.instance.isGameOver = true;
        StopCoroutine(gameplay);
        isGameRunning = false;
        gamePanel.SetActive(false);
        StopCoroutine(ControlGameSpeed());
        Time.timeScale = 0.5f;
        PP_AudioManager.instance.PlayAudio("PlayerDie");
        //PP_ScoreManager.instance.SetScoreValues();
        StartCoroutine(PP_GameUIController.instance.gameOver);
    }
}
