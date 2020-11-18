using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Button : MonoBehaviour
{
    [SerializeField]
    private Vector2 _directionFacing;

    [SerializeField]
    private Vector2 _offset;

    [SerializeField]
    private float _speed;

    

    private Vector2 _unpressedPos;
    private Vector2 _pressedPos;

    private Vector2 _targetPos;


    [SerializeField]
    private LayerMask _whatCanPress;

    private AudioSource _audio;

    protected bool _isPressed;
    private int _count = 0;

    private List<GameObject> _insideTrigger = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _unpressedPos = this.transform.position;
        _pressedPos = _unpressedPos + _offset;
        _targetPos = _unpressedPos;
    }

    // Update is called once per frame
    void Update()
    {
        //_isPressed = IsPressed();
        if (_isPressed)
        {
            _targetPos = _pressedPos;
        }
        else
        {
            _targetPos = _unpressedPos;
        }

        ShiftPosition();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        _count++;
        _isPressed = true;
        if (_insideTrigger.Count == 0)
        {
            _audio.Play();
        }
        _insideTrigger.Add(collision.gameObject);

        if (collision.tag.Contains("Movable"))
        {
            collision.transform.GetComponent<Movable>().PressingButton();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        _count--;
        _isPressed = _count == 0 ? false : true;

        _insideTrigger.Remove(collision.gameObject);
        if (_insideTrigger.Count == 0)
        {
            _audio.Play();
        }

        if (collision.tag.Contains("Movable"))
        {
            collision.transform.GetComponent<Movable>().ReleaseButton();
        }
    }

    void ShiftPosition()
    {
        transform.position = Vector2.Lerp(transform.position, _targetPos, Time.deltaTime * _speed);
    }
}
