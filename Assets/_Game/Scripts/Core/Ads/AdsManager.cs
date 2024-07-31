using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : SingletonMonoBehaviour<AdManager>
{
#if UNITY_EDITOR
    [SerializeField, ExposedScriptableObject]
    MediationConfig mediationConfig;
#endif
    [SerializeField] float interstitialTimeCounter = -1f;
    [SerializeField] float rewardTimeCounter = -1f;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject adNotLoadedYet;

    event Action interstitialAdLoaded;
    event Action interstitialAdDisplayed;
    event Action interstitialAdFailedToDisplay;
    event Action interstitialAdClicked;

    event Action rewardedAdDisplayed;
    event Action<string> rewardedAdFailedToDisplay;
    event Action rewardedAdHidden;
    event Action rewardedAdReceivedReward;

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

    void Start()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.Initialized += OnInitialized;

        MaxManager.Instance.InterstitialAdLoaded += OnInterstitialAdLoaded;
        MaxManager.Instance.InterstitialAdDisplayed += OnInterstitialAdDisplayed;
        MaxManager.Instance.InterstitialAdFailedToDisplay += OnInterstitialAdFailedToDisplay;
        MaxManager.Instance.InterstitialAdClicked += OnInterstitialAdClicked;

        MaxManager.Instance.RewardedAdLoaded += OnRewardedAdLoaded;
        MaxManager.Instance.RewardedAdDisplayed += OnRewardedAdDisplayed;
        MaxManager.Instance.RewardedAdFailedToDisplay += OnRewardedAdFailedToDisplay;
        MaxManager.Instance.RewardedAdHidden += OnRewardedAdHidden;
        MaxManager.Instance.RewardedAdReceivedReward += OnRewardedAdReceivedReward;

        MaxManager.Instance.AdRevenuePaid += OnAdRevenuePaid;
#elif IRON_SOURCE
        IronSourceManager.Instance.Initialized += OnInitialized;

        IronSourceManager.Instance.InterstitialAdLoaded += OnInterstitialAdLoaded;
        IronSourceManager.Instance.InterstitialAdOpened += OnInterstitialAdDisplayed;
        IronSourceManager.Instance.InterstitialAdShowFailed += OnInterstitialAdFailedToDisplay;
        IronSourceManager.Instance.InterstitialAdClicked += OnInterstitialAdClicked;

        IronSourceManager.Instance.RewardedAdAvailable += OnRewardedAdLoaded;
        IronSourceManager.Instance.RewardedAdOpened += OnRewardedAdDisplayed;
        IronSourceManager.Instance.RewardedAdShowFailed += OnRewardedAdFailedToDisplay;
        IronSourceManager.Instance.RewardedAdClosed += OnRewardedAdHidden;
        IronSourceManager.Instance.RewardedAdReceivedReward += OnRewardedAdReceivedReward;

        IronSourceManager.Instance.ImpressionDataReady += OnAdRevenuePaid;
#endif
    }

    void OnDestroy()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.Initialized -= OnInitialized;

        MaxManager.Instance.InterstitialAdLoaded -= OnInterstitialAdLoaded;
        MaxManager.Instance.InterstitialAdDisplayed -= OnInterstitialAdDisplayed;
        MaxManager.Instance.InterstitialAdFailedToDisplay -= OnInterstitialAdFailedToDisplay;
        MaxManager.Instance.InterstitialAdClicked -= OnInterstitialAdClicked;

        MaxManager.Instance.RewardedAdLoaded -= OnRewardedAdLoaded;
        MaxManager.Instance.RewardedAdDisplayed -= OnRewardedAdDisplayed;
        MaxManager.Instance.RewardedAdFailedToDisplay -= OnRewardedAdFailedToDisplay;
        MaxManager.Instance.RewardedAdHidden -= OnRewardedAdHidden;
        MaxManager.Instance.RewardedAdReceivedReward -= OnRewardedAdReceivedReward;

        MaxManager.Instance.AdRevenuePaid -= OnAdRevenuePaid;
#elif IRON_SOURCE
        IronSourceManager.Instance.Initialized -= OnInitialized;

        IronSourceManager.Instance.InterstitialAdLoaded -= OnInterstitialAdLoaded;
        IronSourceManager.Instance.InterstitialAdOpened -= OnInterstitialAdDisplayed;
        IronSourceManager.Instance.InterstitialAdShowFailed -= OnInterstitialAdFailedToDisplay;
        IronSourceManager.Instance.InterstitialAdClicked -= OnInterstitialAdClicked;

        IronSourceManager.Instance.RewardedAdAvailable -= OnRewardedAdLoaded;
        IronSourceManager.Instance.RewardedAdOpened -= OnRewardedAdDisplayed;
        IronSourceManager.Instance.RewardedAdShowFailed -= OnRewardedAdFailedToDisplay;
        IronSourceManager.Instance.RewardedAdClosed -= OnRewardedAdHidden;
        IronSourceManager.Instance.RewardedAdReceivedReward -= OnRewardedAdReceivedReward;

        IronSourceManager.Instance.ImpressionDataReady -= OnAdRevenuePaid;
#endif
    }

    void Update()
    {
        if (interstitialTimeCounter > 0)
        {
            interstitialTimeCounter -= Time.deltaTime;
        }

        if (rewardTimeCounter > 0)
        {
            rewardTimeCounter -= Time.deltaTime;
        }
    }

    public void Initialize()
    {
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

        CalculateBannerHeight();
    }

    void OnInitialized()
    {
        StartCoroutine(ShowBannerRoutine());
    }

    IEnumerator ShowBannerRoutine()
    {
        yield return new WaitUntil(() =>
            DataManager.Instance.IsLoaded &&
            LoadSceneManager.Instance.CurrentScene != SceneId.Load &&
            LoadSceneManager.Instance.CurrentScene != SceneId.None);

        ShowBanner();
    }

    void NotifyAdNotLoadedYet() => adNotLoadedYet.gameObject.SetActive(true);
    void EnableBlocker() => blocker.SetActive(true);
    void DisableBlocker() => blocker.SetActive(false);

    #region PRIVACY OPTIONS FORM
    public void ShowPrivacyOptionsForm()
    {
#if APPLOVIN_MAX
        MaxManager.Instance.ShowPrivacyOptionsForm();
#elif IRON_SOURCE
        AdMobManager.Instance.ShowPrivacyOptionsForm();
#endif
    }

    public bool IsPrivacyOptionsRequired()
    {
#if APPLOVIN_MAX
        return MaxManager.Instance.IsPrivacyOptionsRequired();
#elif IRON_SOURCE
        return AdMobManager.Instance.IsPrivacyOptionsRequired();
#else
        return false;
#endif
    }
    #endregion

    #region INTERSTITIAL AD
    void OnInterstitialAdLoaded()
    {
        AppsFlyerManager.Instance.SendInterstitialAdApiCalled();
        interstitialAdLoaded?.Invoke();
    }

    void OnInterstitialAdDisplayed()
    {
        AppsFlyerManager.Instance.SendInterstitialAdDisplayed();
        DisableBlocker();
        interstitialAdDisplayed?.Invoke();
    }

    void OnInterstitialAdFailedToDisplay()
    {
        DisableBlocker();
        interstitialAdFailedToDisplay?.Invoke();
    }

    void OnInterstitialAdClicked()
    {
        interstitialAdClicked?.Invoke();
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
#else
        return false;
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

    bool CanShowInterstitial() => GameSettings.Instance.IsEnableAds && !GameData.Instance.IsRemoveAds && interstitialTimeCounter <= 0 && rewardTimeCounter <= 0;

    void SetupShowInterstitial(string placement, Action onFinished)
    {
        AppsFlyerManager.Instance.SendInterstitialAdEligible();
        interstitialTimeCounter = ConfigManager.Instance.InterstitialAdCapping;
        EnableBlocker();

        interstitialAdDisplayed = () =>
        {
            FirebaseManager.Instance.LogInterstitialAdShow(placement);
            onFinished?.Invoke();
        };

        interstitialAdFailedToDisplay = () =>
        {
            FirebaseManager.Instance.LogInterstitialAdFailed(placement);
            onFinished?.Invoke();
        };

        interstitialAdLoaded = () =>
        {
            FirebaseManager.Instance.LogInterstitialAdLoad(placement);
        };

        interstitialAdClicked = () =>
        {
            FirebaseManager.Instance.LogInterstitialAdClick(placement);
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
            Debug.LogWarning("Interstitial ad not ready");

            LoadInterstitial();
            DisableBlocker();
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
    void OnRewardedAdLoaded()
    {
        AppsFlyerManager.Instance.SendRewardedAdApiCalled();
    }

    void OnRewardedAdDisplayed()
    {
        AppsFlyerManager.Instance.SendRewardedAdDisplayed();
        rewardedAdDisplayed?.Invoke();
    }

    void OnRewardedAdFailedToDisplay(string error)
    {
        DisableBlocker();
        rewardedAdFailedToDisplay?.Invoke(error);
    }

    void OnRewardedAdHidden()
    {
        DisableBlocker();
        rewardedAdHidden?.Invoke();
    }

    void OnRewardedAdReceivedReward()
    {
        AppsFlyerManager.Instance.SendRewardedAdComplete();
        DisableBlocker();
        rewardedAdReceivedReward?.Invoke();
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
#else
        return false;
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
        if (!GameSettings.Instance.IsEnableAds)
        {
            onReceivedReward?.Invoke();
            return;
        }

        AppsFlyerManager.Instance.SendRewardedAdEligible();
        FirebaseManager.Instance.LogRewardedAdClick(placement, buttonName);
        rewardTimeCounter = ConfigManager.Instance.RewardedAdCapping;
        EnableBlocker();

        rewardedAdDisplayed = () =>
        {
            FirebaseManager.Instance.LogRewardedAdShow(placement, buttonName);
        };

        rewardedAdReceivedReward = () =>
        {
            FirebaseManager.Instance.LogRewardedAdComplete(placement, buttonName);
            onReceivedReward?.Invoke();
        };

        rewardedAdHidden = () =>
        {
            onFailed?.Invoke();
        };

        rewardedAdFailedToDisplay = (errorInfo) =>
        {
            FirebaseManager.Instance.LogRewardedAdFailed(placement, buttonName);
            onFailed?.Invoke();
        };

        if (IsRewardedAdReady())
        {
            ShowRewardedAd();
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready");

            LoadRewardedAd();
            DisableBlocker();
            NotifyAdNotLoadedYet();
            onFailed?.Invoke();
        }
    }
    #endregion

    #region BANNER AD
    public void ShowBanner()
    {
        if (!GameSettings.Instance.IsEnableAds || GameData.Instance.IsRemoveAds) return;

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

    void CalculateBannerHeight()
    {
#if APPLOVIN_MAX
        bannerHeight = MaxManager.Instance.BannerHeight;
#elif IRON_SOURCE
        bannerHeight = IronSourceManager.Instance.BannerHeight;
#endif
    }
    #endregion

    #region REVENUE
    void OnAdRevenuePaid(ImpressionData data)
    {
        FirebaseManager.Instance.LogAdRevenue(data);
        AppsFlyerManager.Instance.LogAdRevenue(data.Revenue, data.ToDictionary());
    }
    #endregion
}
