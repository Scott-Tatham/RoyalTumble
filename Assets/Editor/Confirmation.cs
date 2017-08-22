using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Confirmation : EditorWindow
{
    Rect window;

    static Action<bool> confirmResult;

    public static void ShowWindow(Action<bool> callback)
    {
        GetWindow<Confirmation>();
        confirmResult = callback;
    }

    void Awake()
    {
        window = new Rect((Screen.width / 2), (Screen.height / 2), 200, 100);
    }

    void OnGUI()
    {
        position = window;

        EditorGUI.LabelField(new Rect(30, 10, 140, 30), "Do you wish to save the\n current weapon's data?");

        if (GUI.Button(new Rect(30, 60, 50, 15), "Yes"))
        {
            confirmResult(true);
            Close();
        }

        if (GUI.Button(new Rect(120, 60, 50, 15), "No"))
        {
            confirmResult(false);
            Close();
        }
    }
}
