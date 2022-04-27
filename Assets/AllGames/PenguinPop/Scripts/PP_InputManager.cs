using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YipliFMDriverCommunication;

public class PP_InputManager : MonoBehaviour
{
    
    public Vector2 touchPosition;
    public event Action player1JumpEvent;
    public event Action player2JumpEvent;
    public static PP_InputManager instance;

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
        if (Input.GetKeyDown(KeyCode.Space))
            PlayerOneJump();

        if (Input.GetKeyDown(KeyCode.Return))
            PlayerTwoJump();

    }

    public void PlayerOneJump()
    {
        Debug.Log("Event Test- Player 1 JUMP event PP Function Call Start");
        try
        {
            PP_PlayerOneController.instance.Jump();
        }
        catch(Exception e)
        {
            Debug.Log("Event Test- Calling Function failed: " + e);
        }
        Debug.Log("Event Test- Player 1 JUMP event PP Function Call End");
    }

    public void PlayerTwoJump()
    {
        Debug.Log("Event Test- Player 2 JUMP event PP Function Call Start");
        PP_PlayerTwoController.instance.Jump();
        Debug.Log("Event Test- Player 2 JUMP event PP Function Call End");
    }


}
