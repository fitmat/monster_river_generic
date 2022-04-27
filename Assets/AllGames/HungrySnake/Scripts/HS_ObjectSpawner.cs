using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_ObjectSpawner : MonoBehaviour
{
    public float maxX, minX, maxZ, minZ;
    public int itemCount;

    private Vector3 spawnPosition;

    private void Start()
    {
        StartCoroutine(SpawnItem());
    }


    private IEnumerator SpawnItem()
    {
        while (true)
        {
            spawnPosition = new Vector3(Random.Range(minX, maxX), 0.4f, Random.Range(minZ, maxZ));

            Debug.Log(spawnPosition);
            Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, 4f);

            if (hitColliders.Length != 0)
            {
                break;
            }

        }

        Debug.Log("Spawning Item");

        GameObject _tempObject = HS_ObjectPooler.instance.SpawnFromPool("Item" + Random.Range(0, itemCount), spawnPosition, Quaternion.identity);

        _tempObject.GetComponent<Transform>().position = spawnPosition;

        yield return new WaitForSecondsRealtime(3f);
        StartCoroutine(SpawnItem());

    }

}
