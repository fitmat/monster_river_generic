using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_InputController : MonoBehaviour
{
    public static HS_InputController instance;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerTwoLeftTap();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerTwoRightTap();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerOneLeftTap();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerOneRightTap();
        }
    }

    public void PlayerOneRightTap()
    {
        if (HS_GameController.instance.gameState == HS_GameController.GameStates.playing)
        {
            StartCoroutine(HS_PlayerOneController.instance.TurnRight());
        }
    }
    public void PlayerTwoRightTap()
    {
        if (HS_GameController.instance.gameState == HS_GameController.GameStates.playing)
        {
            StartCoroutine(HS_PlayerTwoController.instance.TurnRight());
        }
    }
    public void PlayerOneLeftTap()
    {
        if (HS_GameController.instance.gameState == HS_GameController.GameStates.playing)
        {
            StartCoroutine(HS_PlayerOneController.instance.TurnLeft());
        }
    }
    public void PlayerTwoLeftTap()
    {
        if (HS_GameController.instance.gameState == HS_GameController.GameStates.playing)
        {
            StartCoroutine(HS_PlayerTwoController.instance.TurnLeft());
        }
    }
}
