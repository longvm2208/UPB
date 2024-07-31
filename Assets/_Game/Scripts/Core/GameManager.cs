using System;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] bool multiTouchEnabled = true;
    [SerializeField] int targetFps = 60;

    [SerializeField, ExposedScriptableObject]
    GameSettings gameSettings;
    public GameSettings GameSettings => gameSettings;

    DateTime startupTime;

    public DateTime Now => startupTime + TimeSpan.FromSeconds(Time.realtimeSinceStartup);

    void Awake()
    {
        Application.targetFrameRate = targetFps;
        Input.multiTouchEnabled = multiTouchEnabled;

        startupTime = DateTime.Now;
    }

    public void SetStartupTime(DateTime startupTime)
    {
        this.startupTime = startupTime;
    }
}
