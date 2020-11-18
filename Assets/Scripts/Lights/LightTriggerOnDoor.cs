using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class LightTriggerOnDoor : MonoBehaviour, LightTrigger
{
    [SerializeField]
    private Gate[] _gates;

    [SerializeField]
    private GameObject _on;

    private List<LightReflection> _LRs = new List<LightReflection>();

    private bool _isOn;

    [SerializeField]
    private AudioSource _audioStart;
    [SerializeField]
    private AudioSource _audioLoop;

    [SerializeField]
    private int _level;

    private float _targetVolume = 0;

    private void Start()
    {
        GameEvents.Instance.onNewLevel += AdjustVolume;
        _isOn = false;
        _on.SetActive(false);
    }

    private void Update()
    {
        _audioStart.volume = Mathf.Lerp(_audioStart.volume, _targetVolume, 0.125f);
        _audioLoop.volume = Mathf.Lerp(_audioLoop.volume, _targetVolume, 0.125f);
    }

    public void AddOneLight(LightReflection rl)
    {
        if (!_LRs.Contains(rl))
        {
            _LRs.Add(rl);

            if (_LRs.Count == 1)
            {
                TurnOnTrigger();
            }
        }
    }

    public void RemoveOneLight(LightReflection rl)
    {
        if (_LRs.Contains(rl))
        {
            _LRs.Remove(rl);

            if (_LRs.Count == 0)
            {
                TurnOffTrigger();
            }
        }        
    }

    private void TurnOnTrigger()
    {
        StartCoroutine(PlayLightOnAudio());
        _isOn = true;
        _on.SetActive(true);
        foreach (Gate g in _gates)
        {
            g.LightOn();
        }
    }

    private void TurnOffTrigger()
    {
        _audioStart.Stop();
        _audioLoop.Stop();
        _isOn = false;
        _on.SetActive(false);
        foreach (Gate g in _gates)
        {
            g.LightOff();
        }
    }

    private void AdjustVolume()
    {
        if (GameManager.Instance.GetCurrentLevel() == _level)
        {
            _targetVolume = 1;
        }
        else
        {
            _targetVolume = 0;
        }
    }

    public bool IsOn()
    {
        return _isOn;
    }

    public bool IsOff()
    {
        return !_isOn;
    }

    IEnumerator PlayLightOnAudio()
    {
        _audioStart.Play();
        float timePassed = 0f;
        while (timePassed < 13.22f)
        {
            yield return null;
            timePassed += Time.unscaledDeltaTime;
            if (!_isOn)
            {
                yield break;
            }
        }
        _audioLoop.Play();
    }
}
