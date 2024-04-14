using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable, InlineProperty]
public struct IntRange
{
    [HorizontalGroup("row", LabelWidth = 23)]
    public int min;
    [HorizontalGroup("row", LabelWidth = 27)]
    public int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int GetRandom()
    {
        return Random.Range(min, max + 1);
    }
}
