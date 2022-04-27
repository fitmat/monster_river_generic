using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Mat Button Controller
 * Handles the global UI control across all scenes
 * Maintains button list
 * Maintains UI panels
 * 
 * Global script will be active across all scenes
 */

public class MM_MatButtonController : MonoBehaviour
{
    #region Dont Destroy on Load Singleton declaration

    public static MM_MatButtonController instance;

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
    }

    #endregion

    #region Variable declaration

    public List<Button> currentButtonList;
    public List<Button> resultsScreenButtons;
    public Button currentButton, resultScreenCurrentButton /* This is result Screen Currrent selected button */;


    public int currentButtonIndex, resultScreenButtonIndex;
    public bool isInResultScreen;


    public enum Panels { mainMenu, gameSelect, changePlayers, playerOneSelect, playerTwoSelect, resultsScreen };
    public Panels activePanel;

    #endregion

    #region Unity directives

    private void Start()
    {
        currentButtonIndex = 0;
        isInResultScreen = false;
    }

    private void Update()
    {
        // Handles Keyboard Input
        GetMatKeyInputs();
    }

    #endregion

    #region Active Panel handler functions

    // Function to handle Main Menu panel active
    public IEnumerator SetMainMenuActive()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Debug.Log("UI Test- Active Panel- Main Menu");
        activePanel = Panels.mainMenu;
        currentButtonList = MM_UIController.instance.mainMenuButtons;
        currentButtonIndex = 0;

        currentButton = currentButtonList[currentButtonIndex];
        Debug.Log("UI Test- Selected Button- " + currentButton.name);
        currentButton.Select();
    }

    // Function to handle Players panel active
    public IEnumerator SetChangePlayersActive(int defaultIndex)
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Debug.Log("UI Test- Active Panel- Change Players");
        activePanel = Panels.changePlayers;
        currentButtonList = MM_UIController.instance.changePlayersButtons;
        currentButtonIndex = defaultIndex;

        currentButton = currentButtonList[currentButtonIndex];
        Debug.Log("UI Test- Selected Button- " + currentButton.name);
        currentButton.Select();
    }

    // Function to handle Select Player One list active
    public IEnumerator SetPlayerOneSelectActive()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Debug.Log("UI Test- Active Panel- Player One Select");
        activePanel = Panels.playerOneSelect;
        currentButtonList = MM_UIController.instance.playerOneSelectButtons;
        currentButtonIndex = 0;

        currentButton = currentButtonList[currentButtonIndex];
        Debug.Log("UI Test- Selected Button- " + currentButton.name);
        currentButton.Select();
    }

    // Function to handle Select Player Two list active
    public IEnumerator SetPlayerTwoSelectActive()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Debug.Log("UI Test- Active Panel- Player Two Select");
        activePanel = Panels.playerTwoSelect;
        currentButtonList = MM_UIController.instance.playerTwoSelectButtons;
        currentButtonIndex = 0;

        currentButton = currentButtonList[currentButtonIndex];
        Debug.Log("UI Test- Selected Button- " + currentButton.name);
        currentButton.Select();
    }

    // Function to handle Select Game panel active
    public IEnumerator SetGameSelectActive()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        MM_SelectGame.instance.HighlightButton();
        Debug.Log("UI Test- Active Panel- Game Select");
        activePanel = Panels.gameSelect;
        currentButtonList = MM_UIController.instance.gameSelectButtons;
        currentButtonIndex = 0;

        currentButton = currentButtonList[currentButtonIndex];
        Debug.Log("UI Test- Selected Button- " + currentButton.name);
        currentButton.Select();
    }

    // Function to handle Report Card screen active
    public IEnumerator SetResultsScreenActive()
    {
        resultScreenButtonIndex = 0;
        Debug.Log("Result Test- UI Test- Active Panel- Results Select");

        yield return new WaitForSecondsRealtime(0.25f);
        try
        {
            activePanel = Panels.resultsScreen;
            //currentButtonList = resultsScreenButtons;

            resultScreenCurrentButton = resultsScreenButtons[resultScreenButtonIndex];
            Debug.Log("Result Test- UI Test- Selected Button- " + resultScreenCurrentButton.name);
            resultScreenCurrentButton.Select();
        }
        catch (Exception e)
        {
            Debug.Log("Result Test- Fail in second section: " + e.Message);
        }
    }

    #endregion

    #region Keyboard Input function

    // Function to handle Keyboard Input
    private void GetMatKeyInputs()
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            MM_AudioManager.instance.PlayAudio("Click");
            if (activePanel == Panels.gameSelect)
            {
                MM_SelectGame.instance.ButtonClick();
            }
        }
    }

    #endregion

    #region Button Scrolling functions

    // Function to handle Left tap or Left arrow key click
    public void GetPreviousButton()
    {
        // Seperate handling for result screen
        if (isInResultScreen)
        {
            Debug.Log("Result Test- UI Test- current button index pre decrement- " + resultScreenButtonIndex);
            resultScreenButtonIndex--;
            Debug.Log("Result Test- UI Test- current button index post decrement- " + resultScreenButtonIndex);
            if (resultScreenButtonIndex < 0)
            {
                resultScreenButtonIndex = resultsScreenButtons.Count - 1;
            }
            Debug.Log("Result Test- UI Test- current button index post cycling- " + resultScreenButtonIndex);
            for (int i = 0; i < resultsScreenButtons.Count; i++)
            {
                Debug.Log("Result Test- UI Test- " + i + " button name- " + resultsScreenButtons[i].gameObject.name);
            }
            try
            {
                resultScreenCurrentButton = resultsScreenButtons[resultScreenButtonIndex];
            }
            catch (Exception e)
            {
                Debug.Log("Result Test- UI Test- Error- " + e.Message);
            }
            Debug.Log("Result Test- UI Test- Selected Button- " + resultScreenCurrentButton.name);
            resultScreenCurrentButton.Select();
        }
        else 
        {
            Debug.Log("Action Test- Getting Previous Button");
            currentButtonIndex--;
            if (currentButtonIndex < 0)
            {
                currentButtonIndex = currentButtonList.Count - 1;
            }
            Debug.Log("Cycled Button List");
            Debug.Log("Action Test- UI Test- current button list length- " + currentButtonList.Count);
            Debug.Log("Action Test- UI Test- current button index- " + currentButtonIndex);
            for (int i = 0; i < currentButtonList.Count; i++)
            {
                Debug.Log("Action Test- UI Test- i button name- " + currentButtonList[i].gameObject.name);
            }
            try
            {
                currentButton = currentButtonList[currentButtonIndex];
            }
            catch(Exception e)
            {
                Debug.Log("Action Test- UI Test- Error- " + e);
            }
            Debug.Log("Action Test- UI Test- Selected Button- " + currentButton.gameObject.name);
            if (activePanel == Panels.playerOneSelect)
            {
                MM_UIController.instance.ScrollPlayerOneList(currentButtonIndex);
            }
            if (activePanel == Panels.playerTwoSelect)
            {
                MM_UIController.instance.ScrollPlayerTwoList(currentButtonIndex);
            }
            currentButton.Select();
        }
        if (MM_SelectGame.instance.inSelectGamePanel)
        {
            // Unique functionality for scrolling games list
            StartCoroutine(MM_SelectGame.instance.GetPreviousButton());
            StartCoroutine(MM_UIController.instance.ScrollGamesList(currentButtonIndex));
        }
    }

    // Function to handle Right tap or Right arrow key click
    public void GetNextButton()
    {
        // Seperate handling for result screen
        if (isInResultScreen)
        {
            Debug.Log("Result Test- UI Test- current button index pre increment- " + resultScreenButtonIndex);
            resultScreenButtonIndex++;
            Debug.Log("Result Test- UI Test- current button index post increment- " + resultScreenButtonIndex);
            if (resultScreenButtonIndex >= resultsScreenButtons.Count)
            {
                resultScreenButtonIndex = 0;
            }
            Debug.Log("Result Test- UI Test- current button index post cycling- " + resultScreenButtonIndex);
            for (int i = 0; i < resultsScreenButtons.Count; i++)
            {
                Debug.Log("Result Test- UI Test- " + i + " button name- " + resultsScreenButtons[i].gameObject.name);
            }
            try
            {
                resultScreenCurrentButton = resultsScreenButtons[resultScreenButtonIndex];
            }
            catch (Exception e)
            {
                Debug.Log("Result Test- UI Test- Error- " + e.Message);
            }
            Debug.Log("Result Test- UI Test- Selected Button- " + resultScreenCurrentButton.name);
            resultScreenCurrentButton.Select();
        }
        else
        {
            Debug.Log("Action Test- Getting Next Button");
            currentButtonIndex++;
            if (currentButtonIndex >= currentButtonList.Count)
            {
                currentButtonIndex = 0;
            }
            Debug.Log("Cycled Button List");
            Debug.Log("Action Test- UI Test- current button list length- " + currentButtonList.Count);
            Debug.Log("Action Test- UI Test- current button index- " + currentButtonIndex);
            for (int i = 0; i < currentButtonList.Count; i++)
            {
                Debug.Log("Action Test- UI Test- i button name- " + currentButtonList[i].gameObject.name);
            }
            try
            {
                currentButton = currentButtonList[currentButtonIndex];
            }
            catch (Exception e)
            {
                Debug.Log("Action Test- UI Test- Error- " + e);
            }
            Debug.Log("Action Test- UI Test- Selected Button- " + currentButton.gameObject.name);
            if (activePanel == Panels.playerOneSelect)
            {
                MM_UIController.instance.ScrollPlayerOneList(currentButtonIndex);
            }
            if (activePanel == Panels.playerTwoSelect)
            {
                MM_UIController.instance.ScrollPlayerTwoList(currentButtonIndex);
            }
            currentButton.Select();
        }
        if (MM_SelectGame.instance.inSelectGamePanel)
        {
            // Unique functionality for scrolling games list
            StartCoroutine(MM_SelectGame.instance.GetNextButton());
            StartCoroutine(MM_UIController.instance.ScrollGamesList(currentButtonIndex));
        }
    }

    // Function to handle On Click of button on user Jump
    public void SelectButton()
    {
        MM_AudioManager.instance.PlayAudio("Click");
        Debug.Log("Event Test- ENTER event handling started");
        if (isInResultScreen)
        {
            resultScreenCurrentButton.onClick.Invoke();
        }
        else if (!MM_SelectGame.instance.inSelectGamePanel)
        {
            Debug.Log("Event Test- ENTER event handling out of game select");
            Debug.Log("Event Test- Active panel- "+activePanel);
            Debug.Log("Selecting Button- " + currentButton.name);
            currentButton.onClick.Invoke();
        }
        else
        {
            if (activePanel == Panels.gameSelect)
            {
                Debug.Log("Event Test- ENTER event handling in game select");
                MM_SelectGame.instance.ButtonClick();
            }
        }
    }

    #endregion

}
