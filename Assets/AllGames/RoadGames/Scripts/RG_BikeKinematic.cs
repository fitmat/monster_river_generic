using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_BikeKinematic : MonoBehaviour
{

    [SerializeField] GameObject bikeObject, frontWheel, backWheel;
    [SerializeField] Rigidbody bikeBody;
    [SerializeField] Animator playerAnimator;

    public Vector3 bikeVelocity;

    public float speed, jumpHeight, timeStep, jumpTime, currentRotation, turnAmount, turnSpeed, minVelocity;

    public int jumpForce, gravity;

    public int playerNumber;

    public bool isMoving, isJumping, isGrounded, isTurning, isFalling, isSlowed;

    public enum RoadMaterials { pavedRoad, dirtRoad };//, forestTheme };
    public RoadMaterials currentRoad, nextRoad, previousRoad;

    [SerializeField] GameObject currentParticleSystem, dizzyParticles;
    [SerializeField] GameObject pavedParticles, dirtParticles;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("RG_Obstacle"))
        {
            StartCoroutine(SlowBike());
            StartCoroutine(DestroyObstacle(other.gameObject.transform.parent.gameObject));
        }
        else if (other.gameObject.CompareTag("RG_ClearObstacle"))
        {
            StartCoroutine(DespawnObstacle(other.gameObject.transform.parent.gameObject));
        }
        else if (other.gameObject.CompareTag("RG_Tornado"))
        {
            Time.timeScale = 0;
            Debug.Log(playerNumber + " Loses");
        }

    }

    private IEnumerator SlowBike()
    {
        if (!isSlowed)
        {
            Debug.Log(playerNumber + " Collide");
            isSlowed = true;
            dizzyParticles.SetActive(true);
            if (RG_GameController.instance.isEarlyGame)
            {
                yield return new WaitForSecondsRealtime(2f);
            }
            else
            {
                yield return new WaitForSecondsRealtime(1f);
            }
            isSlowed = false;
            dizzyParticles.SetActive(false);
        }
    }

    private IEnumerator DespawnObstacle(GameObject obstacle)
    {
        yield return new WaitForSecondsRealtime(5f);
        obstacle.SetActive(false);
    }
    private IEnumerator DestroyObstacle(GameObject obstacle)
    {
        obstacle.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        obstacle.transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        obstacle.transform.GetChild(0).gameObject.SetActive(true);
        obstacle.transform.GetChild(1).gameObject.SetActive(false);
        obstacle.SetActive(false);
    }

    public void MoveAhead()
    {
        if (!isSlowed)
        {
            speed = RG_GameController.instance.currentVelocity;
        }
        else
        {
            speed = RG_GameController.instance.currentVelocity * 0.95f;
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        frontWheel.transform.Rotate(Vector3.right * speed);
        backWheel.transform.Rotate(Vector3.right * speed);
    }

    public void StartMoving()
    {
        if (!isMoving)
        {
            playerAnimator.SetTrigger("StartWheelie");
            StartCoroutine(DoStunt());
            isMoving = true;
        }
        if (!RG_CameraController.instance.isMoving)
        {
            StartCoroutine(RG_CameraController.instance.PlayMoveCinematic());
        }
    }

    public IEnumerator DoStunt()
    {
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        if (isGrounded)
        {
            playerAnimator.SetFloat("Blend", Mathf.RoundToInt(Random.Range(0f, 1f)));
            playerAnimator.SetTrigger("Stunt");
            yield return new WaitForSeconds(Random.Range(7f, 15f));
        }
        StartCoroutine(DoStunt());
    }

    public void StopMoving()
    {
        if (isMoving)
        {
            playerAnimator.SetTrigger("Brake");
            isMoving = false;
        }
    }

    public IEnumerator TurnRight()
    {
        if (!isTurning)
        {
            isTurning = true;
            while (currentRotation < turnAmount)
            {
                currentRotation += Time.deltaTime * turnSpeed * speed;
                transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(0, turnAmount, 0);
            while (currentRotation > 0)
            {
                currentRotation -= Time.deltaTime * turnSpeed * speed;
                transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isTurning = false;
        }
    }
    public IEnumerator TurnLeft()
    {
        if (!isTurning)
        {
            isTurning = true;
            while (currentRotation > -turnAmount)
            {
                currentRotation -= Time.deltaTime * turnSpeed * speed;
                transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(0, -turnAmount, 0);
            while (currentRotation < 0)
            {
                currentRotation += Time.deltaTime * turnSpeed * speed;
                transform.localRotation = Quaternion.Euler(0, currentRotation, 0);
                yield return null;
            }
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isTurning = false;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            bikeBody.isKinematic = false;
            isFalling = false;
            playerAnimator.SetTrigger("StartWheelie");
            bikeBody.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);

            if (!RG_CameraController.instance.isFast)
            {
                if (Random.Range(0, 5) <= 3)
                {
                    StartCoroutine(RG_CameraController.instance.PlayJumpCinematic());
                }
            }
        }
    }

    public void Land()
    {
        bikeBody.isKinematic = true;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        isGrounded = true;
        isFalling = false;
    }

    public void CheckGroundState(RaycastHit groundCheck)
    {
        if (groundCheck.collider.gameObject.CompareTag("RG_PavedRoad"))
        {
            currentRoad = RoadMaterials.pavedRoad;
        }
        else if (groundCheck.collider.gameObject.CompareTag("RG_DirtRoad"))
        {
            currentRoad = RoadMaterials.dirtRoad;
        }

        if (currentRoad != previousRoad)
        {
            previousRoad = currentRoad;
            currentParticleSystem.SetActive(false);

            if (currentRoad == RoadMaterials.pavedRoad)
            {
                RG_TornadoController.instance.SwitchToPaved();
                currentParticleSystem = pavedParticles;
            }
            else if (currentRoad == RoadMaterials.dirtRoad)
            {
                RG_TornadoController.instance.SwitchToDirt();
                currentParticleSystem = dirtParticles;
            }
            currentParticleSystem.SetActive(true);
        }

        if (isMoving)
        {
            currentParticleSystem.SetActive(true);
        }
        else
        {
            currentParticleSystem.SetActive(false);
        }
    }

    private void Update()
    {
        if (isMoving || !isGrounded)
        {
            MoveAhead();
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Color.red);

        RaycastHit groundCheck;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down, out groundCheck, 0.5f))
        {
            if (!isGrounded)
            {
                Land();
            }
            if (isGrounded)
            {
                CheckGroundState(groundCheck);
            }
        }
        else
        {
            isGrounded = false;
            currentParticleSystem.SetActive(false);



            if (isMoving && bikeVelocity.magnitude < minVelocity && !isFalling)
            {
                isFalling = true;
            }

            if (isFalling)
            {
                bikeBody.AddForce(new Vector3(0, -1, 0) * gravity, ForceMode.Force);
            }
        }

    }
}
