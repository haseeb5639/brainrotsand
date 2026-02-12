#if UNITY_IOS
using DeadMosquito.IosGoodies;
#endif
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GoogleMobileAds.Api.AdValue;

public class GoogleAdsManager : MonoBehaviour
{
    public bool DebugMode = false;
    [HideInInspector]
    public bool CanShowAppOpen = true;

    private Action<bool> rewardCallback;
#if UNITY_ANDROID
    AdsConfigAndroid ads;
#endif
#if UNITY_IOS
    AdsConfigIOS ads;
#endif
    #region Singleton
    private static GoogleAdsManager _Instance;
    public static GoogleAdsManager Instance
    {
        get
        {

            if (!_Instance) _Instance = FindObjectOfType<GoogleAdsManager>();

            return _Instance;
        }
    }
    void Awake()
    {
        if (!_Instance) _Instance = this;
    }


    #endregion




    private void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);



#if UNITY_IOS
        ads = AdsConfigLoader.Instance.adsConfigIOS;
#endif
#if UNITY_ANDROID

        ads = AdsConfigLoader.Instance.adsConfigAndroid;

#endif
    }

    public void Init(bool state)
    {
        InitializeAdmob(state);
        InitializeAppOpen();
    }
    public void InitializeAdmob(bool state)
    {

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Configure TagForChildDirectedTreatment and test device IDs.
        if (state)
        {

            RequestConfiguration requestConfiguration = new RequestConfiguration
            {
                TestDeviceIds = deviceIds,
                TagForChildDirectedTreatment = TagForChildDirectedTreatment.Unspecified



            };

            MobileAds.SetRequestConfiguration(requestConfiguration);


        }
        else
        {

            RequestConfiguration requestConfiguration = new RequestConfiguration
            {
                TestDeviceIds = deviceIds,
                TagForChildDirectedTreatment = TagForChildDirectedTreatment.Unspecified



            };
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);


    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        Print("Initialization complete.");

        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            // RequestBannerAd();

            // LoadAppOpenAd();
            // LoadAd();
        });
    }
    void InitializeAppOpen()
    {
        // Listen to application foreground / background events.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

    }



    #region BANNER ADS
    private BannerView bannerView;


    void RequestBanner()
    {
        if (!NativeAdsManager.Instance.CanShowBanner)
            return;




#if UNITY_ANDROID
        string adUnitId = null;
        if (AdsManager.Instance.HaveMemory())
        {
            adUnitId = ads.BannerID;

        }
        else
        {
            adUnitId = ads.LowBanner;
        }


#endif
#if UNITY_IOS
        string adUnitId = ads.BannerID;
#endif

        if (string.IsNullOrEmpty(adUnitId))
        {
            StartCoroutine(OnBannerFailed());
        }


        ClearMemory();

        // Clean up banner before reusing
        DestroyBanner();

        // Create a 320x50 banner at the top of the screen.

        if (AdsManager.Instance.Banner == AdsManager.BannerType.SmallBanner)
        {
#if UNITY_IOS
            if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
            {
                AdSize adSize = new AdSize(468, 90);
                bannerView = new BannerView(adUnitId, adSize, GetBannerPos());
            }
            else
            {
                bannerView = new BannerView(adUnitId, AdSize.Banner, GetBannerPos());
            }
#endif
#if UNITY_ANDROID

            if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
            {
                AdSize adSize = new AdSize(468, 90);
                bannerView = new BannerView(adUnitId, adSize, GetBannerPos());

            }
            else
            {
                bannerView = new BannerView(adUnitId, AdSize.Banner, GetBannerPos());

            }

#endif
        }
        else if (AdsManager.Instance.Banner == AdsManager.BannerType.LargeBanner)
        {
            bannerView = new BannerView(adUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), GetBannerPos());

        }
        Debug.Log("aftab => Requsting Admob First Banner");

        // Add Event Handlers
        bannerView.OnBannerAdLoaded += () =>
        {
            Print("Banner ad loaded.");

            InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
            InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
            MaxAdsManager.Instance.HideBanner();
            LevelPlayAdsManager.Instance.HideBanner();
            Debug.Log("aftab => Admob First Banner Loaded");
            // LabAnalytics.Instance.LogEvent("ad_impression_ADMOB_Banner");
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Print("Banner ad failed to load with error: " + error.GetMessage());
            Debug.Log("aftab => Admob First Banner Failed");
            RequestSecondBanner();
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Print("Banner ad opening.");
            Debug.Log("aftab =>Admob First Banner Showed");

        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Print("Banner ad closed.");

        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        adValue.CurrencyCode,
                                        adValue.Value);
            Print(msg);
            AdUnitRevenueCallBack("ad_impression_Admob_Banner", adValue);
            AdUnitRevenueCallBack("all_ad_impression_Banner", adValue);
            AllAdsImpression(adValue);
        };


        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    void RequestSecondBanner()
    {
        if (!NativeAdsManager.Instance.CanShowBanner)
            return;
        string adUnitId = null;

        adUnitId = ads.LowBanner;


        if (string.IsNullOrEmpty(adUnitId))
        {
            StartCoroutine(OnBannerFailed());
        }
        // Clean up banner before reusing
        DestroyBanner();

        // Create a 320x50 banner at the top of the screen.
        if (AdsManager.Instance.Banner == AdsManager.BannerType.SmallBanner)
        {
#if UNITY_IOS
            if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
            {
                AdSize adSize = new AdSize(468, 90);
                bannerView = new BannerView(adUnitId, adSize, GetBannerPos());
            }
            else
            {
                bannerView = new BannerView(adUnitId, AdSize.Banner, GetBannerPos());
            }
#endif
#if UNITY_ANDROID

            if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
            {
                AdSize adSize = new AdSize(468, 90);
                bannerView = new BannerView(adUnitId, adSize, GetBannerPos());
            }
            else
            {
                bannerView = new BannerView(adUnitId, AdSize.Banner, GetBannerPos());
            }
#endif
        }
        else if (AdsManager.Instance.Banner == AdsManager.BannerType.LargeBanner)
        {


            bannerView = new BannerView(adUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), GetBannerPos());

        }
        Debug.Log("aftab => Requsting Admob Second Banner");
        // Add Event Handlers
        bannerView.OnBannerAdLoaded += () =>
        {
            Print("Banner ad loaded.");
            Debug.Log("aftab => Admob Second Banner Loaded");
            InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
            InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
            MaxAdsManager.Instance.HideBanner();
            LevelPlayAdsManager.Instance.HideBanner();
            //LabAnalytics.Instance.LogEvent("ad_impression_ADMOB_Banner");
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("aftab => Admob Second Banner Failed");
            Print("Banner ad failed to load with error: " + error.GetMessage());
            StartCoroutine(OnBannerFailed());
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Print("Banner ad opening.");
            Debug.Log("aftab => Admob Second Banner Show");
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Print("Banner ad closed.");

        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        adValue.CurrencyCode,
                                        adValue.Value);
            Print(msg);
            AdUnitRevenueCallBack("ad_impression_Admob_Banner", adValue);
            AdUnitRevenueCallBack("all_ad_impression_Banner", adValue);
            AllAdsImpression(adValue);
        };


        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    IEnumerator OnBannerFailed()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        HideBanner();
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        LevelPlayAdsManager.Instance.ShowBanner();

    }
    public void ShowBanner()
    {
        if (AdsManager.Instance.BannerBehaviourOnHide == AdsManager.BannerBehavior.HideBanner)
        {
            if (bannerView != null)
            {
                bannerView.Show();

            }
            else
            {
                RequestBanner();
            }
        }
        else if (AdsManager.Instance.BannerBehaviourOnHide == AdsManager.BannerBehavior.DestroyBanner)
        {
            RequestBanner();
        }
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();

        }
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
    }
    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
    }

    AdPosition GetBannerPos()
    {
        switch (AdsManager.Instance.bannerPosition)
        {
            case AdsManager.BannerPosition.Top:
                return AdPosition.Top;
            case AdsManager.BannerPosition.Bottom:
                return AdPosition.Bottom;
            default:
                return AdPosition.Top;
        }
    }
    #endregion

    #region BANNER COLLAPASIBLE ADS

    private BannerView bannercollapsible;
    void RequestCollapsibleBanner()
    {
        if (!NativeAdsManager.Instance.CanShowBanner)
            return;




#if UNITY_ANDROID
        string adUnitId = null;
        if (AdsManager.Instance.HaveMemory())
        {
            adUnitId = ads.CollapsibleBannerID;

        }
        else
        {
            adUnitId = ads.CollapsibleBannerID;
        }


#endif
#if UNITY_IOS
        string adUnitId = ads.CollapsibleBannerID;
#endif

        if (string.IsNullOrEmpty(adUnitId))
        {
            StartCoroutine(OnBannerCollapsibleFailed());
        }



        ClearMemory();

        // Clean up banner before reusing
        DestroyCollapsibleBanner();

        // Create a 320x50 banner at the top of the screen.

        if (AdsManager.Instance.collapsibleBanner == AdsManager.BannerType.SmallBanner)
        {
#if UNITY_IOS
            if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
            {
                AdSize adSize = new AdSize(468, 90);
                bannercollapsible = new BannerView(adUnitId, adSize, AdsManager.Instance.collapsibleBannerPosition);
            }
            else
            {
                bannercollapsible = new BannerView(adUnitId, AdSize.Banner, AdsManager.Instance.collapsibleBannerPosition);
            }
#endif
#if UNITY_ANDROID

            if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
            {
                AdSize adSize = new AdSize(468, 90);
                bannercollapsible = new BannerView(adUnitId, adSize, AdsManager.Instance.collapsibleBannerPosition);

            }
            else
            {
                bannercollapsible = new BannerView(adUnitId, AdSize.Banner, AdsManager.Instance.collapsibleBannerPosition);

            }

#endif
        }
        else if (AdsManager.Instance.collapsibleBanner == AdsManager.BannerType.LargeBanner)
        {


            bannercollapsible = new BannerView(adUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdsManager.Instance.collapsibleBannerPosition);

        }
        Debug.Log("aftab => Requsting Admob Collapsible Banner");

        // Add Event Handlers
        bannercollapsible.OnBannerAdLoaded += () =>
        {
            Print("Collapsible Banner ad loaded.");

            InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
            InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();

            LevelPlayAdsManager.Instance.HideBanner();
            Debug.Log("aftab => Admob Collapsible Banner Loaded");
            // LabAnalytics.Instance.LogEvent("ad_impression_ADMOB_Banner");
        };
        bannercollapsible.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Print("Collapsible Banner ad failed to load with error: " + error.GetMessage());
            Debug.Log("aftab => Admob Collapsible Banner Failed");
            RequestBanner();
        };
        bannercollapsible.OnAdFullScreenContentOpened += () =>
        {
            Print("Banner ad opening.");
            Debug.Log("aftab =>Admob Collapsible Banner Showed");

        };
        bannercollapsible.OnAdFullScreenContentClosed += () =>
        {
            Print("Collapsible Banner ad closed.");

        };
        bannercollapsible.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        adValue.CurrencyCode,
                                        adValue.Value);
            Print(msg);
            AdUnitRevenueCallBack("ad_impression_Admob_Collapsible_Banner", adValue);
            AdUnitRevenueCallBack("all_ad_impression_Collapsible_Banner", adValue);
            AllAdsImpression(adValue);
        };

        var adRequest = new AdRequest();
        adRequest.Extras.Add("collapsible", "bottom");

        bannercollapsible.LoadAd(adRequest);
    }

    IEnumerator OnBannerCollapsibleFailed()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        HideCollapsibleBanner();
        if (bannercollapsible != null)
        {
            bannercollapsible.Destroy();
            bannercollapsible = null;
        }


    }
    public void ShowCollapsibleBanner()
    {
        if (AdsManager.Instance.BannerBehaviourOnHide == AdsManager.BannerBehavior.HideBanner)
        {
            if (bannercollapsible != null)
            {
                bannercollapsible.Show();

            }
            else
            {
                RequestCollapsibleBanner();
            }
        }
        else if (AdsManager.Instance.BannerBehaviourOnHide == AdsManager.BannerBehavior.DestroyBanner)
        {
            RequestCollapsibleBanner();
        }
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
    }

    public void HideCollapsibleBanner()
    {
        if (bannercollapsible != null)
        {
            bannercollapsible.Hide();

        }
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
    }
    public void DestroyCollapsibleBanner()
    {
        if (bannercollapsible != null)
        {
            bannercollapsible.Destroy();
            bannercollapsible = null;
        }
        InHouseAdsController.Instance.BannerInHouse.HideInhouseAd();
        InHouseAdsController.Instance.LargeBannerInHouse.HideInhouseAd();
    }



    #endregion

    #region NATIVEBANNER ADS
    private BannerView nativeBanner;
    void RequestNativeBanner()
    {

        if (!NativeAdsManager.Instance.CanShowNative)
            return;

#if UNITY_ANDROID
        string adUnitId = null;
        if (AdsManager.Instance.HaveMemory())
        {
            adUnitId = ads.NativeBanner;

        }
        else
        {
            adUnitId = ads.LowNativeBanner;
        }


#endif
#if UNITY_IOS
        string adUnitId = ads.NativeBanner;
#endif


        if (string.IsNullOrEmpty(adUnitId))
        {
            StartCoroutine(OnNativeFailed());
        }
        // Clean up banner before reusing
        DestroyNativeBanner();
#if UNITY_IOS
        if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
        {
            AdSize adSize = new AdSize(400, 400);
            nativeBanner = new BannerView(adUnitId, adSize, 0, 300);
        }
        else if (IGDevice.Name.ToUpper().IndexOf("IPOD") > -1)
        {
            AdSize adSize = new AdSize(125, 125);
            nativeBanner = new BannerView(adUnitId, adSize, GetNativeBannerPos());
        }
        else
        {

            nativeBanner = new BannerView(adUnitId, AdSize.MediumRectangle, GetNativeBannerPos());
        }
#endif
#if UNITY_ANDROID

        if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
        {
            AdSize adSize = new AdSize(400, 400);
            nativeBanner = new BannerView(adUnitId, adSize, GetNativeBannerPos());
        }
        else
        {
            nativeBanner = new BannerView(adUnitId, AdSize.MediumRectangle, GetNativeBannerPos());
        }
#endif


        // Add Event Handlers
        nativeBanner.OnBannerAdLoaded += () =>
        {
            Print("Banner ad loaded.");
            //  AdmobNativeAds.Instance.HideNativeAd();
            MaxAdsManager.Instance.HideNativeBanner();
            InHouseAdsController.Instance.NativeInHouse.HideInhouseAd();
            // LabAnalytics.Instance.LogEvent("ad_impression_ADMOB_N_Banner");
        };
        nativeBanner.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Print("Banner ad failed to load with error: " + error.GetMessage());
            //  RequestSecondNativeBanner();
            StartCoroutine(OnNativeFailed());
        };
        nativeBanner.OnAdFullScreenContentOpened += () =>
        {
            Print("Banner ad opening.");

        };
        nativeBanner.OnAdFullScreenContentClosed += () =>
        {
            Print("Banner ad closed.");

        };
        nativeBanner.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        adValue.CurrencyCode,
                                        adValue.Value);
            Print(msg);
            AdUnitRevenueCallBack("ad_impression_Admob_N_Banner", adValue);
            AdUnitRevenueCallBack("all_ad_impression_N_Banner", adValue);
            AllAdsImpression(adValue);
        };


        // Load a banner ad
        nativeBanner.LoadAd(CreateAdRequest());
    }


    void RequestSecondNativeBanner()
    {

        if (!NativeAdsManager.Instance.CanShowNative)
            return;

        string adUnitId = null;
        adUnitId = ads.LowNativeBanner;



        if (string.IsNullOrEmpty(adUnitId))
        {
            StartCoroutine(OnNativeFailed());
        }
        // Clean up banner before reusing
        DestroyNativeBanner();
#if UNITY_IOS
        if (IGDevice.Name.ToUpper().IndexOf("IPAD") > -1)
        {
            AdSize adSize = new AdSize(400, 400);
            nativeBanner = new BannerView(adUnitId, adSize, 0, 300);
        }
        else if (IGDevice.Name.ToUpper().IndexOf("IPOD") > -1)
        {
            AdSize adSize = new AdSize(125, 125);
            nativeBanner = new BannerView(adUnitId, adSize, GetNativeBannerPos());
        }
        else
        {

            nativeBanner = new BannerView(adUnitId, AdSize.MediumRectangle, GetNativeBannerPos());
        }
#endif
#if UNITY_ANDROID

        if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
        {
            AdSize adSize = new AdSize(400, 400);
            nativeBanner = new BannerView(adUnitId, adSize, GetNativeBannerPos());
        }
        else
        {
            nativeBanner = new BannerView(adUnitId, AdSize.MediumRectangle, GetNativeBannerPos());
        }
#endif


        // Add Event Handlers
        nativeBanner.OnBannerAdLoaded += () =>
        {
            Print("Banner ad loaded.");
            //  AdmobNativeAds.Instance.HideNativeAd();
            MaxAdsManager.Instance.HideNativeBanner();
            InHouseAdsController.Instance.NativeInHouse.HideInhouseAd();

        };
        nativeBanner.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Print("Banner ad failed to load with error: " + error.GetMessage());
            StartCoroutine(OnNativeFailed());
        };
        nativeBanner.OnAdFullScreenContentOpened += () =>
        {
            Print("Banner ad opening.");

        };
        nativeBanner.OnAdFullScreenContentClosed += () =>
        {
            Print("Banner ad closed.");

        };
        nativeBanner.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        adValue.CurrencyCode,
                                        adValue.Value);
            Print(msg);
            AdUnitRevenueCallBack("ad_impression_Admob_N_Banner", adValue);
            AdUnitRevenueCallBack("all_ad_impression_N_Banner", adValue);
            AllAdsImpression(adValue);
        };


        // Load a banner ad
        nativeBanner.LoadAd(CreateAdRequest());
    }

    public void ShowNativeBanner()
    {
        if (AdsManager.Instance.NativeBehaviourOnHide == AdsManager.NativeBehavior.HideNative)

        {
            if (nativeBanner != null)
            {
                nativeBanner.Show();
            }
            else
            {
                RequestNativeBanner();
            }
        }

        else if (AdsManager.Instance.NativeBehaviourOnHide == AdsManager.NativeBehavior.DestroyNative)
        {
            RequestNativeBanner();
        }

        InHouseAdsController.Instance.NativeInHouse.HideInhouseAd();
    }

    AdPosition GetNativeBannerPos()
    {
        switch (AdsManager.Instance.nativeAdsPositions)
        {
            case AdsManager.NativeBannerPosition.Top:
                return AdPosition.Top;

            case AdsManager.NativeBannerPosition.Bottom:
                return AdPosition.Bottom;

            case AdsManager.NativeBannerPosition.TopLeft:
                return AdPosition.TopLeft;

            case AdsManager.NativeBannerPosition.TopRight:
                return AdPosition.TopRight;

            case AdsManager.NativeBannerPosition.BottomLeft:
                return AdPosition.BottomLeft;

            case AdsManager.NativeBannerPosition.BottomRight:
                return AdPosition.BottomRight;

            default:
                return AdPosition.BottomLeft;
        }
    }


    public void HideNativeBanner()
    {
        if (nativeBanner != null)
        {
            nativeBanner.Hide();

        }
        InHouseAdsController.Instance.NativeInHouse.HideInhouseAd();
    }

    public void DestroyNativeBanner()
    {
        if (nativeBanner != null)
        {
            nativeBanner.Destroy();
            nativeBanner = null;
        }

        InHouseAdsController.Instance.NativeInHouse.HideInhouseAd();
    }


    public bool NativeStatus()
    {
        if (this.nativeBanner != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    IEnumerator OnNativeFailed()
    {
        yield return new WaitForSecondsRealtime(0.1f);

        //  AppLovinAdsController.Instance.HideNativeBanner();
        HideNativeBanner();
        if (nativeBanner != null)
        {
            nativeBanner.Destroy();
            nativeBanner = null;

        }

        if (MaxAdsManager.Instance.CanLoadNativeBanner())
        {
            MaxAdsManager.Instance.ShowNativeBanner();
        }
        else
        {
            if (NativeAdsManager.Instance.CanShowNative)
                InHouseAdsController.Instance.NativeInHouse.ShowAd();
        }



    }

    #endregion

    #region INTERSTITIAL ADS
    private InterstitialAd interstitialAd;
    public void LoadInterstitialAd()
    {
#if UNITY_ANDROID
        string adUnitId = null;

        if (AdsManager.Instance.HaveMemory())
        {
            adUnitId = ads.InterstitialID;
        }
        else
        {
            adUnitId = ads.LowInterstitialID;
        }


#endif
#if UNITY_IOS
        string adUnitId = ads.InterstitialID;
#endif

        if (string.IsNullOrEmpty(adUnitId))
        {
            interstitialAd = null;
            return;
        }

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        // Load an interstitial ad
        InterstitialAd.Load(adUnitId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Print("Interstitial ad failed to load with error: " +
                        loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    Print("aftab => Admob Interstitial ad failed to load.");
                    return;
                }

                Print("aftab => Admob Interstitial ad loaded.");
                interstitialAd = ad;
                ad.OnAdFullScreenContentOpened += () =>
                {
                    Print("Interstitial ad opening.");
                    CanShowAppOpen = false;
                   // LabAnalytics.Instance.LogEvent("ADMOB_Inter_Shown");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Print("Interstitial ad closed.");
                    AdsManager.Instance.LoadAd();
                   // LabAnalytics.Instance.LogEvent("ADMOB_Inter_End");

                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Print("Interstitial ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Print("Interstitial ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Print("Interstitial ad failed to show with error: " +
                                error.GetMessage());
                    AdsManager.Instance.LoadAd();
                    //LabAnalytics.Instance.LogEvent("ADMOB_Inter_Error");
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "Interstitial ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);
                    Print(msg);
                    AdUnitRevenueCallBack("ad_impression_Admob_inter", adValue);
                    AdUnitRevenueCallBack("all_ad_impression_Inter", adValue);
                    AllAdsImpression(adValue);
                };
            });
    }

    public void ShowInterstitialAd()
    {


        if (interstitialAd != null && isInterstitialLoaded())
        {
            interstitialAd.Show();
        }

    }
    public bool isInterstitialLoaded()
    {
        if (interstitialAd != null)
        {
            return interstitialAd.CanShowAd();
        }
        else
        {
            return false;
        }
    }


    #endregion

    #region REWARDED ADS
    private RewardedAd rewardedAd;
    bool RewardedLoaded = false;
    public void LoadRewardedAd()
    {

        if (!AdsManager.Instance.HaveMemory())
            return;


        rewardedAd = null;
        string adUnitId = ads.RewardedID;

        if (string.IsNullOrEmpty(adUnitId))
        {
            rewardedAd = null;
            return;
        }

        // create new rewarded ad instance
        RewardedAd.Load(adUnitId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Print("Rewarded ad failed to load with error: " +
                                loadError.GetMessage());
                    RewardedLoaded = false;
                    return;
                }
                else if (ad == null)
                {
                    Print("aftab => Admob Rewarded ad failed to load.");
                    RewardedLoaded = false;
                    return;
                }

                Print("aftab => Admob Rewarded ad loaded.");
                rewardedAd = ad;
                RewardedLoaded = true;
                ad.OnAdFullScreenContentOpened += () =>
                {
                    Print("Rewarded ad opening.");
                    RewardedLoaded = false;
                    CanShowAppOpen = false;
                  //  LabAnalytics.Instance.LogEvent("ADMOB_Rewarded_Shown");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Print("Rewarded ad closed.");
                    RewardedLoaded = false;
                    AdsManager.Instance.LoadAd();
                   // LabAnalytics.Instance.LogEvent("ADMOB_Rewarded_End");
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Print("Rewarded ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Print("Rewarded ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Print("Rewarded ad failed to show with error: " +
                               error.GetMessage());
                    RewardedLoaded = false;
                    rewardCallback(false);
                    AdsManager.Instance.LoadAd();
                   // LabAnalytics.Instance.LogEvent("ADMOB_Reward_Error");
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "Rewarded ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);
                    Print(msg);
                    AdUnitRevenueCallBack("ad_impression_Admob_Rewarded", adValue);
                    AdUnitRevenueCallBack("all_ad_impression_Rewarded", adValue);
                    AllAdsImpression(adValue);
                };
            });
    }

    public void ShowRewardedAd(Action<bool> callback = null)
    {
        if (!AdsManager.Instance.HaveMemory())
            return;
        rewardCallback = callback;
        if (rewardedAd != null && isRewardedLoaded())
        {
            rewardedAd.Show((Reward reward) =>
            {
                rewardCallback(true);
                Print("Rewarded ad granted a reward: " + reward.Amount);
            });
        }
        else
        {
            rewardCallback(false);
        }

    }
    public bool isRewardedLoaded()
    {

        return RewardedLoaded;
    }



    #endregion

    #region REWARDED INTERSTITIAL ADS
    private RewardedInterstitialAd rewardedInterstitialAd;
    private bool RewardedInterstitialLoaded = false;
    public void LoadRewardedInterstitialAd()
    {

        if (!AdsManager.Instance.HaveMemory())
            return;


        string adUnitId = ads.RewardedInterstitial;

        if (string.IsNullOrEmpty(adUnitId))
        {
            RewardedInterstitialLoaded = false;
            return;
        }

        // Create a rewarded interstitial.
        RewardedInterstitialAd.Load(adUnitId, CreateAdRequest(),
            (RewardedInterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Print("Rewarded intersitial ad failed to load with error: " +
                                loadError.GetMessage());
                    RewardedInterstitialLoaded = false;
                    return;
                }
                else if (ad == null)
                {
                    Print("Rewarded intersitial ad failed to load.");
                    RewardedInterstitialLoaded = false;
                    return;
                }

                Print("Rewarded interstitial ad loaded.");
                rewardedInterstitialAd = ad;
                RewardedInterstitialLoaded = true;
                ad.OnAdFullScreenContentOpened += () =>
                {
                    Print("Rewarded intersitial ad opening.");
                    RewardedInterstitialLoaded = false;
                    CanShowAppOpen = false;

                    LabAnalytics.Instance.LogEvent("ADMOB_Rewarded_Shown");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Print("Rewarded intersitial ad closed.");
                    RewardedInterstitialLoaded = false;
                    AdsManager.Instance.LoadAd();

                    LabAnalytics.Instance.LogEvent("ADMOB_Rewarded_End");
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Print("Rewarded intersitial ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Print("Rewarded intersitial ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Print("Rewarded intersitial ad failed to show with error: " +
                                error.GetMessage());
                    RewardedInterstitialLoaded = false;
                    rewardCallback(false);
                    AdsManager.Instance.LoadAd();


                    LabAnalytics.Instance.LogEvent("ADMOB_Reward_Error");
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                                "Rewarded intersitial ad received a paid event.",
                                                adValue.CurrencyCode,
                                                adValue.Value);
                    Print(msg);
                    AdUnitRevenueCallBack("ad_impression_Admob_Rewrded_inter", adValue);
                    AdUnitRevenueCallBack("all_ad_impression_Rewrded_inter", adValue);
                    AllAdsImpression(adValue);
                };
            });
    }


    public void ShowRewardedInterstitialAd(Action<bool> callback = null)
    {
        if (!AdsManager.Instance.HaveMemory())
            return;

        rewardCallback = callback;
        if (rewardedInterstitialAd != null && isRewardedInterLoaded())
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                rewardCallback(true);

                Print("Rewarded interstitial granded a reward: " + reward.Amount);
            });
        }
        else
        {
            rewardCallback(false);
            Print("Rewarded Interstitial ad is not ready yet.");
        }
    }
    public bool isRewardedInterLoaded()
    {
        return RewardedInterstitialLoaded;
    }
    #endregion

    #region APPOPEN ADS
    private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
    private DateTime appOpenExpireTime;
    private AppOpenAd appOpenAd;

    public bool IsAppOpenAdAvailable
    {
        get
        {
            return (appOpenAd != null
                    && appOpenAd.CanShowAd()
                    && DateTime.Now < appOpenExpireTime);
        }
    }

    public void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        Print("App State is " + state);
        // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {

            if (state == AppState.Foreground && CanShowAppOpen)
            {
                AdsManager.Instance.ShowAppOpenAd();
            }
        });
    }

    public void LoadAppOpenAd()
    {
        Print("Requesting App Open ad.");
        Debug.Log("aftab => Requsting...AppOpenAd");
        ClearMemory();


        if (IsAppOpenAdAvailable)
        {
            return;
        }
        // destroy old instance.

        DestroyAppOpenAd();
#if UNITY_ANDROID
        string adUnitId;

        if (AdsManager.Instance.HaveMemory())
        {
            adUnitId = ads.AppOpenID;
        }
        else
        {
            adUnitId = ads.LowAppOpenID;

        }

#endif
#if UNITY_IOS
        string adUnitId = ads.AppOpenID;


#endif

        if (string.IsNullOrEmpty(adUnitId))
        {

            return;
        }


        // Create a new app open ad instance.
        AppOpenAd.Load(adUnitId, CreateAdRequest(),
            (AppOpenAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Print("App open ad failed to load with error: " +
                        loadError.GetMessage());

                    return;
                }
                else if (ad == null)
                {
                    Print("App open ad failed to load.");

                    return;
                }

                Print("App Open ad loaded. Please background the app and return.");
                this.appOpenAd = ad;
                this.appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    Print("App open ad opened.");
                    CanShowAppOpen = false;
                    HideBanner();
                    LevelPlayAdsManager.Instance.HideBanner();
                    Debug.Log("aftab => Banner Hide");
                   // LabAnalytics.Instance.LogEvent("ADMOB_AppOpen_Shown");
                    Debug.Log("aftab => AppOpen Loaded And Open");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("aftab => Loading...AppOpen Closed");

                    LoadAppOpenAd();
                    CanShowAppOpen = true;
                    AdsManager.Instance.ShowBanner();
                   // LabAnalytics.Instance.LogEvent("ADMOB_AppOpen_End");

                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Print("App open ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Print("App open ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("aftab => Loading...AppOpen Failed");
                    Print("App open ad failed to show with error: " +
                        error.GetMessage());
                    // AdsManager.Instance.LoadAd();
                    LoadAppOpenAd();
                    AdsManager.Instance.ShowBanner();
                   // LabAnalytics.Instance.LogEvent("ADMOB_AppOpen_Error");
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "App open ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);
                    Print(msg);
                    AdUnitRevenueCallBack("ad_impression_Admob_AppOpen", adValue);
                    AdUnitRevenueCallBack("all_ad_impression_AppOpen", adValue);
                    AllAdsImpression(adValue);
                };
            });
    }

    public void DestroyAppOpenAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
    }

    public void ShowAppOpenAd()
    {
        CanShowAppOpen = false;
        if (IsAppOpenAdAvailable)
        {
            // InHouseAdsController.Instance.AppOpen.HideInhouseAd();
            appOpenAd.Show();
        }


    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest();
    }
    public void Print(string str)
    {
        if (DebugMode)
            MonoBehaviour.print(str);
    }


    public void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    #endregion

    public void AdUnitRevenueCallBack(string adimpression, AdValue value)
    {
        if (value != null)
        {
            float revenue = value.Value / 1000000f;
            string currencyCode = value.CurrencyCode;
            PrecisionType precision = value.Precision;


            var impressionParameters = new[] {

            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("currency", "USD"),
    };
            Firebase.Analytics.FirebaseAnalytics.LogEvent(adimpression, impressionParameters);
            //Firebase.Analytics.FirebaseAnalytics.LogEvent("all_ad_impression", impressionParameters);

        }
    }
    public void AllAdsImpression(AdValue value)
    {
        if (value != null)
        {
            float revenue = value.Value / 1000000f;
            string currencyCode = value.CurrencyCode;
            PrecisionType precision = value.Precision;

            var impressionParameters = new[] {

            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("currency", "USD"),
    };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("all_ad_impression", impressionParameters);
            Debug.Log($"aftab => Revenue is Admob is:  {revenue}");
        }
    }

}
