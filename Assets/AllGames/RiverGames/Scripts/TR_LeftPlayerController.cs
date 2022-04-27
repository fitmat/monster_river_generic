using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_LeftPlayerController : MonoBehaviour
{
    public static TR_LeftPlayerController instance;
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

    public List<GameObject> backLeftEnemies, backRightEnemies;
    public GameObject targetedEnemy;

    public int range;

    public bool isShootingRight;


    // Update is called once per frame
    void Update()
    {
        Vector3 forward = shootpoint.transform.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(shootpoint.transform.position, forward, Color.red);
    }

    #region MR Code
    
    public void ShootBackLeft()
    {
        playerBody.GetComponent<Animator>().ResetTrigger("Right");
        playerBody.GetComponent<Animator>().SetTrigger("Left");
        StartCoroutine(ShootLeftDelay());
        isShootingRight = false;
        targetedEnemy = null;
        Debug.Log("ShootBackLeft");
        if (backLeftEnemies.Count == 0)
        {
            playerContainer.transform.LookAt(backTarget.transform);
            MR_AudioManager.instance.PlayAudio("Shotgun");
            //Shoot();
            StartCoroutine(ShootArea());
        }
        else
        {
            targetedEnemy = backLeftEnemies[0];
            //int i = 1;
            //while (!targetedEnemy.activeSelf)
            //{
            //    targetedEnemy = backLeftEnemies[i];
            //    i++;
            //    if (i == backLeftEnemies.Count)
            //    {
            //        MR_AudioManager.instance.PlayAudio("Shotgun");
            //        player.GetComponent<Animator>().SetTrigger("Shoot");
            //        bullets.Stop();
            //        bullets.Play();
            //        //Shoot();
            //        StartCoroutine(ShootArea());
            //        return;
            //    }
            //}
            playerContainer.transform.LookAt(targetedEnemy.transform);
            MR_AudioManager.instance.PlayAudio("Shotgun");
            //Shoot();
            StartCoroutine(ShootArea());
            targetedEnemy = null;
        }
    }
    public void ShootBackRight()
    {
        playerBody.GetComponent<Animator>().ResetTrigger("Left");
        playerBody.GetComponent<Animator>().SetTrigger("Right");
        StartCoroutine(ShootRightDelay());
        isShootingRight = true;
        targetedEnemy = null;
        Debug.Log("ShootBackRight");
        if (backRightEnemies.Count == 0)
        {
            playerContainer.transform.LookAt(backTarget.transform);
            MR_AudioManager.instance.PlayAudio("Shotgun");
            //Shoot();
            StartCoroutine(ShootArea());
        }
        else
        {
            targetedEnemy = backRightEnemies[0];
            //int i = 1;
            //while (!targetedEnemy.activeSelf)
            //{
            //    targetedEnemy = backRightEnemies[i];
            //    i++;
            //    if (i == backRightEnemies.Count)
            //    {
            //        MR_AudioManager.instance.PlayAudio("Shotgun");
            //        player.GetComponent<Animator>().SetTrigger("Shoot");
            //        bullets.Stop();
            //        bullets.Play();
            //        //Shoot();
            //        StartCoroutine(ShootArea());
            //        return;
            //    }
            //}
            playerContainer.transform.LookAt(targetedEnemy.transform);
            MR_AudioManager.instance.PlayAudio("Shotgun");
            //Shoot();
            StartCoroutine(ShootArea());
            targetedEnemy = null;
        }
    }
    
    #endregion
    /*
    public void ShootBack()
    {
        targetedEnemy = null;
        if (backEnemies.Count == 0)
        {
            MR_AudioManager.instance.PlayAudio("Shotgun");
            player.GetComponent<Animator>().SetTrigger("Shoot");
            bullets.Stop();
            bullets.Play();
            //Shoot();
            StartCoroutine(ShootArea());
            return;
        }
        else
        {
            targetedEnemy = backEnemies[0];

            int i = 1;
            while (!targetedEnemy.activeSelf)
            {
                targetedEnemy = backEnemies[i];
                i++;
                if (i == backEnemies.Count)
                {
                    MR_AudioManager.instance.PlayAudio("Shotgun");
                    player.GetComponent<Animator>().SetTrigger("Shoot");
                    bullets.Stop();
                    bullets.Play();
                    //Shoot();
                    StartCoroutine(ShootArea());
                    return;
                }
            }
        }
        playerContainer.transform.LookAt(targetedEnemy.transform);
        MR_AudioManager.instance.PlayAudio("Shotgun");
        player.GetComponent<Animator>().SetTrigger("Shoot");
        bullets.Stop();
        bullets.Play();
        //Shoot();
        StartCoroutine(ShootArea());
        targetedEnemy = null;

    }
    */
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
            hits = Physics.SphereCastAll(shootpoint.transform.position, 6, shootpoint.transform.forward, range/2, enemyLayer);

            foreach (RaycastHit targetHit in hits)
            {
                if (targetHit.collider.gameObject.tag == "MeeleMonster")
                {
                    MR_GameController.instance.playerOneKills++;
                    targetHit.collider.gameObject.GetComponent<TR_MeeleMonsterController>().Die();
                }
                else if (targetHit.collider.gameObject.tag == "RangedMonster")
                {
                    MR_GameController.instance.playerOneKills++;
                    targetHit.collider.gameObject.GetComponent<TR_RangedMonsterController>().Die();
                }
            }
            currentTime += timeDelay;
            yield return new WaitForSeconds(timeDelay);
        }
    }

    private IEnumerator ShootLeftDelay()
    {
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
