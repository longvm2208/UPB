using System;

[Serializable]
public class InterstitialAdConfig
{
    public int EnableLevel;
    public int Capping;

    public InterstitialAdConfig(int enableLevel, int capping)
    {
        EnableLevel = enableLevel;
        Capping = capping;
    }
}
