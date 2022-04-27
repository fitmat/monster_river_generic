using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YipliFMDriverCommunication;

public class ToW_InputController : MonoBehaviour
{
    public static ToW_InputController instance;

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


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //KeyboardInput();
#endif
    }

    public void PullLeftJump()
    {
        Debug.Log("Event Test- Player 1 JUMP event ToW Function Call Start");
        FindObjectOfType<ToW_GameplayController>().PullLeft(350);
        Debug.Log("Event Test- Player 1 JUMP event ToW Function Call End");
    }
    public void PullRightJump()
    {
        Debug.Log("Event Test- Player 2 JUMP event ToW Function Call Start");
        FindObjectOfType<ToW_GameplayController>().PullRight(350);
        Debug.Log("Event Test- Player 2 JUMP event ToW Function Call End");
    }

    public void PullLeftRun()
    {
        Debug.Log("Event Test- Player 1 RUNNING ToW Function Call Start");
        FindObjectOfType<ToW_GameplayController>().PullLeft(95);
        Debug.Log("Event Test- Player 1 RUNNING ToW Function Call End");
    }
    public void PullRightRun()
    {
        Debug.Log("Event Test- Player 2 RUNNING event ToW Function Call Start");
        FindObjectOfType<ToW_GameplayController>().PullRight(95);
        Debug.Log("Event Test- Player 2 RUNNING event ToW Function Call End");
    }

    public void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            FindObjectOfType<ToW_GameplayController>().PullLeft(400);
            //ToW2_Player1LeftController.instance.PullLeft(150);
        }
        // On pressing Return key, call player 2 jump event if not in single player mode
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Return");
            FindObjectOfType<ToW_GameplayController>().PullRight(400);
        }
    }

}
