using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable]
public class MMSound
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

public class MM_AudioManager : MonoBehaviour
{

    public static MM_AudioManager instance;
    public MMSound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        foreach (MMSound sound in sounds)
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
        MMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Play();
    }
    public void StopAudio(string name)
    {
        MMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Stop();
    }
    public void StopAllAudio()
    {
        foreach (MMSound sound in sounds)
        {
            sound.audioSource.Stop();
        }
    }
    public void SetTrackVolume(string name, float volume)
    {
        MMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.volume = volume;
    }
    public void PauseAudio(string name)
    {
        MMSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.audioSource.Pause();
    }

}
