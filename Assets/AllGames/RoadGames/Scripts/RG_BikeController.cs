using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_BikeController : MonoBehaviour
{
    [SerializeField] private GameObject frontWheel, rearWheel, steering;
    [SerializeField] private Rigidbody bikeBody;
    [SerializeField] private WheelCollider frontRightWheelCollider, frontLeftWheelCollider, rearRightWheelCollider, rearLeftWheelCollider;
    [SerializeField] private Animator playerAnimator;

    private Quaternion wheelRotation;
    private Vector3 wheelPosition;


    public float torque, steerAngle, jumpForce, brakeForce, stopForce, maxVelocity, minVelocity, maxAllowedVelocity, minAllowedVelocity, velocityIncrement;
    public bool isAccelerating, hasPassedBlock, isGrounded, isTurning, isGearShifting;


    public Vector3 bikeVelocity;

    public float accInput, steerInput;

    private void Start()
    {
        hasPassedBlock = false;
        //StartCoroutine(StartMoving());
    }

    public IEnumerator StartMoving()
    {
        yield return new WaitForSecondsRealtime(2f);
        StartCoroutine(StartAccelerating());
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject _collidedObject = other.gameObject;

        if (_collidedObject.CompareTag("RG_BlockEnd") && gameObject.CompareTag("RG_PlayerOne") && !hasPassedBlock)
        {
            StartCoroutine(PassBlock());
            Debug.Log("Hit");
            hasPassedBlock = true;
        }
    }

    public IEnumerator DoStunt()
    {
        yield return new WaitForSecondsRealtime(Random.Range(3f, 6f));
        if (isGrounded)
        {
            playerAnimator.SetFloat("Blend", Mathf.RoundToInt(Random.Range(0f, 1f)));
            playerAnimator.SetTrigger("Stunt");
            yield return new WaitForSecondsRealtime(Random.Range(7f, 15f));
        }
        StartCoroutine(DoStunt());
    }
    public IEnumerator PassBlock()
    {
        StartCoroutine(RG_LevelGenerator.instance.SpawnNewBlock());
        StartCoroutine(RG_LevelGenerator.instance.DespawnOldBlock());
        yield return null;
        hasPassedBlock = false;
    }

    public void Accelerate()
    {
        if (isAccelerating)
        {
            rearRightWheelCollider.motorTorque = torque * accInput;
            rearLeftWheelCollider.motorTorque = torque * accInput;

            rearRightWheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
            rearWheel.transform.localRotation = wheelRotation;
            frontWheel.transform.localRotation = wheelRotation;
        }

    }

    public IEnumerator StartAccelerating()
    {
        bikeBody.isKinematic = false;
        playerAnimator.SetTrigger("StartWheelie");
        isAccelerating = true;
        accInput = 1;
        yield return new WaitForSecondsRealtime(0.5f);
        playerAnimator.SetTrigger("StopWheelie");
        StartCoroutine(DoStunt());

    } 

    public IEnumerator StopAccelerating()
    {
        playerAnimator.SetTrigger("Brake");
        accInput = 0;
        yield return null;
        isAccelerating = false;
        Brake();

    }

    public IEnumerator SteerRight()
    {
        playerAnimator.SetTrigger("StartRightTurn");
        steerInput = 0;
        while (steerInput < 1)
        {
            steerInput += Time.deltaTime * 5;
            frontRightWheelCollider.steerAngle = steerInput * steerAngle;
            frontLeftWheelCollider.steerAngle = steerInput * steerAngle;
            yield return null;
        }
        //yield return new WaitForSecondsRealtime(0.5f);
        while (steerInput > 0)
        {
            steerInput -= Time.deltaTime * 5;
            frontRightWheelCollider.steerAngle = steerInput * steerAngle;
            frontLeftWheelCollider.steerAngle = steerInput * steerAngle;
            yield return null;
        }
        playerAnimator.SetTrigger("StopRightTurn");
        steerInput = 0;
        frontRightWheelCollider.steerAngle = steerInput * steerAngle;
        frontLeftWheelCollider.steerAngle = steerInput * steerAngle;

    }
    public IEnumerator SteerLeft()
    {
        playerAnimator.SetTrigger("StartLeftTurn");
        steerInput = 0;
        while (steerInput > -1)
        {
            steerInput -= Time.deltaTime * 5;
            frontRightWheelCollider.steerAngle = steerInput * steerAngle;
            frontLeftWheelCollider.steerAngle = steerInput * steerAngle;
            yield return null;
        }
        //yield return new WaitForSecondsRealtime(0.5f);
        while (steerInput < 0)
        {
            steerInput += Time.deltaTime * 5;
            frontRightWheelCollider.steerAngle = steerInput * steerAngle;
            frontLeftWheelCollider.steerAngle = steerInput * steerAngle;
            yield return null;
        }
        playerAnimator.SetTrigger("StopLeftTurn");
        steerInput = 0;
        frontRightWheelCollider.steerAngle = steerInput * steerAngle;
        frontLeftWheelCollider.steerAngle = steerInput * steerAngle;

    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerAnimator.SetTrigger("StartWheelie");
            bikeBody.AddForce(new Vector3(0, 1, 0) * jumpForce, ForceMode.Impulse);
        }
    }

    public void Brake()
    {
        bikeBody.drag = brakeForce;
    }
    public void StopBrake()
    {
        bikeBody.drag = 0;
    }

    public void AirBrake()
    {
        //bikeBody.drag = stopForce;
    }

    public void StopBike()
    {
        playerAnimator.SetTrigger("Idle");
        bikeVelocity = Vector3.zero;
        bikeBody.drag = stopForce;
        bikeBody.isKinematic = true;
    }

    private void Update()
    {
        bikeVelocity = bikeBody.velocity;

        Accelerate();

        if (bikeVelocity.magnitude > maxVelocity)
        {
            Brake();
        }
        else if(isAccelerating && isGrounded)
        {
            StopBrake();
        }
        else if(!isAccelerating && bikeVelocity.magnitude < minVelocity && isGrounded)
        {
            StopBike();
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
                isGrounded = true;
                playerAnimator.SetTrigger("StopWheelie");
            }
        }
        else
        {
            AirBrake();
            isGrounded = false;
        }

    }


}
