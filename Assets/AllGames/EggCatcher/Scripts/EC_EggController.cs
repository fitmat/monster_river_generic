using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_EggController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        // Reset position and animation state of egg
        transform.position = Vector2.zero;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetTrigger("reset");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Crack egg on collision with floor
        if (collision.gameObject.tag == "EC_Floor")
        {
            GetComponent<Animator>().ResetTrigger("reset");
            GetComponent<Animator>().SetTrigger("break");
            EC_AudioManager.instance.PlayAudio("EggCrack");
            StartCoroutine(Despawn());
        }
    }

    public IEnumerator Despawn()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}
