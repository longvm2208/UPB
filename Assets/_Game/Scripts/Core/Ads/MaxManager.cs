using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class MaxManager : SingletonMonoBehaviour<MaxManager>
{
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

    public event Action OnInitialized;

    public event Action OnInterstitialAdLoaded;
    public event Action OnInterstitialAdDisplayed;
    public event Action OnInterstitialAdFailedToDisplay;
    public event Action OnInterstitialAdClicked;

    public event Action OnRewardedAdLoaded;
    public event Action OnRewardedAdDisplayed;
    public event Action<MaxSdkBase.ErrorInfo> OnRewardedAdFailedToDisplay;
    public event Action OnRewardedAdHidden;
    public event Action OnRewardedAdReceivedReward;

    public event Action<MaxSdkBase.AdInfo> OnAdRevenuePaid;

    int interstitialRetryAttempt;
    int rewardedRetryAttempt;

    public void Initialize()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (configuration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            InitializeInterstitialAds();
            InitializeRewardedAd();
            InitializeBannerAd();
            //InitializeMRecAds();

            if (enableMediationDebugger)
            {
                MaxSdk.ShowMediationDebugger();
            }

            OnInitialized?.Invoke();
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
    public void InitializeInterstitialAds()
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
        OnInterstitialAdLoaded?.Invoke();
    }

    void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke(nameof(LoadInterstitial), (float)retryDelay);
    }

    void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnInterstitialAdDisplayed?.Invoke();
    }

    void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
        OnInterstitialAdFailedToDisplay?.Invoke();
    }

    void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnInterstitialAdClicked?.Invoke();
    }

    void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
    }

    void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaid?.Invoke(adInfo);
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
        OnRewardedAdLoaded?.Invoke();
    }

    void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnRewardedAdDisplayed?.Invoke();
    }

    void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        OnRewardedAdFailedToDisplay?.Invoke(errorInfo);
    }

    void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        OnRewardedAdHidden?.Invoke();
    }

    void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        OnRewardedAdReceivedReward?.Invoke();
    }

    void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaid?.Invoke(adInfo);
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
    }

    void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }
    void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        OnAdRevenuePaid?.Invoke(adInfo);
    }

    public void ShowBanner() => MaxSdk.ShowBanner(BannerAdUnitId);
    public void HideBanner() => MaxSdk.HideBanner(BannerAdUnitId);
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
        // Interstitial ad revenue paid. Use this callback to track user revenue.
        OnAdRevenuePaid?.Invoke(adInfo);
    }

    public void ShowMRec() => MaxSdk.ShowMRec(MrecAdUnitId);
    public void HideMRec() => MaxSdk.HideMRec(MrecAdUnitId);
    #endregion
}
