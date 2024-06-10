using UnityEngine;

public class LogUtils
{
    public static void Log(Color color, string log)
    {
        Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>" +
            "",
            (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f), log));
    }
}
