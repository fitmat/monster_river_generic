using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TR_RangedMonsterController : MonoBehaviour
{
    private enum State { floating, idle, attacking, dead, waiting };
    [SerializeField]
    private State currentState;

    [SerializeField] private float speed;

    [SerializeField] private SkinnedMeshRenderer material;
    [SerializeField] private Material[] skins;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fireball;
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

        selectedTarget = targetMarkers[UnityEngine.Random.Range(0, targetMarkers.Length)].gameObject;

        int skinChoice = UnityEngine.Random.Range(0, skins.Length);
        Material[] mats = material.materials;
        mats[0] = skins[skinChoice];
        material.materials = mats;

        TR_UIController.instance.activeEnemies.Add(gameObject);

        animator.SetTrigger("Move");
        currentState = State.floating;
        StartCoroutine(FloatToTarget());
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
        Debug.Log($"Touched {other.name}");

        if (other.gameObject.tag == "RangeRadius" && currentState == State.floating)
        {
            currentState = State.attacking;
            animator.SetTrigger("Idle");
            StartCoroutine(FloatAndAttack());
        }
        else if ((other.gameObject.tag == "MeeleMonster" || other.gameObject.tag == "RangedMonster") && currentState == State.floating)
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(gameObject.transform.position.x - 5f, gameObject.transform.position.y, gameObject.transform.position.z), 5);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RangeRadius" && currentState == State.attacking)
        {
            currentState = State.floating;
            StartCoroutine(FloatToTarget());
        }
        if (other.gameObject.tag == "EnemyDespawn")
        {
            Die();
        }
    }


    private IEnumerator WaitToMove()
    {
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(gameObject.transform.position.x - 5f, gameObject.transform.position.y, gameObject.transform.position.z), 5, enemyLayer);
        if (hitColliders.Length == 0)
        {
            Debug.Log("Not Colliding");
            currentState = State.floating;
            StartCoroutine(FloatToTarget());
        }
        else
        {
            currentState = State.waiting;
            Debug.Log("Colliding");
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(WaitToMove());
        }
    }


    private IEnumerator FloatAndAttack()
    {
        if (currentState == State.attacking)
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(1f);
            ThrowFire();
            yield return new WaitForSeconds(2f);
            StartCoroutine(FloatAndAttack());
        }
    }

    private IEnumerator FloatToTarget()
    {
        if (currentState == State.floating)
        {
            transform.LookAt(selectedTarget.transform);
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            yield return new WaitForFixedUpdate();
            StopCoroutine(FloatToTarget());
            StartCoroutine(FloatToTarget());
        }
    }

    private void ThrowFire()
    {
        fireball.SetActive(true);
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
        animator.SetTrigger("Die");
        currentState = State.dead;
        TR_UIController.instance.activeEnemies.Remove(gameObject);
        StartCoroutine(DelayDespawn());
    }

    private IEnumerator DelayDespawn()
    {
        fireball.SetActive(false);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
