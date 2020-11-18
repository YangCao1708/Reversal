using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialByDistance : Tutorial
{

    [SerializeField]
    private float _triggerDistance;

    private void Update()
    {
        if (WithinDistance())
        {
            ShowIcon();
        }
        else
        {
            HideIcon();
        }
    }


    private bool WithinDistance()
    {
        return Vector2.Distance(this.transform.position, _player.position) <= _triggerDistance;
    }

    private void ShowIcon()
    {
        _icon.SetTransparency(1f);
    }

    private void HideIcon()
    {
        _icon.SetTransparency(0f);
    }

}
