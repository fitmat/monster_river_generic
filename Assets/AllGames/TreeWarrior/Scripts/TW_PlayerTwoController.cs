using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;

public class TW_PlayerTwoController : MonoBehaviour
{
    public static TW_PlayerTwoController instance;

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

        TW_InputController.instance.playerTwoMove += GetInput;
        TW_PlayerTwoUIController.instance.playerTwoMove += GetInput;
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
            Debug.Log("Player Two Expected:" + directionIndex + ", Actual:" + playerMoveIndex);
            if (directionIndex == playerMoveIndex)
            {
                Debug.Log("Sucessful hit");
                StartCoroutine(PlayerAttack());
            }
            else
            {
                Debug.Log("Fail");
                if (TW_PlayerTwoTreeController.instance.treeHealth < TW_PlayerTwoTreeController.instance.treeMaxHealth - 4)
                {
                    TW_PlayerTwoTreeController.instance.treeHealth += 3;
                }
                StartCoroutine(PlayerStun());
            }
        }
    }

    private IEnumerator AIController()
    {
        float delay = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(delay);
        if (Random.Range(0, 11) < 10)
        {
            StartCoroutine(PlayerAttack());
        }
        else
        {
            if (TW_PlayerTwoTreeController.instance.treeHealth < TW_PlayerTwoTreeController.instance.treeMaxHealth - 4)
            {
                TW_PlayerTwoTreeController.instance.treeHealth += 3;
            }
            StartCoroutine(PlayerStun());
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
        Debug.Log("Player Two Start Stun");
        isReady = false;
        tree.GetComponent<Animator>().SetTrigger("Attack");
        TW_AudioManager.instance.PlayAudio("TreeAttack");
        yield return new WaitForSeconds(0.1f);
        playerAnimator.SetTrigger("Stunned");
        TW_AudioManager.instance.PlayAudio("PlayerDamage");
        treeAttackBlow.GetComponent<Animator>().SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        TW_PlayerTwoTreeController.instance.PlayIdleAnimation();
        isReady = true;
        treeWindCountdown = treeWindTime;
        TW_PlayerTwoTreeController.instance.CheckTreeHealth();

        Debug.Log("End Stun");
    }

    private IEnumerator PlayerAttack()
    {
        Debug.Log("Player Two Start Attack");
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
        TW_PlayerTwoTreeController.instance.HitTree(attackDamage);

        yield return new WaitForSeconds(0.2f);
        treeWindCountdown = treeWindTime;
        if (!hasPlayerEnded)
        {
            StartInstruction();
        }
        yield return new WaitForSeconds(0.05f);
        TW_PlayerTwoTreeController.instance.PlayIdleAnimation();
        isReady = true;
        Debug.Log("End Attack");
    }
}
