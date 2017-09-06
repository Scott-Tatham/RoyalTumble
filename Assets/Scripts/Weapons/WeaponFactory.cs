using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct WeaponEffect
{
    int id;
    string effectName;
    Action effectFunction;

    public int GetID() { return id; }
    public string GetName() { return effectName; }
    public Action GetEffect() { return effectFunction; }

    public WeaponEffect(int id, string effectName, Action effectFunction)
    {
        this.id = id;
        this.effectName = effectName;
        this.effectFunction = effectFunction;
    }
}

public class WeaponFactory
{
    GameObject weaponObj;
    JsonData weaponData;
    List<WeaponEffect> wEffects = new List<WeaponEffect>();

    public string[] GetEffectNames()
    {
        List<string> weaponNames = new List<string>();

        for (int i = 0; i < wEffects.Count; i++)
        {
            weaponNames.Add(wEffects[i].GetName());
        }

        return weaponNames.ToArray();
    }

    public string[] GetEffectNames(List<int> uniqueIDs)
    {
        List<string> weaponNames = new List<string>();

        for (int i = 0; i < wEffects.Count; i++)
        {
            if (!uniqueIDs.Contains(wEffects[i].GetID()))
            {
                weaponNames.Add(wEffects[i].GetName());
            }
        }

        return weaponNames.ToArray();
    }

    public string GetEffectName(int id)
    {
        for (int i = 0; i < wEffects.Count; i++)
        {
            if (id == wEffects[i].GetID())
            {
                return wEffects[i].GetName();
            }
        }

        return null;
    }

    public string GetEffectNameIndexed(int index)
    {
        return wEffects[index].GetName();
    }

    public Action GetEffect(int index)
    {
        return wEffects[index].GetEffect();
    }

    public int GetEffectID(string effect)
    {
        for (int i = 0; i < wEffects.Count; i++)
        {
            if (wEffects[i].GetName() == effect)
            {
                return wEffects[i].GetID();
            }
        }

        return 0;
    }

    public WeaponFactory()
    {
        weaponData = File.ReadAllText("Assets/StreamData/WeaponData.json");
        weaponObj = (GameObject)Resources.Load("Prefabs/Weapon");
        wEffects.Add(new WeaponEffect(0, "Burn", () => WeaponEffects.Burn(0, 0, 0, 0, 0, DamageType.PHYSICAL, DamageType.PHYSICAL, null, null)));
        wEffects.Add(new WeaponEffect(1, "Freeze", () => WeaponEffects.Freeze(0, 0, DamageType.PHYSICAL, null, null)));
        wEffects.Add(new WeaponEffect(2, "Knockback", () => WeaponEffects.Knockback(0, 0, DamageType.PHYSICAL, null, null)));
        wEffects.Add(new WeaponEffect(3, "Shockwave", () => WeaponEffects.Shockwave(0, 0, 0, DamageType.PHYSICAL, null, null)));
        wEffects.Add(new WeaponEffect(4, "Displace", () => WeaponEffects.Displace(0, 0, DamageType.PHYSICAL, null, null)));
        wEffects.Add(new WeaponEffect(5, "Poison", () => WeaponEffects.Poison(0, 0, 0, 0, 0, DamageType.PHYSICAL, null, null)));
    }

    public void GenerateWeapon(int weaponID, Action<Weapon> callback)
    {
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        weapon.SetWeaponName((string)weaponData[weaponID][0]);
        weapon.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load((string)weaponData[weaponID][1]);
        weapon.GetComponent<Renderer>().material = (Material)Resources.Load((string)weaponData[weaponID][2]);

        for (int i = 0; i < weaponData[weaponID][3].Count; i++)
        {
            switch ((int)weaponData[weaponID][3][i])
            {
                case 0:
                    weapon.AddWeaponEffect((GameObject player, GameObject target) => WeaponEffects.Burn((float)weaponData[weaponID][4][1][0], (int)weaponData[weaponID][4][0][0], (float)weaponData[weaponID][4][1][1], (float)weaponData[weaponID][4][1][2], (float)weaponData[weaponID][4][1][3], (DamageType)(int)weaponData[weaponID][4][0][1], (DamageType)(int)weaponData[weaponID][4][0][2], player, target));

                    break;

                case 1:
                    weapon.AddWeaponEffect((GameObject player, GameObject target) => WeaponEffects.Freeze((float)weaponData[weaponID][4][1][0], (float)weaponData[weaponID][4][1][1], (DamageType)(int)weaponData[weaponID][4][0][0], player, target));

                    break;

                case 2:
                    weapon.AddWeaponEffect((GameObject player, GameObject target) => WeaponEffects.Knockback((float)weaponData[weaponID][4][1][0], (float)weaponData[weaponID][4][1][1], (DamageType)(int)weaponData[weaponID][4][0][0], player, target));

                    break;

                case 3:
                    weapon.AddWeaponEffect((GameObject player, GameObject target) => WeaponEffects.Shockwave((float)weaponData[weaponID][4][1][0], (float)weaponData[weaponID][4][1][1], (float)weaponData[weaponID][4][1][2], (DamageType)(int)weaponData[weaponID][4][0][0], player, target));

                    break;

                case 4:
                    weapon.AddWeaponEffect((GameObject player, GameObject target) => WeaponEffects.Displace((float)weaponData[weaponID][4][1][0], (float)weaponData[weaponID][4][1][1], (DamageType)(int)weaponData[weaponID][4][0][0], player, target));

                    break;

                case 5:
                    weapon.AddWeaponEffect((GameObject player, GameObject target) => WeaponEffects.Poison((float)weaponData[weaponID][4][1][0], (int)weaponData[weaponID][4][0][0], (float)weaponData[weaponID][4][1][1], (float)weaponData[weaponID][4][1][2], (float)weaponData[weaponID][4][1][3], (DamageType)(int)weaponData[weaponID][4][0][1], player, target));

                    break;
            }
        }

        callback(weapon);
        // Call instantiation on character.
    }
}