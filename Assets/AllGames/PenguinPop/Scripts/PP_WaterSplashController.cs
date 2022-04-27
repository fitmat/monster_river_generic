using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_WaterSplashController : MonoBehaviour
{
    public GameObject waterSplash;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            PP_AudioManager.instance.PlayAudio("WaterSplash");
            waterSplash.SetActive(true);
        }
    }
}
