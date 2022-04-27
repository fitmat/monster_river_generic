using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamelibSounds
{
    public string name;
    public AudioClip audioclip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 1f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}