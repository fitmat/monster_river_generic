using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HB_PlayerController : MonoBehaviour
{
    private Rigidbody playerBody;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Vector3 centrePosition;

    public float moveForce, jumpForce, backLimit, frontLimit;



    public float moveAmount, jumpAmount, moveTime, jumpTime, moveSpeed, animationSpeed;
    public int direction;

    Vector3 startPosition, endPosition, currentPosition;

    public bool isGrounded, isMoving;

    private void Start()
    {
        isGrounded = true;
        isMoving = false;
        playerBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            playerBody.AddForce(Vector3.down * 60, ForceMode.Acceleration);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HB_Ground"))
        {
            isGrounded = true;
            playerAnimator.SetTrigger("Move");
            playerAnimator.SetFloat("Speed", 0);
        }
    }
    /*private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("HB_Ground"))
        {
            isGrounded = false;
        }
    }*/

    public IEnumerator MoveAhead()
    {
        if (isGrounded && !isMoving)
        {
            isMoving = true;
            direction = 1;
            StartCoroutine(MovePlayer(moveSpeed));
            playerAnimator.SetFloat("Speed", direction);
        }
        else if(!isMoving)
        {

            isMoving = true;
            direction = 1;
            StartCoroutine(MovePlayer(moveSpeed / 2));
        }
        yield return null;
    }

    public IEnumerator MoveBack()
    {
        if (isGrounded && !isMoving)
        {
            isMoving = true;
            direction = -1;
            StartCoroutine(MovePlayer(moveSpeed));
            playerAnimator.SetFloat("Speed", direction);
        }
        else if (!isMoving)
        {

            isMoving = true;
            direction = -1;
            StartCoroutine(MovePlayer(moveSpeed / 2));
        }
        yield return null;
    }

    public void Kick()
    {
        playerAnimator.SetTrigger("Kick");
    }
    public void Head()
    {
        playerAnimator.SetTrigger("Head");
    }

    public IEnumerator Jump()
    {
        if (isGrounded)
        {
            playerBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            playerAnimator.SetTrigger("Jump");
        }
        yield return null;
    }

    private IEnumerator MovePlayer(float moveSpeed)
    {
        float currentTime;
        bool isStopped = false;
        currentTime = 0f;

        //if (HB_GameController.instance.isGameRunning)
        //{
        while (currentTime < moveTime)
        {
            transform.Translate(Vector3.forward * direction * Time.deltaTime * moveSpeed);
            if (transform.localPosition.z < backLimit || transform.localPosition.z > frontLimit)
            {
                transform.Translate(Vector3.forward * -direction * Time.deltaTime * moveSpeed);
                isStopped = true;
                isMoving = false;
                playerAnimator.SetFloat("Speed", 0);
                yield break;
            }
            yield return null;
            currentTime += Time.deltaTime;
            if (currentTime > moveTime / 2 && !isStopped)
            {
                isStopped = true;
                isMoving = false;
            }
        }
        if (!isMoving)
        {
            playerAnimator.SetFloat("Speed", 0);
        }
    }

    public IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("ResetPosition");
        gameObject.transform.localPosition = centrePosition;
    }

}
