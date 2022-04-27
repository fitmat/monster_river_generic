using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YP_BallController : MonoBehaviour
{
    public static YP_BallController instance;

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

    public int direction;
    public float force;
    private float angle;

    [SerializeField] private ParticleSystem leftGoalParticles, rightGoalParticles;

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("YP_LeftGoal"))
        {
            YP_GameController.instance.IncreasePlayerTwoScore();
            YP_AudioManager.instance.PlayAudio("Goal");
            leftGoalParticles.Play();
        }
        else if (collision.CompareTag("YP_RightGoal"))
        {
            YP_GameController.instance.IncreasePlayerOneScore();
            YP_AudioManager.instance.PlayAudio("Goal");
            rightGoalParticles.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            YP_AudioManager.instance.PlayAudio("Knock");
        }
        else if (collision.gameObject.CompareTag("PlayerOne"))
        {
            YP_AudioManager.instance.PlayAudio("PaddleHit");
        }
        else if (collision.gameObject.CompareTag("PlayerTwo"))
        {
            YP_AudioManager.instance.PlayAudio("PaddleHit");
        }
    }


    public void ThrowBall()
    {
        YP_AudioManager.instance.PlayAudio("Begin");
        angle = Random.Range(-0.25f, 0.25f);
        if (angle > -0.10f && angle < 0)
        {
            angle = -0.10f;
        }
        else if (angle < 0.10f && angle > 0)
        {
            angle = 0.10f;
        }
        GetComponent<Rigidbody2D>().AddForce(new Vector2(1f, angle) * force * direction, ForceMode2D.Impulse);
        force += 1;

    }
}
