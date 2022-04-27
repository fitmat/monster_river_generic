using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacaleOutApprearance : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float speed = 0.5f;

    private void OnEnable()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(ScaleUpAnimation());
    }

    IEnumerator ScaleUpAnimation()
    {
        //Keep Scaling up
        for (int i = 0; i < 7; i++)
        {
            if (gameObject.transform.localScale == new Vector3(1, 1, 1) * 0f)
                gameObject.transform.localScale = new Vector3(1, 1, 1) * .05f;
            else
            {
                gameObject.transform.localScale *= 1.5f;
                yield return new WaitForSecondsRealtime(.1f);
            }
        }
        //yield return new WaitForSecondsRealtime(3f);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
