using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HB_InputController : MonoBehaviour
{
    [SerializeField] private HB_PlayerController playerOne, playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(playerOne.MoveAhead());
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(playerOne.MoveBack());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(playerOne.Jump());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(playerTwo.MoveAhead());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(playerTwo.MoveBack());
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(playerTwo.Jump());
        }


    }
}
