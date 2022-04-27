using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFacialExpressionSwapping : MonoBehaviour
{
    [SerializeField] private Material facialExpressionMaterial;
    [SerializeField] private Texture[] facialExpressionTextures;

    int i = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (i >= facialExpressionTextures.Length)
                i = 0;
            else
                i++;

            facialExpressionMaterial.mainTexture = facialExpressionTextures[i];

        }
    }

}
