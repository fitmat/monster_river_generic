using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_EnemySpawner : MonoBehaviour
{
    public static TR_EnemySpawner instance;

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

    [SerializeField] private GameObject[] backSpawnpoints;
    [SerializeField] private GameObject selectedSpawnPoint;

    private int randomInt;
    private float delay;

    public void StartSpawning()
    {
        if (MR_GameController.instance.isGameRunning)
        {
            StartCoroutine(SpawnBackMonster());
        }
    }

    private IEnumerator SpawnBackMonster()
    {
        selectedSpawnPoint = backSpawnpoints[Random.Range(0, backSpawnpoints.Length)];

        randomInt = Random.Range(1, 9);
            MR_ObjectPooler.instance.SpawnFromPool("Monster" + randomInt.ToString(), selectedSpawnPoint.transform.position, selectedSpawnPoint.transform.rotation);
        
        delay = Random.Range(0.5f, 1f);
        yield return new WaitForSecondsRealtime(delay);
        if (MR_GameController.instance.isGameRunning)
        {
            StartCoroutine(SpawnBackMonster());
        }
    }
}
