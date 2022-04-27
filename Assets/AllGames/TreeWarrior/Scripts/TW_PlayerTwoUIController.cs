using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TW_PlayerTwoUIController : MonoBehaviour
{
    public static TW_PlayerTwoUIController instance;

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

    public event Action<int> playerTwoMove;
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
        Debug.Log("Player Two Left Button");
        playerMoveIndex = 0;
        playerTwoMove?.Invoke(playerMoveIndex);
    }

    public void UpButton()
    {
        Debug.Log("Player Two Up Button");
        playerMoveIndex = 1;
        playerTwoMove?.Invoke(playerMoveIndex);
    }

    public void RightButton()
    {
        Debug.Log("Player Two Right Button");
        playerMoveIndex = 2;
        playerTwoMove?.Invoke(playerMoveIndex);
    }

}
