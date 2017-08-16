using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Rigidbody rb;
    GameManager gm;
    WeaponFactory wf;

    List<Action> WeaponE = new List<Action>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.GetInstance();
        wf = gm.GetWeaponFactory();

        // Add effectors by indexing from json file to create weapon.
        // E.g.
        WeaponE.Add(wf.GetEffect(0));
        WeaponE.Add(wf.GetEffect(1));
    }

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.GetComponent<IHealth>() != null && _col.transform != transform.parent)
        {
            Debug.Log(_col.impulse.magnitude);
            _col.gameObject.GetComponent<IHealth>().DealDamage(_col.impulse.magnitude);

            foreach (Action effect in WeaponE)
            {
                Debug.Log("Here");
                effect();
            };
        }
    }
}