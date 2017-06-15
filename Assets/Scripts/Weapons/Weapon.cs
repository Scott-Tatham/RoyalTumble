using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision _col)
    {
        if (_col.gameObject.GetComponent<IHealth>() != null && _col.transform != transform.parent)
        {
            Debug.Log(_col.impulse.magnitude);
            _col.gameObject.GetComponent<IHealth>().DealDamage(_col.impulse.magnitude);
        }
    }
}