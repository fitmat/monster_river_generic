using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// Script controls state of dart to decide if it has been thrown or sliced fruit

public class FS_DartController : MonoBehaviour
{
    public bool isThrown, hasSliced;
    public int dartOwner;
    [SerializeField] GameObject trail;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        isThrown = false;
        hasSliced = false;
        dartOwner = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        trail.SetActive(false);
        // Disable dart and bounce it down with physics on collision with plate
        if (collision.gameObject.tag=="FS_Plate")
        {
            hasSliced = true;
            DOTween.Restart("2");
            FS_AudioManager.instance.PlayAudio("KnifeHit");
            GetComponentInChildren<ParticleSystem>().Play();
        }
        StartCoroutine(DisableDart());

    }

    private void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().useGravity)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 50f, ForceMode.Force);
        }
    }

    private IEnumerator DisableDart()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
