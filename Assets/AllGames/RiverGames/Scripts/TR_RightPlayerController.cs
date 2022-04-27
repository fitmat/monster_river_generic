using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_RightPlayerController : MonoBehaviour
{
    public static TR_RightPlayerController instance;
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

    public GameObject player, playerContainer;

    public int range;

    // Update is called once per frame
    void Update()
    {
        //Vector3 forward = shootpoint.transform.TransformDirection(Vector3.forward) * range;
        //Debug.DrawRay(shootpoint.transform.position, forward, Color.red);
    }
    #region MR Code
    /*
    public void ShootFrontRight()
    {
        targetedEnemy = null;
        Debug.Log("ShootFrontRight");
        if (frontRightEnemies.Count == 0)
        {
            MR_AudioManager.instance.PlayAudio("Raygun");
            player.GetComponent<Animator>().SetTrigger("Shoot");
            bullets.Stop();
            bullets.Play();
            //Shoot();
            StartCoroutine(ShootArea());
        }
        else
        {
            targetedEnemy = frontRightEnemies[0];
            int i = 1;
            while (!targetedEnemy.activeSelf)
            {
                targetedEnemy = frontRightEnemies[i];
                i++;
                if (i == frontRightEnemies.Count)
                {
                    MR_AudioManager.instance.PlayAudio("Raygun");
                    player.GetComponent<Animator>().SetTrigger("Shoot");
                    bullets.Stop();
                    bullets.Play();
                    //Shoot();
                    StartCoroutine(ShootArea());
                    return;
                }
            }
            playerContainer.transform.LookAt(targetedEnemy.transform);
            MR_AudioManager.instance.PlayAudio("Raygun");
            player.GetComponent<Animator>().SetTrigger("Shoot");
            bullets.Stop();
            bullets.Play();
            //Shoot();
            StartCoroutine(ShootArea());
            targetedEnemy = null;
        }
    }
    public void ShootFrontLeft()
    {
        targetedEnemy = null;
        Debug.Log("ShootFrontLeft");
        if (frontLeftEnemies.Count == 0)
        {
            MR_AudioManager.instance.PlayAudio("Raygun");
            player.GetComponent<Animator>().SetTrigger("Shoot");
            bullets.Stop();
            bullets.Play();
            //Shoot();
            StartCoroutine(ShootArea());
        }
        else
        {
            targetedEnemy = frontLeftEnemies[0];
            int i = 1;
            while (!targetedEnemy.activeSelf)
            {
                targetedEnemy = frontLeftEnemies[i];
                i++;
                if (i == frontLeftEnemies.Count)
                {
                    MR_AudioManager.instance.PlayAudio("Raygun");
                    player.GetComponent<Animator>().SetTrigger("Shoot");
                    bullets.Stop();
                    bullets.Play();
                    //Shoot();
                    StartCoroutine(ShootArea());
                    return;
                }
            }
            playerContainer.transform.LookAt(targetedEnemy.transform);
            MR_AudioManager.instance.PlayAudio("Raygun");
            player.GetComponent<Animator>().SetTrigger("Shoot");
            bullets.Stop();
            bullets.Play();
            //Shoot();
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
            hits = Physics.SphereCastAll(shootpoint.transform.position, 3, shootpoint.transform.forward, range, enemyLayer);

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
    */
    #endregion

    public void MoveRaft()
    {
        player.GetComponent<Animator>().SetTrigger("Row");
        TR_RaftController.instance.MoveRaft();
    }
}
