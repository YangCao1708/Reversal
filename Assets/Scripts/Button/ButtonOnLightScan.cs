using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnLightScan : Button
{
    [SerializeField]
    private LightScan _lightScan;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _lightScan.TurnOn();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (!_isPressed)
        {
            _lightScan.TurnOff();
        }
    }

}
