using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CZ_GameController : MonoBehaviour
{
    public static CZ_GameController instance;

    public GameObject startText;
    public TMP_Text startCountdownText, instructionText;
    public Transform startPoint, spawnPoint;
    public IEnumerator gameplay;


    private int enemyPicker;
    private float delay;
    public float timeScale, timeLeft;
    public bool isGameRunning;

    public string[] enemyList;

    public bool isEndless;

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
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        timeScale = 1;

        PP_AudioManager.instance.PlayAudio("StartTimer");
        startCountdownText.text = "Ready";
        yield return new WaitForSeconds(1f);
        startCountdownText.text = "Set";
        yield return new WaitForSeconds(1f);
        startCountdownText.text = "Go";
        yield return new WaitForSeconds(1f);
        startText.SetActive(false);

        CZ_ScoreManager.instance.ResetScoreValues();
        isGameRunning = true;
        StartCoroutine(Gameplay());

    }

    public IEnumerator Gameplay()
    {
        enemyPicker = Random.Range(0, enemyList.Length);

        PP_ObjectPooler.instance.SpawnFromPool(enemyList[enemyPicker], spawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        if (isGameRunning)
        {
            if (CZ_ScoreManager.instance.gameTime == 0)
            {
                EndGame();
                yield break;
            }
            else
            {
                StartCoroutine(Gameplay());
            }
        }
        else
        {
            yield break;
        }
    }

    public IEnumerator CameraShake()
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0.0f;
        float duration = 0.25f, magnitude = 0.1f;

        while (elapsed < duration)
        {
            if (CZ_UIController.instance.isPaused)
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
        StopCoroutine(gameplay);
        isGameRunning = false;
        Time.timeScale = 1;
        PP_AudioManager.instance.PlayAudio("DeathSound");
        CZ_ScoreManager.instance.SetScoreValues();
        StartCoroutine(CZ_UIController.instance.gameOver);
    }
}
