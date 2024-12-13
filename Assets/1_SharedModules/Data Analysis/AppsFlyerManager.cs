using AppsFlyerSDK;
using System.Collections.Generic;
using UnityEngine;

public static class AppsFlyerManager
{
    public static void Init()
    {
        AppsFlyerAdRevenue.start();
    }

    #region SEND EVENT
    public static void SendEvent(string name, Dictionary<string, string> values)
    {
        if (Debug.isDebugBuild || Application.isEditor) return;

        AppsFlyer.sendEvent(name, values);
    }

    #region Interstitial
    public static void af_inters_ad_eligible()
    {
        SendEvent("af_inters_ad_eligible", new());
    }

    public static void af_inters_api_called()
    {
        SendEvent("af_inters_api_called", new());
    }

    public static void af_inters_displayed()
    {
        SendEvent("af_inters_displayed", new());
    }
    #endregion

    #region Rewarded
    public static void af_rewarded_ad_eligible()
    {
        SendEvent("af_rewarded_ad_eligible", new());
    }

    public static void af_rewarded_api_called()
    {
        SendEvent("af_rewarded_api_called", new());
    }

    public static void af_rewarded_displayed()
    {
        SendEvent("af_rewarded_displayed", new());
    }

    public static void af_rewarded_ad_completed()
    {
        SendEvent("af_rewarded_ad_completed", new());
    }
    #endregion

    public static void af_purchase(decimal af_revenue, string af_currency, int af_quantity, string af_content_id)
    {
        float modifiedRevenue = (float)af_revenue * 0.63f;
        SendEvent("af_purchase", new()
        {
            { "af_revenue", modifiedRevenue.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) },
            { "af_currency", af_currency },
            { "af_quantity", af_quantity.ToString() },
            { "af_content_id", af_content_id },
        });
    }
    #endregion

    #region Log Ad Revenue
    public static void LogAdRevenue(double revenue, Dictionary<string, string> parameters)
    {
        AppsFlyerAdRevenue.logAdRevenue("applovin_max",
            AppsFlyerAdRevenueMediationNetworkType.
            AppsFlyerAdRevenueMediationNetworkTypeApplovinMax,
            revenue, "USD", parameters);
    }
    #endregion
}
