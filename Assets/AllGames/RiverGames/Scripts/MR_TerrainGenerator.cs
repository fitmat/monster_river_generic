using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MR_TerrainGenerator : MonoBehaviour
{
    public static MR_TerrainGenerator instance;

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

    private float randomInt, currentX, backX, frontX;
    [SerializeField] private int noOfTerrains;
    public float terrainLength;
    private Vector3 currentPosition, backPosition, frontPosition;
    public int terrainsPassed;


    // Start is called before the first frame update
    void Start()
    {
        terrainsPassed = 0;

        currentX = 0;
        currentPosition = new Vector3(currentX, 0, 0);
        randomInt = Random.Range(0, noOfTerrains);
        currentTerrain = MR_TerrainPooler.instance.SpawnFromPool("Terrain" + randomInt.ToString(), currentPosition, Quaternion.identity);

        backX = -terrainLength;
        backPosition = new Vector3(backX, 0, 0);
        randomInt = Random.Range(0, noOfTerrains);
        backTerrain = MR_TerrainPooler.instance.SpawnFromPool("Terrain" + randomInt.ToString(), backPosition, Quaternion.identity);

        frontX = terrainLength;
        frontPosition = new Vector3(frontX, 0, 0);
        randomInt = Random.Range(0, noOfTerrains);
        frontTerrain = MR_TerrainPooler.instance.SpawnFromPool("Terrain" + randomInt.ToString(), frontPosition, Quaternion.identity);
    }

    public void SpawnNewTerrain()
    {
        backX = currentX;
        currentX = frontX;
        frontX += terrainLength;

        terrainsPassed++;

        if (terrainsPassed == MR_GameController.instance.gameLength)
        {
            backTerrain.SetActive(false);
            backTerrain = currentTerrain;
            currentTerrain = frontTerrain;
            frontPosition = new Vector3(frontX, 0, 0);
            frontTerrain = MR_TerrainPooler.instance.SpawnFromPool("Terrain" + noOfTerrains.ToString(), frontPosition, Quaternion.identity);
            MR_GameController.instance.isRiverOver = true;
            Debug.Log("River crossed- Length= " + terrainLength + " * " + terrainsPassed + " = " + MR_GameController.instance.gameplayObject.transform.position.x);
        }
        else if (terrainsPassed < MR_GameController.instance.gameLength)
        {
            backTerrain.SetActive(false);
            backTerrain = currentTerrain;
            currentTerrain = frontTerrain;
            frontPosition = new Vector3(frontX, 0, 0);
            randomInt = Random.Range(0, noOfTerrains);
            frontTerrain = MR_TerrainPooler.instance.SpawnFromPool("Terrain" + randomInt.ToString(), frontPosition, Quaternion.identity);
        }
    }
}
