using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ballBody;
    [SerializeField] private Vector2 ballPosition, contactPoint;

    [SerializeField] private GameObject ball;
    [SerializeField] private Transform spawnPoint;

    public bool isActive;

    private void Start()
    {
        isActive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PB_Paddle"))
        {
            contactPoint = collision.contacts[0].point;
            ballPosition = ballBody.transform.position;
            collision.transform.parent.gameObject.GetComponent<PB_PaddleController>().HitBall(ballBody, ballPosition, contactPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PB_RightEnd") && isActive)
        {
            collision.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            Debug.Log("PlayerOnePoint");
            PB_GameController.instance.IncreasePlayerTwoDrops();
            DisableBall();
            StartCoroutine(DisableEnd(collision.gameObject));
            //PB_GameController.instance.ReleaseBall();
        }
        if (collision.gameObject.CompareTag("PB_LeftEnd") && isActive)
        {
            collision.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            PB_GameController.instance.IncreasePlayerOneDrops();
            DisableBall();
            StartCoroutine(DisableEnd(collision.gameObject));
            //PB_GameController.instance.ReleaseBall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (ballBody.velocity.magnitude >= 20f)
            {
                ballBody.drag = 1.5f;
            }
            else if (ballBody.velocity.magnitude <= 15f)
            {
                ballBody.drag = 0.3f;
            }
            else
            {
                ballBody.drag = 1.0f;
            }
        }
    }

    private void DisableBall()
    {
        PB_AudioManager.instance.PlayAudio("Drop");
        ballBody.drag = 2.5f;
        isActive = false;
        ballBody.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }
    public IEnumerator DisableEnd(GameObject endPoint)
    {
        yield return new WaitForSeconds(0.10f);
        endPoint.GetComponent<Collider2D>().isTrigger = false;
        yield return new WaitForSeconds(0.75f);
        endPoint.GetComponent<Collider2D>().isTrigger = true;
    }

}
