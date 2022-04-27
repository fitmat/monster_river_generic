using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using YipliFMDriverCommunication;
using System;
using DG.Tweening;

/* Multiplayer Mayhem Select Game Script
 * Handles the UI on Main Menu scene
 * Opens panels
 * Displays data to user
 */

public class MM_SelectGame : MonoBehaviour
{

    #region Singleton declaration

    public static MM_SelectGame instance;

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

    #endregion

    #region Variable declaration

    [SerializeField] private List<Button> currentButtonList;
    [SerializeField] private Button currentButton;

    public int currentButtonIndex;

    [SerializeField] private GameObject centerButton;
    [SerializeField] private Image centerButtonImage, leftButtonImage, rightButtonImage, farLeftButtonImage, farRightButtonImage;
    [SerializeField] private GamesData gamesData;
    [SerializeField] private TMP_Text gameName, gameDescription;
    [SerializeField] private Image screenshot, difficultySprite;
    [SerializeField] private GameObject player1Model, player2Model;
    [SerializeField] private GameObject gameData, homeButton, shuffleButton, gameInstructionPanelPosition;
    [SerializeField] private Animator gamesList;
    
    private int currentIndex = -1, leftIndex, rightIndex, farLeftIndex, farRightIndex;
    public bool inSelectGamePanel,isScrollAllowed;

    #endregion

    #region Unity directives

    private void Start()
    {
        inSelectGamePanel = false;
        isScrollAllowed = true;
        ResetScroller();
    }

    #endregion

    #region Scroll Handler functions

    // Function to reset game scroll to initial game
    public void ResetScroller()
    {
        leftIndex = gamesData.Games.Count - 1;
        farLeftIndex = leftIndex - 1;
        currentIndex = 0;
        rightIndex = 1;
        farRightIndex = rightIndex + 1;
        MM_GameUIManager.instance.loadScreen = gamesData.Games[currentIndex].gameLoadingScreen;
        gameName.text = gamesData.Games[currentIndex].gameName;
        difficultySprite.sprite = gamesData.Games[currentIndex].gameDifficulty;
        Destroy(gameInstructionPanelPosition.transform.GetChild(0).gameObject);
        Instantiate(gamesData.Games[currentIndex].gameInstructionPanel, gameInstructionPanelPosition.transform);
        screenshot.sprite = gamesData.Games[currentIndex].gameScreenshot;
    }

    // Function to disable scroll temporarily
    public IEnumerator DisableScroll()
    {
        isScrollAllowed = false;
        yield return new WaitForSecondsRealtime(0.25f);
        isScrollAllowed = true;
    }

    #endregion

    #region Select Game button function

    // Function to load and run selected game
    public void SelectGame(int selectedIndex)
    {
        inSelectGamePanel = false;
        Time.timeScale = 1;
        MM_GameUIManager.instance.lastPlayedGameIndex = selectedIndex;
        StartCoroutine(MM_GameUIManager.instance.LoadGame(selectedIndex));
    }

    #endregion

    #region Games list manager functions

    // Function to scroll to the previous game in list
    public IEnumerator GetPreviousButton()
    {
        if (isScrollAllowed)
        {
            //StartCoroutine(DisableScroll());
            gamesList.SetTrigger("ScrollDown");
            yield return new WaitForSecondsRealtime(0.25f);
            if (inSelectGamePanel)
            {
                currentIndex--;

                // Cycle list
                if (currentIndex == 0)
                {
                    leftIndex = gamesData.Games.Count - 1;
                    rightIndex = currentIndex + 1;
                }
                else if (currentIndex < 0)
                {
                    currentIndex = gamesData.Games.Count - 1;
                    leftIndex = currentIndex - 1;
                    rightIndex = 0;
                }
                else
                {
                    leftIndex = currentIndex - 1;
                    rightIndex = currentIndex + 1;
                }

                farLeftIndex = leftIndex - 1;
                if (farLeftIndex < 0)
                {
                    farLeftIndex = gamesData.Games.Count - 1;
                }
                farRightIndex = rightIndex + 1;
                if (farRightIndex == gamesData.Games.Count)
                {
                    farRightIndex = 0;
                }

                HighlightButton();
            }
        }
    }

    // Function to scroll to the next game in list
    public IEnumerator GetNextButton()
    {
        if (isScrollAllowed)
        {
            //StartCoroutine(DisableScroll());
            gamesList.SetTrigger("ScrollUp");
            yield return new WaitForSecondsRealtime(0.25f);
            if (inSelectGamePanel)
            {
                currentIndex++;

                // Cycle list
                if (currentIndex == gamesData.Games.Count - 1)
                {
                    leftIndex = currentIndex - 1;
                    rightIndex = 0;
                }
                else if (currentIndex > gamesData.Games.Count - 1)
                {
                    currentIndex = 0;
                    leftIndex = gamesData.Games.Count - 1;
                    rightIndex = currentIndex + 1;
                }
                else
                {
                    leftIndex = currentIndex - 1;
                    rightIndex = currentIndex + 1;
                }

                farLeftIndex = leftIndex - 1;
                if (farLeftIndex < 0)
                {
                    farLeftIndex = gamesData.Games.Count - 1;
                }
                farRightIndex = rightIndex + 1;
                if (farRightIndex == gamesData.Games.Count)
                {
                    farRightIndex = 0;
                }

                HighlightButton();
            }
        }
    }

    // Function to Highlight Selected game and display game information
    public void HighlightButton()
    {
        MM_AudioManager.instance.PlayAudio("Swoosh");
        // Special handling for home button
        if (currentIndex == gamesData.Games.Count - 2)
        {
            gameData.SetActive(false);
            homeButton.GetComponent<Animator>().ResetTrigger("Deselect");
            homeButton.GetComponent<Animator>().SetTrigger("Select");
            shuffleButton.GetComponent<Animator>().ResetTrigger("Select");
            shuffleButton.GetComponent<Animator>().SetTrigger("Deselect");
        }
        // Special handling for shuffle button
        else if (currentIndex == gamesData.Games.Count - 1)
        {
            gameData.SetActive(false);
            shuffleButton.GetComponent<Animator>().ResetTrigger("Deselect");
            shuffleButton.GetComponent<Animator>().SetTrigger("Select");
            homeButton.GetComponent<Animator>().ResetTrigger("Select");
            homeButton.GetComponent<Animator>().SetTrigger("Deselect");
        }
        // Handle Game select
        else 
        {
            gameData.SetActive(true);
            homeButton.GetComponent<Animator>().ResetTrigger("Select");
            homeButton.GetComponent<Animator>().SetTrigger("Deselect");
            shuffleButton.GetComponent<Animator>().ResetTrigger("Select");
            shuffleButton.GetComponent<Animator>().SetTrigger("Deselect");
        }

        // Show info of selected game
        gameName.text = gamesData.Games[currentIndex].gameName;
        difficultySprite.sprite = gamesData.Games[currentIndex].gameDifficulty;
        // Destroy old instruction panel and instantiate new one
        Destroy(gameInstructionPanelPosition.transform.GetChild(0).gameObject);
        Instantiate(gamesData.Games[currentIndex].gameInstructionPanel, gameInstructionPanelPosition.transform);
        screenshot.sprite = gamesData.Games[currentIndex].gameScreenshot;

        DOTween.Restart("1");

    }

    // Function to handle user input by Jump to select
    public void ButtonClick()
    {
        if (inSelectGamePanel)
        {
            // Special handling for home button
            if (gamesData.Games[currentIndex].sceneName == "Return")
            {
                inSelectGamePanel = false;
                homeButton.GetComponent<Animator>().ResetTrigger("Select");
                homeButton.GetComponent<Animator>().SetTrigger("Deselect");
                MM_UIController.instance.BackToHome();
                ResetScroller();
            }
            // Special handling for shuffle button
            else if (gamesData.Games[currentIndex].sceneName == "Shuffle")
            {
                Debug.Log("Select");
                inSelectGamePanel = false;
                Time.timeScale = 1;
                MM_GameUIManager.instance.PlayRandomGame();
            }
            // Load and run selected game
            else
            {
                Debug.Log("Select");
                inSelectGamePanel = false;
                Time.timeScale = 1;
                MM_GameUIManager.instance.lastPlayedGameIndex = currentIndex;
                StartCoroutine(MM_GameUIManager.instance.LoadGame(currentIndex));
            }
        }
    }

    #endregion

}
