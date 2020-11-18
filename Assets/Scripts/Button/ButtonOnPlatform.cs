using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnPlatform : Button
{
    [SerializeField]
    private GameObject _platform;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        _platform.SetActive(false);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (!_isPressed)
        {
            _platform.SetActive(true);
        }
    }
}
