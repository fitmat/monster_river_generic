using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_SplashController : MonoBehaviour
{
    public GameObject waterSplash;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            waterSplash.SetActive(true);
        }
    }
}
