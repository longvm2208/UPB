using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsflyerEventRegister : MonoBehaviour
{
    private static void SendEvent(string name, Dictionary<string, string> values)
    {
        AppsFlyer.sendEvent(name, values);
    }

    #region IAP
    public static void SendIAPPurchased(decimal revenue, string currency, int quantity, string contentId)
    {
        float r = (float)revenue * 0.63f;

        SendEvent("iap_purchased", new()
        {
            { "revenue", r.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) },
            { "currency", currency.ToString() },
            { "quantity", quantity.ToString() },
            { "content_id", contentId.ToString() },
        });
    }
    #endregion

    #region ADS
    public static void SendRewardedAdSuccess()
    {
        SendEvent("rewarded_ad_success", new());
    }
    #endregion
}
