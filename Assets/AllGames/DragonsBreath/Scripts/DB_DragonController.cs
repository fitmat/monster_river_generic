using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_DragonController : MonoBehaviour
{
    public static DB_DragonController instance;

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

    [SerializeField] Transform lowFirePoint, highFirePoint;
    [SerializeField] GameObject lowDragon, highDragon;

    public float throwFireMinDelay, throwFireMaxDelay, throwFireMinForce, throwFireMaxForce;

    

    public IEnumerator StartDragons()
    {
        StartCoroutine(ThrowHighFire());
        yield return new WaitForSeconds(Random.Range(throwFireMinDelay, throwFireMaxDelay) / 3f);
        StartCoroutine(ThrowLowFire());
    }

    public IEnumerator ThrowLowFire()
    {

        if (DB_GameController.instance.gameState == DB_GameController.GameStates.playing)
        {
            GameObject _fireBall;
            DB_AudioManager.instance.PlayAudio("BreatheFire");
            lowDragon.GetComponent<Animator>().SetTrigger("Fire");
            yield return new WaitForSeconds(0.36f);
            _fireBall = ObjectPooler.instance.SpawnFromPool("Fireball", lowFirePoint.position, null, Quaternion.identity);

            _fireBall.GetComponent<DB_FireballController>().ThrowFireball(Random.Range(throwFireMinForce, throwFireMaxForce));
            yield return new WaitForSeconds(Random.Range(throwFireMinDelay, throwFireMaxDelay));
            StartCoroutine(ThrowLowFire());
        }
    }



    public IEnumerator ThrowHighFire()
    {
        yield return new WaitForSeconds(Random.Range(throwFireMinDelay, throwFireMaxDelay) * 3.5f);

        if (DB_GameController.instance.gameState == DB_GameController.GameStates.playing)
        {
            GameObject _fireBall;
            DB_AudioManager.instance.PlayAudio("BreatheFire");
            highDragon.GetComponent<Animator>().SetTrigger("Fire");
            yield return new WaitForSeconds(0.36f);
            _fireBall = ObjectPooler.instance.SpawnFromPool("Fireball", highFirePoint.position, null, Quaternion.identity);

            _fireBall.GetComponent<DB_FireballController>().ThrowFireball(Random.Range(throwFireMinForce, throwFireMaxForce));
            StartCoroutine(ThrowHighFire());
        }
    }


}
