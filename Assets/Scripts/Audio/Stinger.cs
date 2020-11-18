using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audio;

    [SerializeField]
    private AudioClip[] _stingers;

    public void PlayStinger1()
    {
        _audio.clip = _stingers[0];
        _audio.Play();
    }
    public void PlayStinger2()
    {
        _audio.clip = _stingers[1];
        _audio.Play();
    }
}
