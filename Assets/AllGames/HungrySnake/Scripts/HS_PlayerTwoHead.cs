using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_PlayerTwoHead : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HS_Boundary") || collision.gameObject.CompareTag("HS_Player"))
        {
            Debug.Log("PlayerTwoDamage");
            StartCoroutine(HS_PlayerTwoController.instance.TakeDamage());
        }

        if (collision.gameObject.CompareTag("HS_LeftHole"))
        {
            StartCoroutine(HS_PlayerTwoController.instance.SpawnAtRight());
        }
        if (collision.gameObject.CompareTag("HS_RightHole"))
        {
            StartCoroutine(HS_PlayerTwoController.instance.SpawnAtLeft());
        }


        if (collision.gameObject.CompareTag("HS_Fruit"))
        {
            StartCoroutine(HS_PlayerTwoController.instance.IncreaseLength(1));
            collision.gameObject.GetComponent<HS_ObjectController>().CallCollect();
            HS_AudioManager.instance.PlayAudio("Eat");
        }

        if (collision.gameObject.CompareTag("HS_Bomb"))
        {
            StartCoroutine(HS_PlayerTwoController.instance.TakeDamage());
            collision.gameObject.GetComponent<HS_ObjectController>().CallCollect();
        }
    }
}
