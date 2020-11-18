using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitchAnimation : MonoBehaviour
{
    private Animator _ani;


    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onGravitySwitchOn += UpsideDownAnimation;
        GameEvents.Instance.onGravitySwitchOff += NormalAnimation;
        _ani = GetComponent<Animator>();
    }

    private void NormalAnimation()
    {
        _ani.SetBool("IsNormal", true);
    }

    private void UpsideDownAnimation()
    {
        _ani.SetBool("IsNormal", false);
    }
}
