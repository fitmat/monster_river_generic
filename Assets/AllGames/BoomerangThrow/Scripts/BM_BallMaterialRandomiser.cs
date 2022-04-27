using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_BallMaterialRandomiser : MonoBehaviour
{
    [SerializeField] private Material[] materialList;

    // Start is called before the first frame update
    void Start()
    {
        // Randonly choose a material from the list for ball
        gameObject.GetComponent<MeshRenderer>().material = materialList[Random.Range(0, materialList.Length)];
    }
}
