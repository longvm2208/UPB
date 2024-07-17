using System;
using UnityEngine;

public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
{
    public readonly DateTime OriginalTime = new DateTime(2024, 2, 29);

    [Header("ADS")]
    public float InterstitialAdCapping = 40f;
    public float RewardedAdCapping = 40f;
}
