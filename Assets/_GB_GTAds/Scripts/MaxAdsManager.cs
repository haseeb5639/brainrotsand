using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
#if UNITY_IOS
using DeadMosquito.IosGoodies;
#endif
public class MaxAdsManager : MonoBehaviour
{
    #region Singleton
    private static MaxAdsManager _Instance;
    public static MaxAdsManager Instance
    {
        get
        {

            if (!_Instance) _Instance = FindObjectOfType<MaxAdsManager>();

            return _Instance;
        }
    }
    void Awake()
    {
        if (!_Instance) _Instance = this;
    }

    #endregion


    public bool ShowDebug = false;

    private Action<bool> rewardCallback;
#if UNITY_ANDROID
    AdsConfigAndroid ads;
#endif
#if UNITY_IOS
    AdsConfigIOS ads;
#endif
    void Start()
    {
#if UNITY_IOS
        ads = AdsConfigLoader.Instance.adsConfigIOS;
#endif
#if UNITY_ANDROID

        ads = AdsConfigLoader.Instance.adsConfigAndroid;

#endif
    }

    public void GetIds()
    {

        HighAppOpenAdUnitId = ads.AppLovinHighAppOpenID;
        LowAppOpenAdUnitId = ads.AppLovinLowAppOpenID;
        bannerAdUnitId = ads.AppLovinBannerID;
        adUnitId = ads.AppLovinInterstitialID;
        RewardedadUnitId = ads.AppLovinRewardedID;
        mrecAdUnitId = ads.AppLovinNativeBannerID;
    }



    #region Initialization


    public void Init()
    {
        if (!AdsManager.Instance.HaveMemory())
            return;
        GetIds();
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            InitializeHighAppOpen();
            InitializeBannerAds();
            InitializeMRECAds();
            //  InitializeLowAppOpen();
            // InitializeInterstitialAds();
            // InitializeRewardedAds();

        };

        // MaxSdk.SetSdkKey("EHT4qVzQsP7HxwaQC8CSIBr3_CwP3vOxSkVm2VubtApdZhEEte4Nz3Ga49TQlwWT0DBBsTvCBr6wIcHhu86Ncn");
        MaxSdk.InitializeSdk();

    }



    public void InitializeHighAppOpen()
    {
        // Attach callback
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnHighAppOpenLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnHighAppOpenLoadFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnHighAppOpenDisplayedEvent;
        MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnHighAppOpenClickedEvent;
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnHighAppOpenHiddenEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnHighAppOpenAdFailedToDisplayEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnHighAppOpenPaidEvent;

        // Load the first AppOpen

        LoadHighAppOpenAd();

    }

    public void InitializeLowAppOpen()
    {

        // Attach callback
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnLowAppOpenLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnLowAppOpenLoadFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnLowAppOpenDisplayedEvent;
        MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnLowAppOpenClickedEvent;
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnLowAppOpenHiddenEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnLowAppOpenAdFailedToDisplayEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnLowAppOpenPaidEvent;
        // Load the first AppOpen

        LoadLowAppOpenAd();

    }

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialPaidEvent;


        // Load the first interstitial
        LoadInterstitial();
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        LoadRewardedAd();
    }

    public void InitializeBannerAds()
    {

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

    }

    public void InitializeMRECAds()
    {

        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdLoadFailedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnMRecAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdExpandedEvent += OnMRecAdExpandedEvent;
        MaxSdkCallbacks.MRec.OnAdCollapsedEvent += OnMRecAdCollapsedEvent;

    }
    #endregion

    #region AppOpen

    string LowAppOpenAdUnitId;
    string HighAppOpenAdUnitId;

    /// <summary>
    /// / This is for High App Open Ads
    /// </summary>

    private void OnHighAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        Logger("Loaded AppOpen");
        // Reset retry attempt
    }
    private void OnHighAppOpenLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        AdsManager.Instance.ShowBanner();
        Logger(errorInfo.ToString());

    }
    private void OnHighAppOpenDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppOpenState(false);

        LabAnalytics.Instance.LogEvent("MAX_HAppOpen_Shown");
    }
    private void OnHighAppOpenAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.

        AdsManager.Instance.LoadAd();

        LabAnalytics.Instance.LogEvent("MAX_HAppOpen_Error");
    }
    private void OnHighAppOpenClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    private void OnHighAppOpenPaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaidEvent(adUnitId, adInfo);
        AdUnitRevenueCallBack("ad_impression_MAX_AppOpen", adInfo);
        AdUnitRevenueCallBack("all_ad_impression_AppOpen", adInfo);
        AllAdsImpression(adInfo);
    }
    private void OnHighAppOpenHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.

        AdsManager.Instance.LoadAd();
        AdsManager.Instance.ShowBanner();
        LabAnalytics.Instance.LogEvent("MAX_HAppOpen_End");

    }
    /// <summary>
    /// //// This is for Low App Open Ads
    /// </summary>
    private void OnLowAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

    }
    private void OnLowAppOpenLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)
        Logger(errorInfo.ToString());

    }
    private void OnLowAppOpenDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppOpenState(false);

        LabAnalytics.Instance.LogEvent("MAX_LAppOpen_Shown");
    }
    private void OnLowAppOpenAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.

        AdsManager.Instance.LoadAd();

        LabAnalytics.Instance.LogEvent("MAX_LAppOpen_Error");
    }
    private void OnLowAppOpenClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    private void OnLowAppOpenPaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaidEvent(adUnitId, adInfo);
        AdUnitRevenueCallBack("ad_impression_MAX_AppOpen", adInfo);
        AdUnitRevenueCallBack("all_ad_impression_AppOpen", adInfo);
        AllAdsImpression(adInfo);
    }
    private void OnLowAppOpenHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.

        AdsManager.Instance.LoadAd();

        LabAnalytics.Instance.LogEvent("MAX_LAppOpen_End");
    }



    public void ShowLowAppOpenAd()
    {
        GetIds();
        AppOpenState(false);
        if (IsLowAppOpenAdAvailable() && !string.IsNullOrEmpty(LowAppOpenAdUnitId))
        {
            //  InHouseAdsController.Instance.AppOpen.HideInhouseAd();
            MaxSdk.ShowAppOpenAd(LowAppOpenAdUnitId);
        }


    }
    public bool IsLowAppOpenAdAvailable()
    {
        if (!string.IsNullOrEmpty(LowAppOpenAdUnitId))
        {
            return MaxSdk.IsAppOpenAdReady(LowAppOpenAdUnitId);
        }
        else
        {
            return false;
        }
    }
    public void LoadLowAppOpenAd()
    {

        GetIds();
        if (!IsLowAppOpenAdAvailable() && !string.IsNullOrEmpty(LowAppOpenAdUnitId))
        {
            MaxSdk.LoadAppOpenAd(LowAppOpenAdUnitId);
        }
    }

    public void ShowHighAppOpenAd()
    {
        AppOpenState(false);
        GetIds();
        if (IsHighAppOpenAdAvailable() && !string.IsNullOrEmpty(HighAppOpenAdUnitId))
        {
            //  InHouseAdsController.Instance.AppOpen.HideInhouseAd();
            MaxSdk.ShowAppOpenAd(HighAppOpenAdUnitId);
        }


    }
    public bool IsHighAppOpenAdAvailable()
    {
        if (!string.IsNullOrEmpty(HighAppOpenAdUnitId))
        {
            return MaxSdk.IsAppOpenAdReady(HighAppOpenAdUnitId);
        }
        else
        {
            return false;
        }
    }
    public void LoadHighAppOpenAd()
    {
        GetIds();
        if (!IsHighAppOpenAdAvailable() && !string.IsNullOrEmpty(HighAppOpenAdUnitId))
        {




            MaxSdk.LoadAppOpenAd(HighAppOpenAdUnitId);
        }
    }

    #endregion

    #region Banner

    string bannerAdUnitId; // Retrieve the ID from your account


    public void ShowBanner()
    {
        if (!NativeAdsManager.Instance.CanShowBanner || string.IsNullOrEmpty(bannerAdUnitId))
            return;

        HideBanner();
        GetIds();
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        // Set background or background color for banners to be fully functional
        //  MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);


        if (AdsManager.Instance.Banner == AdsManager.BannerType.SmallBanner)
        {
            var adViewConfiguration = new MaxSdk.AdViewConfiguration(GetBannerPos())
            {
                IsAdaptive = false,
            };
            MaxSdk.CreateBanner(bannerAdUnitId, adViewConfiguration);
            MaxSdk.ShowBanner(bannerAdUnitId);

#if UNITY_IOS
            if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
            {
                MaxSdk.SetBannerWidth(bannerAdUnitId, 468);
            }
            else
            {
                MaxSdk.SetBannerWidth(bannerAdUnitId, 320);
            }

#endif
#if UNITY_ANDROID

            if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
            {
                MaxSdk.SetBannerWidth(bannerAdUnitId, 468);
            }
            else
            {
                MaxSdk.SetBannerWidth(bannerAdUnitId, 320);
            }
#endif
        }
        else if (AdsManager.Instance.Banner == AdsManager.BannerType.LargeBanner)
        {
            var adViewConfiguration = new MaxSdk.AdViewConfiguration(GetBannerPos())
            {
                IsAdaptive = false,
            };
            MaxSdk.CreateBanner(bannerAdUnitId, adViewConfiguration);
            MaxSdk.ShowBanner(bannerAdUnitId);
            MaxSdk.SetBannerWidth(bannerAdUnitId, Screen.width);
            // MaxSdkUtils.GetAdaptiveBannerHeight();

        }


        MaxSdk.StopBannerAutoRefresh(bannerAdUnitId);
        Debug.Log("aftab => Requsting MAX Banner ");
    }
    public bool CanLoadBanner()
    {
        GetIds();
        if (!string.IsNullOrEmpty(bannerAdUnitId))

        {
            return true;
        }
        else
            return false;
    }
    public void HideBanner()
    {

        if (!AdsManager.Instance.HaveMemory() || string.IsNullOrEmpty(bannerAdUnitId))
            return;

        MaxSdk.HideBanner(bannerAdUnitId);
        // InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
    }
    MaxSdkBase.AdViewPosition GetBannerPos()
    {
        switch (AdsManager.Instance.bannerPosition)
        {
            case AdsManager.BannerPosition.Top:
                return MaxSdkBase.AdViewPosition.TopCenter;
            case AdsManager.BannerPosition.Bottom:
                return MaxSdkBase.AdViewPosition.BottomCenter;
            default:
                return MaxSdkBase.AdViewPosition.TopCenter;
        }
    }


    public void DestroyBanner()
    {
        if (!AdsManager.Instance.HaveMemory() || string.IsNullOrEmpty(bannerAdUnitId))
            return;

        MaxSdk.DestroyBanner(bannerAdUnitId);
    }

    IEnumerator OnBannerFailed()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        HideBanner();

        if (NativeAdsManager.Instance.CanShowBanner)
        {
            // InHouse
            if (AdsManager.Instance.Banner == AdsManager.BannerType.SmallBanner)
            {
                InHouseAdsController.Instance.BannerInHouse.ShowAd();

            }
            else if (AdsManager.Instance.Banner == AdsManager.BannerType.LargeBanner)
            {
                InHouseAdsController.Instance.LargeBannerInHouse.ShowAd();
            }
        }


        LabAnalytics.Instance.LogEvent("Banner_Failed");
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("aftab => MAX Banner Loaded ");
        Logger("Loaded Banner");
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
        GoogleAdsManager.Instance.HideBanner();
        LevelPlayAdsManager.Instance.HideBanner();
        // LabAnalytics.Instance.LogEvent("ad_impression_MAX_Banner");
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Logger(errorInfo.ToString());
        Debug.Log("aftab => MAX Banner Failed ");

        StartCoroutine(OnBannerFailed());

    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaidEvent(adUnitId, adInfo);
        AdUnitRevenueCallBack("ad_impression_MAX_Banner", adInfo);
        AdUnitRevenueCallBack("all_ad_impression_Banner", adInfo);
        AllAdsImpression(adInfo);
    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion

    #region Interstitial

    string adUnitId;



    public void LoadInterstitial()
    {
        if (!AdsManager.Instance.HaveMemory())
            return;

        GetIds();
        if (!isInterstitialLoaded() && !string.IsNullOrEmpty(adUnitId))
        {
            MaxSdk.LoadInterstitial(adUnitId);
        }
    }
    public bool isInterstitialLoaded()
    {
        if (!AdsManager.Instance.HaveMemory())
            return false;

        if (!string.IsNullOrEmpty(adUnitId))
        {
            return MaxSdk.IsInterstitialReady(adUnitId);
        }
        else
        {
            return false;
        }
    }
    public void ShowInterstitialAd()
    {
        if (!AdsManager.Instance.HaveMemory())
            return;
        GetIds();
        if (isInterstitialLoaded() && !string.IsNullOrEmpty(adUnitId))
        {
            MaxSdk.ShowInterstitial(adUnitId);
        }
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        print("aftab => Applovin Interstitial ad loaded.");
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        print("aftab => Applovin Interstitial ad failed to load.");

    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppOpenState(false);
        // InHouseAdsController.Instance.InHouseAd.HideInhouseAd();
       // LabAnalytics.Instance.LogEvent("MAX_Inter_Shown");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.

        AdsManager.Instance.LoadAd();
       // LabAnalytics.Instance.LogEvent("MAX_Inter_Error");
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialPaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaidEvent(adUnitId, adInfo);
        AdUnitRevenueCallBack("ad_impression_MAX_inter", adInfo);
        AdUnitRevenueCallBack("all_ad_impression_Inter", adInfo);
        AllAdsImpression(adInfo);
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        AdsManager.Instance.LoadAd();
      //  LabAnalytics.Instance.LogEvent("MAX_Inter_End");
    }


    #endregion

    #region Rewarded
    string RewardedadUnitId;


    public bool isRewardedLoaded()
    {


        if (!AdsManager.Instance.HaveMemory())
            return false;

        if (!string.IsNullOrEmpty(RewardedadUnitId))
        {
            return MaxSdk.IsRewardedAdReady(RewardedadUnitId);
        }
        else
        {
            return false;
        }
    }
    public void LoadRewardedAd()
    {
        if (!AdsManager.Instance.HaveMemory())
            return;

        GetIds();

        if (!isRewardedLoaded() && !string.IsNullOrEmpty(RewardedadUnitId))
        {
            MaxSdk.LoadRewardedAd(RewardedadUnitId);
        }



    }
    public void ShowRewardedAd(Action<bool> callback = null)
    {
        if (!AdsManager.Instance.HaveMemory())
            return;

        rewardCallback = callback;

        GetIds();
        if (isRewardedLoaded() && !string.IsNullOrEmpty(RewardedadUnitId))
        {
            MaxSdk.ShowRewardedAd(RewardedadUnitId);
        }


    }
    public void ShowRewardedAd()
    {
        if (!AdsManager.Instance.HaveMemory())
            return;



        GetIds();
        if (isRewardedLoaded() && !string.IsNullOrEmpty(RewardedadUnitId))
        {
            MaxSdk.ShowRewardedAd(RewardedadUnitId);
        }


    }
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        print("aftab => Applovin Rewarded ad loaded.");
        
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        print("aftab => Admob Rewarded ad failed to load.");
        Logger(errorInfo.ToString());

    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

        AppOpenState(false);
        // InHouseAdsController.Instance.InHouseAd.HideInhouseAd();
      //  LabAnalytics.Instance.LogEvent("MAX_Rewarded_Shown");
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        AdsManager.Instance.LoadAd();
        rewardCallback(false);
      //  LabAnalytics.Instance.LogEvent("MAX_Rewarded_Error");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        AdsManager.Instance.LoadAd();
       // LabAnalytics.Instance.LogEvent("MAX_Rewarded_End");
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        rewardCallback(true);
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaidEvent(adUnitId, adInfo);
        AdUnitRevenueCallBack("ad_impression_MAX_Rewarded", adInfo);
        AdUnitRevenueCallBack("all_ad_impression_Rewarded", adInfo);
        AllAdsImpression(adInfo);
    }


    #endregion

    #region NativeBanner

    string mrecAdUnitId; // Retrieve the id from your account
    bool NativeState = false;
    public void ShowNativeBanner()
    {
        if (!NativeAdsManager.Instance.CanShowNative || string.IsNullOrEmpty(mrecAdUnitId))
            return;
        GetIds();
        HideNativeBanner();



        var adViewConfiguration = new MaxSdk.AdViewConfiguration(GetNativeBannerPos())
        {
            IsAdaptive = false,
        };

        // MRECs are sized to 300x250 on phones and tablets
        MaxSdk.CreateMRec(mrecAdUnitId, adViewConfiguration);
        MaxSdk.ShowMRec(mrecAdUnitId);
        MaxSdk.StopMRecAutoRefresh(mrecAdUnitId);
        NativeState = true;
    }
    MaxSdkBase.AdViewPosition GetNativeBannerPos()
    {
        switch (AdsManager.Instance.nativeAdsPositions)
        {
            case AdsManager.NativeBannerPosition.Top:
                return MaxSdkBase.AdViewPosition.TopCenter;

            case AdsManager.NativeBannerPosition.Bottom:
                return MaxSdkBase.AdViewPosition.BottomCenter;

            case AdsManager.NativeBannerPosition.TopLeft:
                return MaxSdkBase.AdViewPosition.TopLeft;

            case AdsManager.NativeBannerPosition.TopRight:
                return MaxSdkBase.AdViewPosition.TopRight;

            case AdsManager.NativeBannerPosition.BottomLeft:
                return MaxSdkBase.AdViewPosition.BottomLeft;

            case AdsManager.NativeBannerPosition.BottomRight:
                return MaxSdkBase.AdViewPosition.BottomRight;

            default:
                return MaxSdkBase.AdViewPosition.BottomLeft;
        }
    }
    public bool CanLoadNativeBanner()
    {
        if (!string.IsNullOrEmpty(mrecAdUnitId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void HideNativeBanner()
    {

        if (!AdsManager.Instance.HaveMemory() || string.IsNullOrEmpty(mrecAdUnitId))
            return;

        MaxSdk.HideMRec(mrecAdUnitId);
        NativeState = false;

    }

    public void DestroyNativeBanner()
    {
        if (!AdsManager.Instance.HaveMemory() || string.IsNullOrEmpty(mrecAdUnitId))
            return;

        MaxSdk.DestroyMRec(mrecAdUnitId);
        NativeState = false;
    }
    IEnumerator OnNativeFailed()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        HideNativeBanner();
        //AdmobAdsController.Instance.HideNativeBanner();
        if (NativeAdsManager.Instance.CanShowNative)
            InHouseAdsController.Instance.NativeInHouse.ShowAd();
        LabAnalytics.Instance.LogEvent("N_Banner_Failed");
    }

    public bool NativeStatus()
    {

        return NativeState;
    }



    public void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Logger("Loaded MREC");
        InHouseAdsController.Instance.NativeInHouse.HideInhouseAd();
        GoogleAdsManager.Instance.HideNativeBanner();

        // LabAnalytics.Instance.LogEvent("ad_impression_MAX_N_Banner");
    }

    public void OnMRecAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo error)
    {

        Logger(error.ToString());
        NativeState = false;

        StartCoroutine(OnNativeFailed());

    }

    public void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        OnAdRevenuePaidEvent(adUnitId, adInfo);
        AdUnitRevenueCallBack("ad_impression_MAX_MRec", adInfo);
        AdUnitRevenueCallBack("all_ad_impression_MRec", adInfo);
        AllAdsImpression(adInfo);
    }

    public void OnMRecAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    public void OnMRecAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
    #endregion


    #region Helpers
    public void OpenDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }




    public void AppOpenState(bool status)
    {

        GoogleAdsManager.Instance.CanShowAppOpen = status;

    }
    public void Logger(string str)
    {
        if (ShowDebug)
            Debug.Log(str);
    }



    #endregion


    #region RevenueCallBack

    void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        if (impressionData != null)
        {
            double revenue = impressionData.Revenue;
            var impressionParameters = new[] {
                new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
                new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
                new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
                new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
                new Firebase.Analytics.Parameter("value", revenue),
                new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        }
    }

    public void AdUnitRevenueCallBack(string adimpression, MaxSdkBase.AdInfo impressionData)
    {
        if (impressionData != null)
        {
            double revenue = impressionData.Revenue;
            var impressionParameters = new[] {
               new Firebase.Analytics.Parameter("value", revenue),
               new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent(adimpression, impressionParameters);
            // Firebase.Analytics.FirebaseAnalytics.LogEvent("all_ad_impression", impressionParameters);
        }
    }


    public void AllAdsImpression(MaxSdkBase.AdInfo impressionData)
    {
        if (impressionData != null)
        {
            double revenue = impressionData.Revenue;
            var impressionParameters = new[] {
               new Firebase.Analytics.Parameter("value", revenue),
               new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };

            Firebase.Analytics.FirebaseAnalytics.LogEvent("all_ad_impression", impressionParameters);


            Debug.Log($"aftab => Applovin Revenue is: {revenue}");
        }
    }
    #endregion
}
