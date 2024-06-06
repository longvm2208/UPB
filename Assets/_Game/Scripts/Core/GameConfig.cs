using System;
using UnityEngine;

[Serializable]
public class GameConfig
{
    public static readonly DateTime OriginalTime = new DateTime(2024, 2, 29);

    [Header("TEST~PRODUCT")]
    public bool isInternetTime = true;
    public bool isEnableAds = true;
}
