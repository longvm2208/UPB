using System;
using UnityEngine;

public class MaxManager : SingletonMonoBehaviour<MaxManager>
{
    const string Key = "";

#if UNITY_ANDROID
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MRecAdUnitId = "";
    const string AppOpenAdUnitId = "";
#elif UNITY_IOS
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MRecAdUnitId = "";
    const string AppOpenAdUnitId = "";
#else
    const string InterstitialAdUnitId = "";
    const string RewardedAdUnitId = "";
    const string BannerAdUnitId = "";
    const string MRecAdUnitId = "";
    const string AppOpenAdUnitId = "";
#endif

    [SerializeField] bool showMediationDebugger;
    [SerializeField] bool adaptiveBanner;
    [SerializeField] Color bannerBackgroundColor;

    bool loadingInterstitial;
    bool loadingRewarded;
    int interstitialRetryAttempt;
    int rewardedRetryAttempt;

    Action onInterstitialFinish;
    Action onRewardedSuccess;
    Action onRewardedFail;

    public void Init()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += config =>
        {
            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();
            InitializeMRecAds();

            if (showMediationDebugger)
            {
                MaxSdk.ShowMediationDebugger();
            }
        };

        MaxSdk.SetSdkKey(Key);
        MaxSdk.InitializeSdk();
    }

    #region Interstitial
    public void InitializeInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        if (loadingInterstitial || IsInterstitialReady()) return;

        loadingInterstitial = true;
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        loadingInterstitial = false;
        interstitialRetryAttempt = 0;

        FirebaseManager.ad_inter_load();
        AppsFlyerManager.af_inters_api_called();
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        loadingInterstitial = false;
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke(nameof(LoadInterstitial), (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) 
    {
        FirebaseManager.ad_inter_show();
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();

        FirebaseManager.ad_inter_fail(errorInfo.Message);
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) 
    {
        FirebaseManager.ad_inter_click();
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
    }

    public bool IsInterstitialReady()
    {
        return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    }

    public void ShowInterstitial(Action onFinish)
    {
        if (IsInterstitialReady())
        {
            MaxSdk.ShowInterstitial(InterstitialAdUnitId);
        }
    }
    #endregion

    #region Rewarded
    public void InitializeRewardedAds()
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

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    public bool IsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    }

    public void ShowRewardedAd()
    {
        if (IsRewardedAdReady())
        {
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);
        }
    }
    #endregion

    #region Banner
    public void InitializeBannerAds()
    {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(BannerAdUnitId, "adaptive_banner", adaptiveBanner.ToString());

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, bannerBackgroundColor);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowBanner()
    {
        MaxSdk.ShowBanner(BannerAdUnitId);
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(BannerAdUnitId);
    }

    public void DestroyBanner()
    {
        MaxSdk.DestroyBanner(BannerAdUnitId);
    }
    #endregion

    #region MRec
    public void InitializeMRecAds()
    {
        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(MRecAdUnitId, MaxSdkBase.AdViewPosition.Centered);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;
    }

    public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error) { }

    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowMRec()
    {
        MaxSdk.ShowMRec(MRecAdUnitId);
    }

    public void HideMRec()
    {
        MaxSdk.HideMRec(MRecAdUnitId);
    }

    public void DestroyMRec()
    {
        MaxSdk.DestroyMRec(MRecAdUnitId);
    }
    #endregion
}
