using Firebase.Analytics;
using System.Collections.Generic;

public struct ImpressionData
{
    public string platform;
    public string format;
    public string countryCode;
    public string adUnitId;
    public string network;
    public string placement;
    public string currency;
    public double revenue;

    public ImpressionData(string platform, string format, string countryCode, string adUnitId, string network, string placement, string currency, double revenue)
    {
        this.platform = platform;
        this.format = format;
        this.countryCode = countryCode;
        this.adUnitId = adUnitId;
        this.network = network;
        this.placement = placement;
        this.currency = currency;
        this.revenue = revenue;
    }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dictionary = new()
        {
            { "ad_platform", platform },
            { "ad_format", format },
            { "country_code", countryCode },
            { "ad_unit_id", adUnitId },
            { "network", network },
            { "placement", placement },
            { "value", revenue.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) }
        };

        return dictionary;
    }

    public Parameter[] ToParameters()
    {
        Parameter[] parameters =
        {
            new Parameter("ad_platform", platform),
            new Parameter("ad_format", format),
            new Parameter("country_code", countryCode),
            new Parameter("ad_unit_id", adUnitId),
            new Parameter("network", network),
            new Parameter("placement", placement),
            new Parameter("currency", currency),
            new Parameter("value", revenue),
        };

        return parameters;
    }
}
