using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_InputController : MonoBehaviour
{
    public bool isKinematicMode;

    [SerializeField] private RG_BikeController playerOneController;//, playerTwoController;
    [SerializeField] private RG_BikeKinematic playerOneKinematic;//, playerTwoKinematic;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerOneAccelerate();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //PlayerOneBreak();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerOneRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerOneLeft();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerOneJump();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerTwoAccelerate();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //PlayerTwoBreak();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerTwoRight();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerTwoLeft();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerTwoJump();
        }
    }

    public void PlayerOneAccelerate()
    {
        if (isKinematicMode)
        {
            playerOneKinematic.StartMoving();
        }
        else
        {
            StartCoroutine(playerOneController.StartAccelerating());
        }
    }

    public void PlayerOneBreak()
    {
        if (isKinematicMode)
        {
            playerOneKinematic.StopMoving();
        }
        else
        {
            StartCoroutine(playerOneController.StopAccelerating());
        }
    }

    public void PlayerOneRight()
    {
        if (isKinematicMode)
        {
            StartCoroutine(playerOneKinematic.TurnRight());
        }
        else
        {
            StartCoroutine(playerOneController.SteerRight());
        }
    }

    public void PlayerOneLeft()
    {
        if (isKinematicMode)
        {
            StartCoroutine(playerOneKinematic.TurnLeft());
        }
        else
        {
            StartCoroutine(playerOneController.SteerLeft());
        }
    }

    public void PlayerOneJump()
    {
        if (isKinematicMode)
        {
            playerOneKinematic.Jump();
        }
        else
        {
            playerOneController.Jump();
        }
    }

    public void PlayerTwoAccelerate()
    {
        if (isKinematicMode)
        {
            //playerTwoKinematic.StartMoving();
        }
        else
        {
            //StartCoroutine(playerTwoController.StartAccelerating());
        }
    }

    public void PlayerTwoBreak()
    {
        if (isKinematicMode)
        {
            //playerTwoKinematic.StopMoving();
        }
        else
        {
            //StartCoroutine(playerTwoController.StopAccelerating());
        }
    }

    public void PlayerTwoRight()
    {
        if (isKinematicMode)
        {
            //StartCoroutine(playerTwoKinematic.TurnRight());
        }
        else
        {
            //StartCoroutine(playerTwoController.SteerRight());
        }
    }

    public void PlayerTwoLeft()
    {
        if (isKinematicMode)
        {
            //StartCoroutine(playerTwoKinematic.TurnLeft());
        }
        else
        {
           // StartCoroutine(playerTwoController.SteerLeft());
        }
    }

    public void PlayerTwoJump()
    {
        if (isKinematicMode)
        {
            //playerTwoKinematic.Jump();
        }
        else
        {
            //playerTwoController.Jump();
        }
    }

}
