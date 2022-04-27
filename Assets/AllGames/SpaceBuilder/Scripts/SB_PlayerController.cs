using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SB_PlayerController : MonoBehaviour
{
    public bool isReady;

    public int playerNumber;

    private GameObject hangingBlock;

    [SerializeField] GameObject playerBlockHolder, playerTower;

    [SerializeField] GameObject[] playerBlocks;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNewBlock());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DropBlock()
    {
        if (isReady)
        {
            TB_AudioManager.instance.PlayAudio("Release");
            hangingBlock.transform.parent = playerTower.transform;

            //hangingBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            hangingBlock.GetComponent<Rigidbody>().useGravity = true;
            hangingBlock.GetComponent<Collider>().enabled = true;
            isReady = false;
            StartCoroutine(SpawnNewBlock());
            StartCoroutine(MoveCrane());
        }
    }

    public IEnumerator MoveCrane()
    {
        yield return new WaitForSeconds(0.5f);
        if (playerNumber == 1)
        {
            DOTween.Restart("SB_LeftCraneUp",false);
        }
        else if (playerNumber == 2)
        {
            DOTween.Restart("SB_RightCraneUp", false);
        }
        yield return new WaitForSeconds(3f);
        if (playerNumber == 1)
        {
            DOTween.Restart("SB_LeftCraneDown", false);
        }
        else if (playerNumber == 2)
        {
            DOTween.Restart("SB_RightCraneDown", false);
        }
    }

    public IEnumerator SpawnNewBlock()
    {
        TB_AudioManager.instance.PlayAudio("Crane");
        yield return new WaitForSeconds(4f);
        hangingBlock = Instantiate(playerBlocks[Random.Range(0, playerBlocks.Length)], playerBlockHolder.transform, false);
        yield return new WaitForSeconds(2f);
        isReady = true;
    }

}
