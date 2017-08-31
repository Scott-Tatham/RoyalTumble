using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    PHYSICAL,
    FIRE,
    ICE,
    WIND,
    EARTH,
    LIGHTNING,
    POISON,
}

public class WeaponEffects
{
    public static void Burn(float dmg, int ticks, float interval, float resistMod, float duration, DamageType dType, DamageType resistType, GameObject player, GameObject target)
    {
        target.GetComponent<IHealth>().DealDOTDamage(dmg, ticks, interval, dType);
        target.GetComponent<IHealth>().ModifyResist(resistMod, duration, resistType);
    }

    public static void Freeze(float dmg, float duration, DamageType dType, GameObject player, GameObject target)
    {
        target.GetComponent<Character>().StopMove(true, true, duration);
        target.GetComponent<IHealth>().DealDamage(dmg, dType);
    }

    public static void Knockback(float dmg, float power, DamageType dType, GameObject player, GameObject target)
    {
        target.GetComponent<Rigidbody>().AddForce((player.transform.position - target.transform.position).normalized * power);
        target.GetComponent<IHealth>().DealDamage(dmg, dType);
    }

    public static void Shockwave(float dmg, float power, float radius, DamageType dType, GameObject player, GameObject target)
    {
        target.GetComponent<Rigidbody>().AddExplosionForce(power, player.transform.position, radius);
        target.GetComponent<IHealth>().DealDamage(dmg, dType);
    }

    public static void Displace(float dmg, float radius, DamageType dType, GameObject player, GameObject target)
    {
        Vector2 randomPos = Random.insideUnitCircle;
        target.transform.position = new Vector3(target.transform.position.x + randomPos.x, target.transform.position.y, target.transform.position.z + randomPos.y);
        target.GetComponent<IHealth>().DealDamage(dmg, dType);
    }

    public static void Poison(float dmg, int ticks, float interval, float slow, float duration, DamageType dType, GameObject player, GameObject target)
    {
        target.GetComponent<Character>().SlowMove(true, true, slow, duration);
        target.GetComponent<IHealth>().DealDOTDamage(dmg, ticks, interval, dType);
    }
}