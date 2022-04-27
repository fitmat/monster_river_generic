using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_InputController : MonoBehaviour
{
    public static FS_InputController instance;

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
    }

    public void PlayerOneThrow()
    {
        Debug.Log("Event Test- Player 1 JUMP event FS Function Call Start");
        FS_LeftPlayerController.instance.ThrowDart();
        playerOneAction = true;
        Debug.Log("Event Test- Player 1 JUMP event FS Function Call End");
    }

    public void PlayerTwoThrow()
    {
        Debug.Log("Event Test- Player 2 JUMP event FS Function Call Start");
        FS_RightPlayerController.instance.ThrowDart();
        playerTwoAction = true;
        Debug.Log("Event Test- Player 2 JUMP event FS Function Call End");
    }


    public int idleTime;
    private bool playerOneAction, playerTwoAction, isDisplayingMessage = false;
    private int playerOneIdleTime, playerTwoIdleTime;

    public IEnumerator PlayerOneIdling()
    {
        playerOneIdleTime = 0;
        playerOneAction = false;
        isDisplayingMessage = false;
        while (!playerOneAction && playerOneIdleTime < idleTime)
        {
            playerOneIdleTime++;
            yield return new WaitForSecondsRealtime(1f);
        }
        if (playerOneAction && playerOneIdleTime < idleTime)
        {
            StartCoroutine(PlayerOneIdling());
        }
        else if (!playerOneAction && playerOneIdleTime == idleTime && FS_GameController.instance.gameState == FS_GameController.GameStates.playing)
        {
            isDisplayingMessage = true;
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Hey " + PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne.Substring(0, 10) + "!\nWhy arent you playing? Continue game actions to play.", 2f));
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(PlayerOneIdling());
        }
    }


    public IEnumerator PlayerTwoIdling()
    {
        playerTwoIdleTime = 0;
        playerTwoAction = false;
        isDisplayingMessage = false;
        while (!playerTwoAction && playerTwoIdleTime < idleTime)
        {
            playerTwoIdleTime++;
            yield return new WaitForSecondsRealtime(1f);
        }
        if (playerTwoAction && playerTwoIdleTime < idleTime)
        {
            StartCoroutine(PlayerTwoIdling());
        }
        else if (!playerTwoAction && playerTwoIdleTime == idleTime && !isDisplayingMessage && FS_GameController.instance.gameState == FS_GameController.GameStates.playing)
        {
            isDisplayingMessage = true;
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Hey " + PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo.Substring(0, 10) + "! Why arent you playing? Continue game actions to play.", 2f));
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(PlayerTwoIdling());
        }
    }
}
