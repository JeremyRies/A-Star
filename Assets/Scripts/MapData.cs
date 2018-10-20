using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
     
    public int Width = 10;
    public int Height = 5;

     
    public TextAsset TextAsset;

     
    public Texture2D TextureMap;

     
    public string ResourcePath = "Mapdata";

     
    public Color32 OpenColor = Color.white;
    public Color32 BlockedColor = Color.black;
    public Color32 LightTerrainColor = new Color32(124, 194, 78, 255);
    public Color32 MediumTerrainColor = new Color32(252, 255, 52, 255);
    public Color32 HeavyTerrainColor = new Color32(255, 129, 12, 255);

     
    static readonly Dictionary<Color32, NodeType> TerrainLookupTable = new Dictionary<Color32, NodeType>();

    void Awake()
    {
         
        SetupLookupTable();
    }

    void Start()
    {
         
        string levelName = SceneManager.GetActiveScene().name;

        if (TextureMap == null)
        {
            TextureMap = Resources.Load(ResourcePath + "/" + levelName) as Texture2D;
        }

        if (TextAsset == null)
        {

            TextAsset = Resources.Load(ResourcePath + "/" + levelName) as TextAsset;
        }
    }
       
     
    public List<string> GetMapFromTextFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

         
        if (tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\r\n", "\n" };
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MAPDATA GetTextFromFile Error: invalid TextAsset");
        }
        return lines;
    }


     
    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(TextAsset);
    }

     
    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string newLine = "";

                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);

                    if (TerrainLookupTable.ContainsKey(pixelColor))
                    {
                        NodeType nodeType = TerrainLookupTable[pixelColor];
                        int nodeTypeNum = (int)nodeType;
                        newLine += nodeTypeNum;
                    }
                    else
                    {
                        newLine += '0';
                    }
                }
                lines.Add(newLine);
            }
        }

        return lines;
    }

     
    public void SetDimensions(List<string> textLines)
    {
        Height = textLines.Count;

        foreach (string line in textLines)
        {
            if (line.Length > Width)
            {
                Width = line.Length;
            }
        }
    }
     
    public int[,] MakeMap()
    {
        List<string> lines = new List<string>();

        if (TextureMap != null)
        {
            lines = GetMapFromTexture(TextureMap);
        }
        else
        {
            lines = GetMapFromTextFile(TextAsset);
        }

        SetDimensions(lines);

        int[,] map = new int[Width, Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }
        return map;

    }

     
    void SetupLookupTable()
    {
        TerrainLookupTable.Add(OpenColor, NodeType.Open);
        TerrainLookupTable.Add(BlockedColor, NodeType.Blocked);
        TerrainLookupTable.Add(LightTerrainColor, NodeType.LightTerrain);
        TerrainLookupTable.Add(MediumTerrainColor, NodeType.MediumTerrain);
        TerrainLookupTable.Add(HeavyTerrainColor, NodeType.HeavyTerrain);

    }

     
    public static Color GetColorFromNodeType(NodeType nodeType)
    {
         
        if (TerrainLookupTable.ContainsValue(nodeType))
        {
            Color colorKey = TerrainLookupTable.FirstOrDefault(x => x.Value == nodeType).Key;
            return colorKey;
        }

        return Color.white;
    }
}
