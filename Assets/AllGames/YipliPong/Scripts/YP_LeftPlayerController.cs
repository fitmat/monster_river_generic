using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YP_LeftPlayerController : MonoBehaviour
{
    public static YP_LeftPlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public int moveDistance;
    public float steps;

    private Vector2 startPosition, endPosition, currentPosition;

    [SerializeField] Animator glow;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("YP_Ball"))
        {
            GetComponentInChildren<ParticleSystem>().Play();
            glow.SetTrigger("Glow");
        }
    }

    public IEnumerator MoveDown()
    {
        startPosition = transform.position;
        endPosition = new Vector2(transform.position.x, transform.position.y - moveDistance);
        currentPosition = startPosition;
        if (endPosition.y <= -6.5f)
        {
            endPosition.y = -6.5f;
        }

        while (currentPosition.y > endPosition.y)
        {
            currentPosition = new Vector2(transform.position.x, currentPosition.y - moveDistance / steps);
            transform.position = currentPosition;
            yield return null;
        }

    }
    public IEnumerator MoveUp()
    {
        startPosition = transform.position;
        endPosition = new Vector2(transform.position.x, transform.position.y + moveDistance);
        currentPosition = startPosition;
        if (endPosition.y >= 6.5f)
        {
            endPosition.y = 6.5f;
        }

        while (currentPosition.y < endPosition.y)
        {
            currentPosition = new Vector2(transform.position.x, currentPosition.y + moveDistance / steps);
            transform.position = currentPosition;
            yield return null;
        }
    }

}
