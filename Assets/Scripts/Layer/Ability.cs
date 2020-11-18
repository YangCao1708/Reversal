using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability
{

    void UseAbility();

    void EndAbility();

    bool InUse();

    void Reset();


    bool CanUse();

}
