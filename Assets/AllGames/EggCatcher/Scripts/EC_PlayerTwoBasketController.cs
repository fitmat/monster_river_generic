﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_PlayerTwoBasketController : MonoBehaviour
{
    [SerializeField] private GameObject whiteParticles, goldParticles, blackParticles;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect type of egg caught
        if (collision.gameObject.tag == "EC_WEgg")
        {
            if (EC_GameController.instance.gameState == EC_GameController.GameStates.playing)
            {
                collision.gameObject.SetActive(false);
                EC_AudioManager.instance.PlayAudio("Collect");
                EC_GameController.instance.changeScore(2, 1);
                Instantiate(whiteParticles, transform.parent);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "EC_GEgg")
        {
            if (EC_GameController.instance.gameState == EC_GameController.GameStates.playing)
            {
                collision.gameObject.SetActive(false);
                EC_AudioManager.instance.PlayAudio("Collect");
                EC_GameController.instance.changeScore(2, 2);
                Instantiate(goldParticles, transform.parent);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "EC_BEgg")
        {
            if (EC_GameController.instance.gameState == EC_GameController.GameStates.playing)
            {
                collision.gameObject.SetActive(false);
                EC_GameController.instance.changeScore(2, -1);
                Instantiate(blackParticles, transform.parent);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
