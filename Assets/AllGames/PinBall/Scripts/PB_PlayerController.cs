using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_PlayerController : MonoBehaviour
{
    public static PB_PlayerController instance;

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

    [SerializeField] private GameObject playerOneLeftPaddle, playerOneRightPaddle, playerTwoLeftPaddle, playerTwoRightPaddle;


    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerOneLeftHit();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerOneRightHit();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerTwoLeftHit();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerTwoRightHit();
        }
    }

    public void PlayerOneLeftHit()
    {
        StartCoroutine(playerOneLeftPaddle.GetComponent<PB_PaddleController>().ActivatePaddle());
    }
    public void PlayerOneRightHit()
    {
        StartCoroutine(playerOneRightPaddle.GetComponent<PB_PaddleController>().ActivatePaddle());
    }
    public void PlayerTwoLeftHit()
    {
        StartCoroutine(playerTwoLeftPaddle.GetComponent<PB_PaddleController>().ActivatePaddle());
    }
    public void PlayerTwoRightHit()
    {
        StartCoroutine(playerTwoRightPaddle.GetComponent<PB_PaddleController>().ActivatePaddle());
    }


}
