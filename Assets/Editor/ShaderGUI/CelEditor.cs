using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CelEditor : ShaderGUI
{
    bool pointLight;
    MaterialEditor materialEditor;
    MaterialProperty[] properties;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        this.materialEditor = materialEditor;
        this.properties = properties;
        SetCelKeywords();
        Properties();
    }

    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
        base.AssignNewShaderToMaterial(material, oldShader, newShader);

        if (oldShader.name != "Cel")
        {

        }
    }

    MaterialProperty FindProperty(string propertyName)
    {
        return FindProperty(propertyName, properties);
    }

    void SetCelKeywords()
    {
        foreach (Material m in materialEditor.targets)
        {
            pointLight = m.IsKeywordEnabled("_POINT_LIGHT");
        }
    }

    void SetKeyWords()
    {
        foreach (Material m in materialEditor.targets)
        {
            if (pointLight)
            {
                m.EnableKeyword("_POINT_LIGHT");
            }

            else
            {
                m.DisableKeyword("_POINT_LIGHT");
            }
        }
    }

    void Properties()
    {
        EditorGUI.BeginChangeCheck();
        pointLight = EditorGUILayout.Toggle("Recieve PointLights", pointLight);
        materialEditor.PropertiesDefaultGUI(properties);

        if (EditorGUI.EndChangeCheck())
        {
            SetKeyWords();
        }
    }
}
