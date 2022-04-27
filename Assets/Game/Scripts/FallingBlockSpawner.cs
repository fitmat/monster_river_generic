using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class FallingBlockSpawner : MonoBehaviour
    {
        public GameObject[] fallingObstacles;

        private void Awake()
        {
            OnDisable();
        }

        private void OnEnable()
        {
            for (int i = 0; i < fallingObstacles.Length; i++)
            {
                fallingObstacles[i].SetActive(true);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < fallingObstacles.Length; i++)
            {
                fallingObstacles[i].SetActive(false);
            }
        }

    }
}
