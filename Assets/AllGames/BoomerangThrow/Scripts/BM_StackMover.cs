using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script to control the stacks of balls

public class BM_StackMover : MonoBehaviour
{
    private BM_TopBallController topBall;

    public int activeBalls;
    public int lives;
    public float rightLimit, leftLimit, speed, direction;

    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        topBall = GetComponentInChildren<BM_TopBallController>();

        activeBalls = 5;
        transform.position = new Vector3(Random.Range(leftLimit, rightLimit), 0, transform.position.z);
        speed = Random.Range(2f, 5f);

        // Randomly decide starting direction of stack
        direction = Random.Range(-1f, 1f);
        if (direction >= 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BM_GameController.instance.gameState == BM_GameController.GameStates.playing)
        {
            if (direction == 1)
            {
                // Move ball stack
                transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
                if (transform.position.x > rightLimit)
                {
                    // Change direction and speed
                    direction = -1;
                    speed = Random.Range(2f, 5f);
                    // If only Top Ball is alive, double speed
                    if (activeBalls == 1)
                    {
                        speed *= 2;
                    }
                }
            }
            else if (direction == -1)
            {
                transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
                if (transform.position.x < leftLimit)
                {
                    direction = 1;
                    speed = Random.Range(2f, 5f);
                    if (activeBalls == 1)
                    {
                        speed *= 2;
                    }
                }
            }
        }
    }

    // Function to handle when a ball from stack is popped
    public void BallPopped()
    {
        activeBalls--;
        // If only Top Ball is alive
        if (activeBalls == 1)
        {
            // Increase movement speed and bounce, start respawn process
            speed *= 2;
            topBall.isBouncing = true;
            StartCoroutine(topBall.BounceBall());
            StartCoroutine(RespawnBalls());
        }
    }

    // Function to destroy ball stack when top ball is popped
    public void DestroyStack()
    {
        activeBalls--;
        isDead = true;
        // Burst all remaining balls in stack
        while (activeBalls > 0)
        {
            StartCoroutine(transform.GetChild(activeBalls).GetComponent<BM_BallController>().BurstBall());
        }
    } 

    // Coroutine to respawn balls
    public IEnumerator RespawnBalls()
    {
        yield return new WaitForSeconds(5f);
        // If top ball is alive and stack lives are not 0
        if (activeBalls == 1 && lives > 0)
        {
            // Reduce movement speed of stack
            speed /= 2;
            lives--;
            while (activeBalls < 5 && !isDead)
            {
                // Retrieve position of top ball from stack
                Vector3 respawnPosition = topBall.StartRespawn();
                yield return new WaitForSeconds(0.25f);
                // Respawn ball from stack
                transform.GetChild(activeBalls).GetComponent<BM_BallController>().RespawnBall(respawnPosition);
                activeBalls++;
                yield return new WaitForSeconds(1.5f);
            }
        }
    }
}
