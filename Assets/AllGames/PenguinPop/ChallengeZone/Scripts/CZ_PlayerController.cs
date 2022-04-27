using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player 1 Controller script to handle control and behavior of Player 1 penguin
 * */
public class CZ_PlayerController : MonoBehaviour
{
    private Rigidbody2D playerBody;
    public Transform losePoint;
    private Animator animator;
    private float playerPosition, losePosition;
    public int jumpStrength;
    public bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        // Set player 1 jump action on event call
        CZ_InputManager.instance.playerJumpEvent += Jump;
        // Set initial values
        losePosition = losePoint.position.x;
        isJumping = false;
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Handle player 1 collision with triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject;
        // Collision with fish trigger
        if (collision.gameObject.tag == "Enemy")
        {
            // Find parent object of trigger
            collidedObject = collision.gameObject;
            if (!collidedObject.GetComponent<PP_EnemyController>().ignorePlayer1)
            {
                collidedObject.GetComponent<PP_EnemyController>().ignorePlayer1 = true;
                animator.SetTrigger("HitNoMove");

                if (isJumping)
                {
                    CZ_ScoreManager.instance.UpdateJumpCount(-1);
                }
                StartCoroutine(CZ_GameController.instance.CameraShake());
                PP_AudioManager.instance.PlayAudio("PenguinHit");
            }
        }
    }

    // Handle player 1 collision with physical colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            isJumping = false;
        }
    }

    // Function to handle player 1 jump events
    private void Jump()
    {
        //Check if player 1 is already jumping
        if (!isJumping && CZ_GameController.instance.isGameRunning)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
            PP_AudioManager.instance.PlayAudio("PenguinJump");
            playerBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            CZ_ScoreManager.instance.UpdateJumpCount(1);
        }
    }
}
