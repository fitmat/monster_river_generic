using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Game UI Manager
 * Handles the global UI across all scenes
 * Opens global game panels
 * 
 * Global script will be active across all scenes
 */

public class MM_GameUIManager : MonoBehaviour
{

    #region Dont Destroy on Load Singleton declaration

    public static MM_GameUIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1280, 720, true);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    #endregion

    #region Variable declaration

    [SerializeField] private GamesData gamesData;

    [SerializeField] GameObject blackFlashPanel;
    [SerializeField] Animator blackFlash;

    public GameObject startPanel, resultsPanel, loadingPanel, exitingPanel, messagePanel, loadingScreenPosition;
    public GameObject loadScreen;

    public string gameName, winnerName;
    public float playerOneFP, playerOneCal, playerOneXP;
    public float playerTwoFP, playerTwoCal, playerTwoXP;
    public int countdown;
    public bool isExiting;
    public TMP_Text gameNameText, resultText;
    public TMP_Text countdownText, messageText;
    public TMP_Text playerOneNameText, playerTwoNameText;
    public TMP_Text playerOneFPText, playerOneCalText, playerOneXPText;
    public TMP_Text playerTwoFPText, playerTwoCalText, playerTwoXPText;

    public GameObject playerOneCrown, playerTwoCrown, gameBackground, versusObject, tieObject;


    public enum Games { mainmenu, penguinpop, treewarrior, tugofwar, monsterriver, theraft, puddlehop, fruitslice, boomerang, eggcatcher, yiplipong, fishtrap, towerbuilder, hungrysnake, pinball, headball, dragonbreath };
    public Games activeGame;

    public bool isPlayingGame;
    public bool isOpeningGames, isOpeningPlayers, isOpeningMainMenu;

    public int winnerNumber;
    public int lastPlayedGameIndex;
    public int gameDifficultyAccumulate;

    #endregion

    #region Unity directives

    private void Start()
    {
        isExiting = true;
        isPlayingGame = false;

        isOpeningGames = false;
        isOpeningPlayers = false;
        isOpeningMainMenu = true;
    }

    private void Update()
    {
        if (Application.targetFrameRate != 30)
        {
            Application.targetFrameRate = 30;
        }
    }
    #endregion

    #region Loading Screen functions

    // Function shows plain loading screen
    public void ShowLoadingScreen()
    {
        loadingPanel.SetActive(true);
    }

    // Function shows game specific loading screen alone with tutorial
    public void ShowGameLoadingScreen(int gameIndex)
    {
        loadScreen = gamesData.Games[gameIndex].gameLoadingScreen;
        Instantiate(loadScreen, loadingScreenPosition.transform);
        loadingPanel.SetActive(true);
    }

    // Function closes the loading screen on game load
    public void HideLoadingScreen()
    {
        try
        {
            Destroy(loadingScreenPosition.transform.GetChild(0).gameObject);
        }
        catch
        {   }
        loadingPanel.SetActive(false);
    }

    #endregion

    #region Exit Game functions

    // Function shows warning to user and begins countdown to exit game
    public void ShowExitingWarning()
    {
        if (!isExiting)
        {
            MM_AudioManager.instance.PlayAudio("ExitingGame");
            countdown = 9;
            isExiting = true;
            exitingPanel.SetActive(true);
            StartCoroutine(ExitCountdown());
        }
    }

    // Function handles countdown till user resumes gameplay
    public IEnumerator ExitCountdown()
    {
        if (isExiting && YipliHelper.GetGameClusterId() != 0 && YipliHelper.GetGameClusterId() != 5)
        {
            if (countdown >= 0)
            {
                countdownText.text = countdown.ToString();
                yield return new WaitForSecondsRealtime(0.5f);
                countdown--;
                StartCoroutine(ExitCountdown());
            }
            else
            {
                ExitGame();
            }
        }
        else
        {
            exitingPanel.SetActive(false);
        }
    }

    // Function handles exiting game when user steps off mat
    public void ExitGame()
    {
        ShowLoadingScreen();
        exitingPanel.SetActive(false);

        try
        {
            switch (activeGame)
            {
                case Games.penguinpop:
                    PP_AudioManager.instance.StopAllAudio();
                    break;
                case Games.treewarrior:
                    TW_AudioManager.instance.StopAllAudio();
                    break;
                case Games.tugofwar:
                    ToW_AudioManager.instance.StopAllAudio();
                    break;
                case Games.monsterriver:
                    MR_AudioManager.instance.StopAllAudio();
                    break;
                case Games.theraft:
                    MR_AudioManager.instance.StopAllAudio();
                    break;
                case Games.puddlehop:
                    PH_AudioManager.instance.StopAllAudio();
                    break;
            }
        }
        catch
        {

        }

        YipliHelper.SetGameClusterId(0,0);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region Message display function

    // Function displays a message on screen
    public IEnumerator DisplayMessage(string message, float time)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
        yield return new WaitForSecondsRealtime(time);
        messagePanel.SetActive(false);
    }

    #endregion

    #region In Game Panel handler functions

    // Function shows starting screen and countdown on game start
    public IEnumerator ShowStartScreen()
    {
        HideLoadingScreen();
        gameNameText.text = gameName;
        startPanel.SetActive(true);
        startPanel.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSecondsRealtime(2.4f);
        isExiting = false;
        startPanel.SetActive(false);
    }

    // Function shows report card screen displaying results of game

    public void FlashBlackScreen()
    {
        blackFlashPanel.SetActive(true);
        blackFlash.SetTrigger("BlackFlash");
    }

    public void ShowResultsScreen()
    {
        isPlayingGame = false;
        MM_AudioManager.instance.PlayAudio("Drum");
        MM_MatButtonController.instance.isInResultScreen = true;
        YipliHelper.SetGameClusterId(0,0);
        resultsPanel.SetActive(true);
        messagePanel.SetActive(false);
        StartCoroutine(MM_MatButtonController.instance.SetResultsScreenActive());
        gameBackground.GetComponent<Image>().sprite = gamesData.Games[lastPlayedGameIndex].gameScreenshot;
        isExiting = false;
        exitingPanel.SetActive(false);

        /* Winner Number:
         * 1- Player one wins
         * 2- Player two wins
         * 3- Game tied
         * 4- Both players win
         * 5- Both players lose
         */

        if (winnerNumber == 1)
        {
            winnerName = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne;
            if (winnerName.Length > 11)
            {
                winnerName.Substring(0, 11);
            }
            //resultText.text = winnerName + " Wins!";
            playerOneCrown.SetActive(true);
            playerTwoCrown.SetActive(false);
            versusObject.SetActive(true);
            tieObject.SetActive(false);
        }
        else if (winnerNumber == 2)
        {
            winnerName = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo;
            if (winnerName.Length > 11)
            {
                winnerName.Substring(0, 11);
            }
            //resultText.text = winnerName + " Wins!";
            playerOneCrown.SetActive(false);
            playerTwoCrown.SetActive(true);
            versusObject.SetActive(true);
            tieObject.SetActive(false);

        }
        else if (winnerNumber == 3)
        {
            //resultText.text = "Game Tied!";
            playerOneCrown.SetActive(false);
            playerTwoCrown.SetActive(false);
            versusObject.SetActive(false);
            tieObject.SetActive(true);
        }
        else if (winnerNumber == 4)
        {
            //resultText.text = "Victory!";
            playerOneCrown.SetActive(true);
            playerTwoCrown.SetActive(true);
            versusObject.SetActive(false);
            tieObject.SetActive(true);
        }
        else if (winnerNumber == 0)
        {
            //resultText.text = "Defeat!";
            playerOneCrown.SetActive(false);
            playerTwoCrown.SetActive(false);
            versusObject.SetActive(false);
            tieObject.SetActive(true);
        }

        // Calculate and display fitness stats of both players

        playerOneNameText.text = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne;
        playerTwoNameText.text = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo;

        playerOneFP = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.fitnesssPoints;
        playerTwoFP = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.fitnesssPoints;
        playerOneCal = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.calories;
        playerTwoCal = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.calories;
        //playerOneXP = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails.duration;
        //playerTwoXP = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails.duration;

        playerOneFPText.text = playerOneFP.ToString("n0");
        playerOneCalText.text = playerOneCal.ToString("n1");
        //playerOneXPText.text = playerOneXP.ToString();

        playerTwoFPText.text = playerTwoFP.ToString("n0");
        playerTwoCalText.text = playerTwoCal.ToString("n1");
        //playerTwoXPText.text = playerTwoXP.ToString();

        blackFlashPanel.SetActive(false);
        MM_AudioManager.instance.PlayAudio("MenuTheme");

    }

    #endregion

    #region Game Selection and Loading functions

    // Function selects a random game from games list
    public void PlayRandomGame()
    {
        int randomIndex;
        // Ensure next game is not same as last game and ensures that three high intensity games are not played in a row
        do
        {
            randomIndex = Random.Range(0, gamesData.Games.Count - 2);

            if (gamesData.Games[randomIndex].difficultyValue == 1)
            {
                gameDifficultyAccumulate++;
            }
            else
            {
                gameDifficultyAccumulate = 0;
            }

        }
        while (randomIndex == lastPlayedGameIndex && gameDifficultyAccumulate < 4);
        lastPlayedGameIndex = randomIndex;
        StartCoroutine(LoadGame(randomIndex));
    }

    // Function loads specified game
    public IEnumerator LoadGame(int gameIndex)
    {
        resultsPanel.SetActive(false);
        messagePanel.SetActive(false);
        MM_AudioManager.instance.StopAudio("MenuTheme");
        MM_AudioManager.instance.PlayAudio("Select");
        MM_MatButtonController.instance.isInResultScreen = false;
        Time.timeScale = 1;
        gameName = gamesData.Games[gameIndex].gameName;
        ShowGameLoadingScreen(gameIndex);
        yield return new WaitForSecondsRealtime(3.5f);
        isPlayingGame = true;
        SceneManager.LoadScene(gamesData.Games[gameIndex].sceneName);
    }

    #endregion

    #region Button handler functions

    public void DirectlyOpenGamesList()
    {
        isOpeningPlayers = false;
        isOpeningMainMenu = false;
        isOpeningGames = true;
        GoToHome();
    }
    public void DirectlyOpenPlayersList()
    {
        isOpeningPlayers = true;
        isOpeningMainMenu = false;
        isOpeningGames = false;
        GoToHome();
    }

    public void Restart()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        resultsPanel.SetActive(false);
        messagePanel.SetActive(false);
        MM_MatButtonController.instance.isInResultScreen = false;
        StartCoroutine(LoadGame(lastPlayedGameIndex));
    }
    public void Home()
    {
        isOpeningGames = false;
        isOpeningPlayers = false;
        isOpeningMainMenu = true;

        GoToHome();
    }

    public void GoToHome()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        YipliHelper.SetGameClusterId(0, 0);
        ShowLoadingScreen();
        resultsPanel.SetActive(false);
        messagePanel.SetActive(false);
        MM_MatButtonController.instance.isInResultScreen = false;
        Time.timeScale = 1;
        activeGame = Games.mainmenu;
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

}
