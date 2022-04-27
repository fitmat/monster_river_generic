using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handles water VFX from hole on Fish jumping out and in

public class FTv2_HoleController : MonoBehaviour
{
    private GameObject collisionObject;

    [SerializeField] ParticleSystem splashEffect, splashEffect2, rippleEffect;

    private void OnTriggerEnter(Collider other)
    {
        collisionObject = other.gameObject;
        if (collisionObject.CompareTag("FT_SimpleFish"))
        {
            FTv2_FishController _fishController = collisionObject.GetComponent<FTv2_FishController>();

            if (_fishController.isJumpingUp)
            {
                rippleEffect.Play();
            }
            else if (_fishController.isJumpingDown && !_fishController.isCaught)
            {
                splashEffect.Play();
            }
        }
        else if (collisionObject.CompareTag("FT_DecoyFish"))
        {
            FTv2_FishController _fishController = collisionObject.GetComponent<FTv2_FishController>();

            if (_fishController.isJumpingUp)
            {
                rippleEffect.Play();
            }
            else if (_fishController.isJumpingDown && !_fishController.isCaught)
            {
                splashEffect2.Play();
            }
        }
    }

}
