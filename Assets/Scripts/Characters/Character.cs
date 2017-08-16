using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IHealth
{
    [SerializeField]
    float health, rotSpeed, moveSpeed, boostSpeed, boostTime, boostCD, offSpeed, offDiv;

    bool canBoost, canPulse, canRotate, canMove, boost;
    int playerNo;
    float xRot, yRot, xMove, zMove;
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
        canPulse = true;
        rb = GetComponent<Rigidbody>();
        weaponPoints = new List<GameObject>();
    }

    void Update()
    {
        SetOffset();
        Inputs();
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
        if (canPulse)
        {
            StartCoroutine(AirNoise());
        }

        //if (marker == offset)
        //{
        //    offset = Random.insideUnitSphere / offDiv;
        //}
        //
        //marker = Vector3.MoveTowards(marker, offset, offSpeed * Time.deltaTime);
    }

    void Inputs()
    {
        xRot = Input.GetAxis("P" + playerNo + "Horiz") * rotSpeed;
        yRot = Input.GetAxis("P" + playerNo + "Vert") * rotSpeed;

        xRot = Input.GetAxis("KeyBoardHoriz") * rotSpeed;
        yRot = Input.GetAxis("KeyBoardVert") * rotSpeed;

        xMove = Input.GetAxis("P" + playerNo + "XDir") * moveSpeed;
        zMove = Input.GetAxis("P" + playerNo + "ZDir") * moveSpeed;

        xMove = Input.GetAxis("KeyBoardXDir") * moveSpeed;
        zMove = Input.GetAxis("KeyBoardZDir") * moveSpeed;

        if (xRot != 0 || yRot != 0)
        {
            canRotate = true;
        }

        else
        {
            canRotate = false;
        }

        if (xMove != 0 || zMove != 0)
        {
            canMove = true;
        }

        else
        {
            canMove = false;
        }

        if ((Input.GetButton("P" + playerNo + "Boost") || Input.GetButton("KeyBoardBoost")) && canBoost)
        {
            boost = true;
        }

        //if (boost)
        //{
        //    Debug.Log(xMove);
        //    xMove *= boostSpeed;
        //    Debug.Log(xMove);
        //    zMove *= boostSpeed;
        //}
    }

    void Rotate()
    {
        if (canRotate)
        {
            rb.AddTorque(yRot, xRot, 0, ForceMode.Impulse);
        }
    }

    void Move()
    {
        if (boost && canBoost)
        {
            rb.AddForce(new Vector3(xMove * 10, 0, zMove * 10), ForceMode.Impulse);
            boost = false;
            canBoost = false;
            BoostCD();
        }

        rb.MovePosition(transform.position + new Vector3(xMove, 0, zMove));
    }

    IEnumerator BoostCD()
    {
        yield return new WaitForSeconds(boostCD);
        canBoost = true;
    }

    IEnumerator AirNoise()
    {
        canPulse = false;
        offset = Random.onUnitSphere * offDiv;
        rb.AddForce(offset, ForceMode.Acceleration);
        yield return new WaitForSeconds(0.1f);
        canPulse = true;
    }
}