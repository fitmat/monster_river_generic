using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHv2_LaneController : MonoBehaviour
{
    [SerializeField] Transform rightPoint, leftPoint;

    public string[] tileList;
    private GameObject spawnedTile;
    public float minSpeed, maxSpeed, minDelay, maxDelay;

    public int laneDirection;
    private float laneSpeed;
    private float rightZ, leftZ;

    private void Start()
    {
        laneSpeed = Random.Range(minSpeed, maxSpeed);

        rightZ = rightPoint.localPosition.z;
        leftZ = leftPoint.localPosition.z;

        StartCoroutine(SpawnTiles());

    }

    public IEnumerator SpawnTiles()
    {
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        spawnedTile = ObjectPooler.instance.SpawnFromPool(tileList[Random.Range(0, tileList.Length)], Vector3.zero, transform, Quaternion.identity);

        spawnedTile.GetComponent<PHv2_TileController>().SpawnTile(laneDirection, laneSpeed, rightZ, leftZ);
        StartCoroutine(SpawnTiles());
    }

}
