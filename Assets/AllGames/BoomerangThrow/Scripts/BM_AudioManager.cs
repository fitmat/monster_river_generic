using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class BMSound
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

public class BM_AudioManager : MonoBehaviour
{

    public static BM_AudioManager instance;
    public BMSound[] sounds;

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

        foreach (BMSound sound in sounds)
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
        if (MM_GameUIManager.instance.isPlayingGame)
        {
            BMSound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                return;
            }
            s.audioSource.Play();
        }
    }
    public void StopAudio(string name)
    {
        BMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Stop();
    }
    public void StopAllAudio()
    {
        foreach (BMSound sound in sounds)
        {
            sound.audioSource.Stop();
        }
    }
    public void SetTrackVolume(string name, float volume)
    {
        BMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.volume = volume;
    }
    public void IncreaseTrackPitch(string name, float increment)
    {
        BMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.pitch += increment;
    }
    public void PauseAudio(string name)
    {
        BMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Pause();
    }


}
