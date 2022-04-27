using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YipliFMDriverCommunication;

public class MR_InputManager : MonoBehaviour
{
    public static MR_InputManager instance;

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

    public void ShootFrontLeft()
    {
        Debug.Log("Event Test- Player 2 L_LEG_HOPPING event MR Function Call Start");
        MR_RightPlayerController.instance.ShootFrontRight();
        Debug.Log("Event Test- Player 2 L_LEG_HOPPING event MR Function Call End");
    }
    public void ShootFrontRight()
    {
        Debug.Log("Event Test- Player 2 R_LEG_HOPPING event MR Function Call Start");
        MR_RightPlayerController.instance.ShootFrontLeft();
        Debug.Log("Event Test- Player 2 R_LEG_HOPPING event MR Function Call End");
    }
    public void ShootBackLeft()
    {
        Debug.Log("Event Test- Player 1 L_LEG_HOPPING event MR Function Call Start");
        MR_LeftPlayerController.instance.ShootBackLeft();
        Debug.Log("Event Test- Player 1 L_LEG_HOPPING event MR Function Call End");
    }
    public void ShootBackRight()
    {
        Debug.Log("Event Test- Player 1 R_LEG_HOPPING event MR Function Call Start");
        MR_LeftPlayerController.instance.ShootBackRight();
        Debug.Log("Event Test- Player 1 R_LEG_HOPPING event MR Function Call End");
    }

}
