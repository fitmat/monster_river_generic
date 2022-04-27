using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW_GameEndController : MonoBehaviour
{

    public static ToW_GameEndController instance;

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

    [SerializeField] private GameObject playerOne, playerTwo;

    public void FindPlayers()
    {
        playerOne = GameObject.FindGameObjectWithTag("PlayerOne").transform.GetChild(0).gameObject;
        playerTwo = GameObject.FindGameObjectWithTag("PlayerTwo").transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Game Should End");
        if (collision.gameObject.tag == "PlayerOne")
        {
            Debug.Log("Player One Loses");
            ToW_GameController.instance.winner = 2;

            playerTwo.GetComponent<Animator>().ResetTrigger("Reset");
            playerOne.GetComponent<Animator>().ResetTrigger("Reset");

            playerOne.GetComponent<Animator>().SetTrigger("Lose");
            playerTwo.GetComponent<Animator>().SetTrigger("Win");

            StartCoroutine(ToW_GameController.instance.EndRound());
        }
        else if (collision.gameObject.tag == "PlayerTwo")
        {
            Debug.Log("Player Two Loses");
            ToW_GameController.instance.winner = 1;

            playerTwo.GetComponent<Animator>().ResetTrigger("Reset");
            playerOne.GetComponent<Animator>().ResetTrigger("Reset");

            playerTwo.GetComponent<Animator>().SetTrigger("Lose");
            playerOne.GetComponent<Animator>().SetTrigger("Win");


            StartCoroutine(ToW_GameController.instance.EndRound());
        }
    }
}
