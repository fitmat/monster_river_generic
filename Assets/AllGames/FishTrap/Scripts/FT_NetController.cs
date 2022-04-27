using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_NetController : MonoBehaviour
{
    [SerializeField] private GameObject collectParticles;
    public int playerNumber;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FT_Fish"))
        {
            Debug.Log("Caught Fish");
            FT_AudioManager.instance.PlayAudio("Catch");
            FT_AudioManager.instance.PlayAudio("Collect");
            GameObject fishObject = other.gameObject;
            Instantiate(collectParticles, fishObject.transform.position, Quaternion.identity);
            fishObject.transform.GetChild(0).gameObject.SetActive(false);
            FT_GameController.instance.ChangeScore(playerNumber, 1);
            Destroy(fishObject.GetComponent<Rigidbody>());
        }
    }
}
