using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacleSpawner : MonoBehaviour
{
    public GameObject[] movingObstacles;

    private void Awake()
    {
        OnDisable();
    }

    private void OnEnable()
    {
        for (int i = 0; i < movingObstacles.Length; i++)
        {
            movingObstacles[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < movingObstacles.Length; i++)
        {
            movingObstacles[i].SetActive(false);
        }
    }

}
