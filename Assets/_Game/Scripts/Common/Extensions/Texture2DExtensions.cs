using UnityEngine;

public static class Texture2DExtensions
{
    public static int GetTransparentBorderLeftSize(this Texture2D texture)
    {
        bool isBreak = false;
        int size = 0;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    isBreak = true;
                    break;
                }
            }

            if (isBreak) break;

            size++;
        }

        return size;
    }

    public static int GetTransparentBorderRightSize(this Texture2D texture)
    {
        bool isBreak = false;
        int size = 0;

        for (int x = texture.width - 1; x >= 0; x--)
        {
            for (int y = 0; y < texture.height; y++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    isBreak = true;
                    break;
                }
            }

            if (isBreak) break;

            size++;
        }

        return size;
    }

    public static int GetTransparentBorderBottomSize(this Texture2D texture)
    {
        bool isBreak = false;
        int size = 0;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    isBreak = true;
                    break;
                }
            }

            if (isBreak) break;

            size++;
        }

        return size;
    }

    public static int GetTransparentBorderTopSize(this Texture2D texture)
    {
        bool isBreak = false;
        int size = 0;

        for (int y = texture.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y).a > 0f)
                {
                    isBreak = true;
                    break;
                }
            }

            if (isBreak) break;

            size++;
        }

        return size;
    }
}
