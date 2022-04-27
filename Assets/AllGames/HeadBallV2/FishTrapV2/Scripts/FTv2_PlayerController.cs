using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTv2_PlayerController : MonoBehaviour
{
    [SerializeField] FTv2_BasketController basketController;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator rightPointsAnimation, leftPointsAnimation;

    [SerializeField] GameObject face;
    [SerializeField] Texture normalTexture, happyTexture, winTexture, sadTexture;
    [SerializeField] GameObject chatbox, collectParticle, failParticle;
    [SerializeField] GameObject[] emotes;
    [SerializeField] GameObject basket;

    public bool isCatching, isMakingAction, hasCaught, isDisplaying, isStunned;
    public int playerNumber;


    public IEnumerator CatchLeft()
    {
        if (!isMakingAction && !isStunned)
        {
            // Set state of player as catching
            isMakingAction = true;
            hasCaught = false;
            // Play catching animation
            playerAnimator.SetTrigger("Right");
            yield return new WaitForSeconds(0.15f);
            // Set player as ready to catch
            isCatching = true;
            yield return new WaitForSeconds(0.5f);
            // Set player as no longer ready to catch
            isCatching = false;
            // If no fish was caught, display sad emote
            //if (!hasCaught)
            //{
            //    StartCoroutine(DisplayEmote(0));
            //}
            yield return new WaitForSeconds(0.5f);
            // Set player state ready for new inout
            isMakingAction = false;
        }
    }
    public IEnumerator CatchRight()
    {
        if (!isMakingAction && !isStunned)
        {
            isMakingAction = true;
            hasCaught = false;
            playerAnimator.SetTrigger("Left");
            yield return new WaitForSeconds(0.15f);
            isCatching = true;
            yield return new WaitForSeconds(0.5f);
            isCatching = false;
            //if (!hasCaught)
            //{
            //    StartCoroutine(DisplayEmote(0));
            //}
            yield return new WaitForSeconds(0.5f);
            isMakingAction = false;
        }
    }

    // Function to provide feedback on catching fish
    public IEnumerator CatchFishSuccess()
    {
        hasCaught = true;
        //StartCoroutine(DisplayEmote(1));
        collectParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        collectParticle.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        collectParticle.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        face.GetComponent<MeshRenderer>().material.mainTexture = happyTexture;

        if (playerNumber == 1)
        {
            leftPointsAnimation.SetTrigger("PLeft");
        }
        else if (playerNumber == 2)
        {
            rightPointsAnimation.SetTrigger("PRight");
        }


        FTv2_GameController.instance.ChangeScore(playerNumber, 1);
        FT_AudioManager.instance.PlayAudio("Catch");
        yield return new WaitForSeconds(1f);
        face.GetComponent<MeshRenderer>().material.mainTexture = normalTexture;
    }

    public IEnumerator CatchFishFail()
    {
        isStunned = true;
        hasCaught = true;
        //StartCoroutine(DisplayEmote(2));
        failParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        failParticle.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        face.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;

        if (playerNumber == 1)
        {
            leftPointsAnimation.SetTrigger("NLeft");
        }
        else if (playerNumber == 2)
        {
            rightPointsAnimation.SetTrigger("NRight");
        }

        FTv2_GameController.instance.ChangeScore(playerNumber, -2);
        FT_AudioManager.instance.PlayAudio("Catch");
        yield return new WaitForSeconds(2.5f);
        face.GetComponent<MeshRenderer>().material.mainTexture = normalTexture;
        isStunned = false;
    }


    public IEnumerator DisplayEmote(int emoteLevel)
    {
        if (!isDisplaying)
        {
            isDisplaying = true;
            chatbox.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
            emotes[emoteLevel].SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            emotes[emoteLevel].SetActive(false);
            chatbox.SetActive(false);
            isDisplaying = false;
        }
    }

    public void WinAnimation()
    {
        //playerAnimator.applyRootMotion = true;
        basket.SetActive(false);
        face.GetComponent<MeshRenderer>().material.mainTexture = winTexture;
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Happy");
    }
    
    public void LoseAnimation()
    {
        basket.SetActive(false);
        face.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Sad");
    }
}
