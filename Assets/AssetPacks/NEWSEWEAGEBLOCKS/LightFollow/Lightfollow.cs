using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightfollow : MonoBehaviour
{
    public GameObject theplayer;

     void Update()
    {
        transform.LookAt(theplayer.transform);
        
    }
}
