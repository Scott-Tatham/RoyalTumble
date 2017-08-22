using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct WeaponEffect
{
    string effectName;
    Action effectFunction;

    public string GetName() { return effectName; }
    public Action GetEffect() { return effectFunction; }

    public WeaponEffect(string effectName, Action effectFunction)
    {
        this.effectName = effectName;
        this.effectFunction = effectFunction;
    }
}

public class WeaponFactory
{
    GameObject weaponObj;
    JsonData weaponData;
    Dictionary<int, WeaponEffect> wEffects = new Dictionary<int, WeaponEffect>();

    public string[] GetEffectNames()
    {
        List<string> weaponNames = new List<string>();

        foreach (KeyValuePair<int, WeaponEffect> we in wEffects)
        {
            weaponNames.Add(we.Value.GetName());
        }

        return weaponNames.ToArray();
    }

    public string[] GetEffectNames(Dictionary<int, int> uniqueIDs)
    {
        List<string> weaponNames = new List<string>();

        foreach (KeyValuePair<int, WeaponEffect> we in wEffects)
        {
            if (!uniqueIDs.ContainsValue(we.Key))
            {
                weaponNames.Add(we.Value.GetName());
            }
        }

        return weaponNames.ToArray();
    }

    public string GetEffectName(int index)
    {
        int nameIndex = 0;
        string[] weaponNames = new string[wEffects.Count];

        foreach (KeyValuePair<int, WeaponEffect> we in wEffects)
        {
            weaponNames[nameIndex] = we.Value.GetName();
        }

        return weaponNames[index];
    }

    public Action GetEffect(int index)
    {
        return wEffects[index].GetEffect();
    }

    public int GetEffectID(string effect)
    {
        foreach (KeyValuePair<int, WeaponEffect> we in wEffects)
        {
            if (we.Value.GetName() == effect)
            {
                return we.Key;
            }
        }

        return 0;
    }

    public WeaponFactory()
    {
        weaponData = File.ReadAllText("Assets/StreamData/WeaponData.json");
        weaponObj = (GameObject)Resources.Load("Prefabs/Weapon");
        // Replace int with an id from json.
        wEffects.Add(0, new WeaponEffect("Name", () => WeaponEffects.DoThat(1)));
        wEffects.Add(1, new WeaponEffect("NameOfEffect", () => WeaponEffects.DoThis(18)));
    }

    public void GenerateWeapon(int weaponID, Action<Weapon> callback)
    {
        Weapon weapon = weaponObj.GetComponent<Weapon>();
        weapon.SetWeaponName((string)weaponData[weaponID][0]);
        weapon.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load((string)weaponData[weaponID][1]);
        weapon.GetComponent<Renderer>().material = (Material)Resources.Load((string)weaponData[weaponID][2]);
        callback(weapon);
        // Call instantiation on character.
    }
}