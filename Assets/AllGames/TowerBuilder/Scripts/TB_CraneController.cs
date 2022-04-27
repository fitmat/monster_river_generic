using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_CraneController : MonoBehaviour
{

    public float timePeriod;
    public float currentPhase;
    public float maxX;
    public int direction;
    public bool isMoving, isStopped;

    private void Start()
    {
        float dir = Random.Range(-1f, 1f);
        direction = dir < 0 ? -1 : 1;
        StartCoroutine(StartSwinging());
    }

    // Function to stop swinging and execute half swing from extreme to start position
    public IEnumerator StopSwing()
    {
        currentPhase = 0;
        while (currentPhase <= timePeriod)
        {
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(transform.localRotation.x, 0, 0), Quaternion.Euler(0, 0, 0), (float)currentPhase / timePeriod);
            currentPhase += Time.deltaTime;
            yield return null;
        }
        isStopped = true;
        transform.localRotation = Quaternion.identity;
    }


    // Function to oscillate crane;
    private IEnumerator StartSwinging()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        isStopped = false;
        // Complete half swing from initial position to either extreme position
        if (direction == -1)
        {
            isMoving = true;
            currentPhase = 0;
            while (currentPhase <= timePeriod)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(-maxX, 0, 0), (float)currentPhase / timePeriod);
                currentPhase += Time.deltaTime;
                yield return null;
            }
            direction = -direction;
            isMoving = false;
            StartCoroutine(SwingLeft());
        }
        else if (direction == 1)
        {
            isMoving = true;
            currentPhase = 0;
            while (currentPhase <= timePeriod)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(maxX, 0, 0), (float)currentPhase / timePeriod);
                currentPhase += Time.deltaTime;
                yield return null;
            }
            direction = -direction;
            isMoving = false;
            StartCoroutine(SwingRight());
        }
    }

    // Functions to execute full swing from one extreme position to other

    private IEnumerator SwingRight()
    {
        if (!isMoving)
        {
            isMoving = true;
            currentPhase = 0;
            while (currentPhase <= timePeriod)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(maxX, 0, 0), Quaternion.Euler(-maxX, 0, 0), (float)currentPhase / timePeriod);
                currentPhase += Time.deltaTime;
                yield return null;
            }
            direction = -direction;
            isMoving = false;
            if (!isStopped)
            {
                StartCoroutine(SwingLeft());
            }
        }
    }

    private IEnumerator SwingLeft()
    {
        if (!isMoving)
        {
            isMoving = true;
            currentPhase = 0;
            while (currentPhase <= timePeriod)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(-maxX, 0, 0), Quaternion.Euler(maxX, 0, 0), (float)currentPhase / timePeriod);
                currentPhase += Time.deltaTime;
                yield return null;
            }
            direction = -direction;
            isMoving = false; 
            if (!isStopped)
            {
                StartCoroutine(SwingRight());
            }
        }
    }


}
