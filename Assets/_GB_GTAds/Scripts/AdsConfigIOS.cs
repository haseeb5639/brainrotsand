using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AdsConfigIOS", menuName = "Custom/Ads Configuration (iOS)")]
public class AdsConfigIOS : ScriptableObject
{
    [PropertySpace(25)]
    [Title("ADMOB", null, TitleAlignments.Centered)]
    [ReadOnly]
    public string LowBanner = "default";
    [ReadOnly]
    public string BannerID = "default";

    [ReadOnly]
    public string CollapsibleBannerID = "default";

    [ReadOnly]
    public string AppOpenID = "default";
    [ReadOnly]
    public string InterstitialID = "default";
    [ReadOnly]
    public string RewardedID = "default";
    [ReadOnly]
    public string RewardedInterstitial = "default";
    [ReadOnly]
    public string NativeBanner = "default";
    [ReadOnly]
    public string LowNativeBanner = "default";
    [ReadOnly]
    public string AdmobAppID = "default";


    [PropertySpace(15)]
    [Title("MAX", null, TitleAlignments.Centered)]

    [ReadOnly]
    public string AppLovinSDKKey = "default";
    [ReadOnly]
    public string AppLovinBannerID = "default";
    [ReadOnly]
    public string AppLovinHighAppOpenID = "default";
    [ReadOnly]
    public string AppLovinLowAppOpenID = "default";
    [ReadOnly]
    public string AppLovinInterstitialID = "default";
    [ReadOnly]
    public string AppLovinRewardedID = "default";
    [ReadOnly]
    public string AppLovinNativeBannerID = "default";


    [PropertySpace(15)]
    [Title("LEVELPLAY", null, TitleAlignments.Centered)]
    [ReadOnly]
    public string LevelPlayKey = "default";
    [ReadOnly]
    public string LevelPlayBannerID = "default";
    [ReadOnly]
    public string LevelPlayInterstitialID = "default";
    [ReadOnly]
    public string LevelPlayRewardedID = "default";



    [PropertySpace(15)]
    [Title("PRODUCT INFO", null, TitleAlignments.Centered)]
    [ReadOnly]
    public string AppName = "default";
    [ReadOnly]
    public string PackageName = "default";
    [ReadOnly]
    public string Version = "default";
    [ReadOnly]
    public string BundleCode = "0";
    [ReadOnly]
    public string APPID = "default";


    [PropertySpace(15)]
    [Title("Config Links", null, TitleAlignments.Centered)]
    [ReadOnly]
    public string JsonLink = "";



}

