using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject[] enemies;

        private void Awake()
        {
            OnDisable();

        }

        private void OnEnable()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (GameObject go in enemies)
            {
                go.SetActive(false);
            }
        }

    }
}
