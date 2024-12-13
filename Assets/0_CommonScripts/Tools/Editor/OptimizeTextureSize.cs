using System.IO;
using UnityEditor;
using UnityEngine;

public class OptimizeTextureSize
{
    [MenuItem("Tools/My Tools/Optimize Texture Size/Remove Transparent Borders", false)]
    public static void RemoveTransparentBorders()
    {
        foreach (var obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            Texture2D texture = obj as Texture2D;

            if (texture == null)
            {
                Debug.LogError($"This is not a Texture2D: {obj.name}");
                continue;
            }

            importer.isReadable = true;
            importer.SaveAndReimport();

            SaveTexture(RemoveTransparentBorders(texture), path);

            importer.isReadable = false;
            importer.SaveAndReimport();
        }
    }

    [MenuItem("Tools/My Tools/Optimize Texture Size/Add Transparent Borders - Width And Height Being Multiple Of 4", false)]
    public static void AddTransparentBorders()
    {
        foreach (var obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            Texture2D texture = obj as Texture2D;

            if (texture == null)
            {
                Debug.LogError($"This is not a Texture2D: {obj.name}");
                continue;
            }

            importer.isReadable = true;
            importer.SaveAndReimport();

            SaveTexture(AddTransparentBorders(texture), path);

            importer.isReadable = false;
            importer.SaveAndReimport();
        }
    }

    [MenuItem("Tools/My Tools/Optimize Texture Size/Remove Transparent Borders", true)]
    [MenuItem("Tools/My Tools/Optimize Texture Size/Add Transparent Borders - Width And Height Being Multiple Of 4", true)]
    public static bool RemoveTransparentBordersValidation()
    {
        foreach (var obj in Selection.objects)
        {
            Texture2D texture = obj as Texture2D;
            if (texture == null) return true;
        }

        return false;
    }

    static Texture2D RemoveTransparentBorders(Texture2D texture)
    {
        int left = GetTransparentBorderWidthLeft(texture);
        int right = GetTransparentBorderWidthRight(texture);
        int bottom = GetTransparentBorderHeightBottom(texture);
        int top = GetTransparentBorderHeightTop(texture);

        int width = texture.width - left - right;
        int height = texture.height - bottom - top;

        Texture2D newTexture = new Texture2D(width, height);

        for (int x = left; x < texture.width - right; x++)
        {
            for (int y = bottom; y < texture.height - top; y++)
            {
                Color color = texture.GetPixel(x, y);
                newTexture.SetPixel(x - left, y - bottom, color);
            }
        }

        newTexture.Apply();
        newTexture.name = texture.name;

        return newTexture;
    }

    static Texture2D AddTransparentBorders(Texture2D texture)
    {
        int width = (texture.width + 3) / 4 * 4;
        int height = (texture.height + 3) / 4 * 4;

        Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        Color32[] transparentColors = new Color32[width * height];
        for (int i = 0; i < transparentColors.Length; i++)
        {
            transparentColors[i] = new Color32(0, 0, 0, 0);
        }

        newTexture.SetPixels32(transparentColors);

        int left = (width - texture.width) / 2;
        int bottom = (height - texture.height) / 2;
        newTexture.SetPixels(left, bottom, texture.width, texture.height, texture.GetPixels());

        newTexture.Apply();
        newTexture.name = texture.name;

        return newTexture;
    }

    static void SaveTexture(Texture2D texture, string path)
    {
        File.WriteAllBytes(path, texture.EncodeToPNG());
    }

    static int GetTransparentBorderWidthLeft(Texture2D texture)
    {
        bool broke = false;
        int width = 0;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    broke = true;
                    break;
                }
            }

            if (broke) break;

            width++;
        }

        return width;
    }

    static int GetTransparentBorderWidthRight(Texture2D texture)
    {
        bool broke = false;
        int width = 0;

        for (int x = texture.width - 1; x >= 0; x--)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    broke = true;
                    break;
                }
            }

            if (broke) break;

            width++;
        }

        return width;
    }

    static int GetTransparentBorderHeightBottom(Texture2D texture)
    {
        bool broke = false;
        int height = 0;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    broke = true;
                    break;
                }
            }

            if (broke) break;

            height++;
        }

        return height;
    }

    static int GetTransparentBorderHeightTop(Texture2D texture)
    {
        bool broke = false;
        int height = 0;

        for (int y = texture.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    broke = true;
                    break;
                }
            }

            if (broke) break;

            height++;
        }

        return height;
    }
}
