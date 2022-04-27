using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHv2_InputController : MonoBehaviour
{
    [SerializeField] PHv2_PlayerController playerOneController, playerTwoController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(playerOneController.MakeAction());
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(playerTwoController.MakeAction());
        }
    }

}
