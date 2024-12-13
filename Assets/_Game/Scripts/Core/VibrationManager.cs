using MoreMountains.NiceVibrations;

public static class VibrationManager
{
    public static void Initialize()
    {
        ToggleVibration();
    }

    public static void ToggleVibration()
    {
        MMVibrationManager.SetHapticsActive(GameData.Instance.IsVibrationEnabled);
    }

    public static void Vibrate(HapticTypes type = HapticTypes.MediumImpact)
    {
        if (GameData.Instance.IsVibrationEnabled)
        {
            MMVibrationManager.Haptic(type);
        }
    }
}
