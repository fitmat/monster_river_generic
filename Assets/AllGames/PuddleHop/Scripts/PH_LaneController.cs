using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PH_LaneController : MonoBehaviour
{
    [SerializeField] Transform rightPoint, leftPoint;

    public string[] tileList;
    private GameObject spawnedTile;
    public float minSpeed, maxSpeed, minDelay, maxDelay;

    public int laneDirection;
    private float laneSpeed;
    private float rightX, leftX;

    private void Start()
    {
        // Set speed and endpoints of lane

        laneSpeed = Random.Range(minSpeed, maxSpeed);

        rightX = rightPoint.localPosition.x;
        leftX = leftPoint.localPosition.x;

        // Start spawning tiles
        StartCoroutine(DelaySpawning());

    }

    public IEnumerator DelaySpawning()
    {
        yield return new WaitForSeconds(7f);
        StartCoroutine(SpawnTiles());
    }

    // Function to spawn tiles
    public IEnumerator SpawnTiles()
    {
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

        spawnedTile = PH_ObjectPooler.instance.SpawnFromPool(tileList[Random.Range(0, tileList.Length)], Vector3.zero, transform, Quaternion.identity);

        spawnedTile.GetComponent<PH_TileController>().SpawnTile(laneDirection, laneSpeed, rightX, leftX);
        StartCoroutine(SpawnTiles());
    }

}
