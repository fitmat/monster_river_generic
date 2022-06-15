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

    public List<GameObject> enemySpawned = new List<GameObject>();
    [SerializeField]
    private int numberOfEnemiesAllowed;

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

        //if for controling the number of monsters on the field
        if (numberOfEnemiesAllowed > enemySpawned.Count)
        {
            //Spawnning Monsters if they are less
            randomInt = Random.Range(1, 9);
            GameObject g = MR_ObjectPooler.instance.SpawnFromPool("Monster" + randomInt.ToString(), selectedSpawnPoint.transform.position, selectedSpawnPoint.transform.rotation);

            enemySpawned.Add(g);
        }

        delay = Random.Range(0.5f, 1f);
        yield return new WaitForSecondsRealtime(delay);

        if (MR_GameController.instance.isGameRunning)
        {
            StopCoroutine(SpawnBackMonster());
            StartCoroutine(SpawnBackMonster());
        }
    }
}
