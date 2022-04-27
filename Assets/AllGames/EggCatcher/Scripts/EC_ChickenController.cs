using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_ChickenController : MonoBehaviour
{
    private int chance;
    private bool hasPassed, isLaying;

    [SerializeField] private Transform eggPoint;

    private void OnEnable()
    {
        // Reset state of chicken
        hasPassed = false;
        isLaying = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Start laying eggs when chicken is in stage boundary
        if (collision.gameObject.tag == "EC_CStart")
        {
            StartCoroutine(StartLayingEggs());
        }
        // Stop laying eggs when chicken is out of stage boundary
        else if (collision.gameObject.tag == "EC_CEnd")
        {
            hasPassed = true;
            gameObject.transform.parent = FindObjectOfType<EC_ObjectPooler>().transform;
            gameObject.SetActive(false);
        }
    }

    private IEnumerator StartLayingEggs()
    {
        if (!isLaying && EC_GameController.instance.gameState == EC_GameController.GameStates.playing)
        {
            isLaying = true;
            yield return new WaitForSeconds(Random.Range(0f, 0.5f));
            StartCoroutine(LayEgg());
        }
    }

    private IEnumerator LayEgg()
    {
        GameObject temp;
        chance = Random.Range(0, 10);
        gameObject.GetComponent<Animator>().SetTrigger("layEgg");
        // Select egg to spawn based on chance
        if (chance < 5)
        {
            temp = EC_ObjectPooler.instance.SpawnFromPool("PlainEgg", eggPoint.position, null, Quaternion.identity);
            temp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            temp.GetComponent<Rigidbody2D>().isKinematic = true;
            temp.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else if (chance < 8)
        {
            temp = EC_ObjectPooler.instance.SpawnFromPool("BlackEgg", eggPoint.position, null, Quaternion.identity);
            temp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            temp.GetComponent<Rigidbody2D>().isKinematic = true;
            temp.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        else
        {
            temp = EC_ObjectPooler.instance.SpawnFromPool("GoldEgg", eggPoint.position, null, Quaternion.identity);
            temp.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            temp.GetComponent<Rigidbody2D>().isKinematic = true;
            temp.GetComponent<Rigidbody2D>().isKinematic = false;
        }
        if (!hasPassed)
        {
            yield return new WaitForSecondsRealtime(Random.Range(0.5f, 1.5f));
            StartCoroutine(LayEgg());
        }
    }

}
