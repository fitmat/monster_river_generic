using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MR_RightPlayerController : MonoBehaviour
{
    public static MR_RightPlayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject player, playerContainer, playerBody;
    public GameObject shootpoint, backTarget;
    public ParticleSystem bulletCentre, bulletsRight, bulletsLeft;
    public LayerMask enemyLayer;

    public List<GameObject> frontLeftEnemies, frontRightEnemies;
    public GameObject targetedEnemy;

    public int range;

    public bool isShootingRight;

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = shootpoint.transform.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(shootpoint.transform.position, forward, Color.red);
    }

    public void ShootFrontRight()
    {
        playerBody.GetComponent<Animator>().ResetTrigger("Right");
        playerBody.GetComponent<Animator>().SetTrigger("Left");
        targetedEnemy = null;
        StartCoroutine(ShootRightDelay());
        isShootingRight = true;
        Debug.Log("ShootFrontRight");
        if (frontRightEnemies.Count == 0)
        {
            playerContainer.transform.LookAt(backTarget.transform);
            MR_AudioManager.instance.PlayAudio("Raygun");
            StartCoroutine(ShootArea());
        }
        else
        {
            targetedEnemy = frontRightEnemies[0];
            playerContainer.transform.LookAt(targetedEnemy.transform);
            MR_AudioManager.instance.PlayAudio("Raygun");
            StartCoroutine(ShootArea());
            targetedEnemy = null;
        }
    }
    public void ShootFrontLeft()
    {
        playerBody.GetComponent<Animator>().ResetTrigger("Left");
        playerBody.GetComponent<Animator>().SetTrigger("Right");
        targetedEnemy = null;
        StartCoroutine(ShootLeftDelay());
        isShootingRight = false;
        Debug.Log("ShootFrontLeft");
        if (frontLeftEnemies.Count == 0)
        {
            playerContainer.transform.LookAt(backTarget.transform);
            MR_AudioManager.instance.PlayAudio("Raygun");
            StartCoroutine(ShootArea());
        }
        else
        {
            targetedEnemy = frontLeftEnemies[0];
            playerContainer.transform.LookAt(targetedEnemy.transform);
            MR_AudioManager.instance.PlayAudio("Raygun");
            StartCoroutine(ShootArea());
            targetedEnemy = null;
        }
    }
    public IEnumerator ShootArea()
    {
        float duration, timeDelay, currentTime;
        RaycastHit hit;
        RaycastHit[] hits;
        duration = 0.3f;
        currentTime = 0;
        timeDelay = 0.1f;

        while (currentTime < duration)
        {
            hits = Physics.SphereCastAll(shootpoint.transform.position, 6, shootpoint.transform.forward, range, enemyLayer);

            foreach (RaycastHit targetHit in hits)
            {
                if (targetHit.collider.gameObject.tag == "MeeleMonster")
                {
                    MR_GameController.instance.playerTwoKills++;
                    targetHit.collider.gameObject.GetComponent<MR_MeeleMonsterController>().Die();
                }
                else if (targetHit.collider.gameObject.tag == "RangedMonster")
                {
                    MR_GameController.instance.playerTwoKills++;
                    targetHit.collider.gameObject.GetComponent<MR_RangedMonsterController>().Die();
                }
            }
            currentTime += timeDelay;
            yield return new WaitForSeconds(timeDelay);
        }
    }
    private IEnumerator ShootLeftDelay()
    {
        Debug.Log("Shoot Test- left");
        bulletsLeft.Stop();
        if (isShootingRight)
        {
            yield return new WaitForSeconds(0.35f);
        }
        else
        {
            yield return null;
        }
        player.GetComponent<Animator>().SetTrigger("Shoot");
        bulletsLeft.Play();
    }
    private IEnumerator ShootRightDelay()
    {
        Debug.Log("Shoot Test- right");
        bulletsRight.Stop();
        if (!isShootingRight)
        {
            yield return new WaitForSeconds(0.35f);
        }
        else
        {
            yield return null;
        }
        player.GetComponent<Animator>().SetTrigger("Shoot");
        bulletsRight.Play();
    }
}
