using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class EnemyActivator : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemies;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<Enemies>().activated = true;
                }
            }
        }
    }
}
