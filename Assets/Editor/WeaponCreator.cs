using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class WeaponCreator : EditorWindow
{
    bool newEffect;
    int selectedWeapon, selectedEffect;
    string weaponName;
    Mesh weaponMesh;
    Material weaponMaterial;
    WeaponFactory weaponFactory;
    List<WeaponValues> weaponData;
    Dictionary<int, int> selectedEffects;

    [MenuItem("Window/Weapon Creator")]
    public static void ShowWindow()
    {
        GetWindow<WeaponCreator>();
    }

    void Awake()
    {
        if (File.Exists("Assets/StreamData/WeaponData.json"))
        {
            string jsonOpen = File.ReadAllText("Assets/StreamData/WeaponData.json");

            if (JsonMapper.ToObject<List<WeaponValues>>(jsonOpen) != null)
            {
                weaponData = JsonMapper.ToObject<List<WeaponValues>>(jsonOpen);
            }

            else
            {
                weaponData = new List<WeaponValues>();
            }
        }

        else
        {
            File.Create("Assets/StreamData/WeaponData.json");
            weaponData = new List<WeaponValues>();
            Debug.LogWarning("Created new Weapon Data file. Was this intended?");
        }

        weaponFactory = new WeaponFactory();
        selectedEffects = new Dictionary<int, int>();
    }

    void OnGUI()
    {
        GUI.SetNextControlName("New Weapon");

        if (GUILayout.Button("New Weapon"))
        {
            Confirmation.ShowWindow(Confirm);
            GUI.FocusControl("New Weapon");
        }

        if (GUILayout.Button("Delete Weapon"))
        {
            weaponData.RemoveAt(selectedWeapon);
        }
        
        weaponName = EditorGUILayout.TextField("Weapon Name", weaponName);
        weaponMesh = (Mesh)EditorGUILayout.ObjectField("Weapon Mesh", weaponMesh, typeof(Mesh), false);
        weaponMaterial = (Material)EditorGUILayout.ObjectField("Weapon Material", weaponMaterial, typeof(Material), false);

        if (!newEffect)
        {
            if (GUILayout.Button("Add Weapon Effect"))
            {
                newEffect = true;
            }
        }

        else
        {
            selectedEffect = EditorGUILayout.Popup("Effects", selectedEffect, weaponFactory.GetEffectNames(selectedEffects));

            if (GUILayout.Button("Confirm Effect"))
            {
                selectedEffects.Add(weaponFactory.GetEffectID(weaponFactory.GetEffectName(selectedEffect)), selectedEffect);
                newEffect = false;
            }
        }
    }

    void Confirm(bool confirm)
    {
        if (confirm)
        {
            Debug.Log("Yes");
            WeaponValues wv = new WeaponValues(weaponName, weaponMesh != null ? AssetDatabase.GetAssetPath(weaponMesh) : "", weaponMaterial != null ? AssetDatabase.GetAssetPath(weaponMaterial) : "");
            weaponData.Add(wv);
            //Debug.Log(weaponData[0][0]);
            //weaponData[0].Add(weaponName);
            //weaponData[0].Add(weaponMesh);
            //weaponData[0].Add(weaponMaterial);
            File.WriteAllText("Assets/StreamData/WeaponData.json", JsonMapper.ToJson(weaponData));
            weaponName = "";
            weaponMesh = null;
            weaponMaterial = null;
        }

        else
        {
            Debug.Log("Nope");
            weaponName = "";
            weaponMesh = null;
            weaponMaterial = null;
        }
    }
}

public struct WeaponValues
{
    public string weaponName;
    public string weaponMesh;
    public string weaponMaterial;

    public WeaponValues(string weaponName, string weaponMesh, string weaponMaterial)
    {
        this.weaponName = weaponName;
        this.weaponMesh = weaponMesh;
        this.weaponMaterial = weaponMaterial;
    }
}