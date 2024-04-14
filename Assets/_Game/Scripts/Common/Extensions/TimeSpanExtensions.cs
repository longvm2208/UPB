using System;
using UnityEngine;

public enum StringFormat
{
    [HideInInspector] None = -1,
    Colon = 0,
    Short = 1,
}

public enum TimeFormat
{
    [HideInInspector] None = -1,
    S = 0,
    MS = 1,
    HM = 2,
    HMS = 3,
    DH = 4,
    DHM = 5,
    DHMS = 6,
}

public static class TimeSpanExtensions
{
    public static string ToString(this TimeSpan timeSpan, StringFormat stringFormat, TimeFormat timeFormat)
    {
        switch (stringFormat)
        {
            case StringFormat.Colon:
                return timeSpan.ToStringColonFormat(timeFormat);
            case StringFormat.Short:
                return timeSpan.ToStringShortFormat(timeFormat);
            default:
                Debug.LogError("Invalid format");
                return null;
        }
    }

    private static string ToStringColonFormat(this TimeSpan timeSpan, TimeFormat format)
    {
        int hours = timeSpan.Days * 24 + timeSpan.Hours;
        int minutes = hours * 60 + timeSpan.Minutes;
        int seconds = minutes * 60 + timeSpan.Seconds;

        switch (format)
        {
            case TimeFormat.S:
                return string.Format(@"{0:00}", seconds);
            case TimeFormat.MS:
                return string.Format(@"{0:00}:{1:00}", minutes, timeSpan.Seconds);
            case TimeFormat.HM:
                return string.Format(@"{0:00}:{1:00}", hours, timeSpan.Minutes);
            case TimeFormat.HMS:
                return string.Format(@"{0:00}:{1:00}:{2:00}", hours, timeSpan.Minutes, timeSpan.Seconds);
            case TimeFormat.DH:
                return string.Format(@"{0:00}:{1:00}", timeSpan.Days, timeSpan.Hours);
            case TimeFormat.DHM:
                return string.Format(@"{0:00}:{1:00}:{2:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
            case TimeFormat.DHMS:
                return string.Format(@"{0:00}:{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            default:
                Debug.LogError("Invalid format");
                return null;
        }
    }

    private static string ToStringShortFormat(this TimeSpan timeSpan, TimeFormat format)
    {
        int hours = timeSpan.Days * 24 + timeSpan.Hours;
        int minutes = hours * 60 + timeSpan.Minutes;
        int seconds = minutes * 60 + timeSpan.Seconds;

        switch (format)
        {
            case TimeFormat.S:
                return string.Format(@"{0:0}s", seconds);
            case TimeFormat.MS:
                return string.Format(@"{0:0}m {1:0}s", minutes, timeSpan.Seconds);
            case TimeFormat.HM:
                return string.Format(@"{0:0}h {1:0}m", hours, timeSpan.Minutes);
            case TimeFormat.HMS:
                return string.Format(@"{0:0}h {1:0}m {2:0}s", hours, timeSpan.Minutes, timeSpan.Seconds);
            case TimeFormat.DH:
                return string.Format(@"{0:0}d {1:0}h", timeSpan.Days, timeSpan.Hours);
            case TimeFormat.DHM:
                return string.Format(@"{0:0}d {1:0}h {2:0}m", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
            case TimeFormat.DHMS:
                return string.Format(@"{0:0}d {1:0}h {2:0}m {3:0}s", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            default:
                Debug.LogError("Invalid format");
                return null;
        }
    }

    public static bool TryConvertToSeconds(this TimeSpan timeSpan, out int seconds)
    {
        if (!timeSpan.TotalSeconds.TryConvertToInt32(out seconds))
        {
            Debug.LogError("Convert TimeSpan to seconds (int32) failed");

            seconds = 0;
            return false;
        }

        return true;
    }
}
