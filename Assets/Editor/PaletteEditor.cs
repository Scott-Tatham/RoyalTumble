using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#region DataTypes

public class ColourBand
{
    // Holds data for a section of colour within a row.
    bool marked;
    int start;
    Color colour;

    public bool GetMarked() { return marked; }
    public int GetStart() { return start; }
    public Color GetColour() { return colour; }

    public void SetMarked(bool marked) { this.marked = marked; }
    public void SetStart(int start) { this.start = start; }
    public void SetColour(Color colour) { this.colour = colour; }

    public ColourBand(int start, Color colour)
    {
        this.start = start;
        this.colour = colour;
    }
}

#endregion

public class NewPalette : EditorWindow
{
    // A separate window for creating a new palette.

    #region Variables

    static NewPalette instance;

    string palName;
    int dim;
    Rect newDim, ok;
    Texture2D newPalette;

    #endregion

    public static NewPalette GetInstance() { return instance; }

    public static void ShowEditor()
    {
        GetWindow(typeof(NewPalette), true, "Palette Editor");
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(this);
        }

        newDim = new Rect(16, 48, 192, 48);
        ok = new Rect(160, 96, 64, 16);
    }

    void OnGUI()
    {
        palName = EditorGUILayout.TextField("Name", palName);
        dim = EditorGUI.IntField(newDim, "Dimensions", dim);

        if (GUI.Button(ok, "OK"))
        {
            // Create new texture to be used.
            newPalette = new Texture2D(dim, 1, TextureFormat.ARGB32, false);
            byte[] palPng = newPalette.EncodeToPNG();
            string palPath = "Assets/Textures/" + palName + ".png";
            File.WriteAllBytes(palPath, palPng);
            AssetDatabase.Refresh();
            TextureImporter ti = (TextureImporter)AssetImporter.GetAtPath(palPath);
            ti.isReadable = true;
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Single;
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.filterMode = FilterMode.Point;
            ti.wrapMode = TextureWrapMode.Clamp;
            ti.mipmapEnabled = false;
            ti.alphaIsTransparency = true;
            ti.SaveAndReimport();
            newPalette = (Texture2D)AssetDatabase.LoadAssetAtPath(palPath, typeof(Texture2D));
            PaletteEditor.GetInstance().SetPalette(newPalette);
            Close();
        }

        RectResize();
    }
    
    // Keep window at set size. Scaleable UI is a pain.
    void RectResize()
    {
        if (position.width != 256)
        {
            position = new Rect(position.x, position.y, 256, position.height);
        }

        if (position.height != 128)
        {
            position = new Rect(position.x, position.y, position.width, 128);
        }
    }
}

public class PaletteEditor : EditorWindow
{
    // A window for editing a palette.
    #region Variables

    static PaletteEditor instance;

    Vector2 scroll;
    Rect texInput, newPal, palDis, palRowDis, greyIndex, addColBand, deleteBands, bandScroll, bandScrollView, markedCol, startColSlide, startDithSlide, palColSelect;
    List<ColourBand> colBands;
    Texture2D palette, current;

    #endregion

    public static PaletteEditor GetInstance() { return instance; }

    public void SetPalette(Texture2D palette) { this.palette = palette; }

    [MenuItem("Window/Palette Editor")]
    public static void ShowEditor()
    {
        GetWindow(typeof(PaletteEditor), true, "Palette Editor");
    }

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(this);
        }

        // Temporary palette used when none is present (Should not be used, create a new palette or import an existing palette.
        palette = new Texture2D(8, 1, TextureFormat.ARGB32, false);
        // Colours of texture.
        colBands = new List<ColourBand>();

        // Palettes should always be point to avoid bleeding.
        palette.filterMode = FilterMode.Point;
        palette.wrapMode = TextureWrapMode.Clamp;

        texInput = new Rect(24, 64, 80, 80);
        newPal = new Rect(texInput.x, texInput.y + 96, 80, 16);
        palDis = new Rect(128, 32, 256, 256);
        addColBand = new Rect(48, palDis.y + palDis.height + 32, 128, 16);
        deleteBands = new Rect(336, palDis.y + palDis.height + 32, 128, 16);
        bandScroll = new Rect(0, addColBand.y + 32, 512, 128);
        bandScrollView = new Rect(16, addColBand.y + 32, 496, 32);
        markedCol = new Rect(24, addColBand.y + 32, 8, 8);
        startColSlide = new Rect(48, addColBand.y + 32, 128, 16);
        palColSelect = new Rect(192, addColBand.y + 32, 64, 16);

        // SetTexture is called whenever a texture is needed to be updated.
        SetTexture();
    }

    void OnGUI()
    {
        RectResize();
        PaletteControls();
    }

    void OnDestroy()
    {
        // Here we write the changes to file when we set changes.
        SetTexture(true);
    }

    void PaletteControls()
    {
        // Select an existing palette.
        palette = EditorGUI.ObjectField(texInput, palette, typeof(Texture2D), false) as Texture2D;

        // Create new palette.
        if (GUI.Button(newPal, "New Palette"))
        {
            NewPalette.ShowEditor();
        }

        if (palette != current)
        {
            // Set temporary palette if palette is null.
            if (palette == null)
            {
                palette = new Texture2D(8, 1, TextureFormat.ARGB32, false);
            }

            current = palette;
            colBands = new List<ColourBand>();
            InitPalette();
        }

        if (palette != null)
        {
            // Add a new colour to row.
            if (GUI.Button(addColBand, "Add Colour Band"))
            {
                if (colBands.Count < palette.width)
                {
                    colBands.Add(new ColourBand(0, Color.white));
                    SetTexture();
                }
            }

            // Delete the selcected bands.
            if (GUI.Button(deleteBands, "Delete Selected"))
            {
                List<int> delete = new List<int>();

                for (int i = 0; i < colBands.Count; i++)
                {
                    if (colBands[i].GetMarked())
                    {
                        delete.Add(i);
                    }
                }

                delete.Reverse();

                for (int i = 0; i < delete.Count; i++)
                {
                    colBands.RemoveAt(delete[i]);
                }

                delete.Clear();
                SetTexture();
            }

            // Scroll panel for bands.
            bandScrollView.height = colBands.Count * 32;
            scroll = GUI.BeginScrollView(bandScroll, scroll, bandScrollView);

            // Draw bands of selected row.
            if (colBands != null)
            {
                // Colours.
                for (int i = 0; i < colBands.Count; i++)
                {
                    int startCol = colBands[i].GetStart();
                    Color colourCol = colBands[i].GetColour();

                    markedCol.y = (addColBand.y + 32) + (i * 32);
                    startColSlide.y = markedCol.y;
                    palColSelect.y = markedCol.y;
                    colBands[i].SetMarked(EditorGUI.Toggle(markedCol, colBands[i].GetMarked()));
                    colBands[i].SetStart(EditorGUI.IntSlider(startColSlide, colBands[i].GetStart(), 0, palette.width - 1));
                    colBands[i].SetColour(EditorGUI.ColorField(palColSelect, GUIContent.none, colBands[i].GetColour(), true, true, false, null));

                    if (startCol != colBands[i].GetStart() || colourCol != colBands[i].GetColour())
                    {
                        SetTexture();
                    }
                }
            }

            GUI.EndScrollView();
        }

        else
        {
            palette = new Texture2D(8, 1, TextureFormat.ARGB32, false);
        }

        EditorGUI.DrawTextureTransparent(palDis, palette);
        Undo.RecordObject(palette, "Palette");
        Repaint();
    }

    // Keep window at set size. Scaleable UI is a pain.
    void RectResize()
    {
        if (position.width != 512)
        {
            position = new Rect(position.x, position.y, 512, position.height);
        }

        if (position.height != 544)
        {
            position = new Rect(position.x, position.y, position.width, 544);
        }
    }

    // Initialize palette.
    void InitPalette()
    {
        Color[] palPixels = palette.GetPixels(0, 0, palette.width, 1);

        if (colBands == null)
        {
            colBands = new List<ColourBand>();
        }

        colBands.Add(new ColourBand(0, palPixels[0]));

        for (int j = 1; j < palette.width; j++)
        {
            if (palPixels[j - 1] != palPixels[j] && j != palette.width)
            {
                if (j < palette.width)
                {
                    colBands.Add(new ColourBand(j, palPixels[j]));
                }
            }
        }

        SetTexture();
    }

    // Sorting function for getting colour bands in order.
    List<ColourBand> SortBands()
    {
        List<ColourBand> cb = new List<ColourBand>();

        if (colBands != null)
        {
            cb.AddRange(colBands);

            for (int i = 0; i < cb.Count; i++)
            {
                for (int j = i + 1; j < cb.Count; j++)
                {
                    ColourBand tempBand;

                    if (cb[i].GetStart() > cb[j].GetStart())
                    {
                        tempBand = cb[i];
                        cb[i] = cb[j];
                        cb[j] = tempBand;
                    }
                }
            }
        }

        return cb;
    }

    void SetTexture(bool write = false)
    {
        // Sets palette changes.
        if (colBands != null)
        {
            Debug.Log("Ok");
            Color[] colours = new Color[palette.width];
            List<ColourBand> cb = SortBands();

            for (int j = 0; j < cb.Count; j++)
            {
                int count = j + 1 == cb.Count ? palette.width - cb[j].GetStart() : cb[j + 1].GetStart() - cb[j].GetStart();

                for (int k = 0; k < count; k++)
                {
                    colours[cb[j].GetStart() + k] = cb[j].GetColour();
                }
            }

            palette.SetPixels(0, 0, palette.width, 1, colours);
        }

        palette.Apply();
        EditorUtility.SetDirty(palette);

        if (write)
        {
            byte[] palPng = palette.EncodeToPNG();
            File.WriteAllBytes("Assets/Textures/" + palette.name + ".png", palPng);
            AssetDatabase.Refresh();
        }

        Repaint();
    }
}