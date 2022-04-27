using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script handles movement of players

public class HBv2_PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerBody;
    [SerializeField] Animator playerAnimator;
    [SerializeField] ParticleSystem playerLandParticle;
    [SerializeField] GameObject face;
    [SerializeField] Texture normalTexture, happyTexture, winTexture, sadTexture;
    private GameObject collisionObject;

    public Vector3 playerVelocity, playerStartPosition;

    public int jumpForce, gravity;
    public float jumpTime, minVelocity;
    public float rightLimit, leftLimit;

    public float stepAmount, maxMoveSpeed, speedDecay, timeStep;
    private float moveSpeed;

    private Vector3 startPosition, endPosition, currentPosition;
    private float startZ, endZ, currentZ;

    public int playerNumber;

    public bool isMoving, isJumping, isGrounded, isTurning, isFalling, isSlowed, isResetting;

    private void Start()
    {
        // Mark starting positon of players
        playerStartPosition = playerBody.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionObject = collision.gameObject;

        // Detect when players collide with ground
        if (collisionObject.CompareTag("Ground"))
        {
            // If current state of player is falling but not grounded
            if (!isGrounded && isFalling)
            {
                Land();
            }
        }
    }


    private void FixedUpdate()
    {
        playerVelocity = playerBody.velocity;
        if (!isGrounded)
        {
            if (isFalling)
            {
                // If current state of player is falling but not grounded apply downward force of artificial gravity
                playerBody.AddForce(new Vector3(0, -1, 0) * gravity, ForceMode.Acceleration);
            }
        }

    }

    // Function to move player to right
    public IEnumerator MoveRight()
    {
        if (!isResetting)
        {
            // Calculate start and end position of player
            currentPosition = playerBody.position;
            currentZ = currentPosition.z;
            endZ = currentPosition.z + stepAmount;
            if (endZ > rightLimit) { endZ = rightLimit; }
            endPosition = new Vector3(currentPosition.x, currentPosition.y, endZ);
            moveSpeed = maxMoveSpeed;
            playerLandParticle.Play();

            // play animation of player movement
            if (playerNumber == 1)
            {
                playerAnimator.SetTrigger("Ahead");
            }

            else if (playerNumber == 2)
            {
                playerAnimator.SetTrigger("Back");
            }

            // Move player to end position in steps
            while (currentZ <= endZ && moveSpeed > 0 && !isResetting)
            {
                currentZ += moveSpeed;
                // Decrease move speed over time
                moveSpeed = moveSpeed * speedDecay;
                currentPosition = new Vector3(currentPosition.x, playerBody.position.y, currentZ);
                transform.position = currentPosition;
                yield return new WaitForSeconds(timeStep);
            }
            playerLandParticle.Play();
        }
    }
    public IEnumerator MoveLeft()
    {
        if (!isResetting)
        {
            currentPosition = playerBody.position;
            currentZ = currentPosition.z;
            endZ = currentPosition.z - stepAmount;
            if (endZ < leftLimit) { endZ = leftLimit; }
            endPosition = new Vector3(currentPosition.x, currentPosition.y, endZ);
            moveSpeed = maxMoveSpeed;
            playerLandParticle.Play();

            if (playerNumber == 2)
            {
                playerAnimator.SetTrigger("Ahead");
            }

            else if (playerNumber == 1)
            {
                playerAnimator.SetTrigger("Back");
            }


            while (currentZ >= endZ && moveSpeed > 0 && !isResetting)
            {
                currentZ -= moveSpeed;
                moveSpeed = moveSpeed * speedDecay;
                currentPosition = new Vector3(currentPosition.x, playerBody.position.y, currentZ);
                transform.position = currentPosition;
                yield return new WaitForSeconds(timeStep);
            }
            playerLandParticle.Play();
        }
    }


    // Function to make player jump
    public IEnumerator Jump()
    {
        if (isGrounded && !isResetting)
        {
            playerAnimator.SetTrigger("Jump");
            playerLandParticle.Play();
            // Set state of player as jumping but not falling
            playerBody.isKinematic = false;
            isFalling = false;
            isJumping = true;
            isGrounded = false;
            // Add upward force to player
            playerBody.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(jumpTime);
            isJumping = false;
            // Start players fall after time delay
            StartFall();
        }
    }

    // Start player fall to ground with gravity
    public void StartFall()
    {
        isFalling = true;
    }


    // Reset player to start position
    public IEnumerator ResetPosition()
    {
        Debug.Log("StartReset");
        isResetting = true;
        yield return new WaitForSeconds(4f);
        playerBody.rotation = Quaternion.Euler(0, 0, 0);
        playerBody.position = playerStartPosition;
        face.GetComponent<MeshRenderer>().material.mainTexture = normalTexture;
        yield return new WaitForSeconds(0.5f);
        isResetting = false;
        Debug.Log("StopReset");
    }

    // Function to handle player reaching ground
    public void Land()
    {
        // Set player state as grounded and not falling
        playerBody.isKinematic = true;
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        playerLandParticle.Play();
        isGrounded = true;
        isFalling = false;
    }

    public void Kick()
    {
        playerAnimator.SetTrigger("Kick");
    }

    public void Head()
    {
        playerAnimator.SetTrigger("Head");
    }
    public void Happy()
    {
        face.GetComponent<MeshRenderer>().material.mainTexture = happyTexture;
        RotateToCamera();
        playerAnimator.SetTrigger("Happy");
    }
    public void Sad()
    {
        face.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
        RotateToCamera();
        playerAnimator.SetTrigger("Sad");
    }
    public void Win()
    {
        face.GetComponent<MeshRenderer>().material.mainTexture = winTexture;
        RotateToCamera();
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Win");
    }
    public void Lose()
    {
        face.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
        RotateToCamera();
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Lose");
    }

    public void RotateToCamera()
    {
        if (playerNumber == 1)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 45, 0);
        }
        else if (playerNumber == 2)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, -45, 0);
        }
    }

}
