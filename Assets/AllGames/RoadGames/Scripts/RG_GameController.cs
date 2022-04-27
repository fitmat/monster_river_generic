using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_GameController : MonoBehaviour
{
    public static RG_GameController instance;

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

    public float currentVelocity, maxVelocity, maxAllowedVelocity, fastVelocity, velocityIncrement, acceleration;
    public float earlyDelay, lateDelay;
    public bool isEarlyGame;

    // Start is called before the first frame update
    void Start()
    {
        isEarlyGame = true;
        StartCoroutine(IncreaseVelocity());
        StartCoroutine(RG_CameraController.instance.PlayStartCinematic());
    }


    public IEnumerator IncreaseVelocity()
    {
        if (isEarlyGame)
        {
            yield return new WaitForSecondsRealtime(earlyDelay);
        }
        else
        {
            yield return new WaitForSecondsRealtime(lateDelay);
        }
        currentVelocity = maxVelocity;
        maxVelocity += velocityIncrement;
        if (maxVelocity >= fastVelocity)
        {
            if (!RG_CameraController.instance.isFast)
            {
                while (RG_CameraController.instance.isBothJumping)
                {
                    yield return new WaitForSeconds(1f);
                }
                isEarlyGame = false;
                StartCoroutine(RG_CameraController.instance.PlayFastCinematic());
            }
        }
        if (maxVelocity >= maxAllowedVelocity)
        {
            maxVelocity = maxAllowedVelocity;
        }
        while (currentVelocity < maxVelocity)
        {
            currentVelocity += Time.deltaTime * acceleration;
            yield return null;
        }
        currentVelocity = maxVelocity;

        StartCoroutine(IncreaseVelocity());
    }


    
}
