using NoiseLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalManager : MonoBehaviour
{
    [SerializeField]
    int noiseTexCount, octaves;
    [SerializeField]
    float persistence, frequency, baseAmplitude;
    [SerializeField]
    Material m;

    int index;
    Texture2DArray noiseTexureArray;

    void Start()
    {
        noiseTexureArray = new Texture2DArray(256, 256, (noiseTexCount * 2), TextureFormat.ARGB32, false);
        Texture2D[] noiseTextures = new Texture2D[noiseTexCount * 2];

        for (int i = 0; i < noiseTexCount; i++)
        {
            Texture2D newNoiseTex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            Color[] noiseValues = new Color[256 * 256];
            int y = 0;

            for (int x = 0; x < newNoiseTex.width * newNoiseTex.height; x++)
            {
                if (x % 256 == 0)
                {
                    y++;
                }

                float value = Noise.OctaveSimplex3D(octaves, persistence, frequency, baseAmplitude, new Vector3(x, y, i));
                noiseValues[x] = new Color(value, value, value, value);
            }

            newNoiseTex.SetPixels(noiseValues);
            newNoiseTex.Apply();
            noiseTextures[i] = newNoiseTex;
        }

        for (int i = 0; i < noiseTexCount; i++)
        {
            noiseTextures[noiseTexCount + i] = noiseTextures[noiseTexCount - (i + 1)];
            
        }
        
        for (int i = 0; i < (noiseTexCount * 2); i++)
        {
            noiseTexureArray.SetPixels(noiseTextures[i].GetPixels(), i);
        }

        noiseTexureArray.Apply();
        m.SetTexture("_TexArr", noiseTexureArray);
    }

    void Update()
    {
        OverlayCycle();
    }

    void OverlayCycle()
    {
        index = index == noiseTexCount * 2 ? 0 : index + 1;
        m.SetInt("_Index", index);
    }
}