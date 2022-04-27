using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_BlockController : MonoBehaviour
{
    private Rigidbody blockBody, lowerBlockBody;
    private GameObject lowerBlock;


    public int playerNumber;

    public int gravity;
    public bool isSettled, isHanging, hasCollided;


    // Start is called before the first frame update
    void Start()
    {
        blockBody = GetComponent<Rigidbody>();
        GetComponent<Collider>().enabled = false;
        isHanging = true;
        hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SB_Block") && isHanging && !hasCollided)
        {
            TB_AudioManager.instance.PlayAudio("BlockFall");
            transform.GetChild(1).gameObject.SetActive(true);
            hasCollided = true;
            lowerBlock = collision.gameObject;
            lowerBlockBody = lowerBlock.GetComponent<Rigidbody>();
            lowerBlock.transform.localRotation = Quaternion.Euler(0, 90, 0);
            lowerBlockBody.isKinematic = true;
            lowerBlockBody.useGravity = false;
            StartCoroutine(SB_GameController.instance.AddNewBlock(playerNumber));
        }
        if (collision.gameObject.CompareTag("SB_Base") && isHanging && !hasCollided)
        {
            TB_AudioManager.instance.PlayAudio("BlockFall");
            transform.GetChild(1).gameObject.SetActive(true);
            hasCollided = true;
            StartCoroutine(SB_GameController.instance.AddNewBlock(playerNumber));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") && !isSettled)
        {
            if (playerNumber == 1)
            {
                MM_GameUIManager.instance.winnerNumber = 2;
                SB_GameController.instance.GameOver();
            }
            if (playerNumber == 2)
            {
                MM_GameUIManager.instance.winnerNumber = 1;
                SB_GameController.instance.GameOver();
            }
        }
    }

    private void FixedUpdate()
    {
        if (blockBody.useGravity)
        {
            blockBody.AddForce(Vector3.down * gravity, ForceMode.Force);
        }
    }
}
