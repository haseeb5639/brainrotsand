
using com.unity3d.mediation;
using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_IOS
using DeadMosquito.IosGoodies;
#endif
public class LevelPlayAdsManager : MonoBehaviour
{
    public bool ShowDebug = false;
    [SerializeField] bool isinitialized = false;
    private Action<bool> rewardCallback;
#if UNITY_ANDROID
    AdsConfigAndroid ads;
#endif
#if UNITY_IOS
    AdsConfigIOS ads;
#endif
    #region Singleton
    private static LevelPlayAdsManager _Instance;
    public static LevelPlayAdsManager Instance
    {
        get
        {

            return _Instance;
        }
    }



    private void Awake()
    {
        if (!_Instance) _Instance = this;

    }
    #endregion
    private void Start()
    {
        // Init();

#if UNITY_IOS
        ads = AdsConfigLoader.Instance.adsConfigIOS;
#endif
#if UNITY_ANDROID

        ads = AdsConfigLoader.Instance.adsConfigAndroid;

#endif
    }

    #region Initialization
    void OnEnable()
    {
        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionSuccessEvent;

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdReadyEvent += RewardedVideoOnAdReadyEvent;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent += RewardedVideoOnAdLoadFailedEvent;
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;


    }
    private void OnDestroy()
    {
        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;

        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionDataReadyEvent -= ImpressionSuccessEvent;


        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdReadyEvent -= RewardedVideoOnAdReadyEvent;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent -= RewardedVideoOnAdLoadFailedEvent;
        IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;



        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;




        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent -= BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent -= BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent -= BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent -= BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent -= BannerOnAdLeftApplicationEvent;

    }
    public void Init()
    {
        if (isinitialized)
            return;
        IronSource.Agent.setMetaData("is_test_suite", "enable");

        IronSource.Agent.SetPauseGame(true);
        IronSource.Agent.setManualLoadRewardedVideo(true);

        IronSource.Agent.validateIntegration();
        IronSource.Agent.init(ads.LevelPlayKey);




    }
    private void SdkInitializationCompletedEvent()
    {

        isinitialized = true;
        IronSource.Agent.SetPauseGame(true);
        IronSource.Agent.setManualLoadRewardedVideo(true);
    }




    #endregion


    #region Banner

    public void ShowBanner()
    {
        if (!NativeAdsManager.Instance.CanShowBanner)
            return;
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
        {
            StartCoroutine(OnBannerFailed());
        }
        HideBanner();

        if (AdsManager.Instance.Banner == AdsManager.BannerType.SmallBanner)
        {
#if UNITY_IOS
            if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
            {
                IronSourceBannerSize Size = new IronSourceBannerSize(468, 90);
                IronSource.Agent.loadBanner(Size, GetBannerPos());
            }
            else
            {
                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, GetBannerPos());
            }
#endif
#if UNITY_ANDROID

            if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
            {
                IronSourceBannerSize Size = new IronSourceBannerSize(468, 90);
                IronSource.Agent.loadBanner(Size, GetBannerPos());
            }
            else
            {
                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, GetBannerPos());
            }
#endif
        }
        else if (AdsManager.Instance.Banner == AdsManager.BannerType.LargeBanner)
        {
            IronSourceBannerSize Size = IronSourceBannerSize.SMART;
            Size.SetAdaptive(true);
            IronSource.Agent.loadBanner(Size, GetBannerPos());
        }

        Debug.Log("aftab => Requsting LevelPlay Banner");
    }

    public void HideBanner()
    {
        IronSource.Agent.destroyBanner();
    }

    //Banner Events
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("aftab => LevelPlay Banner Loaded");
        Logger("unity-script: I got BannerAdLoadedEvent");
        GoogleAdsManager.Instance.HideBanner();
        MaxAdsManager.Instance.HideBanner();
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
        //LabAnalytics.Instance.LogEvent("ad_impression_IS_Banner");
    }

    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
        Debug.Log("aftab => LevelPlay Banner Failed");
        Logger("unity-script: I got BannerAdLoadFailedEvent, code: " + ironSourceError.getCode() + ", description : " + ironSourceError.getDescription());
        StartCoroutine(OnBannerFailed());
    }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got BannerAdClickedEvent");
    }

    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got BannerAdLeftApplicationEvent");
    }



    IEnumerator OnBannerFailed()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        HideBanner();
        
        if (MaxAdsManager.Instance.CanLoadBanner())
        {
            MaxAdsManager.Instance.ShowBanner();
        }
        
        else
        {
            if (NativeAdsManager.Instance.CanShowBanner)
            {

                if (AdsManager.Instance.Banner == AdsManager.BannerType.SmallBanner)
                {
                    InHouseAdsController.Instance.BannerInHouse.ShowAd();

                }
                else if (AdsManager.Instance.Banner == AdsManager.BannerType.LargeBanner)
                {
                    InHouseAdsController.Instance.LargeBannerInHouse.ShowAd();
                }
            }
        }


    }


    IronSourceBannerPosition GetBannerPos()
    {
        switch (AdsManager.Instance.bannerPosition)
        {
            case AdsManager.BannerPosition.Top:
                return IronSourceBannerPosition.TOP;
            case AdsManager.BannerPosition.Bottom:
                return IronSourceBannerPosition.BOTTOM;
            default:
                return IronSourceBannerPosition.TOP;
        }
    }



    #endregion

    #region Interstitial


    public void LoadInterstitial()
    {
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
            return;





        IronSource.Agent.loadInterstitial();

    }

    public void ShowInterstitialAd()
    {
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
            return;
        if (isInterstitialLoaded())
        {
            IronSource.Agent.showInterstitial();
        }
    }

    public bool isInterstitialLoaded()
    {
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
            return false;

        return IronSource.Agent.isInterstitialReady();
    }
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {

        Logger("unity-script: I got InterstitialAdReadyEvent");
    }

    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
        Logger("unity-script: I got InterstitialAdLoadFailedEvent, code: " + ironSourceError.getCode() + ", description : " + ironSourceError.getDescription());
    }

    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got InterstitialAdShowSucceededEvent");

        LabAnalytics.Instance.LogEvent("IS_Inter_Shown");
    }

    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        Logger("unity-script: I got InterstitialAdShowFailedEvent, code :  " + ironSourceError.getCode() + ", description : " + ironSourceError.getDescription());
        AdsManager.Instance.LoadAd();
    }

    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        AdsManager.Instance.LoadAd();
        AdsManager.Instance.ShowBanner();

    }

    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
    }


    #endregion

    #region Rewarded

    public void LoadRewarded()
    {
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
            return;
        IronSource.Agent.loadRewardedVideo();
    }

    public void ShowRewardedAd(Action<bool> callback = null)
    {
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
            return;

        if (isRewardedLoaded())
        {
            rewardCallback = callback;
            IronSource.Agent.showRewardedVideo();
        }
    }

    public bool isRewardedLoaded()
    {
        if (string.IsNullOrEmpty(ads.LevelPlayKey))
            return false;

        return IronSource.Agent.isRewardedVideoAvailable();
    }
    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Logger("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent()
    {
        Logger("unity-script: I got RewardedVideoAdOpenedEvent");
        AppOpenState(false);
    }

    void RewardedVideoOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
    }
    void RewardedVideoOnAdLoadFailedEvent(IronSourceError error)
    {
    }
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        AdsManager.Instance.LoadAd();
    }
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        rewardCallback(true);
    }
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        AdsManager.Instance.LoadAd();
        rewardCallback(false);
    }
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }
    #endregion


    #region Helpers


    public void AppOpenState(bool status)
    {
        GoogleAdsManager.Instance.CanShowAppOpen = status;
    }
    public void Logger(string str)
    {
        if (ShowDebug)
            Debug.Log(str);
    }

    public void LoadDebugger()
    {
        IronSource.Agent.launchTestSuite();

    }
    #endregion

    #region RevenueCallBack


     private void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
    {

        if (impressionData != null)
        {
            Firebase.Analytics.Parameter[] AdParameters = {
        new Firebase.Analytics.Parameter("ad_platform", "ironSource"),
        new Firebase.Analytics.Parameter("ad_source", impressionData.adNetwork),
        new Firebase.Analytics.Parameter("ad_unit_name", impressionData.adUnit),
        new Firebase.Analytics.Parameter("ad_format", impressionData.instanceName),
        new Firebase.Analytics.Parameter("currency","USD"),
        new Firebase.Analytics.Parameter("value", impressionData.revenue)
      };
            Debug.Log("ADUNIT IS " + impressionData.adUnit);
            Debug.Log("ADFOrmat IS " + impressionData.instanceName);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", AdParameters);
            if (impressionData.adUnit == "banner")
            {
                AdUnitRevenueCallBack("ad_impression_IS_Banner", impressionData,false);
                AdUnitRevenueCallBack("all_ad_impression_Banner", impressionData,true);
                
            }
            if (impressionData.adUnit == "interstitial")
            {
                AdUnitRevenueCallBack("ad_impression_IS_inter", impressionData,false);
                AdUnitRevenueCallBack("all_ad_impression_Inter", impressionData,true);
               
            }
            if (impressionData.adUnit == "rewarded_video")
            {
                AdUnitRevenueCallBack("ad_impression_IS_Rewarded", impressionData, false);
                AdUnitRevenueCallBack("all_ad_impression_Rewarded", impressionData,true);
                

            }
        }


    }
    public void AdUnitRevenueCallBack(string adimpressionstr, IronSourceImpressionData impressionData, bool AllAds)
    {
        if (impressionData != null)
        {
            double revenue = impressionData.revenue;
            var impressionParameters = new[] {
               new Firebase.Analytics.Parameter("value", revenue),
               new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
       };
            Firebase.Analytics.FirebaseAnalytics.LogEvent(adimpressionstr, impressionParameters);
            if (AllAds)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("all_ad_impression", impressionParameters);
                string ad_platform = impressionData.adNetwork;
                string mediation_platform = impressionData.instanceName;
                string ad_id = impressionData.adFormat;
              //  SolarEngineSDKInit.Instance.trackAdImpression(ad_platform, "ironSource", ad_type, ad_id, impressionData.revenue * 1000);
              
            }
        }
    }
    #endregion
}
