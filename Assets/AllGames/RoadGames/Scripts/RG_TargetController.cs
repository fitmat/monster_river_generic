using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_TargetController : MonoBehaviour
{
    public Transform p1Target, p2Target;
    public Transform target, leadingTarget;

    public float distanceBetweenTargets, p1TargetPosition, p2TargetPosition;
    public int leadingTargetNumber;
    public float maxTargetDistance;

    public bool hasPassedBlock;

    Bounds bounds;

    private void OnTriggerEnter(Collider other)
    {
        GameObject _collidedObject = other.gameObject;

        if (_collidedObject.CompareTag("RG_BlockEnd") && gameObject.CompareTag("RG_Target") && !hasPassedBlock)
        {
            StartCoroutine(PassBlock());
            hasPassedBlock = true;
        }
    }


    void Update()
    {
        p1TargetPosition = p1Target.position.z;
        p2TargetPosition = p2Target.position.z;

        bounds = new Bounds(p1Target.position, Vector3.zero);
        bounds.Encapsulate(p2Target.position);

        target.position = new Vector3(0, 2, bounds.center.z);

        if (p1TargetPosition > p2TargetPosition)
        {
            leadingTargetNumber = 1;

            leadingTarget.position = p1Target.position;

            distanceBetweenTargets = p1TargetPosition - p2TargetPosition;
        }
        else if (p1TargetPosition< p2TargetPosition)
        {
            leadingTargetNumber = 2;

            leadingTarget.position = p2Target.position;

            distanceBetweenTargets = p2TargetPosition - p1TargetPosition;
        }
        else
        {
            leadingTarget.position = target.position;
        }
    }



    public IEnumerator PassBlock()
    {
        StartCoroutine(RG_LevelGenerator.instance.SpawnNewBlock());
        StartCoroutine(RG_LevelGenerator.instance.DespawnOldBlock());
        yield return null;
        hasPassedBlock = false;
    }
}
