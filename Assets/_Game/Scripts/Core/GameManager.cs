using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    const int TargetFps = 60;

#if UNITY_EDITOR
    [SerializeField] bool isInternetAvailable = true;
#endif
    [SerializeField, ExposedScriptableObject]
    GameSettings gameSettings;

    DateTime startupTime;

    public DateTime Now => startupTime + TimeSpan.FromSeconds(Time.realtimeSinceStartup);
    public GameSettings GameSettings => gameSettings;

    void Awake()
    {
        Application.targetFrameRate = TargetFps;
        startupTime = DateTime.Now;
    }

    public void SetStartupTime(DateTime startupTime)
    {
        this.startupTime = startupTime;
    }

    #region INTERNET
    public bool IsInternetAvailable()
    {
#if UNITY_EDITOR
        return isInternetAvailable;
#else
        return !(Application.internetReachability == NetworkReachability.NotReachable);
#endif
    }
    #endregion
}
