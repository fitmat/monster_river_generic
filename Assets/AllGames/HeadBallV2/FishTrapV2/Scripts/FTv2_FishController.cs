using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handles state of fish

public class FTv2_FishController : MonoBehaviour
{
    [SerializeField] GameObject fishCatchParticle1, fishCatchParticle2;

    public bool isJumpingUp, isJumpingDown, isCaught;
    public int noOfAnimations;

    private void OnEnable()
    {
        // Spawn fish and enable model
        transform.GetChild(0).gameObject.SetActive(true);
        isCaught = false;
        // Randomly select animation to play
        GetComponent<Animator>().SetInteger("Leap", Random.Range(1, noOfAnimations + 1));
    }

    // Called by animator, sets state of fish as leaping up
    public void StartJump()
    {
        isJumpingUp = true;
        isJumpingDown = false;
        FT_AudioManager.instance.PlayAudio("FishOut");
    }

    // Called by animator, sets state of fish as falling down
    public void StartFall()
    {
        isJumpingUp = false;
        isJumpingDown = true;
    }

    // Called by animator, sets state of fish as reached water
    public void ReachWater()
    {
        isJumpingUp = false;
        isJumpingDown = false;
        FT_AudioManager.instance.PlayAudio("FishIn");
        StartCoroutine(DisableFish());
    }

    // Sets state of fish as caught and provides feedback
    public void CatchFish()
    {
        isCaught = true;
        // Disable fish model
        transform.GetChild(0).gameObject.SetActive(false);
        Instantiate(fishCatchParticle1, transform.position, Quaternion.identity);
        Instantiate(fishCatchParticle2, transform.position, Quaternion.identity);
    }
    // Diables fish after reaching water

    public IEnumerator DisableFish()
    {
        // Stop fish leaping animation
        GetComponent<Animator>().SetInteger("Leap", 0);
        yield return new WaitForSeconds(0.1f);
        // Enable fish model
        transform.GetChild(0).gameObject.SetActive(true);
        // Change parent to object pooler
        transform.parent = ObjectPooler.instance.gameObject.transform;
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

}
