using System;
using UnityEngine;
using Configuration = MaxSdkBase.SdkConfiguration;

public class MaxManager : SingletonMonoBehaviour<MaxManager>
{
    private const string SdkKey = "";
    private const string UserId = "";

#if UNITY_ANDROID
    private const string interstitialAdUnitId = "";
    private const string rewardedAdUnitId = "";
    private const string bannerAdUnitId = "";
    private const string mrecAdUnitId = "";
#elif UNITY_IOS
    private const string interstitialAdUnitId = "";
    private const string bannerAdUnitId = "";
    private const string rewardedAdUnitId = "";
    private const string mrecAdUnitId = "";
#else
    private const string interstitialAdUnitId = "";
    private const string rewardedAdUnitId = "";
    private const string bannerAdUnitId = "";
    private const string mrecAdUnitId = "";
#endif

    private static Color bannerColor = new Color(1f, 1f, 1f, 0f);

    [SerializeField] private bool isAdaptiveBanner = true;

    private bool isInitialized;
    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    private float bannerHeight = 0f;
    private Configuration configuration;

    public event Action InterstitialAdComplete;
    public event Action RewardedAdFail;
    public event Action RewardedAdSuccess;
    public event Action<MaxSdkBase.AdInfo> AdRevenuePaid;

    public bool IsInitialized => isInitialized;
    public float BannerHeight
    {
        get
        {
            if (Mathf.Approximately(bannerHeight, 0f))
            {
                float height = 50f;
                float density = MaxSdkUtils.GetScreenDensity();

                if (Application.isEditor)
                {
                    if (isAdaptiveBanner)
                    {
                        height = MaxSdkUtils.GetAdaptiveBannerHeight();
                    }
                    else if (MaxSdkUtils.IsTablet())
                    {
                        height = 90f;
                    }
                }

                bannerHeight = height * density;
            }

            return bannerHeight;
        }
    }
    public Configuration Configuration => configuration;

    public void Initialize()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += OnInitialized;

        MaxSdk.SetSdkKey(SdkKey);
        MaxSdk.SetUserId(UserId);
        MaxSdk.InitializeSdk();
    }

    private void OnInitialized(Configuration configuration)
    {
        Debug.Log("Max sdk initialized successfully");

        InitializeInterstitialAds();
        InitializeRewardedAd();
        InitializeBannerAd();
        InitializeMRecAd();

        //MaxSdk.ShowMediationDebugger();

        isInitialized = true;
        this.configuration = configuration;
    }

    public void LoadAndShowCmpFlow()
    {
        var cmpService = MaxSdk.CmpService;

        cmpService.ShowCmpForExistingUser(error =>
        {
            if (error == null)
            {
                // The CMP alert was shown successfully.
            }
        });
    }

    #region INTERSTITIAL ADS
    public void InitializeInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialAdLoaded;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialAdLoadFailed;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialAdDisplayed;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialAdClicked;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialAdHidden;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdDisplayFailed;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaid;

        LoadInterstitialAd();
    }

    private void LoadInterstitialAd()
    {
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }

    private void OnInterstitialAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        interstitialRetryAttempt = 0;
    }

    private void OnInterstitialAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Invoke(nameof(LoadInterstitialAd), (float)retryDelay);
    }

    private void OnInterstitialAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        InterstitialAdComplete?.Invoke();
    }

    private void OnInterstitialAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitialAd();
        InterstitialAdComplete?.Invoke();
    }

    private void OnInterstitialAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitialAd();
    }

    private void OnInterstitialAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdRevenuePaid?.Invoke(adInfo);
    }

    public bool IsInterstitialAdReady()
    {
        return MaxSdk.IsInterstitialReady(interstitialAdUnitId);
    }

    public void ShowInterstitialAd()
    {
        if (IsInterstitialAdReady())
        {
            MaxSdk.ShowInterstitial(interstitialAdUnitId);
        }
        else
        {
            LoadInterstitialAd();
            InterstitialAdComplete?.Invoke();
        }
    }
    #endregion

    #region REWARDED AD
    public void InitializeRewardedAd()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoaded;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailed;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayed;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClicked;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHidden;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdDisplayFailed;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedReward;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaid;

        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    private void OnRewardedAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    private void OnRewardedAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        RewardedAdFail?.Invoke();
    }

    private void OnRewardedAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        RewardedAdFail?.Invoke();
    }

    private void OnRewardedAdReceivedReward(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        RewardedAdSuccess?.Invoke();
    }

    private void OnRewardedAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdRevenuePaid?.Invoke(adInfo);
    }

    public bool IsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(rewardedAdUnitId);
    }

    public void ShowRewardedAd()
    {
        if (IsRewardedAdReady())
        {
            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
        }
        else
        {
            LoadRewardedAd();
            RewardedAdFail?.Invoke();
        }
    }
    #endregion

    #region BANNER AD
    public void InitializeBannerAd()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoaded;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailed;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClicked;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpanded;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsed;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaid;

        // Banners are automatically sized to 320/50 on phones and 728/90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        if (isAdaptiveBanner)
        {
            MaxSdk.SetBannerExtraParameter(bannerAdUnitId, "adaptive_banner", "true");
        }

        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, bannerColor);
    }

    private void OnBannerAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

    private void OnBannerAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdRevenuePaid?.Invoke(adInfo);
    }

    private void OnBannerAdExpanded(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsed(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void ShowBanner()
    {
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerAdUnitId);
    }
    #endregion

    #region MREC AD
    public void InitializeMRecAd()
    {
        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(mrecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoaded;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailed;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClicked;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpanded;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsed;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaid;
    }

    public void OnMRecAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo error) { }

    public void OnMRecAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdExpanded(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsed(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdRevenuePaid?.Invoke(adInfo);
    }

    public void ShowMRec()
    {
        MaxSdk.ShowMRec(mrecAdUnitId);
    }

    public void HideMRec()
    {
        MaxSdk.HideMRec(mrecAdUnitId);
    }
    #endregion
}
