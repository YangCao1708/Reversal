using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTwo : Gate
{
    private Animator _animator;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void LightOn()
    {
        base.LightOn();
        _animator.SetBool("LightOn", true);
    }

    public override void LightOff()
    {
        base.LightOff();
        _animator.SetBool("LightOn", false);
    }
}
