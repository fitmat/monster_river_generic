using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_MeeleMonsterController : MonoBehaviour
{
    private enum State { swimming, attacking, dead, waiting };
    private State currentState;

    [SerializeField] private float speed;

    [SerializeField] private SkinnedMeshRenderer material;
    [SerializeField] private Material[] skins;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject lightening;
    public LayerMask enemyLayer;

    [SerializeField] private MR_TargetMarker[] targetMarkers;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject selectedTarget;


    private void Awake()
    {
        targetMarkers = FindObjectsOfType<MR_TargetMarker>();
    }


    private void OnEnable()
    {
        //MR_AudioManager.instance.PlayAudio("MonsterGrowl1");

        int skinChoice = UnityEngine.Random.Range(0, skins.Length);
        Material[] mats = material.materials;
        mats[0] = skins[skinChoice];
        material.materials = mats;

        TR_UIController.instance.activeEnemies.Add(this.gameObject);

        selectedTarget = targetMarkers[UnityEngine.Random.Range(0, targetMarkers.Length)].gameObject;

        currentState = State.swimming;
        StartCoroutine(SwimToRaft());
    }
    private void OnDisable()
    {
        RemoveFromLists();
    }

    public void RemoveFromLists()
    {
        if (TR_LeftPlayerController.instance.backLeftEnemies.Contains(gameObject))
        {
            TR_LeftPlayerController.instance.backLeftEnemies.Remove(gameObject);
        }
        if (TR_LeftPlayerController.instance.backRightEnemies.Contains(gameObject))
        {
            TR_LeftPlayerController.instance.backRightEnemies.Remove(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MeeleRadius" && currentState == State.swimming)
        {
            currentState = State.attacking;
            StartCoroutine(AttackRaft());
        }
        else if ((other.gameObject.tag == "MeeleMonster" || other.gameObject.tag == "RangedMonster") && currentState == State.swimming)
        {
            StartCoroutine(WaitToMove());
        }
        else if (other.gameObject.tag == "BackLeftRange")
        {
            RemoveFromLists();
            TR_LeftPlayerController.instance.backLeftEnemies.Add(gameObject);
        }
        else if (other.gameObject.tag == "BackRightRange")
        {
            RemoveFromLists();
            TR_LeftPlayerController.instance.backRightEnemies.Add(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MeeleRadius" && currentState == State.attacking)
        {
            StartCoroutine(SwimToRaft());
        }
        if (other.gameObject.tag == "EnemyDespawn")
        {
            Die();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(gameObject.transform.position.x + 10f, gameObject.transform.position.y, gameObject.transform.position.z), 5);
    }

    private IEnumerator WaitToMove()
    {
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(gameObject.transform.position.x + 10f, gameObject.transform.position.y, gameObject.transform.position.z), 5, enemyLayer);
        if (hitColliders.Length == 0)
        {
            Debug.Log("Not Colliding");
            currentState = State.swimming;
            StartCoroutine(SwimToRaft());
        }
        else
        {
            currentState = State.waiting;
            Debug.Log("Colliding");
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(WaitToMove());
        }
    }

    private IEnumerator AttackRaft()
    {
        if (currentState == State.attacking)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(1f);
            if (currentState == State.attacking)
            {
                Attack();
            }
            yield return new WaitForSeconds(2.5f);
            StartCoroutine(AttackRaft());
        }
    }

    private IEnumerator SwimToRaft()
    {
        if (currentState == State.swimming)
        {
            transform.LookAt(selectedTarget.transform);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            yield return null;
            StartCoroutine(SwimToRaft());
        }
    }

    private void Attack()
    {
        TR_RaftController.instance.DamageRaft(selectedTarget);
    }
    private IEnumerator MagicDeath()
    {
        lightening.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Die");
        currentState = State.dead;
        StartCoroutine(DelayDespawn());
    }

    public void DieOnGameEnd()
    {
        StartCoroutine(MagicDeath());
    }


    public void Die()
    {
        MR_AudioManager.instance.PlayAudio("MonsterDie");
        animator.SetTrigger("Die");
        currentState = State.dead;
        TR_UIController.instance.activeEnemies.Remove(gameObject);
        StartCoroutine(DelayDespawn());
    }



    private IEnumerator DelayDespawn()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
