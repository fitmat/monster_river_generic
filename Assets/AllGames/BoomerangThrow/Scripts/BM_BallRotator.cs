using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_BallRotator : MonoBehaviour
{
    void Update()
    {
        if (BM_GameController.instance.gameState == BM_GameController.GameStates.playing)
        {
            transform.Rotate(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
        }
    }

}