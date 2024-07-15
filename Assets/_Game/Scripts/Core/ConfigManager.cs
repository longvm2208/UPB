using System;
using UnityEngine;

public class ConfigManager : SingletonMonoBehaviour<ConfigManager>
{
    public readonly DateTime OriginalTime = new DateTime(2024, 2, 29);

    [Header("TEST~PRODUCT")]
    public bool IsInternetTime = true;
    public bool IsEnableAds = true;

    [Header("ADS")]
    public float InterstitialAdCapping = 40f;
    public float RewardedAdCapping = 40f;
}
