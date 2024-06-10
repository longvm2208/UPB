public class TimeModifier : AmountModifier
{
    public override string Modify(int seconds)
    {
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds / 60;
        seconds -= minutes * 60;

        string time = "";

        if (hours > 0) time += $"{hours}h";
        if (minutes > 0) time += $"{minutes}m";
        if (seconds > 0) time += $"{seconds}s";

        return string.Format(format, time);
    }
}
