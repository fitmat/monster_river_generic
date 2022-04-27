using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_Floor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check for collision with player block and Lose condition
        if (gameObject.tag == "Ground")
        {
            if (other.CompareTag("TB_PlayerOneBlock"))
            {
                TB_AudioManager.instance.PlayAudio("BuildingCollapse");
                MM_GameUIManager.instance.winnerNumber = 2;
                StartCoroutine(TB_GameController.instance.DelayGameOver());
            }
            if (other.CompareTag("TB_PlayerTwoBlock"))
            {
                TB_AudioManager.instance.PlayAudio("BuildingCollapse");
                MM_GameUIManager.instance.winnerNumber = 1;
                StartCoroutine(TB_GameController.instance.DelayGameOver());
            }
        }
        else if (gameObject.tag == "P1Lose")
        {
            if (other.CompareTag("TB_PlayerOneBlock"))
            {
                TB_AudioManager.instance.PlayAudio("BuildingCollapse");
                MM_GameUIManager.instance.winnerNumber = 2;
                StartCoroutine(TB_GameController.instance.DelayGameOver());
            }
        }
        else if (gameObject.tag == "P2Lose")
        {
            if (other.CompareTag("TB_PlayerTwoBlock"))
            {
                TB_AudioManager.instance.PlayAudio("BuildingCollapse");
                MM_GameUIManager.instance.winnerNumber = 1;
                StartCoroutine(TB_GameController.instance.DelayGameOver());
            }
        }
    }
}
