using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_ObjectRotator : MonoBehaviour
{
    public int direction;
    public float speed,spinTime;

    private void Start()
    {
        StartCoroutine(ChangeSpin());
    }


    private void Update()
    {
        transform.Rotate(Vector3.forward * direction * speed * Time.deltaTime);
    }

    private IEnumerator ChangeSpin()
    {
        spinTime = Random.Range(3f, 7f);
        direction = Random.Range(0, 2);
        if (direction == 0)
        {
            direction = -1;
        }
        speed = Random.Range(8f, 11f) * 25;

        yield return new WaitForSeconds(spinTime);
        StartCoroutine(ChangeSpin());
    }
}
