using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MR_MonsterSpawner : MonoBehaviour
{
    public static MR_MonsterSpawner instance;

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

    [SerializeField] private GameObject[] backSpawnpoints, frontSpawnpoints;
    [SerializeField] private GameObject selectedSpawnPoint;

    private int randomInt;
    private float delay;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartSpawning()
    {
        if (MR_GameController.instance.isGameRunning)
        {
            StartCoroutine(SpawnFrontMonster());
            StartCoroutine(SpawnBackMonster());
        }
    }

    private IEnumerator SpawnFrontMonster()
    {
        selectedSpawnPoint = frontSpawnpoints[Random.Range(0, frontSpawnpoints.Length)];

        randomInt = Random.Range(1, 5);

        MR_ObjectPooler.instance.SpawnFromPool("RangedMonster" + randomInt.ToString(), selectedSpawnPoint.transform.position, selectedSpawnPoint.transform.rotation);

        delay = Random.Range(0.5f, 1f);
        yield return new WaitForSecondsRealtime(delay);
        if (MR_GameController.instance.isGameRunning && !MR_GameController.instance.isRiverOver)
        {
            StartCoroutine(SpawnFrontMonster());
        }
    }

    private IEnumerator SpawnBackMonster()
    {
        selectedSpawnPoint = backSpawnpoints[Random.Range(0, backSpawnpoints.Length)];

        randomInt = Random.Range(1, 5);

        MR_ObjectPooler.instance.SpawnFromPool("MeeleMonster" + randomInt.ToString(), selectedSpawnPoint.transform.position, selectedSpawnPoint.transform.rotation);
        delay = Random.Range(0.5f, 1f);
        yield return new WaitForSecondsRealtime(delay);
        if (MR_GameController.instance.isGameRunning && !MR_GameController.instance.isRiverOver)
        {
            StartCoroutine(SpawnBackMonster());
        }
    }
}
