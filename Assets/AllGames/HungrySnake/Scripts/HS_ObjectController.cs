using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_ObjectController : MonoBehaviour
{
    public bool isActive;

    private void OnEnable()
    {
        isActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(SelfDistruct());
    }

    public IEnumerator Collect()
    {
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public void CallCollect()
    {
        StartCoroutine(Collect());
    }

    public IEnumerator SelfDistruct()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);
            if (!isActive)
            {
                yield break;
            }
        }
        isActive = false;
        gameObject.SetActive(false);
    }
}
