using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW2_PlayerController : MonoBehaviour
{
    [SerializeField] Vector3 currentPosition, nextPosition;

    private Rigidbody playerBody;
    [SerializeField] Animator playerAnimator;
    [SerializeField] ParticleSystem playerLandParticle1,playerLandParticle2;

    public float pullStrength, currentMoveAmount, pullDuration, timeStep;

    public bool isPulling;

    [SerializeField] GameObject chatbox;
    [SerializeField] GameObject[] emotes;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Function to pull to left
    public IEnumerator PullLeft()
    {
        // Set state as pulling
        float currentTime = 0;
        isPulling = true;
        // Play pull animation and VFX
        PullAnimation();
        playerLandParticle1.Play();
        playerLandParticle2.Play();
        if (isPulling)
        {
            // While player is pulling, move player position to left
            while (currentTime < pullDuration)
            {
                currentPosition = playerBody.position;
                nextPosition = new Vector3(currentPosition.x - pullStrength, currentPosition.y, currentPosition.z);
                playerBody.position = nextPosition;
                yield return new WaitForSeconds(timeStep);
                currentTime += timeStep;
            }
        }
    }

    // Function to pull to right
    public IEnumerator PullRight()
    {
        float currentTime = 0;
        isPulling = true;
        PullAnimation();
        playerLandParticle1.Play();
        playerLandParticle2.Play();
        if (isPulling)
        {
            while (currentTime < pullDuration)
            {
                currentPosition = playerBody.position;
                nextPosition = new Vector3(currentPosition.x + pullStrength, currentPosition.y, currentPosition.z);
                playerBody.position = nextPosition;
                yield return new WaitForSeconds(timeStep);
                currentTime += timeStep;
            }
        }
    }

    public void WinAnimation()
    {
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Win");
    }
    public void LoseAnimation()
    {
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Lose");
    }
    public void StartAnimation()
    {
        playerAnimator.SetTrigger("Start");
    }
    public void PullAnimation()
    {
        playerAnimator.SetTrigger("Pull");
    }

    public IEnumerator DisplayEmote(int emoteLevel)
    {
        emoteLevel += 3;
        chatbox.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        emotes[emoteLevel].SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        emotes[emoteLevel].SetActive(false);
        chatbox.SetActive(false);
    }
}
