using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CutsceneAudioStorage : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _audios;


    public void PauseAudio()
    {
        foreach (AudioSource a in _audios)
        {
            a.Pause();
        }
    }

    public void UnpauseAudio()
    {
        foreach (AudioSource a in _audios)
        {
            a.UnPause();
        }
    }
}
