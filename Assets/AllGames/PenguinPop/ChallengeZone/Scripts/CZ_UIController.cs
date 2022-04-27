using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * UI Controller script to hande display of UI text and button functionality
 * */
public class CZ_UIController : MonoBehaviour
{
    public static CZ_UIController instance;
    private Button currentButton;
    public Button resumeButton, continueButton;

    public GameObject pauseMenuPanel, gamePanel, resultsMenuPanel, backgroundPanel;
    private GameObject currentPanel;
    public IEnumerator gameOver;

    public int winner;
    public int currentButtonIndex;
    public bool isPaused;

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
        currentPanel = gamePanel;
    }

    private void Update()
    {
        //GetMatKeyInputs();
    }

    // Function to restart the game by reloading the game scene
    public void RestartGame()
    {
        if (isPaused)
        {
            isPaused = false;
            //PP_AudioManager.instance.ResumeAudio();
        }
        Time.timeScale = 1;
        PP_AudioManager.instance.PlayAudio("ButtonClick");
        SceneManager.LoadScene("ChallengeZone");
    }

    // Function to pause gameplay by setting the timescale to 0
    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
        //PP_AudioManager.instance.PauseAudio();
        currentPanel = pauseMenuPanel;
        OpenPanel("PausePanel");
    }

    // Function to resume gameplay
    public void Resume()
    {
        Time.timeScale = PP_GameController.instance.timeScale;
        //PP_AudioManager.instance.ResumeAudio();
        isPaused = false;
        pauseMenuPanel.SetActive(false);
    }

    // Function to handle UI elemnts when game ends
    public IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 0;
        PP_AudioManager.instance.PlayAudio("VictorySound");
        OpenPanel("ResultsPanel");

        //continueButton.Select();
    }




    /*private void GetMatKeyInputs()
    {
        // left to right play, changeplayer, gotoyipli, exit
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetPreviousButton();
        }

        // left to right play, changeplayer, gotoyipli, exit
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetNextButton();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SelectButton();
        }
    }*/

    /*private void GetPreviousButton()
    {
        currentButtonIndex--;
        if (currentButtonIndex < 0)
        {
            currentButtonIndex = currentButtonList.Length - 1;
        }
        currentButton = currentButtonList[currentButtonIndex];
        currentButton.Select();
    }

    private void GetNextButton()
    {
        currentButtonIndex++;
        if (currentButtonIndex >= currentButtonList.Length)
        {
            currentButtonIndex = 0;
        }
        currentButton = currentButtonList[currentButtonIndex];
        currentButton.Select();
    }*/

    private void SelectButton()
    {
        currentButton.onClick.Invoke();
    }

    public void OpenPanel(string openPanel)
    {
        currentPanel.SetActive(false);

        if (openPanel == "PausePanel")
        {
            currentPanel = pauseMenuPanel;
            currentButton = resumeButton;
        }
        else if (openPanel == "ResultsPanel")
        {
            currentPanel = resultsMenuPanel;
            currentButton = continueButton;
        }

        currentPanel.SetActive(true);
        currentButton.Select();
    }
}
