using System.IO;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public class TextureResizer
    {
        [MenuItem("Project/Texture Resizer/Trim Transparent Border")]
        public static void TrimTransparentBorder()
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

                SaveTexture(TrimTransparentBorder(texture), path);

                importer.isReadable = false;
                importer.SaveAndReimport();
            }
        }

        [MenuItem("Project/Texture Resizer/Add Transparent Border - Width And Height Divisible By 4")]
        public static void AddTransparentBorder()
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

                SaveTexture(AddTransparentBorder(texture), path);

                importer.isReadable = false;
                importer.SaveAndReimport();
            }
        }

        private static Texture2D TrimTransparentBorder(Texture2D originalTexture)
        {
            int left = originalTexture.GetTransparentBorderLeftSize();
            int right = originalTexture.GetTransparentBorderRightSize();
            int bottom = originalTexture.GetTransparentBorderBottomSize();
            int top = originalTexture.GetTransparentBorderTopSize();

            int width = originalTexture.width - left - right;
            int height = originalTexture.height - bottom - top;

            Texture2D trimmedTexture = new Texture2D(width, height);

            for (int x = left; x < originalTexture.width - right; x++)
            {
                for (int y = bottom; y < originalTexture.height - top; y++)
                {
                    Color color = originalTexture.GetPixel(x, y);
                    trimmedTexture.SetPixel(x - left, y - bottom, color);
                }
            }

            trimmedTexture.Apply();
            trimmedTexture.name = originalTexture.name;

            return trimmedTexture;
        }

        private static Texture2D AddTransparentBorder(Texture2D originalTexture)
        {
            int newWidth = (originalTexture.width + 3) / 4 * 4;
            int newHeight = (originalTexture.height + 3) / 4 * 4;

            Texture2D newTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);

            Color32[] transparentColors = new Color32[newWidth * newHeight];
            for (int i = 0; i < transparentColors.Length; i++)
            {
                transparentColors[i] = new Color32(0, 0, 0, 0);
            }

            newTexture.SetPixels32(transparentColors);

            int left = (newWidth - originalTexture.width) / 2;
            int bottom = (newHeight - originalTexture.height) / 2;
            newTexture.SetPixels(left, bottom, originalTexture.width, originalTexture.height, originalTexture.GetPixels());

            newTexture.Apply();
            newTexture.name = originalTexture.name;

            return newTexture;
        }

        private static void SaveTexture(Texture2D texture, string path)
        {
            File.WriteAllBytes(path, texture.EncodeToPNG());
        }
    }
}
