using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script controls behavior and feedback of bomb

public class FS_BombController : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FS_Dart")
        {
            if (!collision.gameObject.GetComponent<FS_DartController>().hasSliced)
            {
                // Bursts bomb and provides feedback
                StartCoroutine(FS_GameController.instance.ChangeScore(collision.gameObject.GetComponent<FS_DartController>().dartOwner, -2));
                StartCoroutine(FS_GameController.instance.CameraShake());
                FS_AudioManager.instance.PlayAudio("Bomb");
                StartCoroutine(Explode());
                collision.gameObject.GetComponent<FS_DartController>().hasSliced = true;
            }
        }
    }

    private IEnumerator Explode()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        // Sets position of bomb as empty
        if (gameObject.transform.parent != null)
        {
            FS_PlateController.instance.inactivePositions.Add(gameObject.transform.parent.gameObject);
            FS_PlateController.instance.activePositions.Remove(gameObject.transform.parent.gameObject);
        }
        gameObject.transform.parent = null;
        // Reduces number of bombs and objects currently active
        yield return new WaitForSeconds(0.5f);
        FS_PlateController.instance.readyFruits--;
        FS_PlateController.instance.noOfBombs--;
        Destroy(gameObject);
    }
}
