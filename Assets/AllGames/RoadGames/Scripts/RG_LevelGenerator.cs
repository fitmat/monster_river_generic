using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RG_LevelGenerator : MonoBehaviour
{
    public static RG_LevelGenerator instance;
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
    public Vector3 startPosition, endPosition, currentPosition, nextSpawnPoint;
    public int startBlocks;
    public int newBlockLength, totalRoadLength;
    public enum Themes { cityTheme, desertTheme };//, forestTheme };
    public Themes currentTheme, nextTheme, previousTheme;
    public int totalNumberOfBlocksSpawned, numberOfBlocksDespawned, numberOfBlocksActive, blocksAheadOfPlayer, blocksBehindPlayer;
    public int minNoOfThemeBlocks, maxNoOfThemeBlocks, noOfThemeBlocksSpawned, totalNoOfThemeBlocks;
    public string currentBlockTheme;
    public int noOfThemeBlockTypes, blockNumberToSpawn;
    [SerializeField] private GameObject TerrainPool;
    [SerializeField] private GameObject newBlock, currentBlock, oldBlock;
    [SerializeField] private Queue<GameObject> activeBlocks = new Queue<GameObject>();
    private RG_BlockData blockData;
    private void Start()
    {
        StartCoroutine(InitializeMap());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetNewTheme();
        }

    }
    private void SetNewTheme()
    {
        totalNoOfThemeBlocks = UnityEngine.Random.Range(minNoOfThemeBlocks, maxNoOfThemeBlocks);
        previousTheme = currentTheme;
        while (currentTheme == previousTheme)
        {
            Array values = Enum.GetValues(typeof(Themes));
            currentTheme = (Themes)values.GetValue(UnityEngine.Random.Range(0,values.Length));
        }
        noOfThemeBlocksSpawned = 0;
    }
    public IEnumerator SpawnNewBlock()
    {
        if (noOfThemeBlocksSpawned == totalNoOfThemeBlocks)
        {
            SetNewTheme();
            if (currentTheme == Themes.cityTheme && previousTheme == Themes.desertTheme)
            {
                currentBlockTheme = "DtoC";
            }
            else if (currentTheme == Themes.desertTheme && previousTheme == Themes.cityTheme)
            {
                currentBlockTheme = "CtoD";
            }
        }
        else
        {
            switch (currentTheme)
            {
                case Themes.cityTheme:
                    currentBlockTheme = "City";
                    break;
                case Themes.desertTheme:
                    currentBlockTheme = "Desert";
                    break;
            }
        }
            noOfThemeBlockTypes = RG_TerrainPooler.instance.themeList[currentBlockTheme];
            blockNumberToSpawn = UnityEngine.Random.Range(0, noOfThemeBlockTypes);
            do
            {
                newBlock = RG_TerrainPooler.instance.PickObjectFromPool(currentBlockTheme + blockNumberToSpawn);
                blockData = newBlock.GetComponent<RG_BlockData>();
                blockNumberToSpawn++;
                if (blockNumberToSpawn == noOfThemeBlockTypes)
                {
                    blockNumberToSpawn = 0;
                }
            }
            while (blockData.isActive);
        if (blockData.blockType == currentBlockTheme)
        {
            newBlockLength = blockData.blockLength;
            newBlock.transform.position = nextSpawnPoint;
            newBlock.transform.parent = gameObject.transform;
            newBlock.SetActive(true);
            blockData.isActive = true;
            totalRoadLength += newBlockLength;
            endPosition = new Vector3(0, 0, startPosition.z + totalRoadLength);
            numberOfBlocksActive++;
            blocksAheadOfPlayer++;
            noOfThemeBlocksSpawned++;
            activeBlocks.Enqueue(newBlock);
            nextSpawnPoint = blockData.endPoint.position;
        }
        else
        {
            // Handle Incorrect Block Spawned
        }
        yield return null;
    }
    public IEnumerator DespawnOldBlock()
    {
        yield return null;
        oldBlock = activeBlocks.Dequeue();
        blockData = oldBlock.GetComponent<RG_BlockData>();
        blockData.isActive = false;
        oldBlock.SetActive(false);
        oldBlock.transform.parent = TerrainPool.transform;
        blocksBehindPlayer--;
        numberOfBlocksActive--;
        numberOfBlocksDespawned++;
    }
    public IEnumerator SpawnNewBlockBehind()
    {
        switch (currentTheme)
        {
            case Themes.cityTheme:
                currentBlockTheme = "City";
                noOfThemeBlockTypes = RG_TerrainPooler.instance.themeList[currentBlockTheme];
                break;
        }
        blockNumberToSpawn = UnityEngine.Random.Range(0, noOfThemeBlockTypes);
        do
        {
            newBlock = RG_TerrainPooler.instance.PickObjectFromPool(currentBlockTheme + blockNumberToSpawn);
            blockData = newBlock.GetComponent<RG_BlockData>();
            blockNumberToSpawn++;
            if (blockNumberToSpawn == noOfThemeBlockTypes)
            {
                blockNumberToSpawn = 0;
            }
        }
        while (blockData.isActive);
        if (blockData.blockType == currentBlockTheme)
        {
            newBlockLength = blockData.blockLength;
            startPosition = new Vector3(0, 0, startPosition.z - newBlockLength);
            newBlock.transform.position = startPosition;
            newBlock.transform.parent = gameObject.transform;
            newBlock.SetActive(true);
            blockData.isActive = true;
            totalNoOfThemeBlocks++;
            numberOfBlocksActive++;
            blocksBehindPlayer++;
            blockData.isActive = true;
            activeBlocks.Enqueue(newBlock);
        }
        else
        {
            // Handle Incorrect Block Spawned
        }
        yield return null;
    }
    public IEnumerator InitializeMap()
    {
        yield return null;
        endPosition = startPosition;
        nextSpawnPoint = endPosition;
            SetNewTheme();
        for (int i = 0; i < startBlocks; i++)
        {
            try
            {
                StartCoroutine(SpawnNewBlock());
            }
            catch (Exception e)
            {
                Debug.Log("Exception in Spawning Block" + e);
            }
        }
    }
}