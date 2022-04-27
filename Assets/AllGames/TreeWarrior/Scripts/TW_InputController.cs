using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YipliFMDriverCommunication;

public class TW_InputController : MonoBehaviour
{
    public static TW_InputController instance;

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

    public event Action<int> playerOneMove, playerTwoMove;

    public int playerOneMoveIndex, playerTwoMoveIndex;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            P1Jump();
        if (Input.GetKeyDown(KeyCode.A))
            P1Left();
        if (Input.GetKeyDown(KeyCode.D))
            P1Right();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            P2Jump();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            P2Left();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            P2Right();
    }

    public void P1Left()
    {
        Debug.Log("Event Test- Player 1 LEFTMOVE event TW Function Call Start");
        playerOneMoveIndex = 0;
        playerOneMove?.Invoke(playerOneMoveIndex);
        Debug.Log("Event Test- Player 1 LEFTMOVE event TW Function Call End");
    }
    public void P1Right()
    {
        Debug.Log("Event Test- Player 1 RIGHTMOVE event TW Function Call Start");
        playerOneMoveIndex = 2;
        playerOneMove?.Invoke(playerOneMoveIndex);
        Debug.Log("Event Test- Player 1 RIGHTMOVE event TW Function Call End");
    }
    public void P1Jump()
    {
        Debug.Log("Event Test- Player 1 JUMP event TW Function Call Start");
        playerOneMoveIndex = 1;
        playerOneMove?.Invoke(playerOneMoveIndex);
        Debug.Log("Event Test- Player 1 JUMP event TW Function Call End");
    }

    public void P2Left()
    {
        Debug.Log("Event Test- Player 2 LEFTMOVE event TW Function Call Start");
        playerTwoMoveIndex = 0;
        playerTwoMove?.Invoke(playerTwoMoveIndex);
        Debug.Log("Event Test- Player 2 LEFTMOVE event TW Function Call End");
    }
    public void P2Right()
    {
        Debug.Log("Event Test- Player 2 RIGHTMOVE event TW Function Call Start");
        playerTwoMoveIndex = 2;
        playerTwoMove?.Invoke(playerTwoMoveIndex);
        Debug.Log("Event Test- Player 2 RIGHTMOVE event TW Function Call End");
    }
    public void P2Jump()
    {
        Debug.Log("Event Test- Player 2 JUMP event TW Function Call Start");
        playerTwoMoveIndex = 1;
        playerTwoMove?.Invoke(playerTwoMoveIndex);
        Debug.Log("Event Test- Player 2 JUMP event TW Function Call End");
    }

}
