using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IHealth
{
    [SerializeField]
    float health, rotSpeed, moveSpeed, boostSpeed, boostTime, offSpeed, offDiv;

    bool canBoost;
    int playerNo;
    Vector3 offset;
    Vector3 marker;
    Rigidbody rb;
    List<GameObject> weaponPoints;

    public Vector3 GetOffset() { return offset; }
    public List<GameObject> GetWeaponPoints() { return weaponPoints; }

    public void SetPlayerNo(int _playerNo) { playerNo = _playerNo; }

    void Start()
    {
        canBoost = true;
        rb = GetComponent<Rigidbody>();
        weaponPoints = new List<GameObject>();
    }

    void Update()
    {
        SetOffset();
    }

    void FixedUpdate()
    {
        Rotate();
        Move();
    }

    void Death()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void DealDamage(float _dmg)
    {
        health -= _dmg;
    }

    void SetOffset()
    {
        if (marker == offset)
        {
            offset = Random.insideUnitSphere / offDiv;
        }

        marker = Vector3.MoveTowards(marker, offset, offSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        float xRot = Input.GetAxis("P" + playerNo + "Horiz") * rotSpeed;
        float yRot = Input.GetAxis("P" + playerNo + "Vert") * rotSpeed;

        rb.AddTorque(yRot, xRot, 0, ForceMode.Impulse);
    }

    void Move()
    {
        float xMove = Input.GetAxis("P" + playerNo + "XDir") * moveSpeed;
        float zMove = Input.GetAxis("P" + playerNo + "ZDir") * moveSpeed;

        if (Input.GetButton("P" + playerNo + "Boost") && canBoost)
        {
            //Debug.Log(xMove);
            xMove *= boostSpeed;
            //Debug.Log(xMove);
            zMove *= boostSpeed;
            StartCoroutine(BoostCD());
        }

        rb.MovePosition(rb.position + new Vector3(xMove, 0, zMove));
    }

    IEnumerator BoostCD()
    {
        canBoost = false;
        yield return new WaitForSeconds(boostTime);
        canBoost = true;
    }
}