using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handles plate spin and spawning

public class FS_PlateController : MonoBehaviour
{
    public static FS_PlateController instance;

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

    public int direction, speed, spinTime, readyFruits, maxReadyFruits, noOfBombs;
    public List<GameObject> fruitPositions, activePositions, inactivePositions;
    public GameObject[] fruitList;
    public GameObject bomb;

    private void Start()
    {
        inactivePositions = fruitPositions;
        StartCoroutine(ChangeSpin());
        StartCoroutine(ObjectSpawner());

        noOfBombs = 0;
    }

    private void Update()
    {
        if (FS_GameController.instance.gameState == FS_GameController.GameStates.playing)
        {
            transform.Rotate(Vector3.forward * direction * speed * Time.deltaTime);
        }
    }


    // Randomly change spin speed and direction of plate
    private IEnumerator ChangeSpin()
    {
        spinTime = Random.Range(2, 5);
        direction = Random.Range(0, 2);
        if (direction == 0)
        {
            direction = -1;
        }
        speed = Random.Range(12, 25);
        speed *= 5;
        yield return new WaitForSeconds(spinTime);
        StartCoroutine(ChangeSpin());
    }

    // Spawn objects on plate
    private IEnumerator ObjectSpawner()
    {
        GameObject newPosition;
        if (FS_GameController.instance.gameState == FS_GameController.GameStates.playing)
        {
            if (readyFruits < maxReadyFruits)
            {
                // Choose empty position to spawn at and set it as not empty
                newPosition = inactivePositions[Random.Range(0, inactivePositions.Count)];
                activePositions.Add(newPosition);
                inactivePositions.Remove(newPosition);
                SpawnObject(newPosition);
                readyFruits++;
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(ObjectSpawner());
    }

    // Spawn object at correct position
    private void SpawnObject(GameObject newPosition)
    {
        GameObject newObject;
        // Spawn bomb if no current bomb is present
        if (noOfBombs < 1 || (Random.Range(0, 2f) < 0.75f && noOfBombs < 2))
        {
            newObject = Instantiate(bomb, newPosition.transform);
            newObject.transform.parent = newPosition.transform;
            newObject.transform.localRotation = Quaternion.identity;
            noOfBombs++;
        }
        else
        {
            newObject = Instantiate(fruitList[Random.Range(0, fruitList.Length)], newPosition.transform);
            newObject.transform.parent = newPosition.transform;
            newObject.transform.localRotation = Quaternion.identity;
            //newObject = FS_ObjectPooler.instance.SpawnFromPool("Apple", newPosition.transform, Quaternion.identity);
            //newObject = FS_ObjectPooler.instance.SpawnFromPool(fruitList[Random.Range(0,fruitList.Length)], newPosition.transform, Quaternion.identity);
        }
    }
}
