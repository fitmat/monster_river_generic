using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class PPSound
{
    public string name;
    public AudioClip audioClip;
    [HideInInspector] public AudioSource audioSource;

    public bool loop;
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 3f)]
    public float pitch;
}

public class PP_AudioManager : MonoBehaviour
{

    public static PP_AudioManager instance;
    public PPSound[] sounds;

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

        foreach (PPSound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void PlayAudio(string name)
    {
        PPSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Play();
    }
    public void StopAudio(string name)
    {
        PPSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Stop();
    }
    public void StopAllAudio()
    {
        foreach (PPSound sound in sounds)
        {
            sound.audioSource.Stop();
        }
    }
    public void SetTrackVolume(string name, float volume)
    {
        PPSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.volume = volume;
    }
    public void IncreaseTrackPitch(string name, float increment)
    {
        PPSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.pitch += increment;
    }
    public void PauseAudio(string name)
    {
        PPSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Pause();
    }


    public IEnumerator PlayJumpSound()
    {
        PP_AudioManager.instance.PlayAudio("JumpUp");
        yield return new WaitForSeconds(0.3f);
        PP_AudioManager.instance.PlayAudio("JumpDown");
    }
}
