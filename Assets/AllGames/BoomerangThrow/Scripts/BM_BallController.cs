using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_BallController : MonoBehaviour
{
    private bool isDespawning = false;


    [SerializeField] private GameObject waterSplash;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BT_LBoomerang"))
        {
            Debug.Log("L Boomerang Hit");
            BM_GameController.instance.changeScore(1, 1);
            StartCoroutine(BurstBall());
        }
        else if (collision.gameObject.CompareTag("BT_RBoomerang"))
        {
            Debug.Log("R Boomerang Hit");
            BM_GameController.instance.changeScore(2, 1);
            StartCoroutine(BurstBall());
        }
        else if (collision.gameObject.CompareTag("EnemyDespawn"))
        {
            if (!isDespawning)
            {
                isDespawning = true;
                StartCoroutine(DespawnBall());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BT_Water")
        {
            try
            {
                GameObject water = Instantiate(waterSplash, transform.position, Quaternion.identity);
                water.GetComponent<ParticleSystem>().Play();
            }
            catch
            {

            }
        }
    }

    private IEnumerator DespawnBall()
    {
        float time = Random.Range(7f, 14f);
        yield return new WaitForSeconds(time);
        StartCoroutine(BurstBall());
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }

    public IEnumerator BurstBall()
    {
        GetComponentInParent<BM_StackMover>().BallPopped();
        transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        gameObject.GetComponent<Collider>().isTrigger = true;
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(2).gameObject.SetActive(false);
        BM_AudioManager.instance.PlayAudio("BallBurst");
        yield return new WaitForSeconds(1f);
        //BM_GameController.instance.smallBallsLeft--;
        //if (BM_GameController.instance.smallBallsLeft == 0)
        //{
        //    BM_GameController.instance.GameOver();
        //}
        gameObject.SetActive(false);
    }

    public void RespawnBall(Vector3 spawnPosition)
    {
        // Respawn ball below headball by resetting scale and position
        gameObject.SetActive(true);
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = spawnPosition;
        transform.GetChild(2).gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().isTrigger = false;
    }
}
