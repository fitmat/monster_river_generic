using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_PlayerBodyController : MonoBehaviour
{
    [SerializeField] private GameObject playerHead;

    // When two players collide enable collider to let eggs crack on their heads
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EC_Body")
        {
            playerHead.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    // Disable collider when players move apart
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "EC_Body")
        {
            playerHead.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
