using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_PaddleController : MonoBehaviour
{
    public int forceAmount;

    public bool isStriking;

    [SerializeField] private Collider2D collider;

    private void Start()
    {
        isStriking = false;
    }

    public IEnumerator ActivatePaddle()
    {
        if (!isStriking)
        {
            isStriking = true;
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            yield return new WaitForSeconds(0.25f);
            isStriking = false;
        }
    }

    public void HitBall(Rigidbody2D ballBody, Vector2 ballPosition, Vector2 contactPoint)
    {
        if (isStriking)
        {
            PB_AudioManager.instance.PlayAudio("Hit");
            ballBody.AddForce((ballPosition - contactPoint) * forceAmount, ForceMode2D.Impulse);
            //ballBody.AddForce(Vector2.up * - (forceAmount / 4), ForceMode2D.Impulse);
            //StartCoroutine(DisableCollider());
            isStriking = false;
        }
        else
        {
            //ballBody.AddForce((ballPosition - contactPoint + Vector2.up) * forceAmount / 5, ForceMode2D.Impulse);
            //StartCoroutine(DisableCollider());
        }
    }
    public IEnumerator DisableCollider()
    {
        collider.enabled = false;
        yield return new WaitForSecondsRealtime(0.4f);
        collider.enabled = true;
    }



}
