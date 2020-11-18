using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [SerializeField]
    private AudioMixerGroup _BGMAudioMixer;
    [SerializeField]
    private AudioMixerGroup _SFXAudioMixer;

    [SerializeField]
    private AudioMixerSnapshot _noSoundSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _level2Snapshot;
    [SerializeField]
    private AudioMixerSnapshot _level3Snapshot;
    [SerializeField]
    private AudioMixerSnapshot _gardenSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _creditsSnapshot;

    [SerializeField]
    private AudioSource[] _BGMAudioSources;

    [SerializeField]
    private Sound[] _SFXAudioClips;

    [SerializeField]
    private float _lowPassNormal = 5000f;
    [SerializeField]
    private float _lowPassPaused = 365f;

    private bool _paused = false;

    private IEnumerator _snapshotCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        for (int i = 0; i < _SFXAudioClips.Length; i++)
        {
            Sound s = _SFXAudioClips[i];
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Audio;
            s.Source.outputAudioMixerGroup = _SFXAudioMixer;
            s.Source.playOnAwake = s.PlayOnAwake;
            s.Source.volume = s.Volume;
            s.Source.loop = s.Loop;
        }

        //StartCoroutine(PlayBGM());
    }

    public void PlaySFX(string name)
    {
        if (!_paused)
        {
            Sound entry = _SFXAudioClips.Where(x => x.AudioName == name).FirstOrDefault();
            if (entry != null && entry.Source.enabled)
            {
                if (name == "Walk" && entry.Source.isPlaying)
                {
                    return;
                }
                entry.Source.Play();
            }
        }
    }

    public void Restart()
    {
        _BGMAudioMixer.audioMixer.SetFloat("LowPass", _lowPassNormal);
        if (_snapshotCoroutine != null)
        {
            StopCoroutine(_snapshotCoroutine);
        }

        int level = GameManager.Instance.GetCurrentLevel();
        if (level == 0)
        {
            _snapshotCoroutine = EnterMenu();
            StartCoroutine(_snapshotCoroutine);
        }
        else if (level == 1)
        {
            _snapshotCoroutine = EnterLevel1();
            StartCoroutine(_snapshotCoroutine);
        }
        else if (level == 2)
        {
            EnterLevel2();
        }
        else if (level == 3)
        {
            EnterLevel3();
        }
    }

    public void Silence()
    {
        if (_snapshotCoroutine != null)
        {
            StopCoroutine(_snapshotCoroutine);
        }
        _snapshotCoroutine = EnterMenu();
        StartCoroutine(_snapshotCoroutine);
    }

    IEnumerator EnterMenu()
    {
        _noSoundSnapshot.TransitionTo(3f);
        yield return new WaitForSecondsRealtime(3f);
        foreach (AudioSource a in _BGMAudioSources)
        {
            a.Stop();
        }
    }

    IEnumerator EnterLevel1()
    {
        _noSoundSnapshot.TransitionTo(1f);
        yield return new WaitForSecondsRealtime(1f);
        foreach (AudioSource a in _BGMAudioSources)
        {
            a.Stop();
        }
    }

    private void EnterLevel2()
    {
        // if restart or enter from level 3
        if (_BGMAudioSources[1].isPlaying)
        {
            _level2Snapshot.TransitionTo(1f);
        }
        // if enter from anywhere else
        else
        {
            _BGMAudioSources[0].Play();
            _BGMAudioSources[1].Play();
            _level2Snapshot.TransitionTo(1f);
        }
    }

    private void EnterLevel3()
    {
        // if restart or enter from level 2
        if (_BGMAudioSources[1].isPlaying)
        {
            _level3Snapshot.TransitionTo(1f);
        }
        // if enter from menu
        else
        {
            _BGMAudioSources[0].Play();
            _BGMAudioSources[1].Play();
            _level3Snapshot.TransitionTo(1f);
        }
    }

    public void EnterGarden()
    {
        if (_snapshotCoroutine != null)
        {
            StopCoroutine(_snapshotCoroutine);
        }
        _snapshotCoroutine = EnterGardenScene();
        StartCoroutine(_snapshotCoroutine);
    }

    IEnumerator EnterGardenScene()
    {
        _BGMAudioSources[2].Play();
        _gardenSnapshot.TransitionTo(1f);
        yield return new WaitForSecondsRealtime(1f);
        _BGMAudioSources[0].Stop();
        _BGMAudioSources[1].Stop();
    }

    public void PlayCreditsBGM()
    {
        if (!_BGMAudioSources[3].isPlaying)
        {
            if (_snapshotCoroutine != null)
            {
                StopCoroutine(_snapshotCoroutine);
            }
            _creditsSnapshot.TransitionTo(0.1f);
            _BGMAudioSources[3].Play();
            StartCoroutine(PlayCreditsMusic());
        }
    }

    IEnumerator PlayCreditsMusic()
    {
        _creditsSnapshot.TransitionTo(0.5f);
        _BGMAudioSources[3].Play();
        yield return new WaitForSecondsRealtime(0.5f);
        _BGMAudioSources[0].Stop();
        _BGMAudioSources[1].Stop();
        _BGMAudioSources[2].Stop();
    }

    public void EnterCredits()
    {
        if (_snapshotCoroutine != null)
        {
            StopCoroutine(_snapshotCoroutine);
        }
        PlayCreditsBGM();
        //_creditsSnapshot.TransitionTo(0.1f);
        //StartCoroutine(PlayCreditsMusic());
    }

    //IEnumerator PlayCreditsMusic()
    //{
    //    yield return new WaitForSecondsRealtime(2f);
    //    _BGMAudioSources[3].Play();
    //}


    public void StopSFX(string name)
    {
        if (!_paused)
        {
            Sound entry = _SFXAudioClips.Where(x => x.AudioName == name).FirstOrDefault();
            if (entry != null && entry.Source.enabled)
            {
                entry.Source.Stop();
            }
        }
    }

    public void Pause()
    {
        CutsceneAudioStorage storage = GameObject.Find("CutSceneAudios").GetComponent<CutsceneAudioStorage>();
        storage.PauseAudio();
        _BGMAudioMixer.audioMixer.SetFloat("LowPass", _lowPassPaused);
    }

    public void Resume()
    {
        CutsceneAudioStorage storage = GameObject.Find("CutSceneAudios").GetComponent<CutsceneAudioStorage>();
        storage.UnpauseAudio();
        _BGMAudioMixer.audioMixer.SetFloat("LowPass", _lowPassNormal);
    }

    public void SetBGMVolume(float volume)
    {
        _BGMAudioMixer.audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        _BGMAudioMixer.audioMixer.SetFloat("SFXVolume", volume);
    }

    public float GetBGMVolume()
    {
        float volume;
        _BGMAudioMixer.audioMixer.GetFloat("BGMVolume", out volume);
        return volume;
    }

    public float GetSFXVolume()
    {
        float volume;
        _BGMAudioMixer.audioMixer.GetFloat("SFXVolume", out volume);
        return volume;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            EnterCredits();
        }
        else
        {
            Restart();
        }
    }
}
