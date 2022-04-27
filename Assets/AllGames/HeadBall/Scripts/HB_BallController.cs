using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HB_BallController : MonoBehaviour
{
    public static HB_BallController instance;

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

    [SerializeField] private Rigidbody ballBody;

    [SerializeField] private GameObject ball, goalParticles;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 kickDirection;
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
        GameObject _collisionObject;

        _collisionObject = collision.gameObject;

        if (_collisionObject.CompareTag("HB_RightNet"))
        {
            StartCoroutine(HB_GameController.instance.ScoreGoal(1));
            Instantiate(goalParticles, ball.transform.position, Quaternion.identity);
            Debug.Log("Player One Point");
        }
        if (_collisionObject.CompareTag("HB_LeftNet"))
        {
            StartCoroutine(HB_GameController.instance.ScoreGoal(2));
            Instantiate(goalParticles, ball.transform.position, Quaternion.identity);
            Debug.Log("Player Two Point");
        }
        if (_collisionObject.CompareTag("HB_Bounds"))
        {
            Debug.Log("Miss!");
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject _collisionObject;
        Vector3 _collisionObjectPosition;

        _collisionObject = other.gameObject;
        _collisionObjectPosition = _collisionObject.transform.position;

        if (_collisionObject.CompareTag("HB_Head"))
        {
            if (_collisionObjectPosition.z > ball.transform.position.z)
            {
                _collisionObject.GetComponentInParent<HB_PlayerController>().Head();
                Debug.Log("Head Left");
                if (_collisionObjectPosition.y < ball.transform.position.y)
                {

                    kickDirection = new Vector3(0, Random.Range(0.5f, 1f), -1f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Up");

                }
                else
                {
                    kickDirection = new Vector3(0, -Random.Range(0.5f, 1f), -0.5f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Down");
                }
            }
            else if (_collisionObjectPosition.z < ball.transform.position.z)
            {
                Debug.Log("Head Right");
                _collisionObject.GetComponentInParent<HB_PlayerController>().Head();
                if (_collisionObjectPosition.y < ball.transform.position.y)
                {
                    kickDirection = new Vector3(0, Random.Range(0.5f, 1f), 1f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Up");
                }
                else
                {
                    kickDirection = new Vector3(0, -Random.Range(0.5f, 1f), -0.5f);
                    kickForce = Random.Range(200f, 250f);

                    Debug.Log("Head Down");
                }
            }
            ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
        }

        else if (_collisionObject.CompareTag("HB_Legs"))
        {
            if (_collisionObjectPosition.z > ball.transform.position.z)
            {
                Debug.Log("Kick Left");
                _collisionObject.GetComponentInParent<HB_PlayerController>().Kick();
                kickDirection = new Vector3(0, Random.Range(0.5f, 1.1f), -Random.Range(0.7f,1.2f));
                kickForce = Random.Range(275f, 325f);
            }
            else if (_collisionObjectPosition.z < ball.transform.position.z)
            {
                Debug.Log("Kick Right");
                _collisionObject.GetComponentInParent<HB_PlayerController>().Kick();
                kickDirection = new Vector3(0, Random.Range(0.5f, 1.1f), Random.Range(0.7f, 1.2f));
                kickForce = Random.Range(275f, 325f);
            }
            ballBody.AddForce(kickDirection * kickForce, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (ballBody.velocity.magnitude >= 50f)
            {
                ballBody.drag = 1.5f;
            }
            else if (ballBody.velocity.magnitude <= 30f)
            {
                ballBody.drag = 0.5f;
            }
            else
            {
                ballBody.drag = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y < 6f)
        {
            ballBody.AddForce(Vector3.down * 2, ForceMode.Acceleration);
        }
        else if (transform.position.y < 8f)
        {
            ballBody.AddForce(Vector3.down * 50, ForceMode.Acceleration);
        }
        else if (transform.position.y < 10f)
        {
            ballBody.AddForce(Vector3.down * 100, ForceMode.Acceleration);
        }
        else
        {
            ballBody.AddForce(Vector3.down * 150, ForceMode.Acceleration);
        }
    }

    public IEnumerator ResetBall()
    {
        ball.SetActive(false);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        ball.transform.position = spawnPoint.position;
        ball.SetActive(true);
    }

}
