using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;

public class TW_PlayerOneController : MonoBehaviour
{
    public static TW_PlayerOneController instance;

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

    [SerializeField] private Image playerInstructionOuter, playerInstructionInner;
    [SerializeField] private GameObject player, tree, treeAttackBlow;
    [SerializeField] private Sprite[] directionArrowSprites;
    [SerializeField] private Animator playerAnimator;

    // Directions: 0-Left, 1-Up, 2-Right
    public int directionIndex, playerMoveIndex;

    public bool isReady;
    public int attackDamage = 1;

    private int treeWindCountdown;
    public int treeWindTime;

    public bool hasPlayerEnded;


    private void Start()
    {
        isReady = true;
        treeWindCountdown = treeWindTime;
        TW_InputController.instance.playerOneMove += GetInput;
        TW_PlayerOneUIController.instance.playerOneMove += GetInput;
        hasPlayerEnded = false;
    }


    public void StartInstruction()
    {
        playerInstructionInner.gameObject.SetActive(true);
        playerInstructionOuter.gameObject.SetActive(true);
        playerInstructionInner.color = Color.red;
        directionIndex = Random.Range(0, 3);
        playerInstructionInner.sprite = directionArrowSprites[directionIndex];
        playerInstructionOuter.sprite = directionArrowSprites[directionIndex];
        playerInstructionInner.GetComponent<Animator>().SetTrigger("Prompt");
        playerInstructionOuter.GetComponent<Animator>().SetTrigger("Prompt");
    }

    private void GetInput(int index)
    {
        playerMoveIndex = index;
        CheckAttackState();
    }

    public void CheckAttackState()
    {
        if (isReady && TW_GameController.instance.isGameRunning && !hasPlayerEnded)
        {
            Debug.Log("Player One Expected:" + directionIndex + ", Actual:" + playerMoveIndex);
            if (directionIndex == playerMoveIndex)
            {
                Debug.Log("Sucessful hit");
                StartCoroutine(PlayerAttack());
            }
            else
            {
                Debug.Log("Fail");
                if (TW_PlayerOneTreeController.instance.treeHealth < TW_PlayerOneTreeController.instance.treeMaxHealth - 4)
                {
                    TW_PlayerOneTreeController.instance.treeHealth += 3;
                }
                StartCoroutine(PlayerStun());
            }
        }
    }

    public IEnumerator TreeWindup()
    {
        if (treeWindCountdown == 0 && !hasPlayerEnded)
        {
            StartCoroutine(PlayerStun());
        }
        yield return new WaitForSeconds(1f);
        treeWindCountdown--;
        if (!hasPlayerEnded)
        {
            StartCoroutine(TreeWindup());
        }
    }

    private IEnumerator PlayerStun()
    {
        Debug.Log("Player One Start Stun");
        isReady = false;
        tree.GetComponent<Animator>().SetTrigger("Attack");
        TW_AudioManager.instance.PlayAudio("TreeAttack");
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetTrigger("Stunned");
        TW_AudioManager.instance.PlayAudio("PlayerDamage");
        treeAttackBlow.GetComponent<Animator>().SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        TW_PlayerOneTreeController.instance.PlayIdleAnimation();
        isReady = true;
        treeWindCountdown = treeWindTime;
        TW_PlayerOneTreeController.instance.CheckTreeHealth(); 
        Debug.Log("End Stun");
    }

    private IEnumerator PlayerAttack()
    {
        Debug.Log("Player One Start Attack");
        isReady = false;
        playerInstructionInner.color = Color.green;

        switch (directionIndex)
        {
            case 0:
                playerAnimator.SetTrigger("Attack1");
                TW_AudioManager.instance.PlayAudio("Attack1");
                break;
            case 1:
                playerAnimator.SetTrigger("Attack2");
                TW_AudioManager.instance.PlayAudio("Attack2");
                break;
            case 2:
                playerAnimator.SetTrigger("Attack3");
                TW_AudioManager.instance.PlayAudio("Attack3");
                break;
        }

        yield return new WaitForSeconds(0.2f);
        TW_PlayerOneTreeController.instance.HitTree(attackDamage);

        yield return new WaitForSeconds(0.2f);
        TW_PlayerOneTreeController.instance.PlayIdleAnimation();
        if (!hasPlayerEnded)
        {
            StartInstruction();
        }
        yield return new WaitForSeconds(0.05f);
        isReady = true;
        Debug.Log("End Attack");
        treeWindCountdown = treeWindTime;
    }
}
