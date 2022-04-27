using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class RG_CameraController : MonoBehaviour
{

    public static RG_CameraController instance;

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

    [SerializeField] PlayableDirector startPan, movePan, jumpPan, fastPan;
    [SerializeField] CinemachineVirtualCamera frontCam, sideCam, sceneCam, groundCam, rearCam;

    public bool isStarted, isMoving, isOneJumping, isBothJumping, isFast, isOver;

    // Start is called before the first frame update
    void Start()
    {
        sceneCam.Priority = 15;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PlayStartCinematic()
    {
        isStarted = true;
        startPan.Play();
        yield return new WaitForSeconds(1f);
        sceneCam.Priority = 10;
        frontCam.Priority = 15;
    }
    public IEnumerator PlayMoveCinematic()
    {
        Debug.Log("Play Move Cinematic");
        isMoving = true;
        movePan.Play();
        yield return new WaitForSeconds(0.2f);
        sideCam.Priority = 15;
        frontCam.Priority = 10;
    }
    public IEnumerator PlayFastCinematic()
    {
        isFast = true;
        fastPan.Play();
        yield return new WaitForSeconds(1f);
        rearCam.Priority = 15;
        sideCam.Priority = 10;
    }
    public IEnumerator PlayJumpCinematic()
    {
        if (isOneJumping)
        {
            if (!isBothJumping)
            {
                isBothJumping = true;
                jumpPan.Play();
                Time.timeScale = 0.5f;
                yield return new WaitForSeconds(2f);
                isBothJumping = false;
                Time.timeScale = 1f;
            }
        }
        else
        {
            isOneJumping = true;
            yield return new WaitForSeconds(1f);
            isOneJumping = false;
        }
    }
}
