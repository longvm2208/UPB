using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class FirebaseManager
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

    private static bool isReady;
    private static Queue<CachedEvent> cachedEvents = new Queue<CachedEvent>();

    public static void Initialize()
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
    public static Task FetchDataAsync()
    {
        Debug.Log("Fetch remote config data");

        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private static void FetchComplete(Task fetchTask)
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

    private static int GetInt(string key)
    {
        return (int)FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
    }

    private static bool GetBool(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
    }

    private static void ApplyRemoteConfigData()
    {

    }
    #endregion

    #region USER PROPERTIES
    private static void SetUserProperty(string property, string value)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        FirebaseAnalytics.SetUserProperty(property, value);
    }
    #endregion

    #region LOG EVENT
    private static void LogEvent(string name)
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

    private static void LogEvent(string name, params Parameter[] parameters)
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

    public static void LogEvent(string name, params DebugParameter[] consoleParameters)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("[Firebase] [{0}] ", name);

        for (int i = 0; i < consoleParameters.Length; i++)
        {
            builder.AppendFormat("[{0}-{1}] ", consoleParameters[i].Name, consoleParameters[i].Value);
        }

        DebugLog.Log(Color.green, builder.ToString());
    }

    #region CACHED EVENT
    private static void LogCachedEvents()
    {
        if (cachedEvents.Count > 0)
        {
            for (int i = 0; i < cachedEvents.Count; i++)
            {
                LogCachedEvent(cachedEvents.Dequeue());
            }
        }
    }

    private static void LogCachedEvent(CachedEvent cachedEvent)
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
    public static void LogInterstitialAdLoad(string placement)
    {
        LogEvent("ad_inter_load", new Parameter("placement", placement));
    }

    public static void LogInterstitialAdFailed(string placement)
    {
        LogEvent("ad_inter_fail", new Parameter("placement", placement));
    }

    public static void LogInterstitialAdShow(string placement)
    {
        LogEvent("ad_inter_show", new Parameter("placement", placement));
    }

    public static void LogInterstitialAdClick(string placement)
    {
        LogEvent("ad_inter_click", new Parameter("placement", placement));
    }

    public static void LogRewardedAdOffer(string placement, string buttonName)
    {
        LogEvent("ads_reward_offer",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public static void LogRewardedAdFailed(string placement, string buttonName)
    {
        LogEvent("ads_reward_fail",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public static void LogRewardedAdShow(string placement, string buttonName)
    {
        LogEvent("ads_reward_show",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public static void LogRewardedAdClick(string placement, string buttonName)
    {
        LogEvent("ads_reward_click",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public static void LogRewardedAdComplete(string placement, string buttonName)
    {
        LogEvent("ads_reward_complete",
            new Parameter("placement", placement),
            new Parameter("button_name", buttonName));
    }

    public static void LogAdRevenue(ImpressionData data)
    {
        LogEvent("ad_impression", data.ToParameters());
    }
    #endregion

    #region IAP EVENT
    public static void LogIapPurchase(string packId, string location)
    {
        LogEvent("iapPurchased_confirmed",
            new Parameter("pack_ID", packId),
            new Parameter("location", location));
    }
    #endregion
    #endregion
}