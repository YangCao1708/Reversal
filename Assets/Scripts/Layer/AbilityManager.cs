using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    public static AbilityManager Instance;

    [SerializeField]
    private float _waitTime = 0.15f;

    private Ability _curAbility;
    private GravitySwitch _gs;
    private List<Ability> _abilities;

    private ObjectInteraction _objInteraction;
    private CharacterController _player;

    private bool _isWaiting = false;

    private bool _inCutscene = false;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        _gs = GetComponent<GravitySwitch>();

        _abilities = new List<Ability>();

        _objInteraction = Object.FindObjectOfType<ObjectInteraction>();
        _player = Object.FindObjectOfType<CharacterController>();

        if (GameManager.Instance.GetCurrentLevel() > 1)
        {
            _abilities.Add(_gs);
            _curAbility = _gs;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (_abilities.Count == 0 || _curAbility == null)
        {
            return;
        }

        if (_inCutscene)
        {
            return;
        }

        if (!_isWaiting && _player.IsTouchingGround() && !_player.HaveControl())
        {
            //if (!_inCutscene)
            //{
                _player.CanControl();
            //}
        }

        if (!_objInteraction.IsMovingObject())
        {
            if (Input.GetButtonDown("Switch"))
            {
                //int index = _abilities.IndexOf(_curAbility);
                //int newIndex = (index + 1) % _abilities.Count;
                //_curAbility = _abilities[newIndex];
            }
            else if (!_isWaiting && _player.IsTouchingGround() && Input.GetButtonDown("Invoke"))
            {
                if (!_curAbility.InUse() && _curAbility.CanUse())
                {
                    MetricManagerScript.instance.LogString("Gravity switched at position", _player.transform.position.ToString());
                    //_isWaiting = true;
                    _curAbility.UseAbility();
                    GameEvents.Instance.LayerUp();
                    //StartCoroutine(InvokeAbility());
                }
                else if (_curAbility.InUse() && _curAbility.CanUse())
                {
                    MetricManagerScript.instance.LogString("Gravity switched at position", _player.transform.position.ToString());
                    _curAbility.EndAbility();
                    GameEvents.Instance.LayerDown();
                }
            }
        }
    }


    public void PickUpGravityLayer()
    {
        if (!_abilities.Contains(_gs))
        {
            _abilities.Add(_gs);
        }
        _curAbility = _gs;
    }

    private void ResetAbility()
    {
        if (_abilities.Count > 0 && _curAbility != null)
        {
            _curAbility.Reset();
        }
    }

    private void UseAbility()
    {
        _isWaiting = false;
        _curAbility.UseAbility();
    }

    IEnumerator InvokeAbility()
    {
        _player.CannotControl();
        Debug.Log(Time.time);
        Debug.Log(_player.HaveControl());
        yield return new WaitForSecondsRealtime(_waitTime);
        Debug.Log(Time.time);
        Debug.Log(_player.HaveControl());
        UseAbility();
    }

    public void ReverseGravity()
    {
        _inCutscene = true;
        if (Physics2D.gravity.y > 0)
        {
            _curAbility.EndAbility();
            GameEvents.Instance.LayerDown();
        }
    }

    public void BeginCutscene()
    {
        _inCutscene = true;
    }

    public void EndCutscene()
    {
        _inCutscene = false;
    }
}

