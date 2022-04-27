using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_InputController : MonoBehaviour
{
    public static PB_InputController instance;

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
        PB_PlayerController.instance.PlayerOneRightHit();
    }
    public void PlayerOneLeft()
    {
        PB_PlayerController.instance.PlayerOneLeftHit();
    }

    public void PlayerTwoRight()
    {
        PB_PlayerController.instance.PlayerTwoRightHit();
    }
    public void PlayerTwoLeft()
    {
        PB_PlayerController.instance.PlayerTwoLeftHit();
    }
}
