using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpdownAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private bool bIsCoroutineInProgress;

    public void Update()
    {
        if(bIsCoroutineInProgress==false)
            StartCoroutine(AnimateScaleUpDown());
    }

    private void OnEnable()
    {
        bIsCoroutineInProgress = false;
        StartCoroutine(AnimateScaleUpDown());
    }

    IEnumerator AnimateScaleUpDown()
    {
        bIsCoroutineInProgress = true;

        //Animate the scale up down
        for (int i = 0; i < 4; i++)
        {
            gameObject.transform.localScale *= 1.03f;
            yield return new WaitForSecondsRealtime(.2f);
        }

        gameObject.transform.localScale = new Vector3(1, 1, 1) * 1.2f;

        for (int j = 0; j < 4; j++)
        {
            gameObject.transform.localScale *= 0.95f;
            yield return new WaitForSecondsRealtime(0.25f);
        }

        gameObject.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSecondsRealtime(0.25f);
        bIsCoroutineInProgress = false;
    }
}
