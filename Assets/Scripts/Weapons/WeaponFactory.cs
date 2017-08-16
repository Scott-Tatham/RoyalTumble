using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory
{
    List<Action> WEffects = new List<Action>();

    public Action GetEffect(int index)
    {
        return WEffects[index];
    }

    public WeaponFactory()
    {
        WEffects.Add(() => WeaponEffects.DoThat(1));
        WEffects.Add(() => WeaponEffects.DoThis(18));
    }
}