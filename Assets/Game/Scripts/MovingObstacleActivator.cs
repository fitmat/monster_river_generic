using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class MovingObstacleActivator : MonoBehaviour
    {

        [SerializeField] private GameObject[] movingObstacles;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                for (int i = 0; i < movingObstacles.Length; i++)
                {
                    movingObstacles[i].GetComponent<MovingObstacle>().move = true;
                }
            }
        }
    }
}