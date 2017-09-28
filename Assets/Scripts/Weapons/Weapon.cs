using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    string weaponName;
    Rigidbody rb;
    WeaponFactory wf;
    GameObject player;
    List<Action<GameObject, GameObject>> weaponE = new List<Action<GameObject, GameObject>>();

    public string GetWeaponName() { return weaponName; }

    public void SetWeaponName(string weaponName) { this.weaponName = weaponName; }
    public void AddWeaponEffect(Action<GameObject, GameObject> weaponE) { this.weaponE.Add(weaponE); }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<IHealth>() != null && col.transform != transform.parent)
        {
            foreach (Action<GameObject, GameObject> effect in weaponE)
            {
                effect(player, col.gameObject);
            };
        }
    }
}