using AppsFlyerSDK;
using System.Collections.Generic;
using UnityEngine;

public static class AppsFlyerManager
{
    public static void Initialize()
    {
        AppsFlyerAdRevenue.start();
    }

    #region SEND EVENT
    public static void SendEvent(string name, Dictionary<string, string> values)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        AppsFlyer.sendEvent(name, values);
    }

    #region IAP EVENT
    public static void SendIapPurchase(decimal revenue, string currency, int quantity, string contentId)
    {
        float modifiedRevenue = (float)revenue * 0.63f;
        string revenueString = modifiedRevenue.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);

        SendEvent("af_Purchase_abi", new()
        {
            { "af_revenue", revenueString },
            { "af_currency", currency.ToString() },
            { "af_quantity", quantity.ToString() },
            { "af_content_id", contentId.ToString() },
        });
    }
    #endregion

    #region AD EVENT
    /// <summary>
    /// Trigger khi gọi hàm show interstitial ad
    /// </summary>
    public static void SendInterstitialAdEligible()
    {
        SendEvent("af_inters_ad_eligible", new());
    }

    /// <summary>
    /// Trigger khi load interstitial ad thành công (event của mediation)
    /// </summary>
    public static void SendInterstitialAdApiCalled()
    {
        SendEvent("af_inters_api_called", new());
    }

    /// <summary>
    /// Trigger khi interstitial ad hiển thị thành công (event của mediation)
    /// </summary>
    public static void SendInterstitialAdDisplayed()
    {
        SendEvent("af_inters_displayed", new());
    }

    /// <summary>
    /// Trigger khi gọi hàm show rewarded ad
    /// </summary>
    public static void SendRewardedAdEligible()
    {
        SendEvent("af_rewarded_ad_eligible", new());
    }

    /// <summary>
    /// Trigger khi load rewarded ad thành công (event của mediation)
    /// </summary>
    public static void SendRewardedAdApiCalled()
    {
        SendEvent("af_rewarded_api_called", new());
    }

    /// <summary>
    /// Trigger khi hiển thị rewarded ad thành công (event của mediation)
    /// </summary>
    public static void SendRewardedAdDisplayed()
    {
        SendEvent("af_rewarded_displayed", new());
    }

    /// <summary>
    /// Trigger khi user tắt rewarded ad và nhận reward (event của mediation)
    /// </summary>
    public static void SendRewardedAdComplete()
    {
        SendEvent("af_rewarded_ad_completed", new());
    }
    #endregion
    #endregion

    #region LOG AD REVENUE
    public static void LogAdRevenue(double revenue, Dictionary<string, string> parameters)
    {
        AppsFlyerAdRevenue.logAdRevenue("applovin_max",
            AppsFlyerAdRevenueMediationNetworkType.
            AppsFlyerAdRevenueMediationNetworkTypeApplovinMax,
            revenue, "USD", parameters);
    }
    #endregion
}
