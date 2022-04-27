using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PH_PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerBody;
    [SerializeField] Animator playerAnimator, skinAnimator;
    [SerializeField] ParticleSystem playerLandParticle, waterSplash1, waterSplash2;

    private GameObject collisionObject;
    public GameObject attachedIceberg = null;
    private PH_PlayerController playerController;
    [SerializeField] GameObject face;
    [SerializeField] Texture normalTexture, happyTexture, winTexture, sadTexture;

    private Vector3 currentPosition, spawnPosition;
    private float startX, currentX, endX, offset;

    public int playerNumber;

    public float jumpDistance, moveSpeed, moveAmount, jumpForce, jumpTime, gravity;
    public bool isGrounded, isFalling, isJumping, isMoving, isInStartPosition, isInEndPosition, isInWater;

    private void Start()
    {
        playerController = gameObject.GetComponent<PH_PlayerController>();
        spawnPosition = transform.position;
        isInStartPosition = true;
        isInEndPosition = false;
    }

    private void Update()
    {
        try
        {
            // If player is attached to iceberg, move him with the iceberg
            if (isGrounded && !isInStartPosition && !isInEndPosition && PH_GameController.instance.gameState == PH_GameController.GameStates.playing)
            {
                transform.position = new Vector3(attachedIceberg.transform.position.x + offset, transform.position.y, attachedIceberg.transform.position.z);
            }
        }
        catch
        {

        }
    }

    private void FixedUpdate()
    {
        // If player is falling and not grownded apply increased downward force
        if (!isGrounded)
        {
            if (isFalling)
            {
                playerBody.AddForce(new Vector3(0, -1, 0) * gravity, ForceMode.Acceleration);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionObject = collision.gameObject;

        // If player lands on iceberg
        if (collisionObject.CompareTag("Ground"))
        {
            if (!isGrounded && isFalling && !isInWater)
            {
                // Attach player to iceberg
                attachedIceberg = collisionObject;
                offset = transform.localPosition.x - attachedIceberg.transform.localPosition.x;
                Land();
            }
        }
        // If player reaches endpoint
        if (collisionObject.CompareTag("Target"))
        {
            if (!isGrounded && isFalling)
            {
                // Player wins
                isInEndPosition = true;
                Land();
                Won();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionObject = other.gameObject;
        // Check if player falls in water
        if (collisionObject.CompareTag("WaterSurface1"))
        {
            StartCoroutine(FallInWater());

        }
    }


    // Function to handle Jumping ahead action
    public IEnumerator MakeAction()
    {
        // Check if player is already executing an action
        if (!isMoving && !isInWater)
        {
            isInStartPosition = false;
            Debug.Log("Make Action Attached Block =" + attachedIceberg);

            // If player is on iceberg
            if (attachedIceberg != null)
            {
                // Detach iceberg from player
                attachedIceberg.GetComponent<PH_TileController>().LeaveTile();
                attachedIceberg = null;
            }

            isMoving = true;
            StartCoroutine(MoveForward());
            StartCoroutine(Jump());
            playerLandParticle.Play();
            yield return null;
        }
    }
    // Function to handle Jumping ahead action
    public IEnumerator GetPushed()
    {
        // Check if player is already executing an action
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveForward());
            StartCoroutine(Jump());
            playerLandParticle.Play();
            yield return new WaitForSeconds(jumpTime * 2);
            isMoving = false;
        }
    }


    // Function to move player ahead
    public IEnumerator MoveForward()
    {
        // Calculate start and end positions
        startX = transform.localPosition.z;
        endX = transform.localPosition.z + jumpDistance;
        moveAmount = 0;
        yield return new WaitForSeconds(jumpTime / 2);
        PH_AudioManager.instance.PlayAudio("Jump");

        // Move player to endpoint
        while (moveAmount < 1)
        {
            moveAmount += Time.deltaTime * moveSpeed;
            // Calculate new player X position
            currentX = Mathf.Lerp(startX, endX, moveAmount);
            // Only affect position in X axis
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentX);
            yield return null;
        }
    }

    // Function to make player Jump
    public IEnumerator Jump()
    {
        if (isGrounded)
        {
            playerAnimator.SetTrigger("Jump");
            // Set state of player to jumping
            isFalling = false;
            isJumping = true;
            isGrounded = false;
            // Apply vertical force to player
            playerBody.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(jumpTime);
            // After delay let player start falling
            isJumping = false;
            StartFall();
        }
    }

    // Function to make player fall from jump
    public void StartFall()
    {
        // Play Wobble animation only on condition
        playerAnimator.SetTrigger("Fall");
        playerAnimator.SetInteger("Wobble",Random.Range(0,4));
        isFalling = true;
    }

    // Function to make player land
    public void Land()
    {
        playerAnimator.SetTrigger("Reset");
        Debug.Log("Land");
        playerLandParticle.Play();
        try
        {
            if (!isInEndPosition && !isInWater)
            {
                // Handle landing on tile
                attachedIceberg.GetComponent<PH_TileController>().LandOnTile(playerController, playerNumber);
            }
        }
        catch
        {

        }
        // Set state of player to grounded
        isGrounded = true;
        isFalling = false;
        isMoving = false;
    }

    public void Won()
    {
        gameObject.GetComponent<Animator>().applyRootMotion = true;
        face.GetComponent<MeshRenderer>().material.mainTexture = winTexture;
        gameObject.GetComponent<Animator>().SetTrigger("Spin");
        playerAnimator.applyRootMotion = true;
        playerAnimator.SetTrigger("Happy");
        playerBody.isKinematic = true;
        gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        MM_GameUIManager.instance.winnerNumber = playerNumber;
        PH_GameController.instance.GameOver();
    }

    public void Lose()
    {
        gameObject.GetComponent<Animator>().applyRootMotion = true;
        face.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
        gameObject.GetComponent<Animator>().SetTrigger("Spin");
        playerAnimator.applyRootMotion = true;
        gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        playerAnimator.SetTrigger("Sad");
        playerBody.isKinematic = true;
    }

    // Function to handle falling in water
    public IEnumerator FallInWater()
    {
        if (!isInWater)
        {
            isInWater = true;
            // Detach iceberg from player
            if (attachedIceberg != null)
            {
                attachedIceberg = null;
            }
            Debug.Log("Fall in water Attached Block =" + attachedIceberg);
            // Play water splash SFX and VFX
            waterSplash1.Play();
            waterSplash2.Play();
            PH_AudioManager.instance.PlayAudio("Splash");

            playerBody.isKinematic = true;
            playerAnimator.SetTrigger("Drown");
            yield return new WaitForSeconds(0.5f);
            playerBody.isKinematic = false;


            yield return new WaitForSeconds(1f);

            if (PH_GameController.instance.gameState == PH_GameController.GameStates.playing)
            {
                // Return player to initial position
                transform.position = spawnPosition;
                isGrounded = true;
                isFalling = false;
                isInStartPosition = true;
                playerAnimator.SetTrigger("Reset");
                playerAnimator.SetInteger("Wobble", 3);
                skinAnimator.SetTrigger("Flash");
                yield return null;
                isInWater = false;
                isMoving = false;
            }
        }
    }

    public IEnumerator OnCrackedTile()
    {
        isGrounded = false;
        isFalling = true;
        isMoving = true;
        yield return null;

        // If player is on iceberg
        if (attachedIceberg != null)
        {
            // Detach iceberg from player
            attachedIceberg.GetComponent<PH_TileController>().LeaveTile();
            attachedIceberg = null;
        }

        playerAnimator.SetTrigger("Fall");
    }


}
