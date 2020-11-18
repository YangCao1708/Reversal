using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{

    [SerializeField]
    private float _distance = 1f;
    [SerializeField]
    private LayerMask _interactableMask;
    [SerializeField]
    private Transform _rayOriginBot;
    [SerializeField]
    private Transform _rayOriginMid;

    private Movable _movable;

    private bool _isMoving;

    [SerializeField]
    private Animator _playerAnimator;
    private CharacterController _player;

    // playing an animation
    [SerializeField]
    private float _playerPullSpeed = 0.005f;
    private bool _isPlayingAnimation = false;

    // Start is called before the first frame update
    void Awake()
    {
        _isMoving = false;
        //_playerAnimator = GetComponent<Animator>();
        _player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayingAnimation || !_player.IsTouchingGround())
        {
            if (_movable!=null)
            {
                ReleaseHold();
            }
            return;
        }

        Physics2D.queriesStartInColliders = false;
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        RaycastHit2D hitBot = Physics2D.Raycast((Vector2)_rayOriginBot.position, Vector2.right * direction, _distance, _interactableMask);
        RaycastHit2D hitMid = Physics2D.Raycast((Vector2)_rayOriginMid.position, Vector2.right * direction, _distance, _interactableMask);

        if (!hitBot && !hitMid)
        {
            return;
        }

        RaycastHit2D hit = hitBot;
        if (hitMid) hit = hitMid;

        Tutorial tutorial = hit.transform.GetComponent<Tutorial>();

        // level 1 pull gate
        //if (hit.transform.tag == "Gate" && Input.GetButtonDown("Hold"))
        //{
        //    if (tutorial)
        //    {
        //        tutorial.Destroy();
        //    }
        //    bool moveLeft = _player.transform.position.x < hit.transform.position.x ? true : false;
        //    StartCoroutine(PullOnGate(hit.transform.GetComponent<Animator>(), 1.5f, moveLeft));
        //    return;
        //}
        // level 1 collect gravity switch layer
        if (hit.transform.tag == "Grave" && Input.GetButtonDown("Hold"))
        {
            AnimationTrigger a = hit.transform.GetComponent<AnimationTrigger>();
            if (a)
            {
                if (tutorial)
                {
                    tutorial.Destroy();
                }
                a.TriggerAnimation();
                AbilityManager.Instance.PickUpGravityLayer();
            }
            return;
        }
        // push/pull boxes
        else if (hit.transform.tag.Contains("Movable") && Input.GetButton("Hold"))
        {

            if (tutorial)
            {
                tutorial.Destroy();
            }

            _isMoving = true;
            _movable = hit.transform.gameObject.GetComponent<Movable>();
            if (_movable.IsGrounded() || _movable.GetIsPressingButton())
            {
                HoldObject();
            }
            else
            {
                ReleaseHold();
            }
        }
        else if (_movable != null && Input.GetButtonUp("Hold"))
        {
            ReleaseHold();
        }

        if (!_player.IsTouchingGround())
        {
            ReleaseHold();
        }

        if (_isMoving)
        {
            float moveDirection = _player.MoveDirection();
            if (moveDirection == 0) // doing nothing
            {
                _playerAnimator.SetBool("IsPushing", false);
                _playerAnimator.SetBool("IsPulling", false);
            }
            else if (moveDirection < 0)
            {
                if (this.transform.position.x < _movable.transform.position.x) // pulling
                {
                    //Debug.Log("pulling");
                    _playerAnimator.SetBool("IsPushing", false);
                    _playerAnimator.SetBool("IsPulling", true);
                }
                else // pushing
                {
                    //Debug.Log("pushing");
                    _playerAnimator.SetBool("IsPushing", true);
                    _playerAnimator.SetBool("IsPulling", false);
                }
            }
            else if (moveDirection > 0)
            {
                if (this.transform.position.x < _movable.transform.position.x) // pushing
                {
                    //Debug.Log("pushing");
                    _playerAnimator.SetBool("IsPushing", true);
                    _playerAnimator.SetBool("IsPulling", false);
                }
                else // pulling
                {
                    //Debug.Log("pulling");
                    _playerAnimator.SetBool("IsPushing", false);
                    _playerAnimator.SetBool("IsPulling", true);
                }
            }
        }
        else
        {
            _playerAnimator.SetBool("IsHolding", false);
            _playerAnimator.SetBool("IsPushing", false);
            _playerAnimator.SetBool("IsPulling", false);
        }

    }

    public bool IsMovingObject()
    {
        return _isMoving;
    }

    public void HoldObject()
    {

        _player.HoldingObject(_movable.MoveSpeed);

        _movable.Moving = true;
        _movable.GetComponent<FixedJoint2D>().enabled = true;
        _movable.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();

        _playerAnimator.SetBool("IsHolding", true);
    }


    public void ReleaseHold()
    {
        _player.ReleasingObject();
        _playerAnimator.SetBool("IsHolding", false);

        _isMoving = false;
        if (_movable != null)
        {
            _movable.GetComponent<FixedJoint2D>().enabled = false;
            _movable.GetComponent<Movable>().Fall();

            _movable = null;
        }
        
    }

    IEnumerator PullOnGate(Animator ani, float time, bool moveLeft)
    {
        _isPlayingAnimation = true;
        

        // disable some stuff
        _player.enabled = false;
        AbilityManager.Instance.enabled = false;

        // animation
        ani.SetTrigger("OpenGate");
        _playerAnimator.SetTrigger("OpenGate");

        // move player backwards
        float timeRemained = time;
        while (timeRemained > 0)
        {
            if (moveLeft)
            {
                _player.transform.position += Vector3.left * _playerPullSpeed;
            }
            else
            {
                _player.transform.position += Vector3.right * _playerPullSpeed;
            }
            timeRemained -= Time.deltaTime;
            yield return null;
        }
        //string clipLength = ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //Debug.Log(clipLength);
        //yield return new WaitForSecondsRealtime(1f);

        // enable stuff
        _player.enabled = true;
        AbilityManager.Instance.enabled = true;
        _isPlayingAnimation = false;

        Destroy(ani.gameObject.GetComponent<BoxCollider2D>());
    }
}
