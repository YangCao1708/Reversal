using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPlayerControl : MonoBehaviour
{
    public static LockPlayerControl Instance;
    private CharacterController _player;
    [SerializeField]
    private GameObject _respawns;
    [SerializeField]
    private GameObject[] _lights;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _player = FindObjectOfType<CharacterController>();
    }

    public void DisablePlayerControl()
    {
        if (_player)
        {
            //_respawns.SetActive(false);
            //AbilityManager.Instance.enabled = false;
            AbilityManager.Instance.BeginCutscene();
            _player.CannotControl();
        }
    }

    public void EnablePlayerControl()
    {
        if (_player)
        {
            //_respawns.SetActive(true);
            //AbilityManager.Instance.enabled = true;
            AbilityManager.Instance.EndCutscene();
            _player.CanControl();
        }
    }

    public void DisableLight()
    {
        foreach (GameObject light in _lights)
        {
            light.SetActive(false);
        }
    }

    public void EnableLight()
    {
        foreach (GameObject light in _lights)
        {
            light.SetActive(true);
        }
    }

    public void Level3Finished()
    {
        GameManager.Instance.EnterLevel4();
    }

    public void Silence()
    {
        SoundManager.Instance.Silence();
    }

    public void EnterGarden()
    {
        SoundManager.Instance.EnterGarden();
    }

    public void StartCreditsMusic()
    {
        SoundManager.Instance.PlayCreditsBGM();
    }
}
