using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using YipliFMDriverCommunication;
using UnityEngine.UI;
using System;

public class TR_UIController : MonoBehaviour
{
    public static TR_UIController instance;

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
        //PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "theraft";
    }

    [SerializeField] private GameObject gamePanel, gameOverPanel, startPanel, pausePanel;
    [SerializeField] private GameObject explosion, playerObject;

    public string player1Name, player2Name;
    public int player1Score, player2Score, gameTime;

    public List<GameObject> activeEnemies;

    public int currentButtonIndex;

    private void Start()
    {
        //player1Name = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne;
        //player2Name = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo;
        StartCoroutine(StartGame());
    }



    private void Update()
    {
        PlayerSession.Instance.UpdateDuration();
    }

    public void UpdateHealth()
    {
        //healthText.text = "Raft Health- " + MR_RaftController.instance.raftHealth.ToString();
    }

    public void GameOver()
    {
        MR_GameController.instance.isGameRunning = false;
        Time.timeScale = 0.5f;

        if (MR_GameController.instance.isGameWon)
        {
            StartCoroutine(GameWon());
        }
        else
        {
            StartCoroutine(GameLost());
            MR_AudioManager.instance.PlayAudio("Lose");
        }
    }

    private void GameOverUI()
    {
        //UpdateScore();
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        MR_AudioManager.instance.StopAllAudio();
        MM_GameUIManager.instance.ShowResultsScreen();
        PlayerSession.Instance.StoreMPSession(10, 10);
    }

    private IEnumerator GameWon()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy.tag == "MeeleMonster")
            {
                enemy.GetComponent<TR_MeeleMonsterController>().DieOnGameEnd();
            }
            if (enemy.tag == "RangedMonster")
            {
                enemy.GetComponent<TR_RangedMonsterController>().DieOnGameEnd();
            }
        }

        MR_AudioManager.instance.PlayAudio("End");
        yield return new WaitForSeconds(2f);


        MM_GameUIManager.instance.winnerNumber = 4;
        GameOverUI();
    }
    private IEnumerator GameLost()
    {
        playerObject.SetActive(false);
        explosion.SetActive(true);
        MR_AudioManager.instance.PauseAudio("Ambient");
        MR_AudioManager.instance.PauseAudio("Soundtrack");
        yield return new WaitForSecondsRealtime(0.5f);
        MR_AudioManager.instance.PlayAudio("End");
        yield return new WaitForSeconds(2f);

        MM_GameUIManager.instance.winnerNumber = 0;
        GameOverUI();
    }


    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        //StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        MR_AudioManager.instance.PlayAudio("Start");
        yield return new WaitForSecondsRealtime(1.5f);
        //MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.theraft;
       // PlayerSession.Instance.StartMPSession();

        //YipliHelper.SetGameClusterId(211,205);
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        MR_GameController.instance.StartGame();
        TR_RaftController.instance.StartRaftState();
        TR_EnemySpawner.instance.StartSpawning();

        yield return new WaitForSecondsRealtime(2.5f);
        MR_AudioManager.instance.PlayAudio("Ambient");
        MR_AudioManager.instance.PlayAudio("Soundtrack");

    }

}
