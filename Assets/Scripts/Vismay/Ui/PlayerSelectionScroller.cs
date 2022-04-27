using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionScroller : MonoBehaviour
{
    // playerselection settings
    [Header("Playerselection Settings")]
    public GameObject playerSelectionScrollBar = null;
    float playerSelectionScrollPos = 0f;
    float[] allPos;

    Scrollbar scrollbar = null;

    void Start() {
        scrollbar = playerSelectionScrollBar.GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSelectionSwipe();
    }

    public void PlayerSelectionSwipe() {
        allPos = new float[transform.childCount];
        
        float distance = 1f / (allPos.Length - 1f);

        for (int i = 0; i < allPos.Length; i++) {
            allPos[i] = distance * i;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            playerSelectionScrollPos = scrollbar.value;
        } else {
            for (int i = 0; i < allPos.Length; i++) {
                if ((playerSelectionScrollPos < allPos[i] + (distance/2)) && (playerSelectionScrollPos > allPos[i] - (distance/2))) {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, allPos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < allPos.Length; i++) {
            if ((playerSelectionScrollPos < allPos[i] + (distance/2)) && (playerSelectionScrollPos > allPos[i] - (distance/2))) {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);

                for (int a = 0; i < allPos.Length; a++) {
                    if (a != i) {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.5f, 0.5f), 0.1f);
                    }
                }
            }
        }
    }
}
