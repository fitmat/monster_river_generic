using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class Enemies : MonoBehaviour
    {
        public enum Behaviour
        {
            attacker = 1,
            shielded = 2
        }

        public Behaviour enemyBehaviour;
        [SerializeField] private GameObject bulletInstance;
        [SerializeField] private Transform bulletInstanceSpawnPoint;
        [SerializeField] private float bulletSpeed = 15f;
        [SerializeField] private Animator animator;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private GameObject shieldEffect;
        [SerializeField] private GameObject killEffect;

        private float lastFiredAt = 0f;
        public bool isDead = false;
        public bool activated = false;
        private Vector3 pos;

        private bool deadOnce = false;

        private void Awake()
        {
            pos = this.transform.position;
        }

        private void Update()
        {
            if (!activated)
                return;

            EnemyAction();

        }

        private void EnemyAction()
        {
            switch (enemyBehaviour)
            {
                case Behaviour.attacker:
                    AttackPlayer();
                    break;
                case Behaviour.shielded:
                    ActivateShield();
                    break;
            }
        }

        private void AttackPlayer()
        {
            if (!isDead)
            {
                if (Time.time > fireRate + lastFiredAt)
                {
                    animator.SetTrigger("attack");
                    Instantiate(bulletInstance, bulletInstanceSpawnPoint.position, Quaternion.identity);
                    lastFiredAt = Time.time;
                }
            }
        }

        private void ActivateShield()
        {
            if (!isDead)
            {
                shieldEffect.SetActive(true);
                animator.SetTrigger("shield");
            }
        }

        private void OnEnable()
        {
            isDead = false;
            activated = false;
            killEffect.SetActive(false);
            animator.SetTrigger("spawned");
            this.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
            if(deadOnce)
                this.transform.Translate(Vector3.forward * 10f);
        }

        public void Death() 
        {
            isDead = true;
            killEffect.SetActive(true);
            shieldEffect.SetActive(false);
            animator.SetTrigger("dead");
            this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            this.transform.Translate(Vector3.forward * -10f);
            deadOnce = true;
        }

    }
}
