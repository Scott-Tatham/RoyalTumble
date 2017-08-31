using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum AlphaOperator
{
    OFF,
    FLOOR,
    ROUND,
    CEIL
}

public class CelEditor : ShaderGUI
{
    bool pointLight;
    AlphaOperator alpha;
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
            if (m.IsKeywordEnabled("_FLOOR_ALPHA"))
            {
                alpha = AlphaOperator.FLOOR;
            }
            
            else if (m.IsKeywordEnabled("_ROUND_ALPHA"))
            {
                alpha = AlphaOperator.ROUND;
            }

            else if (m.IsKeywordEnabled("_CEIL_ALPHA"))
            {
                alpha = AlphaOperator.CEIL;
            }

            else
            {
                alpha = AlphaOperator.OFF;
            }
            
            pointLight = m.IsKeywordEnabled("_POINT_LIGHT");
        }
    }

    void SetKeyWords()
    {
        foreach (Material m in materialEditor.targets)
        {
            switch (alpha)
            {
                case AlphaOperator.OFF:
                    m.DisableKeyword("_FLOOR_ALPHA");
                    m.DisableKeyword("_ROUND_ALPHA");
                    m.DisableKeyword("_CEIL_ALPHA");

                    break;

                case AlphaOperator.FLOOR:
                    m.EnableKeyword("_FLOOR_ALPHA");
                    m.DisableKeyword("_ROUND_ALPHA");
                    m.DisableKeyword("_CEIL_ALPHA");

                    break;

                case AlphaOperator.ROUND:
                    m.EnableKeyword("_ROUND_ALPHA");
                    m.DisableKeyword("_FLOOR_ALPHA");
                    m.DisableKeyword("_CEIL_ALPHA");

                    break;

                case AlphaOperator.CEIL:
                    m.EnableKeyword("_CEIL_ALPHA");
                    m.DisableKeyword("_FLOOR_ALPHA");
                    m.DisableKeyword("_ROUND_ALPHA");

                    break;
            }

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
        alpha = (AlphaOperator)EditorGUILayout.EnumPopup("Alpha Operator", alpha);
        pointLight = EditorGUILayout.Toggle("Recieve PointLights", pointLight);
        materialEditor.PropertiesDefaultGUI(properties);

        if (EditorGUI.EndChangeCheck())
        {
            SetKeyWords();
        }

    }
}
