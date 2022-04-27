using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// This script ensures infinite nauture of belt by cycling its parts

public class EC_BeltSpawner : MonoBehaviour
{
    public static EC_BeltSpawner instance;

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

    [SerializeField] private GameObject currentTerrain, backTerrain, frontTerrain;

    private float currentX, backX, frontX;
    [SerializeField] private int noOfTerrains;
    [SerializeField] private float terrainLength;
    private Vector3 currentPosition, backPosition, frontPosition;
    public int terrainsPassed;

    [SerializeField] private GameObject belt;


    // Start is called before the first frame update
    void Start()
    {
        // Determine initial positions

        terrainsPassed = 0;

        currentX = 0;
        currentPosition = new Vector3(currentX, 0, 0);
        currentTerrain = Instantiate(belt, currentPosition, Quaternion.identity);


        // Position of front belt depends on length of belt

        frontX = terrainLength;
        frontPosition = new Vector3(frontX, 0, 0);
        frontTerrain = Instantiate(belt, frontPosition, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cycle belt on reaching end limit
        if (collision.gameObject.tag == "EC_Belt")
        {
            Destroy(collision.gameObject, 1f);
            SpawnNewBelt();
        }
    }

    public void SpawnNewBelt()
    {
        backTerrain = currentTerrain;
        currentTerrain = frontTerrain;
        frontPosition = new Vector3(frontX, 0, 0);
        frontTerrain = Instantiate(belt, frontPosition, Quaternion.identity);
    }
}
