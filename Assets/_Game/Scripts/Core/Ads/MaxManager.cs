using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class MaxManager : SingletonMonoBehaviour<MaxManager>
{
#if APPLOVIN_MAX
    [SerializeField] bool enableMediationDebugger = false;
    [SerializeField] bool isAdaptiveBanner = true;
    [SerializeField] Color bannerColor = new Color(0f, 0f, 0f, 0f);

    const string Key = "";
    //const string UserId = "";

#if UNITY_ANDROID
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MrecAdUnitId = "";
#elif UNITY_IOS
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MrecAdUnitId = "";
#else
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MrecAdUnitId = "";
#endif

    public event Action Initialized;

    public event Action InterstitialAdLoaded;
    public event Action InterstitialAdDisplayed;
    public event Action InterstitialAdFailedToDisplay;
    public event Action InterstitialAdClicked;

    public event Action RewardedAdLoaded;
    public event Action RewardedAdDisplayed;
    public event Action<string> RewardedAdFailedToDisplay;
    public event Action RewardedAdHidden;
    public event Action RewardedAdReceivedReward;

    public event Action<ImpressionData> AdRevenuePaid;

    int interstitialRetryAttempt;
    int rewardedRetryAttempt;
    float bannerHeight = -1;

    public float BannerHeight
    {
        get
        {
            if (bannerHeight < 0)
            {
                CalculateBannerHeight();
            }

            return bannerHeight;
        }
    }

    public void Initialize()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (configuration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            InitializeInterstitialAd();
            InitializeRewardedAd();
            InitializeBannerAd();
            //InitializeMRecAds();

            if (enableMediationDebugger)
            {
                MaxSdk.ShowMediationDebugger();
            }

            Initialized?.Invoke();
        };

        MaxSdk.SetSdkKey(Key);
        //MaxSdk.SetUserId(UserId);
        MaxSdk.InitializeSdk();
    }

    public bool IsPrivacyOptionsRequired() => MaxSdk.GetSdkConfiguration().ConsentFlowUserGeography == MaxSdkBase.ConsentFlowUserGeography.Gdpr;

    public void ShowPrivacyOptionsForm()
    {
        var cmpService = MaxSdk.CmpService;

        cmpService.ShowCmpForExistingUser(error =>
        {
            if (null == error)
            {
                // The CMP alert was shown successfully.
            }
        });
    }

    #region INTERSTITIAL ADS
    public void InitializeInterstitialAd()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        interstitialRetryAttempt = 0;
        InterstitialAdLoaded?.Invoke();
    }

    void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke(nameof(LoadInterstitial), (float)retryDelay);
    }

    void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        InterstitialAdDisplayed?.Invoke();
    }

    void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
        InterstitialAdFailedToDisplay?.Invoke();
    }

    void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        InterstitialAdClicked?.Invoke();
    }

    void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
    }

    void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaid(adInfo);
    }

    public bool IsInterstitialReady() => MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    public void LoadInterstitial() => MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    public void ShowInterstitial() => MaxSdk.ShowInterstitial(InterstitialAdUnitId);
    #endregion

    #region REWARDED AD
    public void InitializeRewardedAd()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        rewardedRetryAttempt = 0;
        RewardedAdLoaded?.Invoke();
    }

    void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        RewardedAdDisplayed?.Invoke();
    }

    void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        RewardedAdFailedToDisplay?.Invoke(errorInfo.Message);
    }

    void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        RewardedAdHidden?.Invoke();
    }

    void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        RewardedAdReceivedReward?.Invoke();
    }

    void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaid(adInfo);
    }

    public bool IsRewardedAdReady() => MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    public void LoadRewardedAd() => MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    public void ShowRewardedAd() => MaxSdk.ShowRewardedAd(RewardedAdUnitId);
    #endregion

    #region BANNER AD
    public void InitializeBannerAd()
    {
        // Banners are automatically sized to 320�50 on phones and 728�90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        if (isAdaptiveBanner)
        {
            MaxSdk.SetBannerExtraParameter(BannerAdUnitId, "adaptive_banner", "true");
        }

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, bannerColor);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

        CalculateBannerHeight();
    }

    void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }
    void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaid(adInfo);
    }

    public void ShowBanner() => MaxSdk.ShowBanner(BannerAdUnitId);
    public void HideBanner() => MaxSdk.HideBanner(BannerAdUnitId);

    void CalculateBannerHeight()
    {
        if (Application.isEditor)
        {
            bannerHeight = 50f;
        }
        else if (isAdaptiveBanner)
        {
            bannerHeight = MaxSdkUtils.GetAdaptiveBannerHeight();
        }
        else if (MaxSdkUtils.IsTablet())
        {
            bannerHeight = 90f;
        }
        else
        {
            bannerHeight = 50f;
        }

        float density = MaxSdkUtils.GetScreenDensity();

        bannerHeight *= density;
    }
    #endregion

    #region MREC AD
    public void InitializeMRecAd()
    {
        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(MrecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
    }

    public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error) { }
    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaid(adInfo);
    }

    public void ShowMRec() => MaxSdk.ShowMRec(MrecAdUnitId);
    public void HideMRec() => MaxSdk.HideMRec(MrecAdUnitId);
    #endregion

    void OnAdRevenuePaid(MaxSdkBase.AdInfo adInfo)
    {
        var data = new ImpressionData(
            "applovin_max",
            adInfo.AdFormat,
            MaxSdk.GetSdkConfiguration().CountryCode,
            adInfo.AdUnitIdentifier,
            adInfo.NetworkName,
            adInfo.Placement,
            "USD",
            adInfo.Revenue);

        AdRevenuePaid?.Invoke(data);
    }
#endif
}
