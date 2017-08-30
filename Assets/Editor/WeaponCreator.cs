using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class WeaponCreator : EditorWindow
{
    bool newWeapon, newEffect;
    int selectedWeapon, selectedEffect, windowIndent, secondIndent, heightOffset, labelWidth, buttonWidth, writeFieldWidth, selectFieldWidth, weaponListWidth;
    string weaponName;
    Vector2 scrollPos;
    Mesh weaponMesh;
    Material weaponMaterial;
    WeaponFactory weaponFactory;
    List<int> selectedEffects;
    List<EffectValues> effectFields;
    List<WeaponValues> weaponData;

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
        selectedEffects = new List<int>();
        effectFields = new List<EffectValues>();

        windowIndent = 10;
        secondIndent = 320;
        heightOffset = 5;
        buttonWidth = 120;
        labelWidth = 80;
        writeFieldWidth = 260;
        selectFieldWidth = 280;
        weaponListWidth = 130;

        AssignFields();
    }

    void OnGUI()
    {
        WeaponList();
        WeaponActions();
        WeaponFields();
    }

    void WeaponList()
    {
        if (weaponData.Count > 0)
        {
            scrollPos = GUI.BeginScrollView(new Rect(secondIndent, windowIndent, weaponListWidth + 20, position.height - 20), scrollPos, new Rect(secondIndent, windowIndent, weaponListWidth, position.height - 20 > weaponData.Count * (heightOffset + EditorGUIUtility.singleLineHeight) ? position.height - 20 : weaponData.Count * (5 + EditorGUIUtility.singleLineHeight)));

            string[] weaponNames = new string[weaponData.Count];

            for (int i = 0; i < weaponData.Count; i++)
            {
                weaponNames[i] = weaponData[i].weaponName;
            }

            EditorGUI.BeginChangeCheck();
            GUI.SetNextControlName("Weapon Select");
            selectedWeapon = GUI.SelectionGrid(new Rect(secondIndent, windowIndent, weaponListWidth, weaponData.Count * (5 + EditorGUIUtility.singleLineHeight)), selectedWeapon, weaponNames, 1);

            if (EditorGUI.EndChangeCheck())
            {
                AssignFields();
                GUI.FocusControl("Weapon Select");
            }

            GUI.EndScrollView();
        }
    }

    void WeaponActions()
    {
        if (GUI.Button(new Rect(windowIndent, windowIndent, buttonWidth, EditorGUIUtility.singleLineHeight), "New Weapon"))
        {
            Confirmation.ShowWindow(Confirm);
            GUI.FocusControl("New Weapon");
            newWeapon = true;
        }

        if (GUI.Button(new Rect(windowIndent, windowIndent + (heightOffset + EditorGUIUtility.singleLineHeight), buttonWidth, EditorGUIUtility.singleLineHeight), "Save Weapon"))
        {
            SaveWeapon(newWeapon);
        }

        if (GUI.Button(new Rect(windowIndent, windowIndent + (2 * (heightOffset + EditorGUIUtility.singleLineHeight)), buttonWidth, EditorGUIUtility.singleLineHeight), "Delete Weapon"))
        {
            weaponData.RemoveAt(selectedWeapon);
            selectedEffects.Clear();
            effectFields.Clear();
            File.WriteAllText("Assets/StreamData/WeaponData.json", JsonMapper.ToJson(weaponData));

            if (weaponData.Count == 0)
            {
                ResetFields();
            }

            else
            {
                AssignFields();
            }
        }
    }

    void WeaponFields()
    {
        weaponName = EditorGUI.TextField(new Rect(windowIndent, windowIndent + (3 * (heightOffset + EditorGUIUtility.singleLineHeight)), writeFieldWidth, EditorGUIUtility.singleLineHeight), "Weapon Name", weaponName);
        weaponMesh = (Mesh)EditorGUI.ObjectField(new Rect(windowIndent, windowIndent + (4 * (heightOffset + EditorGUIUtility.singleLineHeight)), selectFieldWidth, EditorGUIUtility.singleLineHeight), "Weapon Mesh", weaponMesh, typeof(Mesh), false);
        weaponMaterial = (Material)EditorGUI.ObjectField(new Rect(windowIndent, windowIndent + (5 * (heightOffset + EditorGUIUtility.singleLineHeight)), selectFieldWidth, EditorGUIUtility.singleLineHeight), "Weapon Material", weaponMaterial, typeof(Material), false);

        if (!newEffect)
        {
            if (GUI.Button(new Rect(windowIndent, windowIndent + (7 * (heightOffset + EditorGUIUtility.singleLineHeight)), buttonWidth, EditorGUIUtility.singleLineHeight), "Add Weapon Effect"))
            {
                newEffect = true;
            }
        }

        else
        {
            selectedEffect = EditorGUI.Popup(new Rect(windowIndent, windowIndent + (6 * (heightOffset + EditorGUIUtility.singleLineHeight)), selectFieldWidth, EditorGUIUtility.singleLineHeight), "Effects", selectedEffect, weaponFactory.GetEffectNames(selectedEffects));

            if (GUI.Button(new Rect(windowIndent, windowIndent + (7 * (heightOffset + EditorGUIUtility.singleLineHeight)), buttonWidth, EditorGUIUtility.singleLineHeight), "Confirm Effect"))
            {
                selectedEffects.Add(weaponFactory.GetEffectID(weaponFactory.GetEffectName(selectedEffect)));
                effectFields.Add(InstalizeEffectFields(selectedEffects[selectedEffect]));
                newEffect = false;
            }
        }

        float effectHeight = windowIndent + (8 * (heightOffset + EditorGUIUtility.singleLineHeight));

        for (int i = 0; i < selectedEffects.Count; i++)
        {
            EditorGUI.LabelField(new Rect(windowIndent, effectHeight, labelWidth, EditorGUIUtility.singleLineHeight), weaponFactory.GetEffectName(selectedEffects[i]));
            effectHeight += heightOffset + EditorGUIUtility.singleLineHeight;

            switch (selectedEffects[i])
            {
                case 0:
                    effectFields[i].effectInts[0] = EditorGUI.IntField(new Rect(windowIndent, effectHeight, writeFieldWidth, EditorGUIUtility.singleLineHeight), "DoThat Value", effectFields[i].effectInts[0]);
                    effectHeight += heightOffset + EditorGUIUtility.singleLineHeight;

                    break;

                case 1:
                    effectFields[i].effectInts[0] = EditorGUI.IntField(new Rect(windowIndent, effectHeight, writeFieldWidth, EditorGUIUtility.singleLineHeight), "DoThis Value", effectFields[i].effectInts[0]);
                    effectHeight += heightOffset + EditorGUIUtility.singleLineHeight;
                
                    break;

                default:
                    Debug.LogException(new Exception("Effect not yet fully implemented. Harass Scott."));

                    break;
            }

            if (GUI.Button(new Rect(windowIndent, effectHeight, buttonWidth, EditorGUIUtility.singleLineHeight), "Remove Effect"))
            {
                selectedEffects.RemoveAt(i);
            }

            effectHeight += heightOffset + EditorGUIUtility.singleLineHeight;
        }
    }

    void AssignFields()
    {
        if (weaponData.Count > 0)
        {
            weaponName = weaponData[selectedWeapon].weaponName;
            weaponMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/" + weaponData[selectedWeapon].weaponMesh);
            weaponMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/" + weaponData[selectedWeapon].weaponMaterial);
            selectedEffects = new List<int>(weaponData[selectedWeapon].weaponEffects);
            effectFields = new List<EffectValues>(weaponData[selectedWeapon].effectValues);
        }
    }

    void ResetFields()
    {
        weaponName = "";
        weaponMesh = null;
        weaponMaterial = null;
        selectedEffects.Clear();
        effectFields.Clear();
    }

    void SaveWeapon(bool update)
    {
        WeaponValues wv = new WeaponValues(weaponName, weaponMesh != null ? AssetDatabase.GetAssetPath(weaponMesh).Replace("Assets/Resources/", "") : "", weaponMaterial != null ? AssetDatabase.GetAssetPath(weaponMaterial).Replace("Assets/Resources/", "") : "", selectedEffects, effectFields);

        if (newWeapon || weaponData.Count == 0)
        {
            weaponData.Add(wv);
            newWeapon = false;
        }

        else
        {
            weaponData[selectedWeapon] = wv;
        }

        File.WriteAllText("Assets/StreamData/WeaponData.json", JsonMapper.ToJson(weaponData));
    }

    void Confirm(bool confirm)
    {
        if (confirm)
        {
            Debug.Log("Yes");
            SaveWeapon(newWeapon);
            ResetFields();
        }

        else
        {
            Debug.Log("Nope");
            ResetFields();
        }
    }

    EffectValues InstalizeEffectFields(int id)
    {
        switch (id)
        {
            case 0:
                return new EffectValues(new int[1], new float[0]);

            case 1:
                return new EffectValues(new int[1], new float[0]);

            default:
                return new EffectValues();
        }
    }
}

public struct WeaponValues
{
    public string weaponName;
    public string weaponMesh;
    public string weaponMaterial;
    public int[] weaponEffects;
    public EffectValues[] effectValues;

    public WeaponValues(string weaponName, string weaponMesh, string weaponMaterial, List<int> weaponEffects, List<EffectValues> effectValues)
    {
        this.weaponName = weaponName;
        this.weaponMesh = weaponMesh;
        this.weaponMaterial = weaponMaterial;
        this.weaponEffects = weaponEffects.ToArray();
        this.effectValues = effectValues.ToArray();
    }
}

public struct EffectValues
{
    public int[] effectInts;
    public float[] effectFloats;

    public EffectValues(int[] effectInts, float[] effectFloats)
    {
        this.effectInts = effectInts;
        this.effectFloats = effectFloats;
    }
}