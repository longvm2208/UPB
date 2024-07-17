using Firebase.Analytics;
using System.Collections.Generic;

public struct ImpressionData
{
    public string Platform;
    public string Format;
    public string CountryCode;
    public string AdUnitId;
    public string Network;
    public string Placement;
    public string Currency;
    public double Revenue;

    public ImpressionData(string platform, string format, string countryCode, string adUnitId, string network, string placement, string currency, double revenue)
    {
        Platform = platform;
        Format = format;
        CountryCode = countryCode;
        AdUnitId = adUnitId;
        Network = network;
        Placement = placement;
        Currency = currency;
        Revenue = revenue;
    }

    public ImpressionData(string platform, string format, string countryCode, string adUnitId, string network, string placement, string currency, double? revenue)
    {
        Platform = platform;
        Format = format;
        CountryCode = countryCode;
        AdUnitId = adUnitId;
        Network = network;
        Placement = placement;
        Currency = currency;
        Revenue = revenue == null ? 0 : (double)revenue;
    }

    public Dictionary<string, string> ToDictionary()
    {
        Dictionary<string, string> dictionary = new()
        {
            { "ad_platform", Platform },
            { "ad_format", Format },
            { "country_code", CountryCode },
            { "ad_unit_id", AdUnitId },
            { "network", Network },
            { "placement", Placement },
            { "value", Revenue.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) }
        };

        return dictionary;
    }

    public Parameter[] ToParameters()
    {
        Parameter[] parameters =
        {
            new Parameter("ad_platform", Platform),
            new Parameter("ad_format", Format),
            new Parameter("country_code", CountryCode),
            new Parameter("ad_unit_id", AdUnitId),
            new Parameter("network", Network),
            new Parameter("placement", Placement),
            new Parameter("currency", Currency),
            new Parameter("value", Revenue),
        };

        return parameters;
    }
}
