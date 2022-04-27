using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHv2_PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerBody;

    private GameObject collisionObject;

    private Vector3 currentPosition;
    private float startX, currentX, endX;

    public float jumpDistance, moveSpeed, moveAmount, jumpForce, jumpTime, gravity;
    public bool isGrounded, isFalling, isJumping, isMoving;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
    }
    private void FixedUpdate()
    {
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

        if (collisionObject.CompareTag("Ground"))
        {
            if (!isGrounded && isFalling)
            {
                Land();
            }
        }
    }


    public IEnumerator MakeAction()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveForward());
            StartCoroutine(Jump());
            yield return new WaitForSeconds(jumpTime * 2);
            isMoving = false;
        }
    }


    public IEnumerator MoveForward()
    {
        startX = transform.position.x;
        endX = transform.position.x + jumpDistance;
        moveAmount = 0;
        yield return new WaitForSeconds(jumpTime / 2);

        while (moveAmount < 1)
        {
            moveAmount += Time.deltaTime * moveSpeed;
            currentX = Mathf.Lerp(startX, endX, moveAmount);
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(endX, transform.position.y, transform.position.z);
    }
    public IEnumerator Jump()
    {
        if (isGrounded)
        {
            playerBody.isKinematic = false;
            isFalling = false;
            isJumping = true;
            isGrounded = false;
            playerBody.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(jumpTime);
            isJumping = false;
            StartFall();
        }
    }

    public void StartFall()
    {
        isFalling = true;
    }
    public void Land()
    {
        playerBody.isKinematic = true;
        //transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        isGrounded = true;
        isFalling = false;
    }


}
