using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump.Api;
using System;
using UnityEngine;

public class AdMobManager : SingletonMonoBehaviour<AdMobManager>
{
#if UNITY_ANDROID
    // TEST ID
    //private const string appOpenAdUnitId = "ca-app-pub-3940256099942544/9257395921";
    //private const string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";

    // RELEASE ID
    private const string appOpenAdUnitId = "";
    private const string bannerAdUnitId = "";
#elif UNITY_IOS
    private const string appOpenAdUnitId = "";
    private const string bannerAdUnitId = "";
#else
    private const string appOpenAdUnitId = "";
    private const string bannerAdUnitId = "";
#endif

    private bool isInitialized;
    private DateTime expireTime;
    private AppOpenAd appOpenAd;
    private BannerView bannerView;

    public bool IsInitialized => isInitialized;
    public bool IsAppOpenAdAvailable => appOpenAd != null && appOpenAd.CanShowAd() && DateTime.Now < expireTime;

    private void Awake()
    {
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private void OnAppStateChanged(AppState state)
    {
        if (state == AppState.Foreground)
        {
            //ShowAppOpenAd();
        }
    }

    public void Initialize()
    {
        MobileAds.Initialize(status =>
        {
            Debug.Log("AdMob initialized successfully");

            isInitialized = true;

            LoadAppOpenAd();
            LoadBannerAd(false);
        });
    }

    public bool IsConsentFormRequired()
    {
        return ConsentInformation.PrivacyOptionsRequirementStatus ==
            PrivacyOptionsRequirementStatus.Required;
    }

    #region APP OPEN AD
    public void LoadAppOpenAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        var adRequest = new AdRequest();

        AppOpenAd.Load(appOpenAdUnitId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("App open ad failed to load an ad with error: " + error);
                return;
            }

            Debug.Log("App open ad loaded with response: " + ad.GetResponseInfo());
            expireTime = DateTime.Now + TimeSpan.FromHours(4);
            appOpenAd = ad;
            RegisterEventHandlers(ad);
        });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.LogFormat("App open ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content with error: " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadAppOpenAd();
        };
    }

    public void ShowAppOpenAd()
    {
        if (IsAppOpenAdAvailable)
        {
            appOpenAd.Show();
        }
        else
        {
            Debug.LogWarning("App open ad is not ready yet.");
        }
    }
    #endregion

    #region BANNER AD
    public void LoadBannerAd(bool isCollapsible)
    {
        DestroyBannerAd();

        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        var adRequest = new AdRequest();

        if (isCollapsible)
        {
            adRequest.Extras.Add("collapsible", "bottom");
        }

        bannerView.LoadAd(adRequest);
        RegisterEventHandlers(bannerView);
    }

    private void RegisterEventHandlers(BannerView bannerView)
    {
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.LogFormat("Banner ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner ad recorded an impression.");
        };
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner ad was clicked.");
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad full screen content opened.");
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner full screen content closed.");
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner ad load failed: " + error.ToString());
        };
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded");
        };
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion
}
