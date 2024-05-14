using System;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private const int TargetFps = 60;

    private DateTime startupTime;

    public DateTime Now => startupTime + TimeSpan.FromSeconds(Time.realtimeSinceStartup);

    private void Awake()
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
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }
    #endregion
}
