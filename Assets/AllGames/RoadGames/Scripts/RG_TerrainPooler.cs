using System.Collections.Generic;
using UnityEngine;

/*
 * Object Pooler script to create pools of objects that will be repeatedly spawned in Game
 * */
public class RG_TerrainPooler : MonoBehaviour
{
    // Declare pool of objecs to be defined in editor
    [System.Serializable]
    public class TerrainPool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [System.Serializable]
    public class TerrainThemes
    {
        public string theme;
        public int noOfBlocks;
    }

    public List<TerrainThemes> themes;
    public Dictionary<string, int> themeList = null;


    public static RG_TerrainPooler instance;
    public List<TerrainPool> pools;
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
    }

    private void Start()
    {
        BuildThemeTable();

        // Initialize list of object pools
        objectPools = new Dictionary<string, Queue<GameObject>>();
        foreach (TerrainPool pool in pools)
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

    public GameObject PickObjectFromPool(string tag)
    {
        GameObject objectToSpawn = objectPools[tag].Dequeue();
        objectToSpawn.SetActive(false);
        objectToSpawn.SetActive(true);
        objectPools[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    private void BuildThemeTable()
    {
        if (themeList != null)
        {
            return;
        }
        themeList = new Dictionary<string, int>();

        foreach(TerrainThemes terrainType in themes)
        {
            themeList[terrainType.theme] = terrainType.noOfBlocks;
        }
    }
}
