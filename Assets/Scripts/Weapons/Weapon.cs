using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    string weaponName;//, meshPath, materialPath;
    Rigidbody rb;
    //GameManager gm;
    //WeaponFactory wf;
    List<Action> weaponE;

    public string GetWeaponName() { return weaponName; }
    //public string GetMeshPath() { return meshPath; }
    //public string GetMaterialPath() { return materialPath; }

    public void SetWeaponName(string weaponName) { this.weaponName = weaponName; }
    //public void SetMeshPath(string meshPath) { this.meshPath = meshPath; }
    //public void SetMaterialPath(string materialPath) { this.materialPath = materialPath; }

    void Start()
    {
        weaponE = new List<Action>();
        rb = GetComponent<Rigidbody>();
        //gm = GameManager.GetInstance();
        //wf = gm.GetWeaponFactory();

        // Add effectors by indexing from json file to create weapon.
        // E.g.
        // Oi nah I got editor for this now instead.
        //weaponE.Add(wf.GetEffect(0));
        //weaponE.Add(wf.GetEffect(1));
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