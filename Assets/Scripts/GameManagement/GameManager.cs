using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    WeaponFactory weaponFactory;

    public static GameManager GetInstance() { return instance; }
    public WeaponFactory GetWeaponFactory() { return weaponFactory; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        weaponFactory = new WeaponFactory();
    }
}