using UnityEngine;
using UnityEngine.Audio;
using System;

public class YipliAudioManager : MonoBehaviour
{
    public GamelibSounds[] Sounds;

    public static YipliAudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);

        foreach (GamelibSounds s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.clip = s.audioclip;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("MenuGameSound");
    }

    public void Play(string name)
    {
        GamelibSounds s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        GamelibSounds s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void Pause(string name)
    {
        GamelibSounds s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        if (s.source.isPlaying)
            s.source.Pause();
        else
        {
            Debug.LogWarning("Cant pause sound " + name + " as it is not being Played");
        }
    }

    public void Resume(string name)
    {
        GamelibSounds s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }

        if (!s.source.isPlaying)
            s.source.UnPause();
        else
        {
            Debug.LogWarning("Cant play sound " + name + " as it is already playing");
        }
    }
}
