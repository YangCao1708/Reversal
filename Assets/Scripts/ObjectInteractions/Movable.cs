using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{

    // speed of player if they move it
    public float MoveSpeed = 1.2f;

    private Rigidbody2D _rb;

    [HideInInspector]
    public bool Moving;

    // check ground
    [SerializeField]
    private BoxCollider2D _triggerCollider;
    [SerializeField]
    private LayerMask _whatIsGround;
    private bool _isGrounded = true;

    // sfxs
    [SerializeField]
    private AudioSource _fallAudioS;
    [SerializeField]
    private AudioSource _moveAudioS;
    private bool _doNotPlaySound = true;

    private bool _wasMoving = false;

    private bool _isPressingButton = false;

    private Vector2 _fallFromPos;

    void Start()
    {
        GameEvents.Instance.onGravitySwitchOn += Fall;
        GameEvents.Instance.onGravitySwitchOn += Rotate;
        GameEvents.Instance.onGravitySwitchOff += Fall;
        GameEvents.Instance.onGravitySwitchOff += Rotate;
        Moving = false;
        _rb = GetComponent<Rigidbody2D>();
        _triggerCollider = GetComponent<BoxCollider2D>();

        StartCoroutine(RemoveFallSound());
    }

    // Update is called once per frame
    void Update()
    {

        bool previousIsGrounded = _isGrounded;
        _isGrounded = TouchingGround();

        if (!_isGrounded && previousIsGrounded)
        {
            _fallFromPos = this.transform.position;
        }

        if (!_doNotPlaySound && !_fallAudioS.isPlaying)
        {
            if (!previousIsGrounded && _isGrounded)
            {
                if (Vector2.Distance(_fallFromPos, transform.position) > 1f)
                {
                    _fallAudioS.Play();
                }
            }
        }

        if (!Moving)
        {
            if (_isGrounded)
            {
                _rb.isKinematic = true;
                _rb.velocity = Vector2.zero;
            }
            else
            {
                _rb.isKinematic = false;
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }

            if (_wasMoving)
            {
                _moveAudioS.Stop();
            }
        }
        else
        {
            _rb.isKinematic = false;
            if (!_wasMoving)
            {
                _moveAudioS.Play();
            }
        }

        _wasMoving = Moving;
    }

    private bool TouchingGround()
    {
        Vector2 direction;

        if (Physics2D.gravity.y < 0)
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.up;
        }

        RaycastHit2D hit = Physics2D.BoxCast(_triggerCollider.bounds.center, _triggerCollider.bounds.size, 0f, direction, 0.05f, _whatIsGround);
        if (hit && hit.transform.name == this.transform.name)
        {
            return false;
        }
        return hit.collider != null;
    }

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public void Fall()
    {
        _rb.isKinematic = false;
        Moving = false;
    }

    private void Rotate()
    {
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }

    IEnumerator RemoveFallSound()
    {
        yield return new WaitForSecondsRealtime(1);
        _doNotPlaySound = false;
    }

    public void PressingButton()
    {
        _isPressingButton = true;
    }

    public void ReleaseButton()
    {
        _isPressingButton = false;
    }

    public bool GetIsPressingButton()
    {
        return _isPressingButton;
    }
}
