using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_PlayerOneHead : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HS_Boundary") || collision.gameObject.CompareTag("HS_Player"))
        {
            Debug.Log("PlayerOneDamage");
            StartCoroutine(HS_PlayerOneController.instance.TakeDamage());
        }

        if (collision.gameObject.CompareTag("HS_LeftHole"))
        {
            StartCoroutine(HS_PlayerOneController.instance.SpawnAtRight());
        }
        if (collision.gameObject.CompareTag("HS_RightHole"))
        {
            StartCoroutine(HS_PlayerOneController.instance.SpawnAtLeft());
        }

        if (collision.gameObject.CompareTag("HS_Fruit"))
        {
            StartCoroutine(HS_PlayerOneController.instance.IncreaseLength(1));
            collision.gameObject.GetComponent<HS_ObjectController>().CallCollect();
            HS_AudioManager.instance.PlayAudio("Eat");
        }

        if (collision.gameObject.CompareTag("HS_Bomb"))
        {
            StartCoroutine(HS_PlayerOneController.instance.TakeDamage());
            collision.gameObject.GetComponent<HS_ObjectController>().CallCollect();
        }
    }
}
