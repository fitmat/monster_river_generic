using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MR_FireAttackController : MonoBehaviour
{
    private bool hasReached;
    public float speed;


    private MR_TargetMarker[] targetMarkers;
    private GameObject selectedTarget;

    private void Awake()
    {
        targetMarkers = FindObjectsOfType<MR_TargetMarker>();
    }

    private void OnEnable()
    {
        MR_AudioManager.instance.PlayAudio("Fireball");
        gameObject.transform.localPosition = Vector3.zero;

        selectedTarget = targetMarkers[UnityEngine.Random.Range(0, targetMarkers.Length)].gameObject;

        gameObject.transform.LookAt(selectedTarget.transform);
        hasReached = false;
        StartCoroutine(MoveFire());
    }
    private void OnDisable()
    {
        hasReached = true;
        gameObject.transform.localPosition = Vector3.zero;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Raft")
        {
            MR_RaftController.instance.DamageRaft(selectedTarget);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator MoveFire()
    {
        if (!hasReached)
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        yield return null;
            StartCoroutine(MoveFire());
        }
    }
}
