using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_RightPlayerController : MonoBehaviour
{

    public static FT_RightPlayerController instance;

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

    [SerializeField] private Animator characterAnim;
    public bool isCatching;

    private void Start()
    {
        isCatching = false;
    }


    public IEnumerator CatchLeft()
    {
        if (!isCatching)
        {
            isCatching = true;
            FT_AudioManager.instance.PlayAudio("PlayerGrunt");
            characterAnim.SetTrigger("Left");
            yield return new WaitForSeconds(1.5f);
            isCatching = false;
        }
    }
    public IEnumerator CatchRight()
    {
        if (!isCatching)
        {
            FT_AudioManager.instance.PlayAudio("PlayerGrunt");
            characterAnim.SetTrigger("Right");
            yield return new WaitForSeconds(1.5f);
            isCatching = false;
        }
    }

}
