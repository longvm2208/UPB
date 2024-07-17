using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
    public struct CachedEvent
    {
        public string name;
        public Parameter[] parameters;

        public CachedEvent(string name)
        {
            this.name = name;
            parameters = null;
        }

        public CachedEvent(string name, Parameter[] parameters)
        {
            this.name = name;
            this.parameters = parameters;
        }
    }

    private bool isReady;
    private Queue<CachedEvent> cachedEvents = new Queue<CachedEvent>();

    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized successfully");

                isReady = true;
                FetchDataAsync();
                LogCachedEvents();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    #region REMOTE CONFIG
    public Task FetchDataAsync()
    {
        Debug.Log("Fetch remote config data");

        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;

        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
            ApplyRemoteConfigData();
        });
    }

    private int GetInt(string key)
    {
        return (int)FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
    }

    private bool GetBool(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
    }

    private void ApplyRemoteConfigData()
    {

    }
    #endregion

    #region USER PROPERTIES
    private void SetUserProperty(string property, string value)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        FirebaseAnalytics.SetUserProperty(property, value);
    }
    #endregion

    #region LOG EVENT
    private void LogEvent(string name)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        if (!isReady)
        {
            cachedEvents.Enqueue(new CachedEvent(name));
        }
        else
        {
            FirebaseAnalytics.LogEvent(name);
        }
    }

    private void LogEvent(string name, params Parameter[] parameters)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        if (!isReady)
        {
            cachedEvents.Enqueue(new CachedEvent(name, parameters));
        }
        else
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }

    public void LogEvent(string name, params DebugParameter[] consoleParameters)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("[Firebase] [{0}] ", name);

        for (int i = 0; i < consoleParameters.Length; i++)
        {
            builder.AppendFormat("[{0}-{1}] ", consoleParameters[i].Name, consoleParameters[i].Value);
        }

        LogUtils.Log(Color.green, builder.ToString());
    }

    #region CACHED EVENT
    private void LogCachedEvents()
    {
        if (cachedEvents.Count > 0)
        {
            for (int i = 0; i < cachedEvents.Count; i++)
            {
                LogCachedEvent(cachedEvents.Dequeue());
            }
        }
    }

    private void LogCachedEvent(CachedEvent cachedEvent)
    {
        if (cachedEvent.parameters != null)
        {
            LogEvent(cachedEvent.name, cachedEvent.parameters);
        }
        else
        {
            LogEvent(cachedEvent.name);
        }
    }
    #endregion

    #region AD EVENT
    public void LogInterstitialAdLoad(string placement)
    {
        LogEvent("ad_inter_load", new Parameter("placement", placement));
    }

    public void LogInterstitialAdFailed(string placement)
    {
        LogEvent("ad_inter_fail", new Parameter("placement", placement));
    }

    public void LogInterstitialAdShow(string placement)
    {
        LogEvent("ad_inter_show", new Parameter("placement", placement));
    }

    public void LogInterstitialAdClick(string placement)
    {
        LogEvent("ad_inter_click", new Parameter("placement", placement));
    }

    public void LogRewardedAdOffer(string placement, string buttonName)
    {
        LogEvent("ads_reward_offer",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public void LogRewardedAdFailed(string placement, string buttonName)
    {
        LogEvent("ads_reward_fail",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public void LogRewardedAdShow(string placement, string buttonName)
    {
        LogEvent("ads_reward_show",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public void LogRewardedAdClick(string placement, string buttonName)
    {
        LogEvent("ads_reward_click",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public void LogRewardedAdComplete(string placement, string buttonName)
    {
        LogEvent("ads_reward_complete",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public void LogAdRevenue(ImpressionData data)
    {
        LogEvent("ad_impression", data.ToParameters());
    }
    #endregion

    #region IAP EVENT
    public void LogIapPurchase(string packId, string location)
    {
        LogEvent("iapPurchased_confirmed",
            new Parameter("pack_ID", packId),
            new Parameter("location", location));
    }
    #endregion
    #endregion
}
