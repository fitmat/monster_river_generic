using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class CoinSpawner : MonoBehaviour
    {
        public int maxCoin = 5;
        public float probabiltyOfSpawning = 1f;
        public bool forceSpawnAll = false;

        private GameObject[] coins;

        private void Awake()
        {
            coins = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                coins[i] = transform.GetChild(i).gameObject;
            }

            OnDisable();

        }

        private void OnEnable()
        {
            if (Random.Range(0f, 1f) > probabiltyOfSpawning)
                return;

            if (forceSpawnAll)
            {
                for (int i = 0; i < maxCoin; i++)
                {
                    coins[i].SetActive(true);
                }
            }
            else
            {
                int r = Random.Range(0, maxCoin);
                for (int i = 0; i < r; i++)
                {
                    coins[i].SetActive(true);
                }
            }

        }

        private void OnDisable()
        {
            foreach (GameObject gameObject in coins)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
