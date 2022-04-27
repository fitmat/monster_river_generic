using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class MR_Sound
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

public class MR_AudioManager : MonoBehaviour
{

    public static MR_AudioManager instance;
    public MR_Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);    //Independent build code
        }
        DontDestroyOnLoad(gameObject);    //Independent build code

        foreach (MR_Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    private void Start()
    {
        //PlayAudio("MenuTheme");
    }


    public void PlayAudio(string name)
    {
        MR_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Play();
    }

    public void StopAudio(string name)
    {
        MR_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Stop();
    }
    public void StopAllAudio()
    {
        foreach (MR_Sound sound in sounds)
        {
            sound.audioSource.Stop();
        }
    }


    public void SetTrackVolume(string name, float volume)
    {
        MR_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.volume = volume;
    }


    public void PauseAudio(string name)
    {
        MR_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Pause();
    }
}
