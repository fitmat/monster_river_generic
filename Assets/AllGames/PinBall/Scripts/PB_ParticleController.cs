using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_ParticleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeEffect());
    }

    private IEnumerator ChangeEffect()
    {
        var shape = GetComponent<ParticleSystem>().shape;
        shape.arcSpread = 0.5f;
        yield return new WaitForSeconds(2f);
        shape.arcSpread = (float)Random.Range(0, 100) / 100;
        yield return new WaitForSeconds(10f);
        StartCoroutine(ChangeEffect());
    }
}
