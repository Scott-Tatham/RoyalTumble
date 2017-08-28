using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    string weaponName;
    Rigidbody rb;
    WeaponFactory wf;
    List<Action> weaponE;

    public string GetWeaponName() { return weaponName; }

    public void SetWeaponName(string weaponName) { this.weaponName = weaponName; }
    public void AddWeaponEffect(Action weaponE) { this.weaponE.Add(weaponE); }

    void Start()
    {
        weaponE = new List<Action>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.GetComponent<IHealth>() != null && _col.transform != transform.parent)
        {
            Debug.Log(_col.impulse.magnitude);
            _col.gameObject.GetComponent<IHealth>().DealDamage(_col.impulse.magnitude);

            foreach (Action effect in weaponE)
            {
                Debug.Log("Here");
                effect();
            };
        }
    }
}