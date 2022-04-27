using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handles random spawning of fish

public class FTv2_FishSpawnController : MonoBehaviour
{
    public static FTv2_FishSpawnController instance;

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
    [SerializeField] GameObject[] leftHoles, rightHoles;
    public int noOfNormalFish;

    private void Start()
    {
        StartCoroutine(CinematicFish());
    }

    public IEnumerator CinematicFish()
    {
        yield return new WaitForSeconds(5f);
        ObjectPooler.instance.SpawnFromPool("Fish" + Random.Range(0, noOfNormalFish - 2), Vector3.zero, leftHoles[Random.Range(0, 2)].transform, Quaternion.identity);
        ObjectPooler.instance.SpawnFromPool("Fish" + Random.Range(0, noOfNormalFish - 2), Vector3.zero, rightHoles[Random.Range(0, 2)].transform, Quaternion.identity);

    }


    public void StartSpawningFish()
    {
        StartCoroutine(SpawnLeftFish());
        StartCoroutine(SpawnRightFish());
    }

    public IEnumerator SpawnLeftFish()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        // Spawn fish from pool by setting position and parent to correct hole
        ObjectPooler.instance.SpawnFromPool("Fish" + Random.Range(0, noOfNormalFish), Vector3.zero, leftHoles[Random.Range(0, 2)].transform, Quaternion.identity);
        StartCoroutine(SpawnLeftFish());
    }
    public IEnumerator SpawnRightFish()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        ObjectPooler.instance.SpawnFromPool("Fish" + Random.Range(0, noOfNormalFish), Vector3.zero, rightHoles[Random.Range(0, 2)].transform, Quaternion.identity);
        StartCoroutine(SpawnRightFish());
    }
}
