using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToW_GameplayController : MonoBehaviour
{
    [SerializeField] private GameObject leftPlayer1Object, rightPlayer2Object, leftChainJoint, rightChainJoint, midpoint;
    [SerializeField] private GameObject[] chainColliders;
    [SerializeField] private Rigidbody rightFootPoint, leftFootPoint;
    [SerializeField] private GameObject playerOneDrag, playerOnePull, playerTwoDrag, PlayerTwoPull;

    private Vector3 currentPosition;

    private void OnEnable()
    {
        ResetGamePositon();
    }


    public void ResetGamePositon()
    {
        PullLeft(350);
        PullRight(350);
    }

    public void DropChain()
    {
        leftChainJoint.GetComponent<FixedJoint>().connectedBody = leftFootPoint;
        rightChainJoint.GetComponent<FixedJoint>().connectedBody = rightFootPoint;

        rightFootPoint.useGravity = true;
        leftFootPoint.useGravity = true;

        foreach(GameObject chainCollider in chainColliders)
        {
            chainCollider.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    public void PullLeft(int force)
    {
        if (ToW_GameController.instance.isGameRunning)
        {
            Debug.Log("Pull Left");
            ToW_AudioManager.instance.PlayAudio("Skid");
            leftPlayer1Object.GetComponent<Rigidbody>().AddForce(Vector3.right * force, ForceMode.Impulse);
            playerOnePull.GetComponent<ParticleSystem>().Play();
            playerTwoDrag.GetComponent<ParticleSystem>().Play();
        }
    }

    public void PullRight(int force)
    {
        if (ToW_GameController.instance.isGameRunning)
        {
            Debug.Log("Pull Right");
            ToW_AudioManager.instance.PlayAudio("Skid");
            rightPlayer2Object.GetComponent<Rigidbody>().AddForce(Vector3.left * force, ForceMode.Impulse);
            PlayerTwoPull.GetComponent<ParticleSystem>().Play();
            playerOneDrag.GetComponent<ParticleSystem>().Play();
        }
    }

}
