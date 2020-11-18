using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    // move speed
    [SerializeField]
    private float _normalSpeed;
    private float _curSpeed;

    private Rigidbody2D _rb;

    // move left and right
    private float _input;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _deceleration;

    // turn/rotate
    private float _xScale;

    // ground check
    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private LayerMask _whatIsGround;
    private bool _isGrounded;

    // ceiling check
    [SerializeField]
    private Transform _ceilingCheck;
    [SerializeField]
    private float _ceilingCheckRadius;

    // slope check
    [SerializeField]
    private float _slopeCheckRadius;
    [SerializeField]
    private LayerMask _whatIsSlope;
    private bool _isOnSlope;

    // jump
    [SerializeField]
    private float _jumpForce;
    private bool _isJumping;

    // move object
    private ObjectInteraction _obj;

    // gravity switch
    private bool _gravityDown;
    [SerializeField]
    private float _rotationSpeed;
    //[HideInInspector]
    private bool _haveControl = true;
    private bool _hadControl = true;
    private float _yScale;

    // animation
    [SerializeField]
    private Animator _playerAnimator;

    // tutorial hint icons
    [SerializeField]
    private TutorialByActivation _wasd;

    // walk sfx
    private bool _wasWalking = false;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        //_playerAnimator = GetComponent<Animator>();
        _xScale = this.transform.localScale.x;
        _yScale = this.transform.localScale.y;
        _isJumping = false;
        _obj = GetComponent<ObjectInteraction>();
        _gravityDown = true;
        _curSpeed = _normalSpeed;
    }

    private void Start()
    {
        transform.position = GameManager.Instance.GetRespawnPosition();
    }


    private void FixedUpdate()
    {
        // move left/right
        _input = Input.GetAxisRaw("Horizontal");
        if (_wasd && _haveControl && _input != 0)
        {
            Destroy(_wasd.gameObject);
        }

        float x = _input * _curSpeed;
        float y = _rb.velocity.y;

        if (_input == 0 || !_haveControl)
        {
            x = 0;
        }

        // ceiling check
        Collider2D ceiling = Physics2D.OverlapCircle(_ceilingCheck.position, _ceilingCheckRadius, _whatIsGround);
        Collider2D slope = Physics2D.OverlapCircle(_ceilingCheck.position, _ceilingCheckRadius, _whatIsSlope);
        if (ceiling || slope)
        {
            if (_gravityDown && _rb.velocity.y > 0)
            {
                y = 0;
            }
            else if (!_gravityDown && _rb.velocity.y < 0)
            {
                y = 0;
            }
        }

        // check slope
        if (_isOnSlope && !_isGrounded)
        {
            x = 0;
            return;
        }
        _rb.velocity = new Vector2(x, y);

    }

    // Update is called once per frame
    void Update()
    {
        // check slope
        _isOnSlope = Physics2D.OverlapCircle(_groundCheck.position, _slopeCheckRadius, _whatIsSlope);

        // ground check

        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _whatIsGround);

        if (!_isGrounded)
        {
            SoundManager.Instance.StopSFX("Walk");
        }

        if (_isGrounded && !_playerAnimator.GetBool("InAir"))
        {
            _playerAnimator.ResetTrigger("Jump");
            _playerAnimator.SetTrigger("ForceLand");
        }


        if (!_isOnSlope)
        {
            _playerAnimator.SetBool("InAir", !_isGrounded);
            if (_isGrounded == true)
            {
                _isJumping = false;
            }

            // check fall
            if (_isGrounded == false && _isJumping == false)
            {
                _playerAnimator.SetBool("InAir", true);
            }
        }
        else
        {
            if (_isGrounded)
            {
                _isJumping = false;
                _playerAnimator.SetBool("InAir", false);
                _playerAnimator.SetTrigger("LandFromSlope");
            }
            else
            {
                _playerAnimator.SetBool("InAir", true);
            }
            //else if (!_isGrounded && _isJumping)
            //{
            //    Debug.Log("falling.");
            //    _playerAnimator.SetBool("InAir", true);
            //}
            //else if (!_isGrounded && !_isJumping)
            //{
            //    Debug.Log("land.");
            //    _playerAnimator.SetBool("InAir", false);
            //}
        }

        

        if (!_haveControl)
        {
            SoundManager.Instance.StopSFX("Walk");
            return;
        }

        // turn player
        if (_input < 0)
        {
            if (!_obj.IsMovingObject())
            {
                this.transform.localScale = new Vector3(-_xScale, _yScale, 1);
            }
            _playerAnimator.SetBool("IsMoving", true);

            if (_isGrounded)
            {
                SoundManager.Instance.PlaySFX("Walk");
            }

            _wasWalking = true;
        }
        else if (_input > 0)
        {
            if (!_obj.IsMovingObject())
            {
                this.transform.localScale = new Vector3(_xScale, _yScale, 1);
            }
            _playerAnimator.SetBool("IsMoving", true);

            if (_isGrounded)
            {
                SoundManager.Instance.PlaySFX("Walk");
            }

            _wasWalking = true;
        }
        else
        {
            _playerAnimator.SetBool("IsMoving", false);

            SoundManager.Instance.StopSFX("Walk");

            _wasWalking = false;
        }

        // check jump
        if (_isGrounded == true && Input.GetButtonDown("Jump") && !Physics2D.OverlapCircle(_ceilingCheck.position, _ceilingCheckRadius, _whatIsGround))
        {
            if (!_obj.IsMovingObject())
            {
                _playerAnimator.SetTrigger("Jump");
                _playerAnimator.SetBool("InAir", true);
                SoundManager.Instance.StopSFX("Walk");
                SoundManager.Instance.PlaySFX("Jump");
                _isJumping = true;
                if (_gravityDown)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                }
                else
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, -_jumpForce);
                }
            }
        }

        

    }

    public bool IsTouchingGround()
    {
        return _isGrounded;
    }


    public void Rotate()
    {

        _gravityDown = !_gravityDown;
        _yScale = -_yScale;
        transform.localScale = new Vector3(_xScale, _yScale, 1);
    }

    public float MoveDirection()
    {
        return _input * _curSpeed;
    }

    public void CanControl()
    {
        _hadControl = _haveControl;
        _haveControl = true;
    }

    public void CannotControl()
    {
        _hadControl = _haveControl;
        _playerAnimator.SetBool("IsMoving", false);
        _haveControl = false;
    }

    public void HoldingObject(float moveSpeed)
    {
        _curSpeed = moveSpeed;
    }

    public void ReleasingObject()
    {
        _curSpeed = _normalSpeed;
    }

    public bool HaveControl()
    {
        return _haveControl;
    }
}
