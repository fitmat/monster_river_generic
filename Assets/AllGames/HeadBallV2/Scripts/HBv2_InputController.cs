using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBv2_InputController : MonoBehaviour
{
    public static HBv2_InputController instance;

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

    [SerializeField] private HBv2_PlayerController playerOne, playerTwo;

    public void PlayerOneLeft()
    {
        if (HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            StartCoroutine(playerOne.MoveLeft());
            playerOneAction = true;
        }
    }
    public void PlayerOneRight()
    {
        if (HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            StartCoroutine(playerOne.MoveRight());
            playerOneAction = true;
        }
    }
    public void PlayerOneJump()
    {
        if (HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            StartCoroutine(playerOne.Jump());
            playerOneAction = true;
        }
    }
    public void PlayerTwoLeft()
    {
        if (HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            StartCoroutine(playerTwo.MoveLeft());
            playerTwoAction = true;
        }
    }
    public void PlayerTwoRight()
    {
        if (HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            StartCoroutine(playerTwo.MoveRight());
            playerTwoAction = true;
        }
    }
    public void PlayerTwoJump()
    {
        if (HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            StartCoroutine(playerTwo.Jump());
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
        else if (!playerOneAction && playerOneIdleTime == idleTime && HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
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
        else if (!playerTwoAction && playerTwoIdleTime == idleTime && !isDisplayingMessage && HBv2_GameController.instance.gameState == HBv2_GameController.GameStates.playing)
        {
            isDisplayingMessage = true;
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Hey " + PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo.Substring(0, 10) + "! Why arent you playing? Continue game actions to play.", 2f));
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(PlayerTwoIdling());
        }
    }
}
