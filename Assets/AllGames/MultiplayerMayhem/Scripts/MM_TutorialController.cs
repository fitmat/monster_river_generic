using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Tutorial Controller
 * Displays on opening the project
 * Checks to ensure that both players are standing on the mat before starting the game
 */

public class MM_TutorialController : MonoBehaviour
{
    #region Singleton declaration

    // Script is a singleton
    public static MM_TutorialController instance;

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

    [SerializeField] private GameObject instruction1, instruction2, player1Position, player2Position, player1Model, player2Model;
    [SerializeField] private GameObject tutorialObjects,menuObjects;
    public bool isPlayerOneReady, isPlayerTwoReady, isTutorialComplete, isPlayerOneWaiting, isPlayerTwoWaiting;

    #endregion

    #region Player Ready check functions

    // Function handles checking for player input on the Mat
    public void CheckMatInput()
    {
        Debug.Log("Tutorial Test- New Tiles action start- Is player one ready: " + isPlayerOneReady + " Is player two ready: " + isPlayerTwoReady);
        Debug.Log("Tutorial Test- Player One Detected: Values- " + MM_InputController.instance.p1X1 + " " + MM_InputController.instance.p1X2 + " " + MM_InputController.instance.p1X3 + " " + MM_InputController.instance.p1X4);
        Debug.Log("Tutorial Test- Player Two Detected: Values- " + MM_InputController.instance.p2X1 + " " + MM_InputController.instance.p2X2 + " " + MM_InputController.instance.p2X3 + " " + MM_InputController.instance.p2X4);

        try
        {
            // Checks if player one is detected
            if (MM_InputController.instance.p1X1 != 0 && MM_InputController.instance.p1X2 != 0 && MM_InputController.instance.p1X3 != 0 && MM_InputController.instance.p1X4 != 0)
            {
                // If player is detected show player as ready
                Debug.Log("Tutorial Test- Player One detected");
                isPlayerOneReady = true;
                player1Position.SetActive(false);
                player1Model.SetActive(true);
                Debug.Log("Tutorial Test- Player One set as ready");
            }
            else
            {
                // If player not detected show status as waiting
                Debug.Log("Tutorial Test- Player One NOT detected");
                isPlayerOneReady = false;
                Debug.Log("Tutorial Test- player1Position " + player1Position.name);
                player1Position.SetActive(true);
                Debug.Log("Tutorial Test- player1Model " + player1Model.name);
                player1Model.SetActive(false);
                Debug.Log("Tutorial Test- Player One set as not ready");
                StartCoroutine(WaitingForPlayerOne());
            }
            if (MM_InputController.instance.p2X1 != 0 && MM_InputController.instance.p2X2 != 0 && MM_InputController.instance.p2X3 != 0 && MM_InputController.instance.p2X4 != 0)
            {
                // If player is detected show player as ready
                Debug.Log("Tutorial Test- Player Two detected");
                isPlayerTwoReady = true;
                Debug.Log("Tutorial Test- player2Position " + player2Position.name);
                player2Position.SetActive(false);
                Debug.Log("Tutorial Test- player2Model " + player2Model.name);
                player2Model.SetActive(true);
                Debug.Log("Tutorial Test- Player Two set as ready");
            }
            else
            {
                // If player not detected show status as waiting
                Debug.Log("Tutorial Test- Player Two NOT detected");
                isPlayerTwoReady = false;
                player2Position.SetActive(true);
                player2Model.SetActive(false);
                Debug.Log("Tutorial Test- Player Two set as not ready");
                StartCoroutine(WaitingForPlayerTwo());
            }
        }
        catch (Exception e)
        {
            Debug.Log("Tutorial Test- Exception: " + e);
        }
        Debug.Log("Tutorial Test- New Tiles action end- Is player one ready: " + isPlayerOneReady + " Is player two ready: " + isPlayerTwoReady);
    }

    // Function waits for both players to be ready
    private IEnumerator PlayerReadyCheck()
    {
        Debug.Log("Tutorial Test- Starting Game Ready check");
        Debug.Log("Tutorial Test- Pre Ready check- Player One: " + isPlayerOneReady + " Player Two: " + isPlayerTwoReady);

        // While loop waits till both players are ready
        while (!isPlayerOneReady || !isPlayerTwoReady)
        {
            //try
            //{
            //    if (!isPlayerOneReady)
            //    {
            //        player1Position.SetActive(true);
            //        player1Model.SetActive(false);
            //        StartCoroutine(WaitingForPlayerOne());
            //    }
            //    if (!isPlayerTwoReady)
            //    {
            //        player2Position.SetActive(true);
            //        player2Model.SetActive(false);
            //        StartCoroutine(WaitingForPlayerTwo());
            //    }
            //}
            //catch (Exception e)
            //{
            //    Debug.Log("Tutorial Test- Exception: " + e);
            //}

            Debug.Log("Tutorial Cyclic Test- Ready check- Player One: " + isPlayerOneReady + " Player Two: " + isPlayerTwoReady);
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Tutorial Test- Post Ready check- Player One: " + isPlayerOneReady + " Player Two: " + isPlayerTwoReady);

        // After waiting end tutorial
        Debug.Log("Tutorial Test- Ending Game Ready check");
        StartCoroutine(EndTutorial());
    }

#endregion

    #region Tutorial Flow functions

    // Function handles the start of the tutorial
    public void StartTutorial()
    {
        Debug.Log("Tutorial Test- Starting Tutorial Controller Start function");
        // Disables all Main Menu objects and enables Tutorial objects
        menuObjects.SetActive(false);
        tutorialObjects.SetActive(true);
        MM_AudioManager.instance.StopAudio("MenuTheme");

        // Starts tutorial
        StartCoroutine(PlayTutorial());

        // Begins checking for player input on mat
        YipliHelper.SetGameClusterId(5,5);
        MM_InputController.instance.playerTilesEvent += CheckMatInput;

        // Sets all initial default values
        isPlayerOneReady = false;
        isPlayerTwoReady = false;
        isPlayerOneWaiting = false;
        isPlayerTwoWaiting = false;
        isTutorialComplete = false;

        MM_InputController.instance.p1X1 = 0;
        MM_InputController.instance.p1X2 = 0;
        MM_InputController.instance.p1X3 = 0;
        MM_InputController.instance.p1X4 = 0;
        MM_InputController.instance.p2X1 = 0;
        MM_InputController.instance.p2X2 = 0;
        MM_InputController.instance.p2X3 = 0;
        MM_InputController.instance.p2X4 = 0;

        // Sets flag to prevent tutorial from being replayed
        MM_InputController.instance.isTutorialPlayed = true;

        Debug.Log("Tutorial Test- Ending Tutorial Controller Start function");
    }

    // Function plays the tutorial messaged and begins checking player ready status
    private IEnumerator PlayTutorial()
    {
        MM_AudioManager.instance.PlayAudio("TutInstr1");
        instruction2.SetActive(false);
        player1Position.SetActive(false);
        player1Model.SetActive(false);
        player2Position.SetActive(false);
        player2Model.SetActive(false);
        yield return new WaitForSeconds(3f);
        instruction2.SetActive(true);
        MM_AudioManager.instance.PlayAudio("TutInstr2");
        yield return new WaitForSeconds(2f);
        CheckMatInput();
        Debug.Log("Tutorial Test- Starting ready check functions");
        StartCoroutine(PlayerReadyCheck());
        Debug.Log("Tutorial Test- Ending ready check functions");
    }

    // Function ends the tutorial
    private IEnumerator EndTutorial()
    {
        Debug.Log("Tutorial Test- Starting End Tutorial");
        Debug.Log("Tutorial Test- Game Ready");
        // Ends tutorial flow and beging normal gameplay
        YipliHelper.SetGameClusterId(0,0);
        MM_GameUIManager.instance.isExiting = false;
        isTutorialComplete = true;
        MM_AudioManager.instance.PlayAudio("Cheer");
        player1Model.GetComponentInChildren<Animator>().SetTrigger("Victory");
        player2Model.GetComponentInChildren<Animator>().SetTrigger("Victory");
        StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Player's Ready\nProceeding",2f));
        yield return new WaitForSeconds(2f);
        // Disables Tutorial objects and enables Main Menu objects
        Debug.Log("Tutorial Test- Set cluster id 0");
        menuObjects.SetActive(true);
        MM_UIController.instance.BackToHome();
        Debug.Log("Tutorial Test- Menu Objects active");
        tutorialObjects.SetActive(false);
        Debug.Log("Tutorial Test- Ending End Tutorial");
        MM_AudioManager.instance.PlayAudio("MenuTheme");
    }

#endregion

    #region Message Coroutines

    // Function handles waiting for player one and displays message if player is not detected after waiting
    private IEnumerator WaitingForPlayerOne()
    {
        if (!isPlayerOneWaiting)
        {
            isPlayerOneWaiting = true;
            yield return new WaitForSeconds(5);
            if (!isPlayerOneReady && !isTutorialComplete)
            {
                StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Player1 not detected\nEnsure you are standing properly",1.5f));
                isPlayerOneWaiting = false;
            }
        }
    }

    // Function handles waiting for player two and displays message if player is not detected after waiting
    private IEnumerator WaitingForPlayerTwo()
    {
        if (!isPlayerTwoWaiting)
        {
            isPlayerTwoWaiting = true;
            yield return new WaitForSeconds(5);
            if (!isPlayerTwoReady && !isTutorialComplete)
            {
                StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Player2 not detected\nEnsure you are standing properly",1.5f));
                isPlayerTwoWaiting = false;

            }
        }
    }

#endregion

}
