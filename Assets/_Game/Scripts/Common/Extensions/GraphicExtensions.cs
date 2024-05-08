using UnityEngine;
using UnityEngine.UI;

public static class GraphicExtensions
{
    public static void ChangeAlpha(this Graphic graphic, float alpha)
    {
        if (alpha >= 0f && alpha <= 1f)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
}
