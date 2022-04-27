using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class Coin : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            animator.SetTrigger("Spawned");
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.GetCoin();
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                animator.SetTrigger("CoinCollected");
            }
        }
    }
}
