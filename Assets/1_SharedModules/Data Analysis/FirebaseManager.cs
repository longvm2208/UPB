using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
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

    static List<CachedEvent> cachedEvents = new();

    public static bool Ready { get; private set; }
    public static bool FetchRemoteConfigDone { get; private set; }
    public static bool FetchRemoteConfigSuccess { get; private set; }

    public static void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log($"Firebase dependency status: {dependencyStatus}");

                Ready = true;
                FetchRemoteConfig();
                LogCachedEvents();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    #region Remote Config
    static void FetchRemoteConfig()
    {
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;

        remoteConfig.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
        {
            FetchRemoteConfigDone = true;

            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var info = remoteConfig.Info;

            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{info.LastFetchStatus} - {info.LastFetchFailureReason}");
                return;
            }

            remoteConfig.ActivateAsync().ContinueWithOnMainThread(task =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                ApplyRemoteConfigData();
                FetchRemoteConfigSuccess = true;
            });
        });
    }

    static void ApplyRemoteConfigData()
    {

    }

    static bool GetBool(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
    }

    static int GetInt(string key)
    {
        return (int)FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
    }
    #endregion

    #region User Property
    static void SetUserProperty(string property, string value)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        FirebaseAnalytics.SetUserProperty(property, value);
    }
    #endregion

    #region Log Event
    static void LogEvent(string name)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        if (!Ready)
        {
            cachedEvents.Add(new CachedEvent(name));
        }
        else
        {
            FirebaseAnalytics.LogEvent(name);
        }
    }

    static void LogEvent(string name, params Parameter[] parameters)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        if (!Ready)
        {
            cachedEvents.Add(new CachedEvent(name, parameters));
        }
        else
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }

    static void LogCachedEvents()
    {
        if (cachedEvents == null) return;

        for (int i = 0; i < cachedEvents.Count; i++)
        {
            LogCachedEvent(cachedEvents[i]);
        }

        cachedEvents = null;
    }

    static void LogCachedEvent(CachedEvent cachedEvent)
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

    #region Interstitial
    public static void ad_inter_load()
    {
        LogEvent("ad_inter_load");
    }

    public static void ad_inter_fail(string errormsg)
    {
        LogEvent("ad_inter_fail", new Parameter("errormsg", errormsg));
    }

    public static void ad_inter_show()
    {
        LogEvent("ad_inter_show");
    }

    public static void ad_inter_click()
    {
        LogEvent("ad_inter_click");
    }
    #endregion

    #region Rewarded
    public static void ads_reward_offer(string placement)
    {
        LogEvent("ads_reward_offer", new Parameter("placement", placement));
    }

    public static void ads_reward_fail(string placement, string errormsg)
    {
        LogEvent("ads_reward_fail",
            new Parameter("placement", placement),
            new Parameter("errormsg", errormsg));
    }

    public static void ads_reward_show(string placement)
    {
        LogEvent("ads_reward_show", new Parameter("placement", placement));
    }

    public static void ads_reward_click(string placement)
    {
        LogEvent("ads_reward_click", new Parameter("placement", placement));
    }

    public static void ads_reward_complete(string placement)
    {
        LogEvent("ads_reward_complete", new Parameter("placement", placement));
    }
    #endregion

    public static void iap_purchased(string packId, string location)
    {
        LogEvent("iapPurchased_confirmed",
            new Parameter("pack_ID", packId),
            new Parameter("location", location));
    }
    #endregion
}