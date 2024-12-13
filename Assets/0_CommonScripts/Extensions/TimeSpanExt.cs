using System;
using System.Text;

public static class TimeSpanExt
{
    public static string SecondsToString_HMS(this int seconds)
    {
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds / 60;
        seconds -= minutes * 60;
        StringBuilder builder = new StringBuilder();
        if (hours > 0) builder.Append($"{hours}h");
        if (minutes > 0) builder.Append($"{minutes}m");
        if (seconds > 0) builder.Append($"{seconds}s");
        return builder.ToString();
    }

    public static string SecondsToString_DH_HMS(this int seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);

        if (seconds < 60)
        {
            return seconds.ToString("00") + "s";
        }
        else if (seconds < 3600)
        {
            return string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        }
        else if (seconds < 3600 * 24)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
        }
        else
        {
            return string.Format("{0}d {1}h", ts.Days, ts.Hours);
        }
    }
}
