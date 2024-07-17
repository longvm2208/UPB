using UnityEngine;

public static class AnimationCurveExtensions
{
    public static float GetLastKeyValue(this AnimationCurve curve)
    {
        return curve.keys[curve.keys.Length - 1].value;
    }

    public static void SetLastKeyValue(this AnimationCurve curve, float value)
    {
        Keyframe[] keys = curve.keys;
        keys[keys.Length - 1].value = value;
        curve.keys = keys;
    }
}
