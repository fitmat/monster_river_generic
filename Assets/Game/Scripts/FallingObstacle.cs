 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{

    [SerializeField] private GameObject[] fallingObstacles;

    private void OnEnable()
    {
        for (int i = 0; i < fallingObstacles.Length; i++)
        {
            fallingObstacles[i].transform.position = new Vector3(fallingObstacles[i].transform.position.x,
                3f, fallingObstacles[i].transform.position.z);
            fallingObstacles[i].GetComponent<Rigidbody>().isKinematic = true;
            fallingObstacles[i].GetComponent<MeshRenderer>().enabled = false;
            fallingObstacles[i].gameObject.transform.Find("SM_Prop_Barrier_Pole_01 (1)").gameObject.GetComponent<MeshRenderer>().enabled = false;
            fallingObstacles[i].gameObject.transform.Find("SM_Prop_Barrier_Pole_01 (2)").gameObject.GetComponent<MeshRenderer>().enabled = false;
            fallingObstacles[i].gameObject.transform.Find("Sparking").gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            for (int i = 0; i < fallingObstacles.Length; i++)
            { 
                fallingObstacles[i].GetComponent<Rigidbody>().isKinematic = false;
                fallingObstacles[i].GetComponent<MeshRenderer>().enabled = true;
                fallingObstacles[i].gameObject.transform.Find("SM_Prop_Barrier_Pole_01 (1)").gameObject.GetComponent<MeshRenderer>().enabled = true;
                fallingObstacles[i].gameObject.transform.Find("SM_Prop_Barrier_Pole_01 (2)").gameObject.GetComponent<MeshRenderer>().enabled = true;
                fallingObstacles[i].gameObject.transform.Find("Sparking").gameObject.SetActive(true);
            }
        }
    }
}
