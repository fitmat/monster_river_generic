using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_FireballController : MonoBehaviour
{
    public bool hitPlayerOne, hitPlayerTwo;

    GameObject collisionObject;
    Rigidbody2D fireballBody;

    private void Awake()
    {
        fireballBody = GetComponent<Rigidbody2D>();    
    }

    private void OnEnable()
    {
        hitPlayerOne = false;
        hitPlayerTwo = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionObject = collision.gameObject;

        if (collisionObject.CompareTag("PlayerOne"))
        {
            if (!hitPlayerOne)
            {
                hitPlayerOne = true;
                DB_GameController.instance.PlayerJumpFail(1);
            }
        }
        else if (collisionObject.CompareTag("PlayerTwo"))
        {
            if (!hitPlayerTwo)
            {
                hitPlayerTwo = true;
                DB_GameController.instance.PlayerJumpFail(2);
            }
        }
        else if (collisionObject.CompareTag("TerrainEnd"))
        {
            if (!hitPlayerOne)
            {
                DB_GameController.instance.PlayerJumpSuccess(1);
            }
            if (!hitPlayerTwo)
            {
                DB_GameController.instance.PlayerJumpSuccess(2);
            }
        }

    }

    public void ThrowFireball(float fireballStartForce)
    {
        fireballBody.AddForce(Vector2.left * fireballStartForce, ForceMode2D.Impulse);
    }

    public IEnumerator DespawnFireball()
    {
        yield return new WaitForSeconds(1.5f);
        fireballBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }


}
