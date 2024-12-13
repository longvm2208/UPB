using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : SingletonMonoBehaviour<AdsManager>
{
    [SerializeField] bool adsEnabled;
    public bool AdsEnabled => adsEnabled;
    [SerializeField] float interstitialCap = 15;
    public float InterstitialCap => interstitialCap;
    [SerializeField] float rewardedCap = 15;
    public float RewardedCap => rewardedCap;

}
