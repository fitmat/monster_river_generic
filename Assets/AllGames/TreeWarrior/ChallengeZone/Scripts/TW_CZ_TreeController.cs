using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TW_CZ_TreeController : MonoBehaviour
{
    public static TW_CZ_TreeController instance;

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
    private Animator treeAnimator;
    public int treeState;
    public int treeHealth;

    private void Start()
    {
        treeAnimator = gameObject.GetComponent<Animator>();
        treeState = 1;
        PlayIdleAnimation();

        treeHealth = 60;
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
    public void HitTree()
    {
        treeAnimator.SetTrigger("Hit");
        hitBlow.GetComponent<Animator>().SetTrigger("Hit");

        treeHealth--;
        if (treeHealth > 40)
        {
            treeState = 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[0];
        }
        else if (treeHealth > 20)
        {
            treeState = 2;
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[1];
        }
        else if (treeHealth > 0)
        {
            treeState = 3;
            gameObject.GetComponent<SpriteRenderer>().sprite = treeStateSprites[2];
        }

    }

}
