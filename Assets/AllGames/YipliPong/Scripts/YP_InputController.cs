﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YP_InputController : MonoBehaviour
{
    public static YP_InputController instance;


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

    public void PlayerOneRight()
    {
        StartCoroutine(YP_LeftPlayerController.instance.MoveDown());
        playerOneAction = true;
    }
    public void PlayerOneLeft()
    {
        StartCoroutine(YP_LeftPlayerController.instance.MoveUp());
        playerOneAction = true;
    }

    public void PlayerTwoRight()
    {
        StartCoroutine(YP_RightPlayerController.instance.MoveUp());
        playerTwoAction = true;
    }
    public void PlayerTwoLeft()
    {
        StartCoroutine(YP_RightPlayerController.instance.MoveDown());
        playerTwoAction = true;
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
        else if (!playerOneAction && playerOneIdleTime == idleTime && YP_GameController.instance.gameState == YP_GameController.GameStates.playing)
        { 
            isDisplayingMessage = true;
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Hey " + PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOne.Substring(0, 10) + "!\nWhy arent you playing? Continue game actions to play.",2f));
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
        else if (!playerTwoAction && playerTwoIdleTime == idleTime && !isDisplayingMessage && YP_GameController.instance.gameState == YP_GameController.GameStates.playing)
        {
            isDisplayingMessage = true;
            StartCoroutine(MM_GameUIManager.instance.DisplayMessage("Hey " + PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwo.Substring(0, 10) + "! Why arent you playing? Continue game actions to play.",2f));
            yield return new WaitForSecondsRealtime(2f);
            StartCoroutine(PlayerTwoIdling());
        }
    }

}
