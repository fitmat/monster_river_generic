using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_RightPlayerController : MonoBehaviour
{
    public static BM_RightPlayerController instance;

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

    private bool isBoomerangThrown;
    [SerializeField] private Animator BoomerangParent, BoomerangObject, playerAnimator;
    [SerializeField] GameObject playerParent;
    [SerializeField] MeshRenderer playerBoomerang, thrownBoomerang;



    public IEnumerator ThrowBoomerang()
    {
        if (!isBoomerangThrown && BM_GameController.instance.gameState == BM_GameController.GameStates.playing)
        {
            isBoomerangThrown = true;
            playerAnimator.SetTrigger("Throw");
            yield return new WaitForSecondsRealtime(0.2f);
            playerBoomerang.enabled = false;
            thrownBoomerang.enabled = true;

            BoomerangParent.SetTrigger("Throw");
            playerAnimator.ResetTrigger("Catch");
            BoomerangObject.SetBool("isThrown", true);
            BM_AudioManager.instance.PlayAudio("BoomerangThrow");
            yield return new WaitForSecondsRealtime(.25f);
            BM_AudioManager.instance.PlayAudio("BoomerangFly");
            yield return new WaitForSecondsRealtime(1.25f);
            BoomerangObject.SetBool("isThrown", false);
            yield return new WaitForSecondsRealtime(.05f);
            playerAnimator.SetTrigger("Catch");
            yield return new WaitForSecondsRealtime(.25f);
            BM_AudioManager.instance.PlayAudio("BoomerangCatch");
            if (BM_GameController.instance.gameState == BM_GameController.GameStates.playing)
            {
                playerBoomerang.enabled = true;
                thrownBoomerang.enabled = false;
                yield return new WaitForSecondsRealtime(.25f);
                isBoomerangThrown = false;
            }
        }
    }

    public void Win()
    {
        playerBoomerang.enabled = false;
        playerAnimator.applyRootMotion = true;
        playerBoomerang.enabled = false;
        playerAnimator.SetTrigger("Win");
        playerParent.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    public void Lose()
    {
        playerBoomerang.enabled = false;
        playerAnimator.applyRootMotion = true;
        playerBoomerang.enabled = false;
        playerAnimator.SetTrigger("Lose");
        playerParent.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
