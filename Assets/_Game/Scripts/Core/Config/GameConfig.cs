using System;

[Serializable]
public class GameConfig
{
    public static readonly DateTime OriginalTime = new DateTime(2024, 2, 29);

    public AppOpenAdConfig AppOpenAd;
    public CommonConfig Common;
    public InterstitialAdConfig InterstitialAd;
}
