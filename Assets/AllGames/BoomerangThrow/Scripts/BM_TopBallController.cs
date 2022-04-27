using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to control behavior of the topmost ball in the stack

public class BM_TopBallController : MonoBehaviour
{
    [SerializeField] private GameObject baseBall;
    private Rigidbody ballBody;

    public bool isBouncing;
    public int jumpForce, gravity;

    private void Start()
    {
        ballBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        // Give a random rotation to the ball every frame around X axis
        transform.Rotate(Random.Range(0, 20), 0, 0);

        // Add downwards force to the ball
        ballBody.AddForce(Vector3.down * gravity, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BT_LBoomerang")
        {
            StartCoroutine(BurstBall());
            BM_GameController.instance.changeScore(1, 3);
            Debug.Log("L Boomerang Hit");
        }
        else if (collision.gameObject.tag == "BT_RBoomerang")
        {
            StartCoroutine(BurstBall());
            BM_GameController.instance.changeScore(2, 3);
            Debug.Log("R Boomerang Hit");
        }
    }
    private IEnumerator BurstBall()
    {
        GetComponentInParent<BM_StackMover>().DestroyStack();
        BM_GameController.instance.topBallsLeft--;
        // Check game over condition
        if (BM_GameController.instance.topBallsLeft == 0)
        {
            BM_GameController.instance.GameOver();
        }

        // Disable physics on ball
        gameObject.GetComponent<Collider>().isTrigger = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        BM_AudioManager.instance.PlayAudio("BallBurst");
        BM_AudioManager.instance.PlayAudio("PartySound");

        // Play VFX on bursting ball
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
        // Disable ball model in scene
        transform.GetChild(3).gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        // Cause lowermost ball to fall
        baseBall.GetComponent<Rigidbody>().isKinematic = false;
        baseBall.GetComponentInChildren<ParticleSystem>().Stop();
        gameObject.SetActive(false);
    }

    public IEnumerator BounceBall()
    {
        if (isBouncing)
        {
            // Cause ball to bounce by providing upward force
            ballBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(BounceBall());
        }
    }
    public Vector3 StartRespawn()
    {
        // Bounce ball upwards to respawn lower balls
        isBouncing = false;
        ballBody.AddForce(Vector3.up * jumpForce * 1f, ForceMode.Impulse);
        return transform.localPosition;
    }
}
