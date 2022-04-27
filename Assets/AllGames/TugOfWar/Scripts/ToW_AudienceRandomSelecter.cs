using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW_AudienceRandomSelecter : MonoBehaviour
{
    [SerializeField] private GameObject[] audienceMember;

    private void Start()
    {
        Instantiate(audienceMember[Random.Range(0, 6)], gameObject.transform);
    }
}
