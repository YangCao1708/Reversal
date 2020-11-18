using System;
using UnityEngine;

[Serializable]
public class Sound 
{
    public string AudioName;
    public AudioClip Audio;
    [Range(0f, 1f)]
    public float Volume;
    [HideInInspector]
    public AudioSource Source;
    public bool Loop;
    public bool PlayOnAwake;
}
