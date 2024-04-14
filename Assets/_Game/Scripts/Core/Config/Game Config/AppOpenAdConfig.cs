using System;
using UnityEngine;

[Serializable]
public class AppOpenAdConfig
{
    public bool IsShowOnSwitch;
    [Tooltip("0: Disable\n1: First app open\n2: Every app open")]
    public int ShowOnOpen;

    public AppOpenAdConfig(bool isShowOnSwitch, int showOnOpen)
    {
        IsShowOnSwitch = isShowOnSwitch;
        ShowOnOpen = showOnOpen;
    }
}
