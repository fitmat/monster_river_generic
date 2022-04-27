using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* Multiplayer Selection Script
 * Handles the generation of players lists
 * Handles selection of player
 * Handles special cases of player selection 
 */

public class MultiPlayerSelection : MonoBehaviour
{
    #region Singleton declaration

    public static MultiPlayerSelection instance;

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

    [SerializeField] private GameObject playersPanel, switchPlayerPanel;


    public YipliConfig currentYipliConfig;
    private GameObject playerButton;

    public event Action playerSelectedEvent;
    public event Action openPlayerSelectEvent;


    public GameObject PlayersContainerOne, PlayersContainerTwo;
    public GameObject LeftPlayerButtonPrefab,RightPlayerButtonPrefab, playerTwoNameObject;
    private List<GameObject> generatedObjects = new List<GameObject>();
    private List<Button> playerButtons = new List<Button>();
    
    private GameObject playerOneButton, playerTwoButton, computerPlayerButton;
    private int playerOneIndex, playerTwoIndex;

    public bool isSwitchingPlayerOne, isSwitchingPlayerTwo;

    #endregion

    #region Unity directives

    private void Start()
    {
        isSwitchingPlayerOne = false;
        isSwitchingPlayerTwo = false;
    }

    #endregion

    #region List Generation functions

    // Function creates list of available players for player one
    public void CreatePlayersOneList()
    {
        // Clear all existing data
        for (int i = 0; i < PlayersContainerOne.transform.childCount; i++)
        {
            Destroy(PlayersContainerOne.transform.GetChild(i).gameObject);
        }
        generatedObjects.Clear();
        playerButtons.Clear();
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerNames.Clear();

        Quaternion spawnrotation = Quaternion.identity;
        Vector3 playerTilePosition = PlayersContainerOne.transform.localPosition;
        for (int i = 0; i < currentYipliConfig.allPlayersInfo.Count; i++)
        {
            // Generate new button for each player
            PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerNames.Add(currentYipliConfig.allPlayersInfo[i].playerName);
            playerButton = Instantiate(LeftPlayerButtonPrefab, playerTilePosition, spawnrotation) as GameObject;
            playerButton.name = currentYipliConfig.allPlayersInfo[i].playerName;
            playerButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentYipliConfig.allPlayersInfo[i].playerName;
            playerButton.transform.SetParent(PlayersContainerOne.transform, false);

            if (currentYipliConfig.allPlayersInfo[i].playerName == PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne)
            {
                playerOneIndex = i;
                playerOneButton = playerButton;
            }
            generatedObjects.Add(playerButton);
            playerButtons.Add(playerButton.GetComponent<Button>());
        }
        // Add listener to all generated buttons
        for (int i = 0; i < playerButtons.Count; i++)
        {
            playerButtons[i].onClick.RemoveAllListeners();
            playerButtons[i].onClick.AddListener(SelectFirstPlayer);
        }
        MM_UIController.instance.playerOneSelectButtons = playerButtons;
    }

    // Function creates list of available players for player two
    public void CreatePlayersTwoList()
    {
        // Clear all existing data
        for (int i = 0; i < PlayersContainerTwo.transform.childCount; i++)
        {
            Destroy(PlayersContainerTwo.transform.GetChild(i).gameObject);
        }
        generatedObjects.Clear();
        playerButtons.Clear();
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerNames.Clear();

        Quaternion spawnrotation = Quaternion.identity;
        Vector3 playerTilePosition = PlayersContainerTwo.transform.localPosition;
        for (int i = 0; i < currentYipliConfig.allPlayersInfo.Count; i++)
        {
            // Generate new button for each player
            PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerNames.Add(currentYipliConfig.allPlayersInfo[i].playerName);
            playerButton = Instantiate(RightPlayerButtonPrefab, playerTilePosition, spawnrotation) as GameObject;
            playerButton.name = currentYipliConfig.allPlayersInfo[i].playerName;
            playerButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentYipliConfig.allPlayersInfo[i].playerName;
            playerButton.transform.SetParent(PlayersContainerTwo.transform, false);

            // Do not generate button to select player one's player
            if (currentYipliConfig.allPlayersInfo[i].playerName == PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne)
            {
                playerOneIndex = i;
                playerOneButton = playerButton;
                Destroy(playerButton);
                continue;
            }
            else if (currentYipliConfig.allPlayersInfo[i].playerName == PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo)
            {
                playerTwoIndex = i;
                playerTwoButton = playerButton;
            }
            generatedObjects.Add(playerButton);
            playerButtons.Add(playerButton.GetComponent<Button>());
        }
        // Add listener to all generated buttons
        for (int i = 0; i < playerButtons.Count; i++)
        {
            playerButtons[i].onClick.RemoveAllListeners();
            playerButtons[i].onClick.AddListener(SelectSecondPlayer);
        }
        MM_UIController.instance.playerTwoSelectButtons = playerButtons;
    }

    #endregion

    #region Player Selection functions

    // Function to select Player One
    public void SelectFirstPlayer()
    {
        Debug.LogError("Select P1");
        playerOneButton = EventSystem.current.currentSelectedGameObject;
        Debug.LogError("Button P1");

        // If player two's player is selected, clear selection for player two
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerOne(EventSystem.current.currentSelectedGameObject.name);
        if (EventSystem.current.currentSelectedGameObject.name == PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo)
        {
            PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerTwo(null);
        }

        // Special case- If there are only two players, then after selecting player one, other player should be selected as playre two
        if (currentYipliConfig.allPlayersInfo.Count == 2)
        {
            for (int i = 0; i < currentYipliConfig.allPlayersInfo.Count; i++)
            {
                if(currentYipliConfig.allPlayersInfo[i].playerName != PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne)
                {
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerTwo(currentYipliConfig.allPlayersInfo[i].playerName);
                    UserDataPersistence.SaveMultiplayerToDevice();
                    playerTwoNameObject.GetComponent<Animator>().SetTrigger("DefaultSelected");
                    MM_UIController.instance.BackToPlayersPanel(2);
                    return;
                }
            }
        }

        Debug.LogError("Set P1");
        // Save selected player data to data store
        UserDataPersistence.SaveMultiplayerToDevice();
        // Return back to players panel
        MM_UIController.instance.BackToPlayersPanel(1);
        Debug.LogError("Selected P1");
    }

    // Function to select Player Two
    public void SelectSecondPlayer()
    {
        playerTwoButton = EventSystem.current.currentSelectedGameObject;
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.SetPlayerTwo(EventSystem.current.currentSelectedGameObject.name);
        // Save selected player data to data store
        UserDataPersistence.SaveMultiplayerToDevice();
        // Return back to players panel
        MM_UIController.instance.BackToPlayersPanel(2);
    }

    #endregion

}
