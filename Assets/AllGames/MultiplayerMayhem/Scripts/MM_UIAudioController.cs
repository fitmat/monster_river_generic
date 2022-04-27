using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_UIAudioController : MonoBehaviour
{

    public void PlaySwooshAudio()
    {
        MM_AudioManager.instance.PlayAudio("Swoosh");
    }
    public void PlayClickAudio()
    {
        MM_AudioManager.instance.PlayAudio("Click");
    }

}
