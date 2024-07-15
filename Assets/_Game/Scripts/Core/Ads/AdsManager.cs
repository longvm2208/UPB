//#define APPLOVIN_MAX
#define IRON_SOURCE

using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : SingletonMonoBehaviour<AdManager>
{
    [SerializeField] float interstitialTimeCounter = -1f;
    [SerializeField] float rewardTimeCounter = -1f; 
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject adNotLoadedYet;

    event Action onInterstitialAdLoaded;
    event Action onInterstitialAdDisplayed;
    event Action onInterstitialAdFailedToDisplay;
    event Action onInterstitialAdClicked;

    event Action onRewardedAdDisplayed;
#if APPLOVIN_MAX
    event Action<MaxSdkBase.ErrorInfo> onRewardedAdFailedToDisplay;
#elif IRON_SOURCE
    event Action<IronSourceError> onRewardedAdShowFailed;
#endif
    event Action onRewardedAdHidden;
    event Action onRewardedAdReceivedReward;

    GameData gameData => DataManager.Instance.GameData;

    void Start()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.OnInitialized += OnInitialized;

        MaxManager.Instance.OnInterstitialAdLoaded += InterstitialAdLoaded;
        MaxManager.Instance.OnInterstitialAdDisplayed += InterstitialAdDisplayed;
        MaxManager.Instance.OnInterstitialAdFailedToDisplay += InterstitialAdFailedToDisplay;
        MaxManager.Instance.OnInterstitialAdClicked += InterstitialAdClicked;

        MaxManager.Instance.OnRewardedAdLoaded += RewardedAdLoaded;
        MaxManager.Instance.OnRewardedAdDisplayed += RewardedAdDisplayed;
        MaxManager.Instance.OnRewardedAdFailedToDisplay += RewardedAdFailedToDisplay;
        MaxManager.Instance.OnRewardedAdHidden += RewardedAdHidden;
        MaxManager.Instance.OnRewardedAdReceivedReward += RewardedAdReceivedReward;

        MaxManager.Instance.OnAdRevenuePaid += OnAdRevenuePaid;
#elif IRON_SOURCE
        IronSourceManager.Instance.OnInitialized += OnInitialized;

        IronSourceManager.Instance.OnInterstitialAdLoaded += InterstitialAdLoaded;
        IronSourceManager.Instance.OnInterstitialAdOpened += InterstitialAdDisplayed;
        IronSourceManager.Instance.OnInterstitialAdShowFailed += InterstitialAdFailedToDisplay;
        IronSourceManager.Instance.OnInterstitialAdClicked += InterstitialAdClicked;

        IronSourceManager.Instance.OnRewardedAdAvailable += RewardedAdLoaded;
        IronSourceManager.Instance.OnRewardedAdOpened += RewardedAdDisplayed;
        IronSourceManager.Instance.OnRewardedAdShowFailed += RewardedAdShowFailed; ;
        IronSourceManager.Instance.OnRewardedAdClosed += RewardedAdHidden;
        IronSourceManager.Instance.OnRewardedAdReceivedReward += RewardedAdReceivedReward;

        IronSourceManager.Instance.OnImpressionDataReady += ImpressionDataReady;
#endif
    }

    void OnDestroy()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.OnInitialized -= OnInitialized;

        MaxManager.Instance.OnInterstitialAdLoaded -= InterstitialAdLoaded;
        MaxManager.Instance.OnInterstitialAdDisplayed -= InterstitialAdDisplayed;
        MaxManager.Instance.OnInterstitialAdFailedToDisplay -= InterstitialAdFailedToDisplay;
        MaxManager.Instance.OnInterstitialAdClicked -= InterstitialAdClicked;

        MaxManager.Instance.OnRewardedAdLoaded -= RewardedAdLoaded;
        MaxManager.Instance.OnRewardedAdDisplayed -= RewardedAdDisplayed;
        MaxManager.Instance.OnRewardedAdFailedToDisplay -= RewardedAdFailedToDisplay;
        MaxManager.Instance.OnRewardedAdHidden -= RewardedAdHidden;
        MaxManager.Instance.OnRewardedAdReceivedReward -= RewardedAdReceivedReward;

        MaxManager.Instance.OnAdRevenuePaid -= OnAdRevenuePaid;
#elif IRON_SOURCE
        IronSourceManager.Instance.OnInitialized -= OnInitialized;

        IronSourceManager.Instance.OnInterstitialAdLoaded -= InterstitialAdLoaded;
        IronSourceManager.Instance.OnInterstitialAdOpened -= InterstitialAdDisplayed;
        IronSourceManager.Instance.OnInterstitialAdShowFailed -= InterstitialAdFailedToDisplay;
        IronSourceManager.Instance.OnInterstitialAdClicked -= InterstitialAdClicked;

        IronSourceManager.Instance.OnRewardedAdAvailable -= RewardedAdLoaded;
        IronSourceManager.Instance.OnRewardedAdOpened -= RewardedAdDisplayed;
        IronSourceManager.Instance.OnRewardedAdShowFailed -= RewardedAdShowFailed; ;
        IronSourceManager.Instance.OnRewardedAdClosed -= RewardedAdHidden;
        IronSourceManager.Instance.OnRewardedAdReceivedReward -= RewardedAdReceivedReward;

        IronSourceManager.Instance.OnImpressionDataReady -= ImpressionDataReady;
#endif
    }

    void Update()
    {
        if (interstitialTimeCounter > 0)
        {
            interstitialTimeCounter -= Time.deltaTime;
        }
    }

    public void Initialize()
    {
        Debug.Log("AdManager - Initialize");

        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return new WaitUntil(() => AdMobManager.Instance.CanRequestAd);

#if APPLOVIN_MAX
            MaxManager.Instance.Initialize();
#elif IRON_SOURCE
            IronSourceManager.Instance.Initialize();
#endif
        }
    }

    void OnInitialized()
    {
        StartCoroutine(ShowBannerRoutine());
    }

#if APPLOVIN_MAX
    void OnAdRevenuePaid(MaxSdkBase.AdInfo adInfo)
    {
        var data = new ImpressionData(
            "applovin_max",
            adInfo.AdFormat,
            MaxSdk.GetSdkConfiguration().CountryCode,
            adInfo.AdUnitIdentifier,
            adInfo.NetworkName,
            adInfo.Placement,
            adInfo.Revenue);

        Debug.Log(data.ToString());

        FirebaseManager.Instance.LogAdRevenuePaid(data);

        AppsFlyerAdRevenue.logAdRevenue("applovin_max",
            AppsFlyerAdRevenueMediationNetworkType.
            AppsFlyerAdRevenueMediationNetworkTypeApplovinMax,
            data.revenue, "USD", data.ToDictionary());
    }
#elif IRON_SOURCE
    void ImpressionDataReady(IronSourceImpressionData data)
    {
        try
        {
            var revenue = data.revenue.Value;

            Dictionary<string, string> additionalParameters = new Dictionary<string, string>
            {
                { "ad_platform", "ironsource" },
                { "ad_source", data.adNetwork },
                { "ad_unit_name", data.adUnit },
                { "placement", data.placement },
                { "currency", "USD" },
                { "country_code", data.country }
            };

            try
            {
                additionalParameters.Add("precision", data.precision);
                additionalParameters.Add("auction_id", data.auctionId);
                additionalParameters.Add("encrypted_cpm", data.encryptedCPM);
                additionalParameters.Add("value", revenue.ToString());

                if (data.conversionValue.HasValue)
                {
                    additionalParameters.Add("conversion_value", data.conversionValue.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            //AppsFlyerAdRevenue.logAdRevenue(data.adNetwork
            //    , AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource,
            //    data.revenue.Value,
            //    "USD", additionalParameters);

            Firebase.Analytics.Parameter[] AdParameters = {
                new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
                new Firebase.Analytics.Parameter("ad_source", data.adNetwork),
                new Firebase.Analytics.Parameter("ad_unit_name", data.adUnit),
                new Firebase.Analytics.Parameter("ad_format", data.instanceName),
                new Firebase.Analytics.Parameter("currency","USD"),
                new Firebase.Analytics.Parameter("value", data.revenue.Value)
            };

            //FirebaseManager.Instance.LogEvent("ad_impression", AdParameters);
            //FirebaseManager.Instance.LogEvent("ad_impression_abi", AdParameters);
            //FirebaseManager.Instance.LogEvent("ad_impression_IS", AdParameters);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
#endif

    IEnumerator ShowBannerRoutine()
    {
        yield return new WaitUntil(() =>
            DataManager.Instance.IsLoaded &&
            LoadSceneManager.Instance.CurrentScene != SceneId.Load &&
            LoadSceneManager.Instance.CurrentScene != SceneId.None);

        ShowBanner();
    }

    void AdNotLoadedYetNotify()
    {
        adNotLoadedYet.gameObject.SetActive(false);
        adNotLoadedYet.gameObject.SetActive(true);
    }

    #region CONSENT FORM
    public void ShowPrivacyOptionsForm()
    {
        AdMobManager.Instance.ShowPrivacyOptionsForm();
    }

    public bool IsPrivacyOptionsRequired()
    {
        return AdMobManager.Instance.IsPrivacyOptionsRequired();
    }
    #endregion

    #region INTERSTITIAL AD
    void InterstitialAdLoaded()
    {
        //AppsflyerEventRegister.af_interstitial_ad_api_called();
        onInterstitialAdLoaded?.Invoke();
    }

    void InterstitialAdDisplayed()
    {
        //AppsflyerEventRegister.af_interstitial_ad_displayed();
        blocker.SetActive(false);
        onInterstitialAdDisplayed?.Invoke();
    }

    void InterstitialAdFailedToDisplay()
    {
        blocker.SetActive(false);
        onInterstitialAdFailedToDisplay?.Invoke();
    }

    void InterstitialAdClicked()
    {
        onInterstitialAdClicked?.Invoke();
    }

    void LoadInterstitial()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.LoadInterstitial();
#elif IRON_SOURCE
        IronSourceManager.Instance.LoadInterstitial();
#endif
    }

    bool IsInterstitialReady()
    {
#if APPLOVIN_MAX
        return MaxManager.Instance.IsInterstitialReady();
#elif IRON_SOURCE
        return IronSourceManager.Instance.IsInterstitialReady();
#endif
    }

    void ShowInterstitial()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.ShowInterstitial();
#elif IRON_SOURCE
        IronSourceManager.Instance.ShowInterstitial();
#endif
    }

    bool CanShowInterstitial()
    {
        return ConfigManager.Instance.IsEnableAds && !gameData.IsRemoveAds && interstitialTimeCounter <= 0;
    }

    void SetupShowInterstitial(string placement, Action onFinished)
    {
        //AppsflyerEventRegister.af_interstitial_ad_eligible();
        interstitialTimeCounter = ConfigManager.Instance.InterstitialAdCapping;
        blocker.SetActive(true);

        onInterstitialAdDisplayed = () =>
        {
            //FirebaseManager.Instance.ad_inter_show(placement);
            onFinished?.Invoke();
        };

        onInterstitialAdFailedToDisplay = () =>
        {
            //FirebaseManager.Instance.ad_inter_fail(placement);
            onFinished?.Invoke();
        };

        onInterstitialAdLoaded = () =>
        {
            //FirebaseManager.Instance.ad_inter_load(placement);
        };

        onInterstitialAdClicked = () =>
        {
            //FirebaseManager.Instance.ad_inter_click(placement);
        };
    }

    void CheckAndShowInterstitial(Action onFinished)
    {
        if (IsInterstitialReady())
        {
            ShowInterstitial();
        }
        else
        {
            Debug.Log("Interstitial ad not ready");

            LoadInterstitial();
            blocker.SetActive(false);
            onFinished?.Invoke();
        }
    }

    public void ShowInterstitial(string placement, Action onFinished = null)
    {
        if (!CanShowInterstitial())
        {
            onFinished?.Invoke();
            return;
        }

        SetupShowInterstitial(placement, onFinished);
        CheckAndShowInterstitial(onFinished);
    }

    public void ShowInterstitialDelay(string placement, Action onFinished = null)
    {
        if (!CanShowInterstitial())
        {
            onFinished?.Invoke();
            return;
        }

        SetupShowInterstitial(placement, onFinished);

        ScheduleUtils.DelayTask(1f, () => CheckAndShowInterstitial(onFinished));
    }
    #endregion

    #region REWARDED AD
    void RewardedAdLoaded()
    {
        //AppsflyerEventRegister.af_rewarded_ad_api_called();
    }

    void RewardedAdDisplayed()
    {
        //AppsflyerEventRegister.af_rewarded_ad_displayed();
        onRewardedAdDisplayed?.Invoke();
    }

#if APPLOVIN_MAX
    void RewardedAdFailedToDisplay(MaxSdkBase.ErrorInfo info)
    {
        blocker.SetActive(false);
        onRewardedAdFailedToDisplay?.Invoke(info);
    }
#elif IRON_SOURCE
    void RewardedAdShowFailed(IronSourceError error)
    {
        blocker.SetActive(false);
        onRewardedAdShowFailed?.Invoke(error);
    }
#endif

    void RewardedAdHidden()
    {
        blocker.SetActive(false);
        onRewardedAdHidden?.Invoke();
    }

    void RewardedAdReceivedReward()
    {
        //AppsflyerEventRegister.af_rewarded_ad_completed();
        blocker.SetActive(false);
        onRewardedAdReceivedReward?.Invoke();
    }

    void LoadRewardedAd()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.LoadRewardedAd();
#elif IRON_SOURCE
        IronSourceManager.Instance.LoadRewardedVideo();
#endif
    }

    bool IsRewardedAdReady()
    {
#if APPLOVIN_MAX
        return MaxManager.Instance.IsRewardedAdReady();
#elif IRON_SOURCE
        return IronSourceManager.Instance.IsRewardedVideoAvailable();
#endif
    }

    void ShowRewardedAd()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.ShowRewardedAd();
#elif IRON_SOURCE
        IronSourceManager.Instance.ShowRewardedAd();
#endif
    }

    public void ShowRewardedAd(string placement, string buttonName, Action onReceivedReward = null, Action onFailed = null)
    {
        if (!ConfigManager.Instance.IsEnableAds)
        {
            onReceivedReward?.Invoke();
            return;
        }

        //AppsflyerEventRegister.af_rewarded_ad_eligible();
        //FirebaseManager.Instance.ads_reward_click(placement, buttonName);
        interstitialTimeCounter = ConfigManager.Instance.InterstitialAdCapping;
        blocker.SetActive(true);

        onRewardedAdDisplayed = () =>
        {
            //FirebaseManager.Instance.ads_reward_show(placement, buttonName);
        };

        onRewardedAdReceivedReward = () =>
        {
            //FirebaseManager.Instance.ads_reward_complete(placement, buttonName);
            onReceivedReward?.Invoke();
        };

        onRewardedAdHidden = () =>
        {
            onFailed?.Invoke();
        };

#if APPLOVIN_MAX
        onRewardedAdFailedToDisplay = (errorInfo) =>
        {
            FirebaseManager.Instance.ads_reward_fail(placement, buttonName);
            onFailed?.Invoke();
        };
#elif IRON_SOURCE
        onRewardedAdShowFailed = (error) =>
        {
            //FirebaseManager.Instance.ads_reward_fail(placement, buttonName);
            onFailed?.Invoke();
        };
#endif

        if (IsRewardedAdReady())
        {
            ShowRewardedAd();
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready");

            LoadRewardedAd();
            blocker.SetActive(false);
            AdNotLoadedYetNotify();
            onFailed?.Invoke();
        }
    }
    #endregion

    #region BANNER AD
    public void ShowBanner()
    {
        if (!ConfigManager.Instance.IsEnableAds || gameData.IsRemoveAds)
        {
            return;
        }

#if APPLOVIN_MAX
        MaxManager.Instance.ShowBanner();
#elif IRON_SOURCE
        IronSourceManager.Instance.LoadBanner();
#endif
    }

    public void HideBanner()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.HideBanner();
#elif IRON_SOURCE
        IronSourceManager.Instance.DestroyBanner();
#endif
    }
    #endregion
}
