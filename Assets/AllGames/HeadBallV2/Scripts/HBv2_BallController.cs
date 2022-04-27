using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HBv2_BallController : MonoBehaviour
{
    public static HBv2_BallController instance;

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

    [SerializeField] Rigidbody ballBody;
    [SerializeField] Transform ballTransform;
    [SerializeField] GameObject ball;
    [SerializeField] Transform ballTracker;
    [SerializeField] ParticleSystem ballLandParticles;
    [SerializeField] GameObject kickEffect, goalEffect;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Vector3 kickDirection;

    GameObject collisionObject;
    HBv2_PlayerController playerController;

    public float kickForce;

    public bool isActive;

    private void Start()
    {
        ball = gameObject;
        ballBody = GetComponent<Rigidbody>();

        isActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {

        collisionObject = collision.gameObject;

        // Check on which side goal is scored or if ball misses goalpost

        if (collisionObject.CompareTag("LeftGoal"))
        {
            StartCoroutine(HBv2_GameController.instance.ScoreGoal(1));
            //Instantiate(goalParticles, ball.transform.position, Quaternion.identity);
            Debug.Log("Player One Point");
            StartCoroutine(ResetBall(1));
        }
        if (collisionObject.CompareTag("RightGoal"))
        {
            StartCoroutine(HBv2_GameController.instance.ScoreGoal(2));
            //Instantiate(goalParticles, ball.transform.position, Quaternion.identity);
            Debug.Log("Player Two Point");
            StartCoroutine(ResetBall(2));
        }
        if (collisionObject.CompareTag("Bounds"))
        {
            Debug.Log("Miss!");
            StartCoroutine(HBv2_GameController.instance.MissGoal());
        }
        if (collisionObject.CompareTag("Ground"))
        {
            ballLandParticles.Play();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        Vector3 _collisionObjectPosition;

        collisionObject = other.gameObject;
        _collisionObjectPosition = collisionObject.transform.position;

        // Check which player and body part ball collides with

        if (collisionObject.CompareTag("HB_Head"))
        {
            // Ball collided with head
            playerController = collisionObject.GetComponentInParent<HBv2_PlayerController>();
            if (_collisionObjectPosition.z > ball.transform.position.z && playerController.playerNumber == 2)
            {
                // Right player heads ball to left side

                Debug.Log("Head Left");
                if (_collisionObjectPosition.y < ball.transform.position.y)
                {
                    // Adjust vector of applied force to head ball upwards
                    kickDirection = new Vector3(0, Random.Range(0.5f, 1f), -1f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Up");
                    playerController.Head();
                    ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                    Instantiate(kickEffect, _collisionObjectPosition, Quaternion.identity);

                }
                else
                {
                    // Adjust vector of applied force to head ball downwards
                    kickDirection = new Vector3(0, -Random.Range(0.5f, 1f), -0.5f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Down");
                    playerController.Head();
                    ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                    Instantiate(kickEffect, _collisionObjectPosition, Quaternion.identity);
                }
            }
            else if (_collisionObjectPosition.z < ball.transform.position.z && playerController.playerNumber == 1)
            {
                Debug.Log("Head Right");

                // Left player heads ball to right side

                if (_collisionObjectPosition.y < ball.transform.position.y)
                {
                    kickDirection = new Vector3(0, Random.Range(0.25f, 0.5f), 0.5f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Up");
                    playerController.Head();
                    ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                    Instantiate(kickEffect, _collisionObjectPosition, Quaternion.identity);
                }
                else
                {
                    kickDirection = new Vector3(0, -Random.Range(0.5f, 1f), -0.5f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Down");
                    playerController.Head();
                    ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                    Instantiate(kickEffect, _collisionObjectPosition, Quaternion.identity);
                }
            }
        }

        else if (collisionObject.CompareTag("HB_Legs"))
        {
            // Ball collided with legs
            playerController = collisionObject.GetComponentInParent<HBv2_PlayerController>();
            if (_collisionObjectPosition.z > ball.transform.position.z && playerController.playerNumber == 2)
            {
                // Right player kicks ball to left side
                Debug.Log("Kick Left");

                // Adjust vector of applied force to kick ball

                kickDirection = new Vector3(0, Random.Range(0.5f, 1.1f), -Random.Range(0.7f,1.2f));
                kickForce = Random.Range(175f, 225f);
                ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                playerController.Kick();
                Instantiate(kickEffect, _collisionObjectPosition, Quaternion.identity);
            }
            else if (_collisionObjectPosition.z < ball.transform.position.z && playerController.playerNumber == 1)
            {
                // Left player kicks ball to right side
                Debug.Log("Kick Right");
                //_collisionObject.GetComponentInParent<HB_PlayerController>().Kick();
                kickDirection = new Vector3(0, Random.Range(0.5f, 1.1f), Random.Range(0.7f, 1.2f));
                kickForce = Random.Range(175f, 225f);
                ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
                playerController.Kick();
                Instantiate(kickEffect, _collisionObjectPosition, Quaternion.identity);
            }
        }
    }

    private void FixedUpdate()
    {
        // Adjust position of ball tracker on ground to match ball in air
        ballTracker.position = new Vector3(ballBody.position.x, ballTracker.position.y, ballBody.position.z);
        // Adjust scale of tracker according to height of ball in air
        if (ballTransform.localPosition.y > 3)
        {
            ballTracker.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else if (ballTransform.localPosition.y < 0.5f)
        {
            ballTracker.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else
        {
            ballTracker.localScale = new Vector3(1, 1, 1);
        }

        // Clamp velocity of ball by increasing drag if velocity passes threshold
        if (isActive)
        {
            if (ballBody.velocity.magnitude >= 50f)
            {
                ballBody.drag = 2f;
            }
            else if (ballBody.velocity.magnitude <= 30f)
            {
                ballBody.drag = 1f;
            }
            else
            {
                ballBody.drag = 1.5f;
            }
        }

        // Clamp height of ball by increasing gravity if height passes threshold
        if (ballTransform.localPosition.y < 3f)
        {
            ballBody.AddForce(Vector3.down * 20, ForceMode.Acceleration);
        }
        else if (ballTransform.localPosition.y < 4f)
        {
            ballBody.AddForce(Vector3.down * 100, ForceMode.Acceleration);
        }
        else if (ballTransform.localPosition.y < 6f)
        {
            ballBody.AddForce(Vector3.down * 150, ForceMode.Acceleration);
        }
        else
        {
            ballBody.AddForce(Vector3.down * 200, ForceMode.Acceleration);
        }
    }

    // Reset position of ball
    public IEnumerator ResetBall(int playerNumber)
    {
        ball.GetComponent<Collider>().enabled = false;


        var main1 = goalEffect.GetComponent<ParticleSystem>().main;
        var main2 = goalEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;

        //if (playerNumber == 1)
        //{
        //    main1.startColor = Color.red;
        //    main2.startColor = Color.red;
        //}
        //else if (playerNumber == 2)
        //{
        //    main1.startColor = Color.cyan;
        //    main2.startColor = Color.cyan;
        //}
        //else
        //{
        //    main1.startColor = Color.white;
        //    main2.startColor = Color.white;
        //}


        Instantiate(goalEffect, ballTransform.position, Quaternion.identity);
        ballTransform.GetChild(0).gameObject.SetActive(false);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(4f);
        ballBody.isKinematic = false;
        ball.transform.position = spawnPoint.position;
        ball.GetComponent<Collider>().enabled = true;
        ballTransform.GetChild(0).gameObject.SetActive(true);
    }

}
