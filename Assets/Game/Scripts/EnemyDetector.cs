using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class EnemyDetector : MonoBehaviour
    {

        public bool attackMode = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemies>().enemyBehaviour == Enemies.Behaviour.attacker && !other.gameObject.GetComponent<Enemies>().isDead)
            {
                attackMode = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemies>().enemyBehaviour == Enemies.Behaviour.attacker && !other.gameObject.GetComponent<Enemies>().isDead)
            {
                attackMode = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            attackMode = false;
        }
    }
    
}