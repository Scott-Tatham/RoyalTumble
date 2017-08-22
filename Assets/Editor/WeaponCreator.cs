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
    JsonData weaponData;
    WeaponFactory weaponFactory;
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
            weaponData = File.ReadAllText("Assets/StreamData/WeaponData.json");
        }

        else
        {
            File.Create("Assets/StreamData/WeaponData.json");
            weaponData = File.ReadAllText("Assets/StreamData/WeaponData.json");
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
            weaponData[selectedWeapon].Clear();
        }
        
        weaponName = EditorGUILayout.TextField("Weapon Name", weaponName);
        weaponMesh = (Mesh)EditorGUILayout.ObjectField("Weapon Mesh", weaponMesh, typeof(Mesh), false);
        weaponMaterial = (Material)EditorGUILayout.ObjectField("Weapon Material", weaponMaterial, typeof(Material), false);
        //AssetDatabase.GetAssetPath(weaponMaterial);

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
            //File.WriteAllLines("Assets/StreamData/WeaponData.json", );
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