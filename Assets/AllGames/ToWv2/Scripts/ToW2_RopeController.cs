using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW2_RopeController : MonoBehaviour
{

    float currentX, prevX;
    // Start is called before the first frame update
    void Start()
    {
        prevX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentX = transform.localPosition.x;
        if (currentX > prevX)
        {
            Debug.Log("Move Right");
        }
        else if (currentX < prevX)
        {
            Debug.Log("Move Left");
        }
        prevX = currentX;
    }
}
