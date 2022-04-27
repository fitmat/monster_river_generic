using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script handles catching fish in basket on collision

public class FTv2_BasketController : MonoBehaviour
{
    [SerializeField] FTv2_PlayerController playerController;
    private FTv2_FishController fishController;
    private GameObject collisionObject;

    private void OnTriggerEnter(Collider other)
    {
        collisionObject = other.gameObject;

        if (collisionObject.CompareTag("FT_SimpleFish"))
        {
            fishController = collisionObject.GetComponent<FTv2_FishController>();

            if (playerController.isCatching && fishController.isJumpingDown)
            {
                Debug.Log("Catch!");
                fishController.CatchFish();
                StartCoroutine(playerController.CatchFishSuccess());
            }
        }

        if (collisionObject.CompareTag("FT_DecoyFish"))
        {
            fishController = collisionObject.GetComponent<FTv2_FishController>();

            if (playerController.isCatching && fishController.isJumpingDown)
            {
                Debug.Log("Catch!");
                fishController.CatchFish();
                StartCoroutine(playerController.CatchFishFail());
            }
        }
    }
}
