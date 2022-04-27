using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject target, obstacleParent;
    private GameObject obstacleToSpawn;
    public Vector3 spawnPosition;
    public float spawnDistance;
    public int noOfObstacles;


    private void Start()
    {
        StartCoroutine(PlaceObstacle());
    }

    public IEnumerator PlaceObstacle()
    {
        if (RG_GameController.instance.isEarlyGame)
        {
            yield return new WaitForSecondsRealtime(Random.Range(5f, 8f));
        }
        else
        {
            yield return new WaitForSecondsRealtime(Random.Range(3f, 6f));
        }

        spawnDistance = target.transform.position.z + (RG_GameController.instance.maxVelocity * Random.Range(6f, 10f));
        spawnPosition = new Vector3(0, 0, spawnDistance);
        obstacleToSpawn = RG_ObjectPooler.instance.SpawnFromPool("Obstacle" + Random.Range(0, noOfObstacles), spawnPosition, obstacleParent.transform, Quaternion.identity);
        StartCoroutine(PlaceObstacle());
    }
}
