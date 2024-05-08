using DG.Tweening;
using UnityEngine;

public static class RendererExtensions
{
    public static void ChangeAlpha(this SpriteRenderer sr, float alpha)
    {
        if (alpha >= 0f && alpha <= 1f)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    public static Tweener DoFade(this SpriteRenderer sr, float from, float to, float duration)
    {
        return DOVirtual.Float(from, to, duration, value =>
        {
            sr.ChangeAlpha(value);
        });
    }
}
