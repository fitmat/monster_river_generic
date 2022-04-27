using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player 1 Controller script to handle control and behavior of Player 1 penguin
 * */
public class PP_PlayerOneController : MonoBehaviour
{
    public static PP_PlayerOneController instance;
    private void Awake()
    {
        // Declare class as Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    private Rigidbody2D playerBody;
    public Transform losePoint;
    private Animator animator;
    private float playerPosition, losePosition;
    public int jumpStrength;
    public bool isJumping;
    public bool hasPenguinOneLost;

    // Start is called before the first frame update
    void Start()
    {
        // Set player 1 jump action on event call
        PP_InputManager.instance.player1JumpEvent += Jump;
        // Set initial values
        losePosition = losePoint.position.x + 1f;
        isJumping = false;
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        hasPenguinOneLost = false;
    }

    // Handle player 1 collision with triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject;
        // Collision with fish trigger
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Find parent object of trigger
            collidedObject = collision.gameObject;
            if (!collidedObject.GetComponent<PP_EnemyController>().ignorePlayer1)
            {
                playerPosition -= 2f;
                //transform.position = new Vector3(transform.position.x - 2f, transform.position.y);
                animator.SetTrigger("Hit");
                // If player 1 is pushed off the platform declare player 2 as the winner and end game
                if (playerPosition < losePosition && !PP_GameUIController.instance.isGameOver)
                {
                    hasPenguinOneLost = true;
                    PP_GameController.instance.EndGame();
                }
                // Set colliding object to ignore player 2 to prevent repeated collisions
                collidedObject.GetComponent<PP_EnemyController>().ignorePlayer1 = true;

                PP_ScoreManager.instance.player1Score -= 1;
                StartCoroutine(PP_GameController.instance.CameraShake());
                PP_AudioManager.instance.PlayAudio("PenguinHit");
            }
        }
    }

    // Handle player 1 collision with physical colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            isJumping = false;
        }
    }

    // Function to handle player 1 jump events
    public void Jump()
    {
        Debug.Log("Event Test- Player 1 JUMP event PP Jump function start");
        //Check if player 1 is already jumping
        if (!isJumping && PP_GameController.instance.isGameRunning)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
            StartCoroutine(PP_AudioManager.instance.PlayJumpSound());
            playerBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            PP_ScoreManager.instance.player1Jumps++;
        }
        Debug.Log("Event Test- Player 1 JUMP event PP Jump function sucessful");
    }
}
