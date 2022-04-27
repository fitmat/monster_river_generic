using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YipliFMDriverCommunication;

public class TR_InputManager : MonoBehaviour
{
    public static TR_InputManager instance;
    public event Action player1JumpEvent;
    public event Action player2JumpEvent;


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            RightShoot();
        if (Input.GetKeyDown(KeyCode.D))
            LeftShoot();
        if (Input.GetKeyDown(KeyCode.Return))
            MoveRaft();
    }

    public void RightShoot()
    {
        Debug.Log("Event Test- Player 1 RIGHTMOVE event TR Function Call Start");
        TR_LeftPlayerController.instance.ShootBackRight();
        Debug.Log("Event Test- Player 1 RIGHTMOVE event TR Function Call End");
    }

    public void LeftShoot()
    {
        Debug.Log("Event Test- Player 1 LEFTMOVE event TR Function Call Start");
        TR_LeftPlayerController.instance.ShootBackLeft();
        Debug.Log("Event Test- Player 1 LEFTMOVE event TR Function Call End");
    }

    public void MoveRaft()
    {
        Debug.Log("Event Test- Player 2 JUMP event TR Function Call Start");
        TR_RightPlayerController.instance.MoveRaft();
        Debug.Log("Event Test- Player 2 JUMP event TR Function Call End");
    }

}
