using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YipliFMDriverCommunication;
using System;

/*
 * UI Controller script to hande display of UI text and button functionality
 * */
public class PP_GameUIController : MonoBehaviour
{
    public static PP_GameUIController instance;

    public Button[] pauseMenuButtons, resultsMenuButtons, modeSelectButtons;
    public Button[] currentButtonList;
    private Button currentButton;
    public Button continueButton;

    public GameObject pauseMenuPanel, gamePanel, gameOverPanel, resultsMenuPanel, backgroundPanel, modeSelectPanel;
    private GameObject currentPanel;

    public GameObject player1, player2;

    public Transform enemyPool;
    public TMP_Text continueText, winnerText;
    public IEnumerator gameOver;

    public int winner;
    public int currentButtonIndex;
    public bool isPaused;
    public bool isGameOver;


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
        gameOver = GameOver();
        isPaused = false;
        isGameOver = false;
    }

    // Function to handle UI elemnts when game ends
    public IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(2f);
        PP_AudioManager.instance.PlayAudio("GameOver");
        PP_AudioManager.instance.StopAudio("Soundtrack");

        if (PP_ScoreManager.instance.player1Score == PP_ScoreManager.instance.player2Score)
        {
            winner = 3;
        }
        else if (PP_ScoreManager.instance.player1Score > PP_ScoreManager.instance.player2Score)
        {
            winner = 1;
        }
        else if (PP_ScoreManager.instance.player1Score < PP_ScoreManager.instance.player2Score)
        {
            winner = 2;
        }
        yield return new WaitForSecondsRealtime(3f);
        gamePanel.SetActive(false);
        PP_AudioManager.instance.StopAllAudio();
        //MM_GameUIManager.instance.ShowResultsScreen();
    }

}
