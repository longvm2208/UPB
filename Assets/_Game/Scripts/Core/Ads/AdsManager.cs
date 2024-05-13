using System;
using UnityEngine;

public class AdsManager : SingletonMonoBehaviour<AdsManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject adNotLoadedYet;
    [Space]
    [SerializeField] private float interstitialTimeCounter = -1f;

    private bool isShowingInterstitialAd;
    private bool isShowingRewardedAd;
    private float bannerHeight = 0f;
    private string placement = null;

    public event Action InterstitialAdComplete;
    public event Action RewardedAdFail;
    public event Action RewardedAdSuccess;

    public bool IsShowingFullscreenAd => isShowingInterstitialAd || isShowingRewardedAd;
    public float BannerHeight
    {
        get
        {
            if (Mathf.Approximately(bannerHeight, 0f))
            {
                if (Application.isEditor)
                {
                    bannerHeight = 168f;
                }
                else
                {
                    bannerHeight = MaxManager.Instance.BannerHeight / canvas.scaleFactor;
                }
            }

            return bannerHeight;
        }
    }

    private void Update()
    {
        if (interstitialTimeCounter > 0f)
        {
            interstitialTimeCounter -= Time.deltaTime;
        }
    }

    public void Initialize()
    {
        MaxManager.Instance.InterstitialAdComplete += OnInterstitialComplete;
        MaxManager.Instance.RewardedAdFail += OnRewardedAdFail;
        MaxManager.Instance.RewardedAdSuccess += OnRewardedAdSuccess;
        MaxManager.Instance.AdRevenuePaid += OnAdRevenuePaid;

        MaxManager.Instance.Initialize();
        AdMobManager.Instance.Initialize();
    }

    private void OnInterstitialComplete()
    {
        isShowingInterstitialAd = false;
        blocker.SetActive(false);
        InterstitialAdComplete?.Invoke();
    }

    private void OnRewardedAdFail()
    {
        isShowingRewardedAd = false;
        blocker.SetActive(false);
        RewardedAdFail?.Invoke();
    }

    private void OnRewardedAdSuccess()
    {
        isShowingRewardedAd = false;
        blocker.SetActive(false);
        RewardedAdSuccess?.Invoke();

        if (placement != null)
        {
            FirebaseManager.Instance.LogRewardedAdSuccess(placement);
        }
    }

    private void OnAdRevenuePaid(MaxSdkBase.AdInfo adInfo)
    {
        ImpressionData data = new ImpressionData(
            "applovin_max",
            adInfo.AdFormat,
            MaxManager.Instance.Configuration.CountryCode,
            adInfo.AdUnitIdentifier,
            adInfo.NetworkName,
            adInfo.Placement,
            "USD",
            adInfo.Revenue);

        FirebaseManager.Instance.LogAdRevenuePaid(data);
    }

    public void SetPlacement(string placement)
    {
        this.placement = placement;
    }

    public void ShowInterstitialAd(Action onComplete = null)
    {
        MaxManager.Instance.ShowInterstitialAd();
        isShowingInterstitialAd = true;
        blocker.SetActive(true);
        InterstitialAdComplete = onComplete;
    }

    public bool IsRewardedAdReady()
    {
        return MaxManager.Instance.IsRewardedAdReady();
    }

    public void ShowRewardedAd(Action onSuccess = null, Action onFail = null)
    {
        if (!IsRewardedAdReady())
        {
            adNotLoadedYet.SetActive(true);
        }

        MaxManager.Instance.ShowRewardedAd();
        isShowingRewardedAd = true;
        blocker.SetActive(true);
        RewardedAdFail = onFail;
        RewardedAdSuccess = onSuccess;
    }

    public void ShowBanner()
    {
        HideMRec();
        MaxManager.Instance.ShowBanner();
    }

    public void HideBanner()
    {
        MaxManager.Instance.HideBanner();
    }

    public void ShowMRec()
    {
        HideBanner();
        MaxManager.Instance.ShowMRec();
    }

    public void HideMRec()
    {
        MaxManager.Instance.HideMRec();
    }

    public void ShowAppOpenAd()
    {
        AdMobManager.Instance.ShowAppOpenAd();
    }

    public void LoadBannerAd(bool isCollapsible)
    {
        AdMobManager.Instance.LoadBannerAd(isCollapsible);
    }
}