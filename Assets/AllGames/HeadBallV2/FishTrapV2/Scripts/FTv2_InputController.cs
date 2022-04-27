using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTv2_InputController : MonoBehaviour
{
    public static FTv2_InputController instance;

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

    [SerializeField] FTv2_PlayerController leftPlayerController, rightPlayerController;



    public void PlayerOneLeft()
    {
        if (FTv2_GameController.instance.gameState == FTv2_GameController.GameStates.playing)
        {
            StartCoroutine(leftPlayerController.CatchLeft());
            playerOneAction = true;
        }
    }
    public void PlayerOneRight()
    {
        if (FTv2_GameController.instance.gameState == FTv2_GameController.GameStates.playing)
        {
            StartCoroutine(leftPlayerController.CatchRight());
            playerOneAction = true;
        }
    }
    public void PlayerTwoLeft()
    {
        if (FTv2_GameController.instance.gameState == FTv2_GameController.GameStates.playing)
        {
            StartCoroutine(rightPlayerController.CatchLeft());
            playerTwoAction = true;
        }
    }
    public void PlayerTwoRight()
    {
        if (FTv2_GameController.instance.gameState == FTv2_GameController.GameStates.playing)
        {
            StartCoroutine(rightPlayerController.CatchRight());
            playerTwoAction = true;
        }
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
        else if (!playerOneAction && playerOneIdleTime == idleTime && FTv2_GameController.instance.gameState == FTv2_GameController.GameStates.playing)
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
        else if (!playerTwoAction && playerTwoIdleTime == idleTime && !isDisplayingMessage && FTv2_GameController.instance.gameState == FTv2_GameController.GameStates.playing)
        {
            isDisplayingMessage = true;
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Hey " + PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo.Substring(0, 10) + "! Why arent you playing? Continue game actions to play.", 2f));
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(PlayerTwoIdling());
        }
    }
}
