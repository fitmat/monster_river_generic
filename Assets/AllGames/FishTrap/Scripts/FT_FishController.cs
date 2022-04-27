using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_FishController : MonoBehaviour
{
    public float lifetime;

    // Start is called before the first frame update
    void OnEnable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
        GetComponent<Animator>().SetTrigger("Jump");
        StartCoroutine(DisableFish());
    }

    public IEnumerator DisableFish()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}
