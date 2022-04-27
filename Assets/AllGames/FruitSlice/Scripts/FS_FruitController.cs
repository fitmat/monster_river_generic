using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handles slicing of fruit on collision with dart

public class FS_FruitController : MonoBehaviour
{

    [SerializeField] GameObject fruitBlastVFX;
    [SerializeField] GameObject blastPhysicsEffect;

    private void OnTriggerEnter(Collider collision)
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FS_Dart")
        {
            if (!collision.gameObject.GetComponent<FS_DartController>().hasSliced)
            {
                // Disable collider
                gameObject.GetComponent<Collider>().isTrigger = true;

                StartCoroutine(FS_GameController.instance.ChangeScore(collision.gameObject.GetComponent<FS_DartController>().dartOwner, 1));

                // Set fruit position as empty
                if (gameObject.transform.parent != null)
                {
                    FS_PlateController.instance.inactivePositions.Add(gameObject.transform.parent.gameObject);
                    FS_PlateController.instance.activePositions.Remove(gameObject.transform.parent.gameObject);
                }
                gameObject.transform.parent = null;

                // Set state of dart as has sliced fruit
                collision.gameObject.GetComponent<FS_DartController>().hasSliced = true;

                // Set color of particle system based on player
                var main = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
                if (collision.gameObject.GetComponent<FS_DartController>().dartOwner == 1)
                {
                    main.startColor = Color.red;
                }
                else if (collision.gameObject.GetComponent<FS_DartController>().dartOwner == 2)
                {
                    main.startColor = Color.green;
                }

                // Provide feedback of slicing fruit
                FS_AudioManager.instance.PlayAudio("FruitSplash");
                //transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                //transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
                //transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
                fruitBlastVFX.SetActive(true);
                fruitBlastVFX.GetComponent<ParticleSystem>().Play();
                // Drop slices of fruit with physics
                for (int i = 0; i < 2; i++)
                {
                    blastPhysicsEffect.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                }
                StartCoroutine(DisableFruit());
            }
        }
    }
    private IEnumerator DisableFruit()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        FS_PlateController.instance.readyFruits--;
    }
}
