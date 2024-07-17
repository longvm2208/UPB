using MoreMountains.NiceVibrations;

public class VibrationManager : Singleton<VibrationManager>
{
    public void Initialize()
    {
        ToggleVibration();
    }

    public void ToggleVibration()
    {
        MMVibrationManager.SetHapticsActive(GameData.Instance.IsVibrationEnabled);
    }

    public void Vibrate(HapticTypes type = HapticTypes.MediumImpact)
    {
        if (GameData.Instance.IsVibrationEnabled)
        {
            MMVibrationManager.Haptic(type);
        }
    }
}
