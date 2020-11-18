using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public event Action onNewLevel;
    public void NewLevel()
    {
        if (onNewLevel != null)
        {
            onNewLevel();
        }
    }

    public event Action onGravitySwitchOn;
    public void GravitySwitchOn()
    {
        if (onGravitySwitchOn != null)
        {
            onGravitySwitchOn();
        }
    }

    public event Action onGravitySwitchOff;
    public void GravitySwitchOff()
    {
        if (onGravitySwitchOff != null)
        {
            onGravitySwitchOff();
        }
    }

    public event Action onLayerUp;
    public void LayerUp()
    {
        if (onLayerUp != null)
        {
            onLayerUp();
        }
    }

    public event Action onLayerDown;
    public void LayerDown()
    {
        if (onLayerDown != null)
        {
            onLayerDown();
        }
    }

    public event Action onLayerSet;
    public void LayerSet()
    {
        if (onLayerSet != null)
        {
            onLayerSet();
        }
    }

    public event Action onLayerSwitch;
    public void LayerSwitch()
    {
        if (onLayerSwitch != null)
        {
            onLayerSwitch();
        }
    }
}
