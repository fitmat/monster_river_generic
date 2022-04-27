using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player 2 Controller script to handle control and behavior of Player 2 penguin
 * */
public class PP_PlayerTwoController : MonoBehaviour
{
    public static PP_PlayerTwoController instance;
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
    public GameObject AIController;
    public Transform losePoint;
    private Animator animator;
    private float playerPosition, losePosition;
    public int jumpStrength;
    public bool isJumping;
    public bool hasPenguinTwoLost;

    private void Start()
    {
        // Set player 2 jump action on event call
        PP_InputManager.instance.player2JumpEvent += Jump;
        // Set initial values
        losePosition = losePoint.position.x;
        isJumping = false;
        playerBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        hasPenguinTwoLost = false;
    }

    // Handle player 2 collision with triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject;
        // Collision with fish trigger
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Find parent object of trigger
            collidedObject = collision.gameObject;
            if (!collidedObject.GetComponent<PP_EnemyController>().ignorePlayer2)
            {
                playerPosition -= 2f;
                //transform.position = new Vector3(transform.position.x - 2f, transform.position.y);
                animator.SetTrigger("Hit");
                // If player 2 is pushed off the platform declare player 1 as the winner and end game
                if (playerPosition < losePosition && !PP_GameUIController.instance.isGameOver)
                {
                    hasPenguinTwoLost = true;
                    PP_GameController.instance.EndGame();
                }
                // Set colliding object to ignore player 2 to prevent repeated collisions
                collidedObject.GetComponent<PP_EnemyController>().ignorePlayer2 = true;
                // Decrease player 2 score
                PP_ScoreManager.instance.player2Score -= 1;
                StartCoroutine(PP_GameController.instance.CameraShake());
                PP_AudioManager.instance.PlayAudio("PenguinHit");
            }
        }
    }

    // Handle player 2 collision with physical colliders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When player 2 touches the surface set isJumping flag to false
        if (collision.gameObject.CompareTag("Surface"))
        {
            isJumping = false;
        }
    }

    // Function to handle player 2 jump events
    public void Jump()
    {
        Debug.Log("Event Test- Player 2 JUMP event PP Jump function start");
        //Check if player 2 is already jumping
        if (!isJumping && PP_GameController.instance.isGameRunning)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
            StartCoroutine(PP_AudioManager.instance.PlayJumpSound());
            playerBody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            PP_ScoreManager.instance.player2Jumps++;
        }
        Debug.Log("Event Test- Player 2 JUMP event PP Jump function sucessful");
    }

}
