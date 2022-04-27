using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_ColliderController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PB_Ball"))
        {
            PB_AudioManager.instance.PlayAudio("Bounce");
            try
            {
                GetComponentInChildren<ParticleSystem>().Play();
            }
            catch
            {

            }
            try
            {
                GetComponentInChildren<Animator>().SetTrigger("Play");
            }
            catch
            {

            }
        }
    }
}
