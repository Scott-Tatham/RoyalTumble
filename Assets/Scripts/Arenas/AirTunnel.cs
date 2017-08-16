using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTunnel : MonoBehaviour
{
    [SerializeField]
    float fanForce;

    void OnTriggerStay(Collider _col)
    {
        if (_col.GetComponent<Character>() != null)
        {
            float distance = _col.transform.position.y > 1 ? _col.transform.position.y : 1;
            _col.attachedRigidbody.AddForce((((Vector3.up/* * (-_col.attachedRigidbody.velocity.y / 2)*/).normalized) * (fanForce / (1 + distance * distance * distance * distance))), ForceMode.Force);
        }
    }
}