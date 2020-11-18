using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class Gate : MonoBehaviour
{

    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _lightOnAudio;
    [SerializeField]
    private AudioClip _lightOffAudio;

    [SerializeField]
    private List<int> _levels;

    private float _targetVolume = 0;


    protected virtual void Start()
    {
        GameEvents.Instance.onNewLevel += AdjustVolume;
    }

    protected virtual void Update()
    {
        _audio.volume = Mathf.Lerp(_audio.volume, _targetVolume, 0.125f);
    }

    public virtual void LightOn()
    {
        _audio.clip = _lightOnAudio;
        _audio.Play();
    }

    public virtual void LightOff()
    {
        _audio.clip = _lightOffAudio;
        _audio.Play();
    }

    private void AdjustVolume()
    {
        if (_levels.Contains(GameManager.Instance.GetCurrentLevel()))
        {
            _targetVolume = 1;
        }
        else
        {
            _targetVolume = 0;
        }
    }
}
