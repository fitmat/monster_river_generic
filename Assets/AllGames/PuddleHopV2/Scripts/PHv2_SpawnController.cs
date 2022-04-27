using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHv2_SpawnController : MonoBehaviour
{
    [SerializeField] GameObject[] lanes;

    public int numberOfLanes;
    private int randomChoice;

    public void Start()
    {
        randomChoice = Random.Range(0, 2);
        if (randomChoice == 0)
        {
            randomChoice = -1;
        }

        for(int i = 0; i < numberOfLanes; i++)
        {
            lanes[i].GetComponent<PHv2_LaneController>().laneDirection = randomChoice;
            randomChoice = -randomChoice;
        }
    }
}
