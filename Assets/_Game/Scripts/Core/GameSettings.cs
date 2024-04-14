using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Settings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
    public bool IsEnableAds = true;
}