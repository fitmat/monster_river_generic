using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script moves belts and spawns chickens on new belts

public class EC_BeltController : MonoBehaviour
{
    public int speed;

    [SerializeField] private Transform startPoint;
    private Vector2 spawnPoint;
    private float startX, endX, currentX, distance;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = startPoint.position;
        startX = -23;
        endX = 87;
        currentX = startX + Random.Range(0, 5f);
        // Spawn chickens on parts of belt inside camera view
        while (currentX < endX)
        {
            spawnPoint = new Vector2(currentX, spawnPoint.y);
            EC_ObjectPooler.instance.SpawnFromPool("Chicken" + Random.Range(0, 2), spawnPoint, gameObject.transform, Quaternion.identity);
            currentX += Random.Range(6f, 14f);
        }
    }

    private IEnumerator SpawnChickens()
    {
        yield return new WaitForSecondsRealtime(1f);
        while (currentX < endX)
        {
            spawnPoint = new Vector2(currentX, spawnPoint.y);
            EC_ObjectPooler.instance.SpawnFromPool("Chicken" + Random.Range(0, 2), spawnPoint, gameObject.transform, Quaternion.identity);
            currentX += Random.Range(6f, 14f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EC_GameController.instance.gameState == EC_GameController.GameStates.playing)
        {
            transform.Translate(Vector2.right * -1 * speed * Time.deltaTime);
        }
    }
}
