using Firebase.Analytics;

public struct DebugParameter
{
    public string Name;
    public string Value;

    public DebugParameter(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public DebugParameter(string name, long value)
    {
        Name = name;
        Value = value.ToString();
    }

    public DebugParameter(string name, double value)
    {
        Name = name;
        Value = value.ToString();
    }
}
