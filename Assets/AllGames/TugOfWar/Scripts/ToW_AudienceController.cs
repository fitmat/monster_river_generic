using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW_AudienceController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartCheering());
    }

    private IEnumerator StartCheering()
    {
        yield return new WaitForSeconds(Random.Range(0, 1.5f));
        GetComponent<Animator>().SetInteger("CheerType", Random.Range(1, 5));
    }
}
