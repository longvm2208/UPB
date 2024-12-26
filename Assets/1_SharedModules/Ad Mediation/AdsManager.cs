using UnityEngine;

public class AdsManager : SingletonMonoBehaviour<AdsManager>
{
    const string RemoveAdsKey = "RemoveAds";

    [SerializeField] bool adsEnabled = true;
    public bool AdsEnabled => adsEnabled;
    [SerializeField] bool adsRemoved;
    public bool AdsRemoved => adsRemoved;
    [SerializeField] float interstitialCapping = 15;
    public float InterstitialCapping => interstitialCapping;
    [SerializeField] float rewardedCapping = 15;
    public float RewardedCapping => rewardedCapping;

    public void Init()
    {
        adsRemoved = PlayerPrefs.GetInt(RemoveAdsKey, 0) == 1;

        MaxManager.Ins.Init();
    }
}
