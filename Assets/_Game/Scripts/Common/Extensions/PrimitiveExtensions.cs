using System;
using UnityEngine;

public static class PrimitiveExtensions
{
    #region INT
    /// <summary>
    /// Return true if value in range (min..max)
    /// </summary>
    public static bool InRange0(this int value, int min, int max)
    {
        return value > min && value < max;
    }

    /// <summary>
    /// Return true if value in range [min..max)
    /// </summary>
    public static bool InRange1(this int value, int min, int max)
    {
        return value >= min && value < max;
    }

    /// <summary>
    /// Return true if value in range (min..max]
    /// </summary>
    public static bool InRange2(this int value, int min, int max)
    {
        return value > min && value <= max;
    }

    /// <summary>
    /// Return true if value in range [min..max]
    /// </summary>
    public static bool InRange3(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
    #endregion

    #region FLOAT
    /// <summary>
    /// Return true if value in range (min..max)
    /// </summary>
    public static bool InRange0(this float value, float min, float max)
    {
        return value > min && value < max;
    }

    /// <summary>
    /// Return true if value in range [min..max)
    /// </summary>
    public static bool InRange1(this float value, float min, float max)
    {
        return value >= min && value < max;
    }

    /// <summary>
    /// Return true if value in range (min..max]
    /// </summary>
    public static bool InRange2(this float value, float min, float max)
    {
        return value > min && value <= max;
    }

    /// <summary>
    /// Return true if value in range [min..max]
    /// </summary>
    public static bool InRange3(this float value, float min, float max)
    {
        return value >= min && value <= max;
    }
    #endregion

    #region DOUBLE
    public static bool TryConvertToInt32(this double d, out int i)
    {
        if (d <= int.MinValue || d >= int.MaxValue)
        {
            Debug.LogError("Convert double to int32 failed");

            i = 0;
            return false;
        }

        i = Convert.ToInt32(d);
        return true;
    }
    #endregion
}
