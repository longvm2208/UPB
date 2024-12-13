using UnityEngine;

public static class RendererExt
{
    public static void ChangeAlpha(this SpriteRenderer renderer, float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }
}
