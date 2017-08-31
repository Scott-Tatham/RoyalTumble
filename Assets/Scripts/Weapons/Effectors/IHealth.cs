using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    void DealDamage(float dmg, DamageType dType);
    void DealDOTDamage(float dmg, int ticks, float interval, DamageType dType);
    void ModifyResist(float resistMod, float duration, DamageType resistType);
}