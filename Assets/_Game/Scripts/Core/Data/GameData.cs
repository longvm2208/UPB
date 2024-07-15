using System;
using UnityEngine;

[Serializable]
public class GameData
{
    [Header("AUDIO & VIBRATION")]
    public bool IsSoundEnabled;
    public bool IsMusicEnabled;
    public bool IsVibrationEnabled;

    [Header("IAP")]
    public bool IsRemoveAds;

    public GameData()
    {
        // AUDIO & VIBRATION
        IsSoundEnabled = true;
        IsMusicEnabled = true;
        IsVibrationEnabled = true;

        // IAP
        IsRemoveAds = false;
    }
}
