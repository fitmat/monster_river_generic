using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TW_PlayerOneTreeController : MonoBehaviour
{
    public static TW_PlayerOneTreeController instance;

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

    [SerializeField] private Sprite[] treeStateSprites;
    [SerializeField] private GameObject hitBlow;

    [SerializeField] private Image healthBar;

    private Animator treeAnimator;
    public int treeState;
    public float treeHealth, treeMaxHealth;

    private void Start()
    {
        treeAnimator = gameObject.GetComponent<Animator>();
        treeState = 1;
        PlayIdleAnimation();

        treeMaxHealth = 30;
        treeHealth = treeMaxHealth;
    }

    public void PlayIdleAnimation()
    {
        switch (treeState)
        {
            case 1:
                treeAnimator.SetTrigger("Idle1");
                break;
            case 2:
                treeAnimator.SetTrigger("Idle2");
                break;
            case 3:
                treeAnimator.SetTrigger("Idle3");
                break;
        }
    }

    public void HitTree(int damage)
    {
        treeAnimator.SetTrigger("Hit");
        hitBlow.GetComponent<Animator>().SetTrigger("Hit");
        TW_AudioManager.instance.PlayAudio("HitTree");

        treeHealth -= damage;


        CheckTreeHealth();

    }

    public void CheckTreeHealth()
    {
        healthBar.fillAmount = (float)(treeHealth / treeMaxHealth);
        if (treeHealth > 20)
        {
            treeState = 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[0];
        }
        else if (treeHealth > 10)
        {
            treeState = 2;
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[1];
        }
        else if (treeHealth > 0)
        {
            treeState = 3;
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[2];
        }
        else if (treeHealth == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[3];
            treeAnimator.SetTrigger("Die");
            StartCoroutine(DelaySound());

            TW_ScoreManager.instance.playerOneTime = TW_GameController.instance.gameTime;

            if (!TW_PlayerTwoController.instance.hasPlayerEnded)
            {
                TW_GameController.instance.winningPlayer = 1;
            }
            TW_PlayerOneController.instance.hasPlayerEnded = true;
            StartCoroutine(TW_GameController.instance.GameOver());
            //if (TW_PlayerOneController.instance.hasPlayerEnded && TW_PlayerTwoController.instance.hasPlayerEnded)
            //{
            //    StartCoroutine(TW_GameController.instance.GameOver());
            //}
            return;
        }
        PlayIdleAnimation();
    }

    public IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(0.6f);
        TW_AudioManager.instance.PlayAudio("TreeFalling");
    }

}
