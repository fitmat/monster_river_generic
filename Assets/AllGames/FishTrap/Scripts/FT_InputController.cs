using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_InputController : MonoBehaviour
{

    public static FT_InputController instance;

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
        if (FT_GameController.instance.gameState == FT_GameController.GameStates.playing)
        {
            StartCoroutine(FT_LeftPlayerController.instance.CatchLeft());
        }
    }

    public void PlayerOneLeft()
    {
        if (FT_GameController.instance.gameState == FT_GameController.GameStates.playing)
        {
            StartCoroutine(FT_LeftPlayerController.instance.CatchRight());
        }
    }

    public void PlayerTwoRight()
    {
        if (FT_GameController.instance.gameState == FT_GameController.GameStates.playing)
        {
            StartCoroutine(FT_RightPlayerController.instance.CatchLeft());
        }
    }

    public void PlayerTwoLeft()
    {
        if (FT_GameController.instance.gameState == FT_GameController.GameStates.playing)
        {
            StartCoroutine(FT_RightPlayerController.instance.CatchRight());
        }
    }
}
