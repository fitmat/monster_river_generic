using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [SerializeField] private float jumpForce = 4.0f;
        [SerializeField] private float gravity = 12f;
        [SerializeField] private float speed = 7.0f;
        [SerializeField] private float speedIncreaseTime = 7.5f;
        [SerializeField] private float speedIncreaseAmount = 0.1f;
        [SerializeField] private float LANE_DISTANCE = 3.5f;
        [SerializeField] private float TURN_SPEED = 0.05f;
        [SerializeField] private EnemyDetector enemyDetector;

        private float verticalVelocity;
        private int desiredLane = 1;
        private float speedIncreaseLastTick;
        private float originalSpeed;

        private bool sliding = false;
        private bool isRunning = false;
        private bool protectPlayer = false;
        private bool protectPlayerFromShieldEnemy = false;
        private bool hurricaneKickStarted = false;
        private bool isInAir = true;

        private void Start()
        {
            characterController = this.GetComponent<CharacterController>();
            animator = this.GetComponent<Animator>();
            originalSpeed = speed;
        }

        private void Update()
        {

            if (!isRunning)
                return;
             
            if ((Time.time - speedIncreaseLastTick) > speedIncreaseTime)
            {
                speedIncreaseLastTick = Time.time;
                speed += speedIncreaseAmount;
                GameManager.Instance.UpdateModifier(speed - originalSpeed);
            }


            if (Input.GetKeyDown(KeyCode.F))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || TouchInputController.Instance.SwipeLeft)
            {
                MoveLane(false);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || TouchInputController.Instance.SwipeRight)
            {
                MoveLane(true);
            }
            

            Vector3 targetPosition = transform.position.z * Vector3.forward;

            if (desiredLane == 0)
            {
                targetPosition += Vector3.left * LANE_DISTANCE;
            }
            else if (desiredLane == 2)
            {
                targetPosition += Vector3.right * LANE_DISTANCE;
            }

            Vector3 moveVector = Vector3.zero;
            moveVector.x = (targetPosition - transform.position).x * speed;

            bool isGrounded = isPlayerGrounded();
            Debug.Log("isGrounded : " + isGrounded);

            if (isGrounded)
            {
                verticalVelocity = -0.1f;
                animator.SetBool("isGrounded", true);
                RaycastHit hit;
                Debug.DrawLine(this.transform.position + new Vector3(0f, 0.25f, 0f), Vector3.forward, Color.blue, 2f);
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || TouchInputController.Instance.SwipeUp)
                {
                    if (!sliding)
                    {
                        animator.SetBool("isGrounded", false);
                        animator.SetTrigger("jumped");
                        verticalVelocity = jumpForce;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || TouchInputController.Instance.SwipeDown)
                {

                    if (Physics.Raycast(this.transform.position + new Vector3(0f, 0.25f, 0f), Vector3.forward * 10f, out hit, 200f))
                    {
                        Debug.Log("Info : " + hit.collider.tag);
                        //Debug.Log("Enemy InFront : " + Physics.BoxCast(this.transform.position, new Vector3(2f, 1f, 10f), Vector3.forward, out hit, Quaternion.identity, 50f));
                        if (hit.collider.gameObject.tag == "Enemy" &&
                            hit.collider.gameObject.GetComponent<Enemies>().enemyBehaviour == Enemies.Behaviour.attacker)
                        {
                            Attack();
                        }
                        else
                        {
                            StartSliding();
                            Invoke("StopSliding", 1f);
                        }
                    }
                    else
                    {
                        StartSliding();
                        Invoke("StopSliding", 1f);
                    }

                    
                }
            }
            else
            {
                verticalVelocity -= (gravity * Time.deltaTime);

                if (!isInAir)
                {
                    animator.SetBool("isGrounded", false);
                    animator.SetTrigger("Falling");
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || TouchInputController.Instance.SwipeDown)
                {
                    verticalVelocity = -jumpForce * speed;
                    StartSliding();
                    Invoke("StopSliding", 1f);
                }
            }

            moveVector.y = verticalVelocity;
            moveVector.z = speed;

            characterController.Move(moveVector * Time.deltaTime);

            Vector3 direction = characterController.velocity;
            direction.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, direction, TURN_SPEED);

        }

        private void MoveLane(bool goingRight)
        {
            desiredLane += (goingRight) ? 1 : -1;
            desiredLane = Mathf.Clamp(desiredLane, 0, 2);
        }

        private bool isPlayerGrounded()
        {
            Ray groundRay = new Ray(new Vector3(
                    characterController.bounds.center.x,
                    (characterController.bounds.center.y - characterController.bounds.extents.y) + 0.2f,
                    characterController.bounds.center.z),
                    Vector3.down);

            Debug.DrawRay(groundRay.origin, groundRay.direction, Color.red, 1.0f);
            isInAir = Physics.Raycast(transform.position, -transform.up, 2.5f);
            Debug.Log("IsInAir : " + isInAir);
            return Physics.Raycast(groundRay, 0.2f + 0.1f);

        }

        private void StartSliding()
        {
            animator.SetBool("Sliding", true);
            characterController.height /= 2;
            characterController.center = new Vector3(
                    characterController.center.x,
                    characterController.center.y / 2f,
                    characterController.center.z
                );
            sliding = true;
            if(!hurricaneKickStarted)
                protectPlayerFromShieldEnemy = true;
        }

        private void StopSliding()
        {
            animator.SetBool("Sliding", false);
            characterController.height *= 2;
            characterController.center = new Vector3(
                    characterController.center.x,
                    characterController.center.y * 2f,
                    characterController.center.z
                );
            sliding = false;
            protectPlayerFromShieldEnemy = false;
        }

        private void StartHurricaneKick()
        {
            animator.SetBool("attacking", true);
            protectPlayer = true;
            hurricaneKickStarted = true;
        }

        private void StopHurricaneKick()
        {
            animator.SetBool("attacking", false);
            protectPlayer = false;
            hurricaneKickStarted = false;
        }

        private void Crash()
        {
            if (!GameManager.Instance.isDied)
            {
                animator.SetTrigger("Dead");
                isRunning = false;
                GameManager.Instance.OnDeath();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {

            Debug.Log("protectPlayer " + protectPlayer);

            switch (hit.gameObject.tag)
            {
                case "Obstacle":
                    //TODO add invincible mode
                    Crash();
                    break;
                case "Enemy":
                    if (hit.gameObject.GetComponent<Enemies>().enemyBehaviour == Enemies.Behaviour.attacker)
                    {
                        if (protectPlayer)
                        {
                            hit.gameObject.GetComponent<Enemies>().Death();
                            StopHurricaneKick();
                        }
                        else
                            Crash();
                    }
                    else
                    { 
                        if(protectPlayerFromShieldEnemy)
                            hit.gameObject.GetComponent<Enemies>().Death();
                        else
                            Crash();
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                if (!protectPlayer)
                    Crash();
            }
        }

        public void StartRunning()
        {
            isRunning = true;
            animator.SetTrigger("StartRunning");
        }

        public void Attack()
        {
            if (isPlayerGrounded())
            {
                StartHurricaneKick();
                Invoke("StopHurricaneKick", 1f);
            }
        }

    }
}
