using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_PlayerOneController : MonoBehaviour
{
    public static TB_PlayerOneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private GameObject blockHolder, building;
    [SerializeField] private GameObject block;
    [SerializeField] private Animator craneAnimator, cameraAnimator;
    public bool isDroppingBlock;

    public int blocksStacked;

    // Start is called before the first frame update
    void Start()
    {
        blocksStacked = 0;
        isDroppingBlock = false;
    }


    // Function to release block
    public IEnumerator DropBlock()
    {
        if (!isDroppingBlock)
        {
            // Set state as dropping block
            isDroppingBlock = true;
            // Find current block
            GameObject _newBlock;
            _newBlock = blockHolder.transform.GetChild(0).gameObject;
            _newBlock.transform.parent = building.transform;
            TB_AudioManager.instance.PlayAudio("Release");
            // Enable gravity on current block
            _newBlock.GetComponent<Rigidbody>().useGravity = true;
            // Increase number of blocks stacked
            blocksStacked++;
            // Play animations of dropping block
            //craneAnimator.SetTrigger("CraneUp");
            TB_AudioManager.instance.PlayAudio("Crane");
            // Add current block to pile
            StartCoroutine(TB_GameController.instance.AddNewBlock());
            yield return new WaitForSeconds(1.5f);
            // Spawn new block
            Instantiate(block, blockHolder.transform);
            yield return new WaitForSeconds(1.5f);

            // Move crane up if 2 or more blocks are stacked than other player
            //if (blocksStacked > TB_PlayerTwoController.instance.blocksStacked + 2)
            //{
            //    StartCoroutine(TB_GameController.instance.MoveCraneUp());
            //    MM_GameUIManager.instance.winnerNumber = 1;
            //    StartCoroutine(TB_GameController.instance.DelayGameOver());
            //}
            // Play animation of bringing crane down
            //craneAnimator.SetTrigger("CraneDown");
            yield return new WaitForSeconds(0.5f);
            // Set state as ready
            isDroppingBlock = false;
        }
    }

}
