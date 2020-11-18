using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOne : Gate
{

    [SerializeField]
    private Vector2 _openOffset;
    [SerializeField]
    private float _speed;
    private Vector2 _closedPosition;
    private Vector2 _targetPos;

    private void Awake()
    {
        _closedPosition = this.transform.position;
        _targetPos = _closedPosition;
    }

    protected override void Update()
    {
        base.Update();
        transform.position = Vector2.Lerp(transform.position, _targetPos, _speed * Time.deltaTime);
    }

    public override void LightOn()
    {
        base.LightOn();
        _targetPos = _closedPosition + _openOffset;
    }

    public override void LightOff()
    {
        base.LightOff();
        _targetPos = _closedPosition;
    }
}
