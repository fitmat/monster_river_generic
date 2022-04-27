using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_TornadoController : MonoBehaviour
{
    public static RG_TornadoController instance;

    private void Awake()
    {
        // Declare class as Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Transform startPosition, normalPosition, closePosition, farPosition, targetPosition, closestPosition;
    public float currentDistance, normalDistance, farthestDistance, closestDistance, stallDistance, normalVelocity, fastVelocity, slowVelocity, startVelocity, currentVelocity;
    public bool isSlowed, isFast, isStarting, isStationary;

    [SerializeField] GameObject currentParticleSystem;
    [SerializeField] GameObject pavedParticles, dirtParticles;


    private void Start()
    {
        normalDistance = targetPosition.position.z - normalPosition.position.z;
        closestDistance = targetPosition.position.z - closePosition.position.z;
        farthestDistance = targetPosition.position.z - farPosition.position.z;
        stallDistance = targetPosition.position.z - closestPosition.position.z;

        transform.position = startPosition.position;

        isStarting = true;
        normalVelocity = RG_GameController.instance.currentVelocity;
        startVelocity = normalVelocity * 5f;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("RG_Obstacle"))
        {
            StartCoroutine(DestroyObstacle(other.gameObject.transform.parent.gameObject));
        }

    }

    public void SwitchToDirt()
    {
        currentParticleSystem.SetActive(false);
        currentParticleSystem = dirtParticles;
        currentParticleSystem.SetActive(true);
    }
    public void SwitchToPaved()
    {
        currentParticleSystem.SetActive(false);
        currentParticleSystem = pavedParticles;
        currentParticleSystem.SetActive(true);
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
        normalVelocity= RG_GameController.instance.currentVelocity;
        slowVelocity = normalVelocity * .9f;
        fastVelocity = normalVelocity * 1.2f;

        currentDistance = targetPosition.position.z - transform.position.z;

        if (isStarting)
        {
            currentVelocity = startVelocity;
            if (currentDistance < farthestDistance)
            {
                isStarting = false;
                isFast = true;
            }
        }
        if (isFast)
        {
            currentVelocity = fastVelocity;
            if (currentDistance < closestDistance)
            {
                isFast = false;
                isSlowed = true;
            }
        }
        if (isSlowed)
        {
            currentVelocity = slowVelocity;
            if (currentDistance > farthestDistance)
            {
                isSlowed = false;
                isFast = true;
            }
            else if (currentDistance > normalDistance)
            {
                isSlowed = false;
                isFast = false;
            }
        }

        if(currentDistance < stallDistance)
        {
            isSlowed = false;
            isFast = false;
            isStationary = true;
        }

        if (currentDistance > farthestDistance && !isStarting)
        {
            isSlowed = false;
            isFast = true;
            isStationary = false;
        }

        if (isStationary)
        {
            currentVelocity = 0;
            if (currentDistance > farthestDistance)
            {
                isSlowed = false;
                isStationary = false;
                isFast = true;
            }
        }


        transform.Translate(Vector3.forward * currentVelocity * Time.deltaTime);

    }


    private void Update()
    {
        MoveAhead();
    }

}
