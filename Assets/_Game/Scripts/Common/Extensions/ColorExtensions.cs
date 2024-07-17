using UnityEngine;

public static class ColorExtensions
{
    public static Color Blacken(this Color color, float intensity = 0.5f)
    {
        intensity = Mathf.Clamp01(intensity);
        Color black = Color.black;
        black.a = color.a;
        return Color.Lerp(color, black, intensity);
    }
}
