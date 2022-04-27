using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW2_RopeCinematic : MonoBehaviour
{

    public float leftLiftTime, rightLiftTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RopeCinematic());
    }

    public IEnumerator RopeCinematic()
    {
        yield return new WaitForSeconds(leftLiftTime);
        GetComponent<Animator>().SetTrigger("LiftLeft");
        yield return new WaitForSeconds(rightLiftTime);
        GetComponent<Animator>().SetTrigger("LiftRight");
    }
}
