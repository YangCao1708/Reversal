using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LightTrigger
{

    void AddOneLight(LightReflection lr);

    void RemoveOneLight(LightReflection lr);

    bool IsOn();
    bool IsOff();
}
