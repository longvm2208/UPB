using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseManager : SingletonMonoBehaviour<FirebaseManager>
{
    public struct CachedEvent
    {
        public string name;
        public Parameter[] parameters;

        public CachedEvent(string name, Parameter[] parameters)
        {
            this.name = name;
            this.parameters = parameters;
        }
    }

    private bool isReady;
    private Queue<CachedEvent> cachedEvents = new Queue<CachedEvent>();

    #region INITIALIZE
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
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });

        LogCachedEvents();
    }

    private void LogCachedEvents()
    {
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return new WaitUntil(() => isReady);

            if (cachedEvents.Count > 0)
            {
                for (int i = 0; i < cachedEvents.Count; i++)
                {
                    LogCachedEvent(cachedEvents.Dequeue());
                }
            }
        }
    }

    private void LogCachedEvent(CachedEvent cachedEvent)
    {
        LogEvent(cachedEvent.name, cachedEvent.parameters);
    }
    #endregion

    #region SET USER PROPERTIES
    private void SetUserProperty(string property, string value)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;
        FirebaseAnalytics.SetUserProperty(property, value);
    }
    #endregion

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
        if (Application.isEditor) return;

    }
    #endregion

    #region LOG EVENT
    public void LogEvent(string name, params Parameter[] parameters)
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

    public void LogEvent(string name)
    {
        if (Debug.isDebugBuild || Application.isEditor || !isReady) return;
        FirebaseAnalytics.LogEvent(name);
    }

    #region ADS
    public void LogAdRevenuePaid(ImpressionData data)
    {
        LogEvent("ad_impression", data.ToParameters());
    }

    public void LogRewardedAdSuccess(string placement)
    {
        LogEvent("rewarded_ad_success",
            new Parameter("placement", placement));
    }
    #endregion

    #region IAP
    public void LogIAPPurchased(string id)
    {
        LogEvent("iap_purchased",
            new Parameter("id", id));
    }
    #endregion
    #endregion
}
