using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to start spawning tiles

public class PH_SpawnController : MonoBehaviour
{
    [SerializeField] GameObject[] lanes;

    public int numberOfLanes;
    private int randomChoice;

    public void Start()
    {
        // Determine direction of lanes
        randomChoice = Random.Range(0, 2);
        if (randomChoice == 0)
        {
            randomChoice = -1;
        }

        for (int i = 0; i < numberOfLanes; i++)
        {
            lanes[i].GetComponent<PH_LaneController>().laneDirection = randomChoice;
            randomChoice = -randomChoice;
        }
    }
}
