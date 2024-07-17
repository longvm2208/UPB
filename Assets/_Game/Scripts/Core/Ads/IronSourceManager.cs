using Sirenix.OdinInspector;
using System;
using UnityEngine;
using static MaxSdkBase;

public class IronSourceManager : SingletonMonoBehaviour<IronSourceManager>
{
#if IRON_SOURCE
    // Test
    //const string AppKey = "85460dcd";
    // Product
    const string AppKey = "";

    [SerializeField] bool enableTestSuite = false;
    [SerializeField] bool isAdaptiveBanner = true;
    [SerializeField] bool isRewardedVideoManualAPI = true;

    public event Action Initialized;

    public event Action InterstitialAdLoaded;
    public event Action InterstitialAdOpened;
    public event Action InterstitialAdShowFailed;
    public event Action InterstitialAdClicked;

    public event Action RewardedAdAvailable;
    public event Action RewardedAdOpened;
    public event Action<string> RewardedAdShowFailed;
    public event Action RewardedAdClosed;
    public event Action RewardedAdReceivedReward;

    public event Action<ImpressionData> ImpressionDataReady;

    float bannerHeight = -1f;

    public float BannerHeight
    {
        get
        {
            if (bannerHeight < 0f)
            {
                CalculateBannerHeight();
            }

            return bannerHeight;
        }
    }

    void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }

    public void Initialize()
    {
        if (enableTestSuite)
        {
            IronSource.Agent.setMetaData("is_test_suite", "enable");
        }

        if (PlayerPrefs.HasKey("consent"))
        {
            IronSource.Agent.setConsent(PlayerPrefs.GetInt("consent") == 1);
        }

        if (PlayerPrefs.HasKey("do_not_sell"))
        {
            IronSource.Agent.setMetaData("do_not_sell", (PlayerPrefs.GetInt("do_not_sell") == 0).ToString());
        }

        if (PlayerPrefs.HasKey("is_child_directed"))
        {
            IronSource.Agent.setMetaData("is_child_directed", (PlayerPrefs.GetInt("is_child_directed") == 0).ToString());
        }

        IronSource.Agent.init(AppKey);
        IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializationCompleted;
        IronSource.Agent.validateIntegration();
        IronSource.Agent.setManualLoadRewardedVideo(isRewardedVideoManualAPI);
        IronSourceEvents.onImpressionDataReadyEvent += OnImpressionDataReady; ;
        InitializeRewardedVideo();
        InitializeInterstitial();
        InitializeBanner();
    }

    void OnSdkInitializationCompleted()
    {
        if (enableTestSuite)
        {
            IronSource.Agent.launchTestSuite();
        }

        LoadRewardedVideo();
        LoadInterstitial();

        Initialized?.Invoke();
    }

    #region REWARDED VIDEO
    void InitializeRewardedVideo()
    {
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
    }

    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        RewardedAdAvailable?.Invoke();
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable() { }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        RewardedAdOpened?.Invoke();
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        LoadRewardedVideo();
        RewardedAdClosed?.Invoke();
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        LoadRewardedVideo();
        RewardedAdReceivedReward?.Invoke();
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        LoadRewardedVideo();
        RewardedAdShowFailed?.Invoke(error.ToString());
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo) { }

    public void LoadRewardedVideo()
    {
        if (isRewardedVideoManualAPI)
        {
            IronSource.Agent.loadRewardedVideo();
        }
    }

    public bool IsRewardedVideoAvailable() => IronSource.Agent.isRewardedVideoAvailable();
    public void ShowRewardedAd() => IronSource.Agent.showRewardedVideo();
    #endregion

    #region INTERSTITIAL
    void InitializeInterstitial()
    {
        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
    }

    // Invoked when the interstitial ad was loaded succesfully.
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        InterstitialAdLoaded?.Invoke();
    }
    // Invoked when the initialization process has failed.
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
        LoadInterstitial();
    }
    // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        InterstitialAdOpened?.Invoke();
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        InterstitialAdClicked?.Invoke();
    }
    // Invoked when the ad failed to show.
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        LoadInterstitial();
        InterstitialAdShowFailed?.Invoke();
    }
    // Invoked when the interstitial ad closed and the user went back to the application screen.
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        LoadInterstitial();
    }
    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
    // This callback is not supported by all networks, and we recommend using it only if  
    // it's supported by all networks you included in your build. 
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo) { }

    public bool IsInterstitialReady() => IronSource.Agent.isInterstitialReady();
    public void LoadInterstitial() => IronSource.Agent.loadInterstitial();
    public void ShowInterstitial() => IronSource.Agent.showInterstitial();
    #endregion

    #region BANNER
    void InitializeBanner()
    {
        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

        CalculateBannerHeight();
    }

    //Invoked once the banner has loaded
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo) { }
    //Invoked when the banner loading process has failed.
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) { }
    // Invoked when end user clicks on the banner ad
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo) { }
    //Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo) { }
    //Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo) { }
    //Invoked when the user leaves the app
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo) { }

    public void LoadBanner()
    {
        IronSourceBannerSize size = IronSourceBannerSize.BANNER;
        size.SetAdaptive(isAdaptiveBanner);
        IronSource.Agent.loadBanner(size, IronSourceBannerPosition.BOTTOM);
    }

    public void HideBanner() => IronSource.Agent.hideBanner();
    public void DisplayBanner() => IronSource.Agent.displayBanner();
    public void DestroyBanner() => IronSource.Agent.destroyBanner();

    void CalculateBannerHeight()
    {
        if (Application.isEditor)
        {
            bannerHeight = 50f;
        }
        else if (isAdaptiveBanner)
        {
            float width = IronSource.Agent.getDeviceScreenWidth();
            bannerHeight = IronSource.Agent.getMaximalAdaptiveHeight(width);
        }
        else
        {
            bannerHeight = 50f;
        }

        float density = Screen.dpi / 160f;
        bannerHeight *= density;
    }
    #endregion

    #region REVENUE
    private void OnImpressionDataReady(IronSourceImpressionData ironSourceData)
    {
        var data = new ImpressionData(
            "iron_source",
            "",
            ironSourceData.country,
            ironSourceData.adUnit,
            ironSourceData.adNetwork,
            ironSourceData.placement,
            "USD",
            ironSourceData.revenue);

        ImpressionDataReady?.Invoke(data);
    }
    #endregion
#endif
}
