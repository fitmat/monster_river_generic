using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_SpawnBall : MonoBehaviour
{
    [SerializeField] private GameObject basicBall;

    public int direction;


    private void OnEnable()
    {
        StartCoroutine(SpawnBall());
    }


    private IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        if (BM_GameController.instance.isBonusTimeOn)
        {
            Debug.Log("Boomerang Test: Spawn Ball");
            GameObject newBall = Instantiate(basicBall, transform.position, Quaternion.identity);
            newBall.GetComponent<Rigidbody>().AddForce(Vector3.left * direction * Random.Range(4f, 10f), ForceMode.Impulse);
            BM_GameController.instance.smallBallsLeft++;
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            StartCoroutine(SpawnBall());
        }
    }
}
