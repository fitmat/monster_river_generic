using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateToPoint : MonoBehaviour
{

    [SerializeField] private Transform pointToTranslate;
    [SerializeField] private float speed;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointToTranslate.position, speed * Time.deltaTime);
    }
}
