using System.Collections.Generic;
using UnityEngine;

public class WaitForUtils
{
    class FloatComparer : IEqualityComparer<float>
    {
        public bool Equals(float x, float y) => Mathf.Abs(x - y) <= Mathf.Epsilon;
        public int GetHashCode(float obj) => obj.GetHashCode();
    }

    static readonly WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame EndOfFrame => endOfFrame;

    static readonly Dictionary<float, WaitForSeconds> waitForSecondsDictionary = new(100, new FloatComparer());

    public static WaitForSeconds Seconds(float seconds)
    {
        if (seconds < 1f / Application.targetFrameRate) return null;

        if (!waitForSecondsDictionary.TryGetValue(seconds, out var wait))
        {
            wait = new WaitForSeconds(seconds);
            waitForSecondsDictionary.Add(seconds, wait);
        }

        return wait;
    }
}
