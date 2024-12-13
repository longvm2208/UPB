//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
//using GoogleMobileAds.Ump.Api;
//using System;
//using UnityEngine;

//public class AdMobManager : SingletonMonoBehaviour<AdMobManager>
//{
//    // TEST
//    const string appOpenAdUnitId = "ca-app-pub-3940256099942544/9257395921";
//    const string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";

//    // PRODUCT
////#if UNITY_ANDROID
////    const string appOpenAdUnitId = "";
////    const string bannerAdUnitId = "";
////#elif UNITY_IOS
////    const string appOpenAdUnitId = "";
////    const string bannerAdUnitId = "";
////#else
////    const string appOpenAdUnitId = "";
////    const string bannerAdUnitId = "";
////#endif

//    [SerializeField] bool checkConsentStatusOnStart = false;

//    bool canRequestAd;
//    bool isInitialized;
//    DateTime expireTime;
//    AppOpenAd appOpenAd;
//    BannerView bannerView;

//    public bool CanRequestAd => canRequestAd;
//    public bool IsInitialized => isInitialized;
//    public bool IsAppOpenAdAvailable => appOpenAd != null && appOpenAd.CanShowAd() && DateTime.Now < expireTime;

//    void Awake()
//    {
//        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
//    }

//    void Start()
//    {
//        if (checkConsentStatusOnStart)
//        {
//            CheckConsentStatus();
//        }
//    }

//    void OnDestroy()
//    {
//        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
//    }

//    void OnAppStateChanged(AppState state)
//    {
//        if (state == AppState.Foreground)
//        {
//            //ShowAppOpenAd();
//        }
//    }

//    public void CheckConsentStatus()
//    {
//        // Create a ConsentRequestParameters object.
//        ConsentRequestParameters request = new ConsentRequestParameters();

//        // Check the current consent information status.
//        ConsentInformation.Update(request, OnConsentInfoUpdated);
//    }

//    void OnConsentInfoUpdated(FormError consentError)
//    {
//        try
//        {
//            if (consentError != null)
//            {
//                // Handle the error.
//                Debug.LogError(consentError);

//                canRequestAd = true;

//                return;
//            }

//            // If the error is null, the consent information state was updated.
//            // You are now ready to check if a form is available.
//            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
//            {
//                if (formError != null)
//                {
//                    // Consent gathering failed.
//                    Debug.LogError(formError);

//                    canRequestAd = true;

//                    return;
//                }

//                // Consent has been gathered.
//                canRequestAd = true;
//            });

//            if (ConsentInformation.CanRequestAds())
//            {
//                canRequestAd = true;
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.LogError(e);

//            canRequestAd = true;
//        }
//    }

//    public bool IsPrivacyOptionsRequired()
//    {
//        return ConsentInformation.PrivacyOptionsRequirementStatus ==
//            PrivacyOptionsRequirementStatus.Required;
//    }

//    public void ShowPrivacyOptionsForm()
//    {
//        Debug.Log("Showing privacy options form.");

//        ConsentForm.ShowPrivacyOptionsForm((FormError showError) =>
//        {
//            if (showError != null)
//            {
//                Debug.LogError("Error showing privacy options form with error: " + showError.Message);
//            }
//        });
//    }

//    public void Initialize()
//    {
//        MobileAds.Initialize(status =>
//        {
//            Debug.Log("AdMob initialized successfully");

//            isInitialized = true;

//            LoadAppOpenAd();
//            LoadBannerAd(false);
//        });
//    }

//    #region APP OPEN AD
//    public void LoadAppOpenAd()
//    {
//        if (appOpenAd != null)
//        {
//            appOpenAd.Destroy();
//            appOpenAd = null;
//        }

//        var adRequest = new AdRequest();

//        AppOpenAd.Load(appOpenAdUnitId, adRequest, (ad, error) =>
//        {
//            if (error != null || ad == null)
//            {
//                Debug.LogError("App open ad failed to load an ad with error: " + error);
//                return;
//            }

//            Debug.Log("App open ad loaded with response: " + ad.GetResponseInfo());
//            expireTime = DateTime.Now + TimeSpan.FromHours(4);
//            appOpenAd = ad;
//            RegisterEventHandlers(ad);
//        });
//    }

//    void RegisterEventHandlers(AppOpenAd ad)
//    {
//        // Raised when the ad is estimated to have earned money.
//        ad.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.LogFormat("App open ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);
//        };
//        // Raised when an impression is recorded for an ad.
//        ad.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("App open ad recorded an impression.");
//        };
//        // Raised when a click is recorded for an ad.
//        ad.OnAdClicked += () =>
//        {
//            Debug.Log("App open ad was clicked.");
//        };
//        // Raised when an ad opened full screen content.
//        ad.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("App open ad full screen content opened.");
//        };
//        // Raised when the ad closed full screen content.
//        ad.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("App open ad full screen content closed.");

//            // Reload the ad so that we can show another as soon as possible.
//            LoadAppOpenAd();
//        };
//        // Raised when the ad failed to open full screen content.
//        ad.OnAdFullScreenContentFailed += (AdError error) =>
//        {
//            Debug.LogError("App open ad failed to open full screen content with error: " + error);

//            // Reload the ad so that we can show another as soon as possible.
//            LoadAppOpenAd();
//        };
//    }

//    public void ShowAppOpenAd()
//    {
//        if (IsAppOpenAdAvailable)
//        {
//            appOpenAd.Show();
//        }
//        else
//        {
//            Debug.LogWarning("App open ad is not ready yet.");
//        }
//    }
//    #endregion

//    #region BANNER AD
//    public void LoadBannerAd(bool isCollapsible)
//    {
//        DestroyBannerAd();

//        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
//        var adRequest = new AdRequest();

//        if (isCollapsible)
//        {
//            adRequest.Extras.Add("collapsible", "bottom");
//        }

//        bannerView.LoadAd(adRequest);
//        RegisterEventHandlers(bannerView);
//    }

//    void RegisterEventHandlers(BannerView bannerView)
//    {
//        bannerView.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.LogFormat("Banner ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode);
//        };
//        bannerView.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("Banner ad recorded an impression.");
//        };
//        bannerView.OnAdClicked += () =>
//        {
//            Debug.Log("Banner ad was clicked.");
//        };
//        bannerView.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("Banner ad full screen content opened.");
//        };
//        bannerView.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("Banner full screen content closed.");
//        };
//        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
//        {
//            Debug.LogError("Banner ad load failed: " + error.ToString());
//        };
//        bannerView.OnBannerAdLoaded += () =>
//        {
//            Debug.Log("Banner ad loaded");
//        };
//    }

//    public void DestroyBannerAd()
//    {
//        if (bannerView != null)
//        {
//            bannerView.Destroy();
//            bannerView = null;
//        }
//    }
//    #endregion
//}
