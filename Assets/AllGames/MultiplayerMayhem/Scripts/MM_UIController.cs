using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;
using TMPro;
using System;

/* UI Controller
 * Handles the UI on Main Menu scene
 * Opens panels
 * Displays data to user
 */

public class MM_UIController : MonoBehaviour
{

    #region Singleton declaration

    // Script is a singleton
    public static MM_UIController instance;

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

    public MultiPlayerData playerData;

    [SerializeField] private List<Button> currentButtonList;
    [SerializeField] private Button currentButton, quitButton;
    [SerializeField] private GameObject playerPanelBackButton, playerPanelNextButton, playerOneBg, playerTwoBg;

    private GameObject currentPanel;

    [SerializeField] private TMP_Text playerOneName, playerTwoName;

    public int currentButtonIndex;

    private string retrievePlayerOneName;
    private Sprite retrievePlayerOneImage;

    public bool isPlayerSet, isPlayerOnePresent, isPlayerTwoPresent;

    [SerializeField] private GameObject playerOneNameObject, playerOneSelectionPanel, playerTwoNameObject, playerTwoSelectionPanel;
    [SerializeField] private GameObject mainMenuModels, mainMenuBackground, rays;


    [Header("PlayersPanelObjects")]
    [SerializeField] private TMP_Text playerOneNameText;
    [SerializeField] private TMP_Text playerTwoNameText;
    [SerializeField] private Image playerOneImageSprite, playerTwoImageSprite;

    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playersPanel;
    [SerializeField] private GameObject switchPlayerPanel;
    [SerializeField] private GameObject gamesPanel;

    public ScrollRect playerOneScrollRect, playerTwoScrollRect, gamesScrollRect;
    public RectTransform playerOneContentPanel, playerTwoContentPanel, gamesContentPanel;

    [Header("Button Lists")]
    public List<Button> mainMenuButtons;
    public List<Button> changePlayersButtons, playerOneSelectButtons, playerTwoSelectButtons, gameSelectButtons;

    public Transform homeButton, shuffleButton;
    private Vector3 homeButtonPosition, shuffleButtonPosition;

    #endregion

    #region Unity directives

    private void Start()
    {
        MM_GameUIManager.instance.HideLoadingScreen();
        Debug.Log("UI Test- UIController Start");

        //YipliHelper.SetGameClusterId(0,0);
        Debug.Log("UI Test- UIController Cluster Id Set");
        currentButtonIndex = 0;


        homeButtonPosition = homeButton.position;
        shuffleButtonPosition = shuffleButton.position;

        if (MM_GameUIManager.instance.isOpeningGames && !MM_GameUIManager.instance.isOpeningMainMenu)
        {
            ToGameSelect();
            return;
        }
        else if (MM_GameUIManager.instance.isOpeningPlayers && !MM_GameUIManager.instance.isOpeningMainMenu)
        {
            OpenPlayersPanel();
            return;
        }
        

        Debug.Log("UI Test- About to Open Panel");
        StartCoroutine(MM_MatButtonController.instance.SetMainMenuActive());
        Debug.Log("UI Test- Main Panel Opened");
        isPlayerSet = false;
        MM_AudioManager.instance.PauseAudio("MenuTheme");
        MM_AudioManager.instance.PlayAudio("MenuTheme");
        MM_AudioManager.instance.SetTrackVolume("MenuTheme", 1f);
        MM_AudioManager.instance.PlayAudio("Drum");
    }


    #endregion

    #region Data retrieval function

    // Function retrieves initial player data from persistent data store, or sets null values if no data is found
    public void GetInitialPlayerData()
    {
        Debug.Log("Connection Test- Starting Data gathering");
        isPlayerSet = true;
        isPlayerOnePresent = false;
        isPlayerTwoPresent = false;

        Debug.Log("Save data test- Starting get data function");
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager = new MP_GameStateManager();
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerData(playerData);
        Debug.Log("Save data test- Player data set");

        Debug.Log("Save data test- Checking player data on device");

        try
        {

            if (UserDataPersistence.GetSavedMultiplayerFromDevice())
            {

                if (PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.userId != UserDataPersistence.GetMultiplayerUserIdFromDevice())
                {
                    DeletePlayerData();
                }



                Debug.Log("Save data test- Found player data on device");

                Debug.Log("Getting Player Data- Player one not empty");
                Debug.Log("Getting Player Data- Player one- " + playerData.PlayerOneName);
                PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerOne(playerData.PlayerOneName);
                Debug.Log("Save data test- Got player one from scriptable");

                Debug.Log("Getting Player Data- Player two not empty");
                Debug.Log("Getting Player Data- Player two- " + playerData.PlayerTwoName);
                PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerTwo(playerData.PlayerTwoName);
                Debug.Log("Save data test- Got player two from scriptable");


                for (int i = 0; i < PlayerSession.Instance.currentYipliConfig.allPlayersInfo.Count; i++)
                {
                    if (playerData.PlayerOneName == PlayerSession.Instance.currentYipliConfig.allPlayersInfo[i].playerName)
                    {
                        isPlayerOnePresent = true;
                    }
                    if (playerData.PlayerTwoName == PlayerSession.Instance.currentYipliConfig.allPlayersInfo[i].playerName)
                    {
                        isPlayerTwoPresent = true;
                    }

                    if (isPlayerOnePresent && isPlayerTwoPresent)
                    {
                        break;
                    }
                }

                if (!isPlayerOnePresent || !isPlayerTwoPresent)
                {
                    DeletePlayerData();

                    return;

                }

                else
                {
                    playerOneName.text = playerData.PlayerOneName;
                    playerTwoName.text = playerData.PlayerTwoName;

                    playerOneBg.SetActive(true);
                    playerTwoBg.SetActive(true);

                    playerOneNameText.text = playerData.PlayerOneName;
                    playerTwoNameText.text = playerData.PlayerTwoName;
                }
            }
            else
            {
                DeletePlayerData();
            }
        }
        catch
        {
            DeletePlayerData();
            Debug.Log("Data Error, deleting");
        }
    }

    public void DeletePlayerData()
    {
        playerData.PlayerOneName = null;
        playerData.PlayerTwoName = null;
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerOne(null);
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerTwo(null);

        playerOneName.text = " ";
        playerTwoName.text = " ";

        playerOneBg.SetActive(false);
        playerTwoBg.SetActive(false);

        playerOneNameText.text = "Select Player 1";
        playerTwoNameText.text = "Select Player 2";

        UserDataPersistence.DeleteMultiplayerDataFromDevice();
        UserDataPersistence.DeleteMultiplayerUserIdFromDevice();
    }

    #endregion

    #region Panel control functions

    // Function opens the players panel
    public void OpenPlayersPanel()
    {
        MM_AudioManager.instance.PlayAudio("Swoosh");
        MM_AudioManager.instance.SetTrackVolume("MenuTheme", 0.5f);
        quitButton.Select();
        rays.SetActive(false);
        if (!isPlayerSet)
        {
            GetInitialPlayerData();
        }

        mainMenuPanel.SetActive(false);
        //mainMenuModels.SetActive(false);
        //mainMenuBackground.SetActive(false);
        switchPlayerPanel.SetActive(false);
        playersPanel.SetActive(true);

        playerOneSelectionPanel.GetComponent<Animator>().ResetTrigger("Open");
        playerTwoSelectionPanel.GetComponent<Animator>().ResetTrigger("Open");
        playerOneSelectionPanel.GetComponent<Animator>().SetTrigger("Close");
        playerTwoSelectionPanel.GetComponent<Animator>().SetTrigger("Close");

        playerTwoNameObject.transform.localScale = new Vector3(1, 1, 1);

        // Sets default index of button control according to which player is not set
        int defaultIndex = 0;
        if (playerData.PlayerOneName != null)
        {
            defaultIndex = 0;
        }
        else if (playerData.PlayerTwoName != null)
        {
            defaultIndex = 1;
        }
        StartCoroutine(MM_MatButtonController.instance.SetChangePlayersActive(defaultIndex));
    }

    // Function opens the selection list for player one
    public void OpenPlayerOneSelection()
    {
        MM_AudioManager.instance.PlayAudio("Swoosh");
        MultiPlayerSelection.instance.CreatePlayersOneList();
        playerOneNameText.text = " ";
        playerOneNameObject.SetActive(false);
        playerOneSelectionPanel.GetComponent<Animator>().ResetTrigger("Close");
        playerTwoSelectionPanel.GetComponent<Animator>().ResetTrigger("Close");
        playerOneSelectionPanel.GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(MM_MatButtonController.instance.SetPlayerOneSelectActive());

        playerPanelBackButton.SetActive(false);
        playerPanelNextButton.SetActive(false);
    }

    // Function opens the selection list for player two
    public void OpenPlayerTwoSelection()
    {
        MM_AudioManager.instance.PlayAudio("Swoosh");
        MultiPlayerSelection.instance.CreatePlayersTwoList();
        playerTwoNameText.text = " ";
        playerTwoNameObject.SetActive(false);
        playerOneSelectionPanel.GetComponent<Animator>().ResetTrigger("Close");
        playerTwoSelectionPanel.GetComponent<Animator>().ResetTrigger("Close");
        playerTwoSelectionPanel.GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(MM_MatButtonController.instance.SetPlayerTwoSelectActive());

        playerPanelBackButton.SetActive(false);
        playerPanelNextButton.SetActive(false);
    }

    // Function closes the selection lists and return back to players panel and displays name of selected players
    public void BackToPlayersPanel(int defaultIndex)
    {
        MM_AudioManager.instance.PlayAudio("Swoosh");
        playerOneSelectionPanel.GetComponent<Animator>().ResetTrigger("Open");
        playerTwoSelectionPanel.GetComponent<Animator>().ResetTrigger("Open");
        playerOneSelectionPanel.GetComponent<Animator>().SetTrigger("Close");
        playerTwoSelectionPanel.GetComponent<Animator>().SetTrigger("Close");
        playerOneNameObject.SetActive(true);
        playerTwoNameObject.SetActive(true);

        playerPanelBackButton.SetActive(true);
        playerPanelNextButton.SetActive(true);


        if (playerData.PlayerOneName != null)
        {
            playerOneName.text = playerData.PlayerOneName;
            playerOneNameText.text = playerData.PlayerOneName;

            playerOneBg.SetActive(true);
        }
        else
        {
            playerOneName.text = " ";
            playerOneNameText.text = "Select Player 1";

            playerOneBg.SetActive(false);
        }

        if (playerData.PlayerTwoName != null)
        {
            playerTwoName.text = playerData.PlayerTwoName;
            playerTwoNameText.text = playerData.PlayerTwoName;

            playerTwoBg.SetActive(true);
        }
        else
        {
            playerTwoName.text = " ";
            playerTwoNameText.text = "Select Player 2";

            playerTwoBg.SetActive(false);
        }

        StartCoroutine(MM_MatButtonController.instance.SetChangePlayersActive(defaultIndex));
    }

    // Function closes all other panels and returns to Main Menu panek
    public void BackToHome()
    {
        MM_AudioManager.instance.SetTrackVolume("MenuTheme", 1f);
        MM_AudioManager.instance.PlayAudio("Swoosh");
        playersPanel.SetActive(false);
        gamesPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        playerOneSelectionPanel.GetComponent<Animator>().ResetTrigger("Open");
        playerTwoSelectionPanel.GetComponent<Animator>().ResetTrigger("Open");
        playerOneSelectionPanel.GetComponent<Animator>().SetTrigger("Close");
        playerTwoSelectionPanel.GetComponent<Animator>().SetTrigger("Close");

        currentButtonIndex = 0;
        StartCoroutine(MM_MatButtonController.instance.SetMainMenuActive());
        mainMenuModels.SetActive(true);
        mainMenuBackground.SetActive(true);
        rays.SetActive(true);
    }

    // Function opens the game selection panel
    public void ToGameSelect()
    {
        quitButton.Select();
        mainMenuPanel.SetActive(false);
        MM_AudioManager.instance.SetTrackVolume("MenuTheme", 0.5f);

        // If both players have not been selected then user is redirected to the player select panel else game select panel is opened
        if (playerData.PlayerOneName != null && playerData.PlayerTwoName != null)
        {
            //MM_AudioManager.instance.PlayAudio("Swoosh");
            mainMenuBackground.SetActive(false);
            mainMenuModels.SetActive(false);
            homeButton.position = homeButtonPosition;
            homeButton.localScale = new Vector3(1,1,1);
            shuffleButton.position = shuffleButtonPosition;
            shuffleButton.localScale = new Vector3(1, 1, 1);

            playersPanel.SetActive(false);
            gamesPanel.SetActive(true);
            MM_SelectGame.instance.inSelectGamePanel = true;
            StartCoroutine(MM_MatButtonController.instance.SetGameSelectActive());
        }
        else
        {
            MM_AudioManager.instance.PlayAudio("TwoPlayers");
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Two players required to play",3f));
            OpenPlayersPanel();
        }
    }

    #endregion

    #region Scroll handler functions

    /* These functions handle scrolling the lists to ensure highlighted button is always in focus
     * 
     */
    public void ScrollPlayerOneList(int currentIndex)
    {
        Canvas.ForceUpdateCanvases();
        playerOneContentPanel.anchoredPosition =
            (Vector2)playerOneScrollRect.transform.InverseTransformPoint(playerOneContentPanel.position)
            - (Vector2)playerOneScrollRect.transform.InverseTransformPoint(playerOneSelectButtons[currentIndex].transform.position);
        playerOneContentPanel.anchoredPosition = new Vector2(0f, playerOneContentPanel.anchoredPosition.y - 50f);
    }
    public void ScrollPlayerTwoList(int currentIndex)
    {
        Canvas.ForceUpdateCanvases();
        playerTwoContentPanel.anchoredPosition =
            (Vector2)playerTwoScrollRect.transform.InverseTransformPoint(playerTwoContentPanel.position)
            - (Vector2)playerTwoScrollRect.transform.InverseTransformPoint(playerTwoSelectButtons[currentIndex].transform.position);
        playerTwoContentPanel.anchoredPosition = new Vector2(0f, playerTwoContentPanel.anchoredPosition.y - 50f);
    }

    public IEnumerator ScrollGamesList(int currentIndex)
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Canvas.ForceUpdateCanvases();
        gamesContentPanel.anchoredPosition =
            (Vector2)gamesScrollRect.transform.InverseTransformPoint(gamesContentPanel.position)
            - (Vector2)gamesScrollRect.transform.InverseTransformPoint(gameSelectButtons[currentIndex].transform.position);
        gamesContentPanel.anchoredPosition = new Vector2(0f, gamesContentPanel.anchoredPosition.y - 100f);

    }

    #endregion

    #region Button handler functions

    public void RandomButton()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        MM_GameUIManager.instance.PlayRandomGame();
    }

    public void YipliButton()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        YipliHelper.GoToYipli();
    }

    public void PlayerButton()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        PlayerSession.Instance.ChangePlayer();
    }

    public void TutorialButton()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        MM_TutorialController.instance.StartTutorial();
    }

    public void Exit()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        Application.Quit();
    }

    #endregion

}
