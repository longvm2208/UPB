using MoreMountains.NiceVibrations;

public class VibrationManager : SingletonMonoBehaviour<VibrationManager>
{
    private GameData gameData => DataManager.Instance.GameData;

    public void Initialize()
    {
        ToggleVibration();
    }

    public void ToggleVibration()
    {
        MMVibrationManager.SetHapticsActive(gameData.IsVibrationEnabled);
    }

    public void Vibrate(HapticTypes type = HapticTypes.MediumImpact)
    {
        if (gameData.IsVibrationEnabled)
        {
            MMVibrationManager.Haptic(type);
        }
    }
}
