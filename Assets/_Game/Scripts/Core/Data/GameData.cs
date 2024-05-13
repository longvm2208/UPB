using System;
using UnityEngine;

[Serializable]
public class GameData
{
    [Header("AUDIO & VIBRATION")]
    public bool IsSoundEnabled;
    public bool IsMusicEnabled;
    public bool IsVibrationEnabled;

    public GameData()
    {
        // AUDIO & VIBRATION
        IsSoundEnabled = true;
        IsMusicEnabled = true;
        IsVibrationEnabled = true;
    }
}
