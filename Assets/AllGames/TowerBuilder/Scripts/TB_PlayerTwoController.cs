using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_PlayerTwoController : MonoBehaviour
{
    public static TB_PlayerTwoController instance;

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


    public IEnumerator DropBlock()
    {
        if (!isDroppingBlock)
        {
            isDroppingBlock = true;
            GameObject _newBlock;
            _newBlock = blockHolder.transform.GetChild(0).gameObject;
            _newBlock.transform.parent = building.transform;
            TB_AudioManager.instance.PlayAudio("Release");
            _newBlock.GetComponent<Rigidbody>().useGravity = true;
            blocksStacked++;
            craneAnimator.SetTrigger("CraneUp");
            TB_AudioManager.instance.PlayAudio("Crane");
            StartCoroutine(TB_GameController.instance.AddNewBlock());
            yield return new WaitForSeconds(1.5f);
            Instantiate(block, blockHolder.transform);
            yield return new WaitForSeconds(1.5f);
            if (blocksStacked > TB_PlayerOneController.instance.blocksStacked + 2)
            {
                StartCoroutine(TB_GameController.instance.MoveCraneUp());
                MM_GameUIManager.instance.winnerNumber = 2;
                StartCoroutine(TB_GameController.instance.DelayGameOver());
            }
            craneAnimator.SetTrigger("CraneDown"); 
            yield return new WaitForSeconds(0.5f);
            isDroppingBlock = false;
        }
    }

}
