using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TW_CZ_PlayerController : MonoBehaviour
{
    public static TW_CZ_PlayerController instance;

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
    
    [SerializeField] private GameObject leftPlayer, leftTree, treeAttackBlow;
    [SerializeField] private Text countdown, gameTimeText, hitsText;
    [SerializeField] private GameObject gameTimer, fade;
    [SerializeField] private Animator leftPlayerAnimator;

    public int timeLimit;
    [HideInInspector]public int gameTime, successfulMoves;

    // Directions: 0-Left, 1-Up, 2-Right
    public int directionIndex, leftPlayerMoveIndex;

    public bool attackMove1, attackMove2;

    public bool isReady;
    public bool gamePlaying;
    private int attackType;

    private int treeWindCountdown;
    public int treeWindTime = 3;

    private void Start()
    {
        gamePlaying = false;
        isReady = false;
        attackMove1 = false;
        attackMove2 = false;
        successfulMoves = 0;

        StartCoroutine(StartGame());
    }

    private void Update()
    {
        KeyboardInput();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        countdown.gameObject.SetActive(true);
        countdown.text = "Ready";
        countdown.color = Color.red;
        yield return new WaitForSeconds(1f);
        countdown.text = "Set";
        countdown.color = Color.yellow;
        yield return new WaitForSeconds(1f);
        countdown.text = "Go";
        countdown.color = Color.green;
        yield return new WaitForSeconds(1f);
        countdown.gameObject.SetActive(false);

        isReady = true;
        gameTime = timeLimit;
        gamePlaying = true;
        StartCoroutine(Timer());
        treeWindCountdown = treeWindTime;
        StartCoroutine(TreeWindup());
    }

    private IEnumerator TreeWindup()
    {
        if (treeWindCountdown == 0)
        {
            StartCoroutine(PlayerStun());
        }
        yield return new WaitForSeconds(1f);
        treeWindCountdown--;
        StartCoroutine(TreeWindup());
    }

    private IEnumerator Timer()
    {
        gameTimeText.text = "Time: " + ((int)(gameTime / 60)).ToString("00") + ":" + ((int)(gameTime % 60)).ToString("00");
        yield return new WaitForSeconds(1f);
        gameTime--;
        if (gameTime == 10)
        {
            gameTimer.GetComponent<Animator>().SetTrigger("Flash");
        }
        if (gameTime == 0)  // Game Over
        {
            GameOver();
            yield break;
        }
        StartCoroutine(Timer());
    }

    //Game Over Function
    public void GameOver()  
    {
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        gamePlaying = false;
    }

    //Keyboard Input Function
    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!attackMove2 && !attackMove1 && gamePlaying)
            {
                attackMove1 = true;
                StartCoroutine(AttackState1());
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (attackMove1 && gamePlaying)
            {
                attackMove2 = true;
            }
        }
    }

    private IEnumerator AttackState1()
    {
        while (!attackMove2)
        {
            yield return null;
        }
        if (attackMove2)
        {
            StartCoroutine(PlayerAttack());
        }
    }

    private IEnumerator PlayerStun()
    {
        Debug.Log("Start Stun");
        isReady = false;
        leftTree.GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        leftPlayerAnimator.SetTrigger("Stunned");
        treeAttackBlow.GetComponent<Animator>().SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        TW_CZ_TreeController.instance.PlayIdleAnimation();
        isReady = true;
        treeWindCountdown = treeWindTime;
        Debug.Log("End Stun");
    }

    private IEnumerator PlayerAttack()
    {
        while (!isReady)
        {
            yield return null;
        }
        Debug.Log("Start Attack");

        isReady = false;
        successfulMoves++;
        hitsText.text = "Hits: " + successfulMoves;

        attackType = Random.Range(1, 4);
        switch (attackType)
        {
            case 1:
                leftPlayerAnimator.SetTrigger("Attack1");
                break;
            case 2:
                leftPlayerAnimator.SetTrigger("Attack2");
                break;
            case 3:
                leftPlayerAnimator.SetTrigger("Attack3");
                break;
        }

        yield return new WaitForSeconds(0.3f);
        TW_CZ_TreeController.instance.HitTree();

        yield return new WaitForSeconds(0.5f);
        TW_CZ_TreeController.instance.PlayIdleAnimation();
        isReady = true;
        attackMove1 = false;
        attackMove2 = false;
        treeWindCountdown = treeWindTime;
        Debug.Log("End Attack");
    }
}
