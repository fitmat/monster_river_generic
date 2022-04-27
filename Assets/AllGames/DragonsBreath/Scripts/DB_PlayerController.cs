using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D playerBody;
    [SerializeField] Animator playerAnimator, explosionAnimator, dustAnimator1, dustAnimator2;
    [SerializeField] ParticleSystem pebbles, smoke, explosion;

    [SerializeField] GameObject[] healthCapsules;

    private GameObject collisionObject;

    private Vector2 currentPosition, spawnPosition;
    private float startX, currentX, endX, offset;

    public int playerNumber, successfulJumps, successfulJumpsTarget, health;

    public float distancePushed;

    public float moveDistance, moveSpeed, moveAmount, jumpForce, jumpTime, gravity;
    public bool isGrounded, isFalling, isJumping, isMoving, isInEndPosition, isPushed, hasLost;
    

    private void Start()
    {
        startX = transform.localPosition.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        collisionObject = collision.gameObject;

        // If player lands on iceberg
        if (collisionObject.CompareTag("Ground"))
        {
            if (!isGrounded && isFalling)
            {
                Land();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionObject = collision.gameObject;

        if (collision.CompareTag("TerrainEnd"))
        {
            hasLost = true;
            if (DB_GameController.instance.gameState == DB_GameController.GameStates.playing)
            {
                StartCoroutine(DB_GameController.instance.GameOver());
            }
        }


    }


    private void FixedUpdate()
    {
        // If player is falling and not grownded apply increased downward force
        if (!isGrounded)
        {
            if (isFalling)
            {
                playerBody.AddForce(new Vector3(0, -1, 0) * gravity, ForceMode2D.Force);
            }
        }
    }// Function to handle Jumping ahead action
    public IEnumerator MakeAction()
    {
        // Check if player is already executing an action
        if (!isMoving)
        {

            isMoving = true;

            StartCoroutine(Jump());

            // Move player ahead after 3 sucessful jumps
            /*if (successfulJumps == successfulJumpsTarget && distancePushed != 0)
            {
                StartCoroutine(MoveForward());
            }*/

            yield return null;
        }
    }
    // Function to handle Jumping ahead action
    public IEnumerator GetPushed()
    {
        Debug.Log("Get Pushed");
        // Check if player is already executing an action
        if (DB_GameController.instance.gameState == DB_GameController.GameStates.playing)
        {
            if (!isPushed)
            {
                isPushed = true;
                health--;
                healthCapsules[health].SetActive(false);
                // Calculate start and end positions
                playerAnimator.SetTrigger("Hit");
                playerAnimator.ResetTrigger("Jump");
                DB_AudioManager.instance.PlayAudio("Hit"+Random.Range(1,4).ToString());
                //explosionAnimator.SetTrigger("Explode");
                explosion.Play();
                endX = startX - moveDistance;
                moveAmount = 0;
                yield return new WaitForSeconds(jumpTime / 2);
                //PH_AudioManager.instance.PlayAudio("Jump");

                // Move player to endpoint
                while (moveAmount < 1)
                {
                    moveAmount += Time.deltaTime * moveSpeed;
                    // Calculate new player X position
                    currentX = Mathf.Lerp(startX, endX, moveAmount);
                    // Only affect position in X axis
                    transform.localPosition = new Vector3(currentX, transform.localPosition.y);
                    yield return null;
                }
                isPushed = false;
                distancePushed += moveDistance;
                successfulJumps = 0;
                startX = transform.localPosition.x;
            }
        }
    }


    // Function to move player ahead
    public IEnumerator MoveForward()
    {
        // Calculate start and end positions
        endX = startX + moveDistance;
        moveAmount = 0;
        yield return new WaitForSeconds(jumpTime * 2);
        //PH_AudioManager.instance.PlayAudio("Jump");

        // Move player to endpoint
        while (moveAmount < 1)
        {
            if (isPushed)
            {
                yield break;
            }
            moveAmount += Time.deltaTime * moveSpeed;
            // Calculate new player X position
            currentX = Mathf.Lerp(startX, endX, moveAmount);
            // Only affect position in X axis
            transform.localPosition = new Vector2(currentX, transform.localPosition.y);
            yield return null;
        }
        distancePushed -= moveDistance;
        startX = transform.localPosition.x;
        successfulJumps = 0;
    }

    // Function to make player Jump
    public IEnumerator Jump()
    {
        if (isGrounded)
        {
            playerAnimator.SetTrigger("Jump");
            playerAnimator.ResetTrigger("Hit");
            DB_AudioManager.instance.PlayAudio("Jump");
            // Set state of player to jumping
            isFalling = false;
            isJumping = true;
            isGrounded = false;
            // Apply vertical force to player
            playerBody.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(jumpTime);
            // After delay let player start falling
            isJumping = false;
            StartFall();
        }
    }

    // Function to make player fall from jump
    public void StartFall()
    {
        isFalling = true;
    }

    // Function to make player land
    public void Land()
    {
        Debug.Log("Land");
        playerAnimator.ResetTrigger("Jump");
        pebbles.Play();
        smoke.Play();
        dustAnimator1.SetTrigger("Dust");
        dustAnimator2.SetTrigger("Dust");
        // Set state of player to grounded
        isGrounded = true;
        isFalling = false;
        isMoving = false;
    }

    public void Win()
    {
        playerAnimator.SetTrigger("Win");
    }

}
