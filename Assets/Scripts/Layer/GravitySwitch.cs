using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour, Ability
{
    private CharacterController _player;
    private bool _inUse;
    [SerializeField]
    private float _coolDownTime;
    private bool _canUse = true;
    private PostProcessingController _ppc;

    [SerializeField]
    private TutorialByActivation _tutorial;

    // Start is called before the first frame update
    void Awake()
    {
        _player = Object.FindObjectOfType<CharacterController>();
        _inUse = false;
        _ppc = Object.FindObjectOfType<PostProcessingController>();
    }

    public void UseAbility()
    {
        if (_tutorial != null)
        {
            Destroy(_tutorial.gameObject);
        }
        Physics2D.gravity = new Vector2(0, 9.81f);
        _player.CannotControl();
        _player.Rotate();
        _inUse = true;
        GameEvents.Instance.GravitySwitchOn();
        StartCoroutine(CoolDown());
        //_ppc.ColdTemperature();
    }
    
    public void EndAbility()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);
        _player.CannotControl();
        _player.Rotate();
        _inUse = false;
        GameEvents.Instance.GravitySwitchOff();
        StartCoroutine(CoolDown());
        //_ppc.WarmTemperature();
    }

    public bool InUse()
    {
        return _inUse;
    }

    public bool CanUse()
    {
        return _canUse;
    }

    public void Reset()
    {
        _inUse = false;
    }

    IEnumerator CoolDown()
    {
        _canUse = false;
        yield return new WaitForSecondsRealtime(_coolDownTime);
        _canUse = true;
    }
}
