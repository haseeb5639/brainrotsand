using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using GoogleMobileAds.Api;
using System.Collections.Generic;



#if UNITY_EDITOR
using UnityEditor;
using Sirenix.Utilities.Editor;
#endif

#if UNITY_IOS
using DeadMosquito.IosGoodies;
#endif


public class AdsManager : MonoBehaviour
{
    #region Variables

    [Title("Banner Ads", null, TitleAlignments.Centered)]
    
    public BannerType Banner = BannerType.None;
    public BannerPosition bannerPosition = BannerPosition.Top;
    public bool showCollapsibleBanner = false;
    [ShowIf("showCollapsibleBanner", true)]
    public BannerType collapsibleBanner = BannerType.None;
    [ShowIf("showCollapsibleBanner", true)]
    public AdPosition collapsibleBannerPosition = AdPosition.Top;
   
    
    public bool refreshBannerTimer = false;
    [ShowIf("refreshBannerTimer", true)]
    public float bannerRefreshTime = 45f;
    [ShowIf("refreshBannerTimer", true)]
    [ReadOnly] public float timeLeft;
    private Coroutine refreshCoroutine;

    [PropertySpace(10)]
    [Title("Native Banner Ads", null, TitleAlignments.Centered)]
    public NativeBannerPosition nativeAdsPositions = NativeBannerPosition.BottomLeft;

    [PropertySpace(10)]
    [Title("Others", null, TitleAlignments.Centered)]
    public BannerBehavior BannerBehaviourOnHide = BannerBehavior.DestroyBanner;
    public NativeBehavior NativeBehaviourOnHide = NativeBehavior.DestroyNative;
    public Ram Memory = Ram.RAM_1024;



    Coroutine co = null;
    GoogleAdsManager googleAdsManager;
    MaxAdsManager maxAdsManager;
    LevelPlayAdsManager levelPlayAdsManager;
    [HideInInspector]
    public int deviceMem;
    [HideInInspector]
    public int deviceMemNeeded;
    [HideInInspector]
    [ReadOnly] public bool Disable = false;
    [HideInInspector]
    [ReadOnly] public bool DisableCPAds = false;

    

    // [PropertySpace(10)]
    [Title("Firebase Remote Config", null, TitleAlignments.Centered)]

    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] string interstitial_value = "AIM";
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] string rewarded_value = "AIM";
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] string appOpen_value = "AM";
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly] public string game_version = "0.0.0";
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] bool isBannerON = true;
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] bool isNativeBannerON = true;
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] bool isAppOpenON = true;
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] bool isRewardedON = true;
    [FoldoutGroup("Firebase Remote Config", 0)][ReadOnly][SerializeField] bool isIntertitialRewardedON = true;
    [FoldoutGroup("Firebase Remote Config", 0)] public bool CP_Ads = false;




    #endregion

    #region Singleton
    private static AdsManager _Instance;
    public static AdsManager Instance
    {
        get
        {

            if (!_Instance) _Instance = FindObjectOfType<AdsManager>();

            return _Instance;
        }
    }
    void Awake()
    {
        if (!_Instance) _Instance = this;
        SetDefaultRemoteConfigValues();
        CheckRemoteConfigValues();
    }

    #endregion

    #region Unity Start

    IEnumerator Start()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this.gameObject);
        GetRam();

        PlayerPrefs.SetInt("EnableAds", 0);

        AdsManager.Instance.RemoveAds();


        googleAdsManager = GoogleAdsManager.Instance;
        maxAdsManager = MaxAdsManager.Instance;
        levelPlayAdsManager = LevelPlayAdsManager.Instance;


        yield return new WaitForSeconds(1.5f);
        AppTracking.Instance.ShowTracking();
        Init();
        CheckAds();

        yield return new WaitForSeconds(0.5f);
        GoogleAdsManager.Instance.LoadAppOpenAd();
        ShowBanner();

        yield return new WaitForSeconds(3f);
        LoadInterstitialAds();
        PlayerPrefs.SetInt("EnableAds", 1);


        yield return new WaitForSeconds(3f);
        ShowAppOpenAd();

        // yield return new WaitForSeconds(2f);

        yield return new WaitForSeconds(4f);
        LoadRewardedAds();

    }

    public void Init()
    {

        GoogleAdsManager.Instance.Init(true);
        LevelPlayAdsManager.Instance.Init();
        MaxAdsManager.Instance.Init();

        Debug.Log("aftab => Ads Init");

    }


    void LoadInterstitialAds()
    {
        Debug.Log("aftab => Interstitial Ads Is Loading");
        GoogleAdsManager.Instance.LoadInterstitialAd();
        LevelPlayAdsManager.Instance.LoadInterstitial();
        MaxAdsManager.Instance.InitializeInterstitialAds();
    }
    void LoadRewardedAds()
    {
        Debug.Log("aftab => Rewarded Ads Is Loadind");
        GoogleAdsManager.Instance.LoadRewardedAd();
        LevelPlayAdsManager.Instance.LoadRewarded();
        MaxAdsManager.Instance.InitializeRewardedAds();

    }


    void GetRam()
    {

        deviceMem = SystemInfo.systemMemorySize;


        if (Memory == Ram.None)
        {
            deviceMemNeeded = 0;
        }
        else if (Memory == Ram.RAM_1024)
        {
            deviceMemNeeded = 1500;
        }
        else if (Memory == Ram.RAM_2048)
        {
            deviceMemNeeded = 2500;
        }
        else if (Memory == Ram.RAM_3072)
        {
            deviceMemNeeded = 3500;
        }
    }
    public enum BannerType
    {
        SmallBanner,
        LargeBanner,
        None
    }

    public enum BannerBehavior
    {
        HideBanner,
        DestroyBanner
    }
    public enum NativeBehavior
    {
        HideNative,
        DestroyNative
    }

    public enum Ram
    {
        None,
        RAM_1024,
        RAM_2048,
        RAM_3072

    }


    public enum BannerPosition
    {
        Top,
        Bottom,
    }

    public enum NativeBannerPosition
    {
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Centered,

    }

    public bool HaveMemory()
    {

#if UNITY_ANDROID
        if (deviceMem >= deviceMemNeeded)
        {
            //  you do not have enough RAM for this action

            return true;

        }
        else
        {
            return false;
        }
#endif
#if UNITY_IOS
        return true;
#endif
    }

    #endregion

    #region RegionCheck
    [Serializable]

    public class IpApiData
    {
        public string country;

        public static IpApiData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<IpApiData>(jsonString);
        }
    }
    public IEnumerator SetCountry()
    {
        yield return new WaitForSecondsRealtime(1f);

        UnityWebRequest webRequest = UnityWebRequest.Get("https://ipinfo.io/json");
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to get country data");
            yield break;
        }

        string jsonString = webRequest.downloadHandler.text;

        // Parse the JSON response to get the country information.
        IpApiData ipApiData = IpApiData.CreateFromJSON(jsonString);
        // Check if the user is in a country where AdMob ads are banned.
        // isBanned = IsAdBannedCountry(ipApiData.country);
    }
    // Function to check if the user's country is in the list of countries where AdMob ads are banned.
    private bool IsAdBannedCountry(string country)
    {
        country = country.ToLower();
        // List of countries where AdMob ads are banned
        string[] bannedCountries = { "ru", "cu", "ir", "kp", "sd", "sy", "ua" };

        // Check if the user's country is in the list
        return Array.Exists(bannedCountries, c => c == country);
    }


    #endregion

    #region AdsCall

    public void ShowBanner()
    {
        if (Disable)
            return;

        if (!isBannerON)
            return;

        if (NoAdsPurchased() && NoCPPurchased())
            return;


        try
        {
            BannerAds();

            if (refreshBannerTimer == true)
            {
                if (refreshCoroutine != null)
                {
                    StopCoroutine(refreshCoroutine);
                }
                refreshCoroutine = StartCoroutine(RefreshBanner());
            }


        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }

    }
    IEnumerator RefreshBanner()
    {
        while (true) // Infinite loop
        {
            timeLeft = bannerRefreshTime;
            Debug.Log("Banner Refreshing");
            while (timeLeft > 0)
            {
                yield return new WaitForSeconds(1);
                timeLeft -= 1;
            }
            // Code to execute when timeLeft reaches 0 (outside the while loop).
            timeLeft = 0;
            BannerRefresh();

            Debug.Log("Banner Refreshed..........");
        }
    }
    public void BannerRefresh()
    {
        if (refreshCoroutine != null)
        {
            StopCoroutine(refreshCoroutine);
        }
        ShowBanner();
    }
    public void ShowCollapsibleBanner()
    {
        if (Disable)
            return;
        if (!isBannerON)
            return;

        if (NoAdsPurchased() && NoCPPurchased())
            return;


        try
        {

            BannerCollapsibleAds();
        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }

    }
    public void ShowNativeAd()
    {
        if (Disable)
            return;


        if (!isNativeBannerON)
            return;

        if (NoAdsPurchased() && NoCPPurchased())
            return;

        try
        {
            NativeBannerAds();
        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }

    }
    public void ShowAds()
    {
        if (Disable)
            return;

        if (NoAdsPurchased() && NoCPPurchased())
            return;

        try
        {
            InterstitialAds();
        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }

    }
    public void ShowRewardedAds()
    {
        if (Disable)
            return;

        if (!isRewardedON)
            return;

        try
        {
            RewardedAds(null);

        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }
    }
    public void ShowRewardedAds(Action rewardedAction)
    {
        if (Disable)
            return;


        if (!isRewardedON)
            return;

        try
        {
            RewardedAds(() =>
            {
                rewardedAction?.Invoke();
            });
        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }
    }
    public void ShowAppOpenAd()
    {
        if (Disable)
            return;

        if (!isAppOpenON)
            return;

        if (NoAdsPurchased() && NoCPPurchased())
            return;

        try
        {
            AppOpenAds();
        }
        catch (System.Exception e)
        {

            Debug.LogException(e);
        }


    }
    public void RateUs()
    {
#if UNITY_ANDROID

        Application.OpenURL("https://play.google.com/store/apps/details?id=" + AdsConfigLoader.Instance.adsConfigAndroid.PackageName);


#endif

#if UNITY_IOS

        IGAppStore.RequestReview();
#endif
    }
    public void OpenPrivacyPolicy()
    {
        Application.OpenURL("");
    }
    public void MoreGames()
    {

#if UNITY_ANDROID

        Application.OpenURL("https://play.google.com/store/apps/details?id=" + AdsConfigLoader.Instance.adsConfigAndroid.PackageName);



#endif

#if UNITY_IOS

        AppstoreHandler.Instance.openAppInStore(AdsConfigLoader.Instance.adsConfigIOS.APPID);

#endif
    }
    public void RemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        googleAdsManager.DestroyBanner();
        googleAdsManager.DestroyNativeBanner();
        maxAdsManager.DestroyBanner();
        maxAdsManager.DestroyNativeBanner();
        levelPlayAdsManager.HideBanner();

        InHouseManager[] AllInhouses = GameObject.FindObjectsOfType<InHouseManager>();
        for (int i = 0; i < AllInhouses.Length; i++)
        {
            AllInhouses[i].HideInhouseAd();
            if (AllInhouses[i].GetComponent<ScrollViewAdsController>())
            {
                Destroy(AllInhouses[i].gameObject);
            }
        }
        Invoke("ShowBanner", 1f);
    }
    public void RemoveCrossPromo()
    {
        PlayerPrefs.SetInt("RemoveCP", 1);
        googleAdsManager.DestroyBanner();
        googleAdsManager.DestroyNativeBanner();
        maxAdsManager.DestroyBanner();
        maxAdsManager.DestroyNativeBanner();
        levelPlayAdsManager.HideBanner();
        InHouseManager[] AllInhouses = GameObject.FindObjectsOfType<InHouseManager>();
        for (int i = 0; i < AllInhouses.Length; i++)
        {
            AllInhouses[i].HideInhouseAd();
            if (AllInhouses[i].GetComponent<ScrollViewAdsController>())
            {
                Destroy(AllInhouses[i].gameObject);
            }
        }
        Invoke("ShowBanner", 1f);
    }
    public bool NoAdsPurchased()
    {

        return (PlayerPrefs.GetInt("RemoveAds") != 0);
    }
    public bool NoCPPurchased()
    {

        return (PlayerPrefs.GetInt("RemoveCP") != 0);
    }

    #endregion

    #region RemoteConfig AdsCall

    #region Interstitial Ads
    private void ShowAdmobAd()
    {
        Debug.Log("Trying to show Admob ad");
        GoogleAdsManager.Instance.ShowInterstitialAd();
        // Implement logic to check if Admob ad is loaded and show it
        // If successful, set AdmobAdLoaded to true and return true, else return false
    }

    private void ShowIronsourceAd()
    {
        Debug.Log("Trying to show Ironsource ad");
        LevelPlayAdsManager.Instance.ShowInterstitialAd();
        // Implement logic to check if Ironsource ad is loaded and show it
        // If successful, set IronsourceAdLoaded to true and return true, else return false

    }

    private void ShowMaxAd()
    {
        Debug.Log("Trying to show Max ad");
        MaxAdsManager.Instance.ShowInterstitialAd();
        // Implement logic to check if Max ad is loaded and show it
        // If successful, set MaxAdLoaded to true and return true, else return false

    }



    public void ShowAds_RemoteConfig()
    {
        bool adShown = false;

        foreach (char adNetwork in interstitial_value)
        {

            switch (adNetwork)
            {
                case 'A':
                    if (!adShown && googleAdsManager.isInterstitialLoaded())
                    {
                        ShowAdmobAd();
                        adShown = true;
                    }
                    break;

                case 'I':
                    if (!adShown && levelPlayAdsManager.isInterstitialLoaded())
                    {
                        ShowIronsourceAd();
                        // LabAnalytics.Instance.LogEvent("ad_impression_IS_inter");
                        adShown = true;
                    }
                    break;

                case 'M':
                    if (!adShown && maxAdsManager.isInterstitialLoaded())
                    {
                        ShowMaxAd();
                        // LabAnalytics.Instance.LogEvent("ad_impression_MAX_inter");
                        adShown = true;
                    }
                    break;

                default:
                    Debug.LogError("Unknown ad network: " + adNetwork);
                    InHouseAdsController.Instance.InHouseAd.ShowAd();
                    googleAdsManager.HideNativeBanner();
                    googleAdsManager.HideBanner();
                    maxAdsManager.HideBanner();
                    maxAdsManager.HideNativeBanner();
                    levelPlayAdsManager.HideBanner();

                    googleAdsManager.CanShowAppOpen = true;
                    LoadAd();
                    LabAnalytics.Instance.LogEvent("Inter_Failed");
                    break;
            }
        }

        if (!adShown)
        {
            Debug.Log("No ads available.");
        }
    }
    #endregion

    #region Rewarded Ads
    private void R_ShowAdmobAd(Action rewardedAction)
    {
        Debug.Log("Trying to show Admob ad");
        googleAdsManager.ShowRewardedAd((bool rewarded) =>
        {
            if (rewarded)
            {
                rewardedAction?.Invoke();
                //LabAnalytics.Instance.LogEvent("ad_impression_ADMOB_Rewarded");
            }
            else
            {
                WatchCompleteAdPopUp();
            }
        });
    }

    private void R_ShowIronsourceAd(Action rewardedAction)
    {
        Debug.Log("Trying to show Ironsource ad");

        levelPlayAdsManager.ShowRewardedAd((bool rewarded) =>
        {
            if (rewarded)
            {
                rewardedAction?.Invoke();
                // LabAnalytics.Instance.LogEvent("ad_impression_IS_Rewarded");
            }
            else
            {
                WatchCompleteAdPopUp();
            }
        });
    }

    private void R_ShowMaxAd(Action rewardedAction)
    {
        Debug.Log("Trying to show Max ad");
        maxAdsManager.ShowRewardedAd((bool rewarded) =>
        {
            if (rewarded)
            {
                rewardedAction?.Invoke();
                // LabAnalytics.Instance.LogEvent("ad_impression_MAX_Rewarded");
            }
            else
            {
                WatchCompleteAdPopUp();
            }
        });
    }



    public void Rewarded_RemoteConfig(Action rewardedAction)
    {
        bool adShown = false;

        foreach (char adNetwork in rewarded_value)
        {
            switch (adNetwork)
            {
                case 'A':
                    if (!adShown && googleAdsManager.isRewardedLoaded())
                    {
                        R_ShowAdmobAd(rewardedAction);
                        // LabAnalytics.Instance.LogEvent("ad_impression_ADMOB_Rewarded");
                        adShown = true;
                    }
                    break;

                case 'I':
                    if (!adShown && levelPlayAdsManager.isRewardedLoaded())
                    {
                        R_ShowIronsourceAd(rewardedAction);
                        //  LabAnalytics.Instance.LogEvent("ad_impression_IS_Rewarded");
                        adShown = true;
                    }
                    break;

                case 'M':
                    if (!adShown && maxAdsManager.isRewardedLoaded())
                    {
                        R_ShowMaxAd(rewardedAction);
                        //  LabAnalytics.Instance.LogEvent("ad_impression_MAX_Rewarded");
                        adShown = true;
                    }
                    break;

                default:
                    Debug.LogError("Unknown ad network: " + adNetwork);
#if UNITY_IOS
                    IGDialogs.ShowOneBtnDialog("Uh-Oh", "Sorry, no ad is available at the moment. Please try again later.", "Ok", () => print(""));
#endif
#if UNITY_ANDROID
                    PopupView.Instance.OnMessagePopUp("Uh-Oh", "Sorry, no ad is available at the moment. Please try again later.");
#endif
                    googleAdsManager.CanShowAppOpen = true;
                    LoadAd();
                    LabAnalytics.Instance.LogEvent("Rewarded_Failed");
                    break;
            }
        }

        if (!adShown)
        {
#if UNITY_IOS
                    IGDialogs.ShowOneBtnDialog("Uh-Oh", "Sorry, no ad is available at the moment.", "Ok", () => print(""));
#endif
#if UNITY_ANDROID
            PopupView.Instance.OnMessagePopUp("Uh-Oh", "Sorry, no ad is available at the moment.");
#endif
            Debug.Log("No ads available.");
        }
    }
    #endregion

    #region AppOpen Ads

    public void AppOpen_RemoteConfig()
    {
        bool adShown = false;

        foreach (char adNetwork in appOpen_value)
        {
            switch (adNetwork)
            {
                case 'A':
                    if (!adShown && googleAdsManager.IsAppOpenAdAvailable)
                    {
                        ShowAppOpenAd_Admob();
                        adShown = true;
                    }
                    break;

                case 'M':
                    if (!adShown && maxAdsManager.IsHighAppOpenAdAvailable())
                    {
                        ShowAppOpenAd_AppLovin();
                        adShown = true;
                    }
                    break;

                /* case 'M':
                     if (!adShown && maxAdsManager.isRewardedLoaded())
                     {

                         adShown = true;
                     }
                     break;
 */
                default:
                    if (!googleAdsManager.NativeStatus() && !googleAdsManager.NativeStatus())
                        InHouseAdsController.Instance.AppOpen.ShowAd();
                    googleAdsManager.CanShowAppOpen = true;
                    LabAnalytics.Instance.LogEvent("AppOpen_Failed");

                    break;
            }
        }

        if (!adShown)
        {

            Debug.Log("No ads available.");
        }
    }
    public void ShowAppOpenAd_Admob()
    {
        if (googleAdsManager.IsAppOpenAdAvailable)
        {
            googleAdsManager.ShowAppOpenAd();

        }
    }
    public void ShowAppOpenAd_AppLovin()
    {
        if (maxAdsManager.IsHighAppOpenAdAvailable())
        {
            maxAdsManager.ShowHighAppOpenAd();
        }
    }
    #endregion

    #endregion

    #region AdsMethods

    void InterstitialAds()
    {

        googleAdsManager.CanShowAppOpen = false;

        if (!NoAdsPurchased())
        {
            ShowAds_RemoteConfig();
            return;
        }

        if (!NoCPPurchased())
        {

            // ShowInhouseAds
            InHouseAdsController.Instance.InHouseAd.ShowAd();
            googleAdsManager.CanShowAppOpen = true;

            return;
        }

    }
    void RewardedAds(Action rewardedAction)
    {
        googleAdsManager.CanShowAppOpen = false;
        Rewarded_RemoteConfig(rewardedAction);
    }
    void WatchCompleteAdPopUp()
    {
#if UNITY_IOS

        IGDialogs.ShowOneBtnDialog("Attention!", "Watch the full ad to receive your reward.", "Ok", () => print(""));

#endif

#if UNITY_ANDROID
        PopupView.Instance.OnMessagePopUp("Attention", "Watch the full ad to receive your reward.");
#endif
    }
    void AppOpenAds()
    {
        if (!NoAdsPurchased())
        {
            if (PlayerPrefs.GetInt("EnableAds") == 1)
            {
                InHouseAdsController.Instance.AppOpen.HideInhouseAd();
                AppOpen_RemoteConfig();
                /* if (googleAdsManager.IsAppOpenAdAvailable)
                 {
                     googleAdsManager.ShowAppOpenAd();

                 }
                 else if (maxAdsManager.IsHighAppOpenAdAvailable())
                 {
                     maxAdsManager.ShowHighAppOpenAd();
                 }
                 else
                 {
                     if (!googleAdsManager.NativeStatus() && !googleAdsManager.NativeStatus())
                     {
                         InHouseAdsController.Instance.AppOpen.ShowAd();
                     }

                     googleAdsManager.CanShowAppOpen = true;

                     LabAnalytics.Instance.LogEvent("AppOpen_Failed");
                 }
 */
            }
            return;
        }
        if (!NoCPPurchased())
        {
            InHouseAdsController.Instance.AppOpen.ShowAd();
            return;
        }
    }
    void BannerAds()
    {
        if (!NoAdsPurchased())
        {
            if (Banner != BannerType.None)
            {
                googleAdsManager.ShowBanner();
            }
            return;
        }

        if (!NoCPPurchased())
        {
            if (Banner == BannerType.SmallBanner)
            {
                InHouseAdsController.Instance.BannerInHouse.ShowAd();
            }
            else if (Banner == BannerType.LargeBanner)
            {
                InHouseAdsController.Instance.LargeBannerInHouse.ShowAd();
            }
            return;
        }
    }
    void BannerCollapsibleAds()
    {
        if (!NoAdsPurchased())
        {
            if (collapsibleBanner != BannerType.None)
            {
                googleAdsManager.ShowCollapsibleBanner();
            }
            return;
        }

        if (!NoCPPurchased())
        {

            if (Banner == BannerType.SmallBanner)
            {
                InHouseAdsController.Instance.BannerInHouse.ShowAd();
            }
            else if (Banner == BannerType.LargeBanner)
            {
                InHouseAdsController.Instance.LargeBannerInHouse.ShowAd();
            }

            return;
        }
    }
    void NativeBannerAds()
    {

        if (!NoAdsPurchased())
        {
            googleAdsManager.ShowNativeBanner();
            return;
        }
        if (!NoCPPurchased())
        {
            InHouseAdsController.Instance.NativeInHouse.ShowAd();
            return;
        }

    }
    #endregion


    #region AdLoad
    public void LoadAd()
    {
        if (co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(LoadingAds());
        Time.timeScale = 1;
    }
    IEnumerator LoadingAds()
    {
        ClearMemory();
        yield return new WaitForSecondsRealtime(1f);
        googleAdsManager.CanShowAppOpen = true;

        if (!googleAdsManager.isInterstitialLoaded() && !NoAdsPurchased())
        {
            googleAdsManager.LoadInterstitialAd();

        }

        if (!googleAdsManager.isRewardedInterLoaded())
        {
            googleAdsManager.LoadRewardedInterstitialAd();

        }
        if (!googleAdsManager.isRewardedLoaded())
        {
            googleAdsManager.LoadRewardedAd();

        }
        if (!googleAdsManager.IsAppOpenAdAvailable && !NoAdsPurchased())
        {
            googleAdsManager.LoadAppOpenAd();

        }
        if (!levelPlayAdsManager.isInterstitialLoaded() && !NoAdsPurchased())
        {
            levelPlayAdsManager.LoadInterstitial();

        }

        if (!levelPlayAdsManager.isRewardedLoaded())
        {
            levelPlayAdsManager.LoadRewarded();

        }

        if (HaveMemory())
        {
            if (!maxAdsManager.isInterstitialLoaded() && !NoAdsPurchased())
            {
                maxAdsManager.LoadInterstitial();
            }
            if (!maxAdsManager.isRewardedLoaded())
            {
                maxAdsManager.LoadRewardedAd();
            }

            if (!maxAdsManager.IsLowAppOpenAdAvailable() && !NoAdsPurchased())
            {

                maxAdsManager.LoadLowAppOpenAd();
            }
            if (!maxAdsManager.IsHighAppOpenAdAvailable() && !NoAdsPurchased())
            {
                maxAdsManager.LoadHighAppOpenAd();
            }
        }


    }
    public void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }
    #endregion

    #region Firebase Remote config
    public Task CheckRemoteConfigValues()
    {
        Debug.Log("Remote => Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Remote => Retrieval hasn't finished.");

            StartCoroutine(SetDefualtVlaue());
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"Remote => {nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            StartCoroutine(SetDefualtVlaue());
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Remote => Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                interstitial_value = remoteConfig.GetValue("interstitial").StringValue;
                rewarded_value = remoteConfig.GetValue("rewarded").StringValue;
                appOpen_value = remoteConfig.GetValue("appOpen_value").StringValue;
                game_version = remoteConfig.GetValue("game_version").StringValue;

                isBannerON = remoteConfig.GetValue("isBannerON").BooleanValue;
                isNativeBannerON = remoteConfig.GetValue("isNativeBannerON").BooleanValue;
                isAppOpenON = remoteConfig.GetValue("isAppOpenON").BooleanValue;
                isRewardedON = remoteConfig.GetValue("isRewardedON").BooleanValue;
                isIntertitialRewardedON = remoteConfig.GetValue("isIntertitialRewardedON").BooleanValue;
                CP_Ads = remoteConfig.GetValue("CP_Ads").BooleanValue;

                LabAnalytics.Instance.LogEvent("interstitial_remote_value_"+interstitial_value );
                LabAnalytics.Instance.LogEvent("rewarded_remote_value_"+rewarded_value );
                LabAnalytics.Instance.LogEvent("appOpen_remote_value_"+appOpen_value );

                StartCoroutine(CheckNullValue());
            });
    }

    private void SetDefaultRemoteConfigValues()
    {
        var defaults = new Dictionary<string, object>
    {
        { "interstitial", "AIM" },
        { "rewarded", "AIM" },
        { "appOpen_value", "AM" },
        { "game_version", "0.0.0" },
        { "isBannerON", true },
        { "isNativeBannerON", true },
        { "isAppOpenON", true },
        { "isRewardedON", true },
        { "isIntertitialRewardedON", true },
        { "CP_Ads", false }
    };

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
    }

    private IEnumerator SetDefualtVlaue()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        interstitial_value = "AIM";
        rewarded_value = "AIM";
        appOpen_value = "AM";
        game_version = "0.0.0";
        isBannerON = true;
        isNativeBannerON = true;
        isAppOpenON = true;
        isRewardedON = true;
        isIntertitialRewardedON = true;
        CP_Ads = false;

    }
    private IEnumerator CheckNullValue()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (string.IsNullOrWhiteSpace(rewarded_value))
        {
            rewarded_value = "AIM";
            Debug.Log("Remote => Rewarded Defualt Called");
        }

        if (string.IsNullOrWhiteSpace(interstitial_value))
        {
            interstitial_value = "AIM";
            Debug.Log("Remote => interstitial Defualt Called");
        }
        if (string.IsNullOrWhiteSpace(appOpen_value))
        {
            appOpen_value = "AM";
            Debug.Log("Remote => AppOpen Defualt Called");
        }
        if (string.IsNullOrWhiteSpace(game_version))
        {
            game_version = "0.0.0";
            Debug.Log($"Remote => Game Version is {game_version}");
        }

    }
    void CheckAds()
    {
        if (game_version == Application.version)
            Disable = true;
        else
            Disable = false;


        if (CP_Ads == true)
            DisableCPAds = false;
        else
            DisableCPAds = true;

    }
    #endregion




}

