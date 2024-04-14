using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable, InlineProperty]
public struct FloatRange
{
    [HorizontalGroup("row", LabelWidth = 23)]
    public float min;
    [HorizontalGroup("row", LabelWidth = 27)]
    public float max;

    public FloatRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public float GetRandom()
    {
        return Random.Range(min, max);
    }
}
