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
    [SerializeField] GameObject blocker;

    bool loadingInterstitial;
    bool loadingRewarded;
    bool receivedReward;
    int interstitialRetryAttempt;
    int rewardedRetryAttempt;
    float interstitialTimer;
    float rewardedTimer;
    string rewardedPlacement;

    Action onInterstitialFinish;
    Action onRewardedSuccess;
    Action onRewardedFail;

    private void Update()
    {
        if (interstitialTimer > 0)
        {
            interstitialTimer -= Time.deltaTime;
        }

        if (rewardedTimer > 0)
        {
            rewardedTimer -= Time.deltaTime;
        }
    }

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

    bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
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
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        
        LoadInterstitial();
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
        blocker.SetActive(false);

        FirebaseManager.ad_inter_show();
        AppsFlyerManager.af_inters_displayed();
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        blocker.SetActive(false);
        LoadInterstitial();
        onInterstitialFinish?.Invoke();
        onInterstitialFinish = null;

        FirebaseManager.ad_inter_fail(errorInfo.ToString());
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        blocker.SetActive(false);
        LoadInterstitial();
        onInterstitialFinish?.Invoke();
        onInterstitialFinish = null;
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    private void LoadInterstitial()
    {
        if (!AdsManager.Ins.AdsEnabled || AdsManager.Ins.AdsRemoved || loadingInterstitial || !IsInternetAvailable() || IsInterstitialReady()) return;

        loadingInterstitial = true;
        MaxSdk.LoadInterstitial(InterstitialAdUnitId);
    }

    public bool IsInterstitialReady()
    {
        return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
    }

    public void ShowInterstitial(Action onFinish)
    {
        if (!AdsManager.Ins.AdsEnabled || AdsManager.Ins.AdsRemoved || interstitialTimer > 0 || rewardedTimer > 0)
        {
            onFinish?.Invoke();
            return;
        }

        if (IsInterstitialReady())
        {
            blocker.SetActive(true);
            onInterstitialFinish = onFinish;
            interstitialTimer = AdsManager.Ins.InterstitialCapping;
            MaxSdk.ShowInterstitial(InterstitialAdUnitId);

            FirebaseManager.ad_inter_click();
            AppsFlyerManager.af_inters_ad_eligible();
        }
        else
        {
            LoadInterstitial();
            onFinish?.Invoke();
        }
    }
    #endregion

    #region Rewarded
    public void InitializeRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        LoadRewardedAd();
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        loadingRewarded = false;
        rewardedRetryAttempt = 0;

        AppsFlyerManager.af_rewarded_api_called();
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        loadingRewarded = false;
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        blocker.SetActive(false);

        FirebaseManager.ads_reward_show(rewardedPlacement);
        AppsFlyerManager.af_rewarded_displayed();
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        blocker.SetActive(false);
        onRewardedFail?.Invoke();
        onRewardedFail = null;
        LoadRewardedAd();

        FirebaseManager.ads_reward_fail(rewardedPlacement, errorInfo.ToString());
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        blocker.SetActive(false);
        LoadRewardedAd();

        if (receivedReward)
        {
            receivedReward = false;
            onRewardedSuccess?.Invoke();
            onRewardedSuccess = null;
        }
        else
        {
            onRewardedFail?.Invoke();
            onRewardedFail = null;
        }
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        receivedReward = true;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

    private void LoadRewardedAd()
    {
        if (!AdsManager.Ins.AdsEnabled || loadingRewarded || !IsInternetAvailable() || IsRewardedAdReady()) return;

        loadingRewarded = true;
        MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    public bool IsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
    }

    public void ShowRewardedAd(string placement = "", Action onSuccess = null, Action onFail = null)
    {
        if (!AdsManager.Ins.AdsEnabled)
        {
            onSuccess?.Invoke();
            return;
        }

        if (IsRewardedAdReady())
        {
            rewardedPlacement = placement;
            blocker.SetActive(true);
            onRewardedSuccess = onSuccess;
            onRewardedFail = onFail;
            rewardedTimer = AdsManager.Ins.RewardedCapping;
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);

            FirebaseManager.ads_reward_click(placement);
            AppsFlyerManager.af_rewarded_ad_eligible();
        }
        else
        {
            LoadRewardedAd();
            onFail?.Invoke();
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
