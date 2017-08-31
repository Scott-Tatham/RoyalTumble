using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IHealth
{
    [SerializeField]
    float health, rotSpeed, moveSpeed, boostSpeed, boostTime, boostCD, offSpeed, offDiv, physicalRes, fireRes, iceRes, poisonRes, windRes, earthRes, lightningRes;

    bool canBoost, canPulse, canRotate, canMove, boost;
    int playerNo;
    float xRot, yRot, xMove, zMove;
    Vector3 offset;
    Vector3 marker;
    Rigidbody rb;
    WeaponFactory wf;
    List<GameObject> weapons;

    public Vector3 GetOffset() { return offset; }
    public List<GameObject> GetWeapons() { return weapons; }

    public void SetPlayerNo(int _playerNo) { playerNo = _playerNo; }

    void Start()
    {
        canBoost = true;
        canPulse = true;
        rb = GetComponent<Rigidbody>();
        wf = GameManager.GetInstance().GetWeaponFactory();
        weapons = new List<GameObject>();
        //wf.GenerateWeapon(0, BuildWeapon);
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

    public void DealDamage(float dmg, DamageType dType)
    {
        float finalDmg = dmg;

        switch (dType)
        {
            case DamageType.PHYSICAL:
                finalDmg -= finalDmg * physicalRes;

                break;

            case DamageType.FIRE:
                finalDmg -= finalDmg * fireRes;

                break;

            case DamageType.ICE:
                finalDmg -= finalDmg * iceRes;

                break;

            case DamageType.WIND:
                finalDmg -= finalDmg * windRes;

                break;

            case DamageType.EARTH:
                finalDmg -= finalDmg * earthRes;

                break;

            case DamageType.LIGHTNING:
                finalDmg -= finalDmg * lightningRes;

                break;

            case DamageType.POISON:
                finalDmg -= finalDmg * poisonRes;

                break;
        }

        health -= finalDmg;
    }

    public void DealDOTDamage(float dmg, int ticks, float interval, DamageType dType)
    {
        StartCoroutine(DOTDamage(dmg, ticks, interval, dType));
    }

    public void ModifyResist(float modResist, float duration, DamageType resistType)
    {
        StartCoroutine(ModResist(modResist, duration, resistType));
    }

    public void StopMove(bool move, bool spin, float duration)
    {
        if (move)
        {
            StartCoroutine(StopMovement(duration));
        }

        if (spin)
        {
            StartCoroutine(StopSpin(duration));
        }
    }

    public void SlowMove(bool move, bool spin, float slow, float duration)
    {
        if (move)
        {
            StartCoroutine(SlowMovement(slow, duration));
        }

        if (spin)
        {
            StartCoroutine(SlowSpin(slow, duration));
        }
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

        xRot = Input.GetAxis("KeyHoriz") * rotSpeed;
        yRot = Input.GetAxis("KeyVert") * rotSpeed;

        xMove = Input.GetAxis("P" + playerNo + "XDir") * moveSpeed;
        zMove = Input.GetAxis("P" + playerNo + "ZDir") * moveSpeed;

        xMove = Input.GetAxis("KeyXDir") * moveSpeed;
        zMove = Input.GetAxis("KeyZDir") * moveSpeed;

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

        if ((Input.GetButton("P" + playerNo + "Boost") || Input.GetButton("KeyBoost")) && canBoost)
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

    void BuildWeapon(Weapon weapon)
    {
        weapons.Add(weapon.gameObject);
        //weapon.transform.position = Vector3.zero;
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

    IEnumerator StopMovement(float duration)
    {
        rb.velocity = Vector3.zero;
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }

    IEnumerator StopSpin(float duration)
    {
        rb.angularVelocity = Vector3.zero;
        canRotate = false;
        yield return new WaitForSeconds(duration);
        canRotate = true;
    }

    IEnumerator SlowMovement(float slow, float duration)
    {
        rb.velocity -= rb.velocity * slow;
        float slowVal = moveSpeed * slow;
        moveSpeed -= slowVal;
        yield return new WaitForSeconds(duration);
        moveSpeed += slowVal;
    }

    IEnumerator SlowSpin(float slow, float duration)
    {
        rb.angularVelocity -= rb.angularVelocity * slow;
        float slowVal = rotSpeed * slow;
        rotSpeed -= slowVal;
        yield return new WaitForSeconds(duration);
        rotSpeed += slowVal;
    }

    IEnumerator DOTDamage(float dmg, int ticks, float interval, DamageType dType)
    {
        for (int i = 0; i < ticks; i++)
        {
            DealDamage(dmg, dType);
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator ModResist(float modResist, float duration, DamageType resistType)
    {
        switch (resistType)
        {
            case DamageType.PHYSICAL:
                physicalRes -= modResist;

                break;

            case DamageType.FIRE:
                fireRes -= modResist;

                break;

            case DamageType.ICE:
                iceRes -= modResist;

                break;

            case DamageType.WIND:
                windRes -= modResist;

                break;

            case DamageType.EARTH:
                earthRes -= modResist;

                break;

            case DamageType.LIGHTNING:
                lightningRes -= modResist;

                break;

            case DamageType.POISON:
                poisonRes -= modResist;

                break;
        }

        yield return new WaitForSeconds(duration);

        switch (resistType)
        {
            case DamageType.PHYSICAL:
                physicalRes += modResist;

                break;

            case DamageType.FIRE:
                fireRes += modResist;

                break;

            case DamageType.ICE:
                iceRes += modResist;

                break;

            case DamageType.WIND:
                windRes += modResist;

                break;

            case DamageType.EARTH:
                earthRes += modResist;

                break;

            case DamageType.LIGHTNING:
                lightningRes += modResist;

                break;

            case DamageType.POISON:
                poisonRes += modResist;

                break;
        }
    }
}