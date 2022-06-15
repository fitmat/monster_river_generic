using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object in Trigger");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Object in Collision");
    }

}
