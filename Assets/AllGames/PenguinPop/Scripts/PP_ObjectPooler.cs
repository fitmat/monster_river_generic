using System.Collections.Generic;
using UnityEngine;

/*
 * Object Pooler script to create pools of objects that will be repeatedly spawned in Game
 * */
public class PP_ObjectPooler : MonoBehaviour
{
    // Declare pool of objecs to be defined in editor
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static PP_ObjectPooler instance;
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> objectPools;
    
    private void Awake()
    {
        // Declare class as Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Initialize list of object pools
        objectPools = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            objectPools.Add(pool.tag, objectPool);
        }
    }
    
    // Function to spawn object from pool and set its initial position and rotation
    public GameObject SpawnFromPool(string tag, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject objectToSpawn = objectPools[tag].Dequeue();
        objectToSpawn.SetActive(false);
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.transform.rotation = spawnRotation;
        objectPools[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
