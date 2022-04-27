using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DemoScroller : MonoBehaviour
{
    public InfiniteWheel.InfiniteWheelController wheelController;
    public Scrollbar scroll;

    void Update()
    {
        scroll.value = 0.5f + (wheelController.offset / 20);
    }
}
