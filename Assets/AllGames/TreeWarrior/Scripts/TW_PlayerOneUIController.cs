using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TW_PlayerOneUIController : MonoBehaviour
{
    public static TW_PlayerOneUIController instance;

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

    public event Action<int> playerOneMove;
    // Directions: 0-Left, 1-Up, 2-Right
    public int playerMoveIndex;
    public GameObject leftButton, rightButton, upButton;

    private void Start()
    {
        //if (PlayerSession.Instance.currentYipliConfig.onlyMatPlayMode)
        //{
        //    leftButton.SetActive(false);
        //    rightButton.SetActive(false);
        //    upButton.SetActive(false);
        //}
    }

    public void LeftButton()
    {
        Debug.Log("Player One Left Button");
        playerMoveIndex = 0;
        playerOneMove?.Invoke(playerMoveIndex);
    }

    public void UpButton()
    {
        Debug.Log("Player One Up Button");
        playerMoveIndex = 1;
        playerOneMove?.Invoke(playerMoveIndex);
    }

    public void RightButton()
    {
        Debug.Log("Player One Right Button");
        playerMoveIndex = 2;
        playerOneMove?.Invoke(playerMoveIndex);
    }

}
