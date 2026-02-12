using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using Sirenix.Utilities;

public class AdsConfigLoader : MonoBehaviour
{
    #region Singleton
    private static AdsConfigLoader _Instance;
    public static AdsConfigLoader Instance
    {
        get
        {

            if (!_Instance) _Instance = FindObjectOfType<AdsConfigLoader>();

            return _Instance;
        }
    }
    void Awake()
    {
        if (!_Instance) _Instance = this;
    }

    #endregion


    public AdsConfigIOS adsConfigIOS;
    public AdsConfigAndroid adsConfigAndroid;

    public TextAsset iOSIdsTextFile;
    public TextAsset androidIdsTextFile;

    private const string KEY_IOS = "iOS";
    private const string KEY_ANDROID = "Android";
    private string AdIdsJson;




    private void Start()
    {

        LoadIds();
#if UNITY_IOS
        AdIdsJson = adsConfigIOS.JsonLink;
#endif
#if UNITY_ANDROID
        AdIdsJson = adsConfigAndroid.JsonLink;
#endif
        if (!string.IsNullOrEmpty(AdIdsJson))
        {
            StartCoroutine(DownloadAdIdsFile());
        }
        else
        {
            LoadIds();
        }

    }
    public void LoadIds()
    {

        LoadIdsFromTextFile(iOSIdsTextFile, adsConfigIOS);
        LoadIdsFromTextFile(androidIdsTextFile, adsConfigAndroid);
    }

    private void LoadIdsFromTextFile(TextAsset textFile, ScriptableObject config)
    {
        if (textFile != null)
        {
            string[] lines = textFile.text.Split('\n');
            foreach (string line in lines)
            {


                string[] parts = line.Trim().Split(new string[] { "X:X" }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();

                    string value = parts[1].Trim();
                    //if(key == "JsonLink")
                    //{
                    //    if(string.IsNullOrEmpty(value))
                    //    {
                    //        SetField(config, key, value);
                    //    }
                    //    return;
                    //}
                    SetField(config, key, value);
                }
                //if(parts.Length == 3)
                //{
                //    string key = parts[0].Trim();
                //    string value = parts[1].Trim() + ":" + parts[2].Trim();
                //    SetField(config, key, value);

                //}
            }
        }
    }


    public void SetField(ScriptableObject config, string fieldName, string value)
    {
        FieldInfo field = config.GetType().GetField(fieldName);

        if (field != null)
        {
            try
            {
                if (field.FieldType == typeof(string))
                {
                    field.SetValue(config, value);
                }
                else if (field.FieldType == typeof(int))
                {
                    int intValue;
                    if (int.TryParse(value, out intValue))
                    {
                        field.SetValue(config, intValue);
                    }
                    else
                    {
                        Debug.LogError($"Failed to parse '{value}' as an integer for field {fieldName}");
                    }
                }
                // Add more data type cases as needed

                // You can handle other data types here
            }
            catch (Exception e)
            {
                Debug.LogError($"Error setting field {fieldName}: {e.Message}");
            }
        }
        else
        {
            Debug.LogError($"Field {fieldName} not found in AdsConfig");
        }
    }



    #region JsonIds

    private IEnumerator DownloadAdIdsFile()
    {

#if UNITY_IOS
            string platformSuf = KEY_IOS;
#else
        string platformSuf = KEY_ANDROID;
#endif
        string url = AdIdsJson; // Replace with the shared link of your Dropbox file
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonData = www.downloadHandler.text;
            JSONNode rootNode = JSON.Parse(jsonData);
            JSONArray appsArray = rootNode[platformSuf].AsArray;



            for (int i = 0; i < appsArray.Count; ++i)
            {
                AdIdsData item = parseIDs(appsArray[i]);
                AssignIds(item);
            }




        }
        else
        {

            LoadIds();

            Debug.LogError("Error downloading JSON file: " + www.error);
        }
    }

    private void AssignIds(AdIdsData data)
    {

#if UNITY_ANDROID

        /// Assigning Admob values to Android AdsConfig
        adsConfigAndroid.LowBanner = data.LowBanner;
        adsConfigAndroid.BannerID = data.BannerID;
        adsConfigAndroid.BannerID = data.CollapsibleBannerID;
        adsConfigAndroid.AppOpenID = data.AppOpenID;
        adsConfigAndroid.LowAppOpenID = data.LowAppOpenID;
        adsConfigAndroid.LowInterstitialID = data.LowInterstitialID;
        adsConfigAndroid.InterstitialID = data.InterstitialID;
        adsConfigAndroid.RewardedID = data.RewardedID;
        adsConfigAndroid.RewardedInterstitial = data.RewardedInterstitial;
        adsConfigAndroid.NativeBanner = data.NativeBanner;
        adsConfigAndroid.LowNativeBanner = data.LowNativeBanner;
        adsConfigAndroid.AdmobAppID = data.AdmobAppID;


        // Assigning Max values to Android AdsConfig
        adsConfigAndroid.AppLovinSDKKey = data.AppLovinSDKKey;
        adsConfigAndroid.AppLovinBannerID = data.AppLovinBannerID;
        adsConfigAndroid.AppLovinHighAppOpenID = data.AppLovinHighAppOpenID;
        adsConfigAndroid.AppLovinLowAppOpenID = data.AppLovinLowAppOpenID;
        adsConfigAndroid.AppLovinInterstitialID = data.AppLovinInterstitialID;
        adsConfigAndroid.AppLovinRewardedID = data.AppLovinRewardedID;
        adsConfigAndroid.AppLovinNativeBannerID = data.AppLovinNativeBannerID;


        // Assigning LevelPlay values to Android AdsConfig
        adsConfigAndroid.LevelPlayKey = data.LevelPlayKey;
        adsConfigAndroid.LevelPlayBannerID = data.LevelPlayBannerID;
        adsConfigAndroid.LevelPlayInterstitialID = data.LevelPlayInterstitialID;
        adsConfigAndroid.LevelPlayRewardedID = data.LevelPlayRewardedID;

#endif


#if UNITY_IOS

        /// Assigning Admob values to iOS AdsConfig
        adsConfigIOS.LowBanner = data.LowBanner;
        adsConfigIOS.BannerID = data.BannerID;
        adsConfigIOS.BannerID = data.CollapsibleBannerID;
        adsConfigIOS.AppOpenID = data.AppOpenID;
        adsConfigIOS.InterstitialID = data.InterstitialID;
        adsConfigIOS.RewardedID = data.RewardedID;
        adsConfigIOS.RewardedInterstitial = data.RewardedInterstitial;
        adsConfigIOS.NativeBanner = data.NativeBanner;
        adsConfigIOS.LowNativeBanner = data.LowNativeBanner;
        adsConfigIOS.AdmobAppID = data.AdmobAppID;

      
        // Assigning Max values to iOS AdsConfig
        adsConfigIOS.AppLovinSDKKey = data.AppLovinSDKKey;
         adsConfigIOS.AppLovinBannerID = data.AppLovinBannerID;
        adsConfigIOS.AppLovinHighAppOpenID = data.AppLovinHighAppOpenID;
        adsConfigIOS.AppLovinLowAppOpenID = data.AppLovinLowAppOpenID;
        adsConfigIOS.AppLovinInterstitialID = data.AppLovinInterstitialID;
        adsConfigIOS.AppLovinRewardedID = data.AppLovinRewardedID;
        adsConfigIOS.AppLovinNativeBannerID = data.AppLovinNativeBannerID;
       

        // Assigning LevelPlay values to iOS AdsConfig
        adsConfigIOS.LevelPlayKey = data.LevelPlayKey;
        adsConfigIOS.LevelPlayBannerID = data.LevelPlayBannerID;
        adsConfigIOS.LevelPlayInterstitialID = data.LevelPlayInterstitialID;
        adsConfigIOS.LevelPlayRewardedID = data.LevelPlayRewardedID;

#endif

        MaxAdsManager.Instance.GetIds();


        AdsManager.Instance.LoadAd();

        if (AdsManager.Instance.game_version == Application.version)
        {
            AdsManager.Instance.Disable = true;
        }
        else
        {
            AdsManager.Instance.Disable = false;
        }
        if (AdsManager.Instance.CP_Ads == true)
        {
            AdsManager.Instance.DisableCPAds = false;
        }
        else
        {
            AdsManager.Instance.DisableCPAds = true;
        }


        /*        if (data.DisableAdsVersion == Application.version)
                {
                    AdsManager.Instance.Disable = true;
                }
                else
                {
                    AdsManager.Instance.Disable = false;
                }*/


        /* if (data.DisableCPAds == "0")
         {
             AdsManager.Instance.DisableCPAds = true;
         }
         else if (data.DisableCPAds == "1")
         {
             AdsManager.Instance.DisableCPAds = false;
         }
         else
         {
             AdsManager.Instance.DisableCPAds = false;
         }*/

    }


    private static AdIdsData parseIDs(JSONNode node)
    {
        AdIdsData adids = new AdIdsData();
#if UNITY_ANDROID
        // Assigning Admob values to Android AdIdsData
        adids.LowBanner = node["LowBanner"];
        adids.BannerID = node["BannerID"];
        adids.BannerID = node["CollapsibleBannerID"];
        adids.AppOpenID = node["AppOpenID"];
        adids.LowAppOpenID = node["LowAppOpenID"];
        adids.LowInterstitialID = node["LowInterstitialID"];
        adids.InterstitialID = node["InterstitialID"];
        adids.RewardedID = node["RewardedID"];
        adids.RewardedInterstitial = node["RewardedInterstitial"];
        adids.NativeBanner = node["NativeBanner"];
        adids.LowNativeBanner = node["LowNativeBanner"];
        adids.AdmobAppID = node["AdmobAppID"];

        // Assigning Max values to Android AdIdsData
        adids.AppLovinSDKKey = node["AppLovinSDKKey"];
        adids.AppLovinBannerID = node["AppLovinBannerID"];
        adids.AppLovinHighAppOpenID = node["AppLovinHighAppOpenID"];
        adids.AppLovinLowAppOpenID = node["AppLovinLowAppOpenID"];
        adids.AppLovinInterstitialID = node["AppLovinInterstitialID"];
        adids.AppLovinRewardedID = node["AppLovinRewardedID"];
        adids.AppLovinNativeBannerID = node["AppLovinNativeBannerID"];


        // Assigning LevelPlay values to Android AdIdsData
        adids.LevelPlayKey = node["LevelPlayKey"];
        adids.LevelPlayBannerID = node["LevelPlayBannerID"];
        adids.LevelPlayInterstitialID = node["LevelPlayInterstitialID"];
        adids.LevelPlayRewardedID = node["LevelPlayRewardedID"];


        // Assigning DisableAdsVersion and DisableCPAds values to Android AdIdsData
        adids.DisableAdsVersion = node["DisableAdsVersion"];
        adids.DisableCPAds = node["DisableCPAds"];

#endif

#if UNITY_IOS

        // Assigning Admob values to iOS AdIdsData
        adids.LowBanner = node["LowBanner"];
        adids.BannerID = node["BannerID"];
        adids.BannerID = node["CollapsibleBannerID"];
        adids.AppOpenID = node["AppOpenID"];
        adids.InterstitialID = node["InterstitialID"];
        adids.RewardedID = node["RewardedID"];
        adids.RewardedInterstitial = node["RewardedInterstitial"];
        adids.NativeBanner = node["NativeBanner"];
        adids.LowNativeBanner = node["LowNativeBanner"];
        adids.AdmobAppID = node["AdmobAppID"];

      
        // Assigning Max values to iOS AdIdsData
        adids.AppLovinSDKKey = node["AppLovinSDKKey"];
         adids.AppLovinBannerID = node["AppLovinBannerID"];
        adids.AppLovinHighAppOpenID = node["AppLovinHighAppOpenID"];
        adids.AppLovinLowAppOpenID = node["AppLovinLowAppOpenID"];
        adids.AppLovinInterstitialID = node["AppLovinInterstitialID"];
        adids.AppLovinRewardedID = node["AppLovinRewardedID"];
        adids.AppLovinNativeBannerID = node["AppLovinNativeBannerID"];

        // Assigning LevelPlay values to iOS AdIdsData
        adids.LevelPlayKey = node["LevelPlayKey"];
        adids.LevelPlayBannerID = node["LevelPlayBannerID"];
        adids.LevelPlayInterstitialID = node["LevelPlayInterstitialID"];
        adids.LevelPlayRewardedID = node["LevelPlayRewardedID"];

        // Assigning DisableAdsVersion and DisableCPAds values to iOS AdIdsData
        adids.DisableAdsVersion = node["DisableAdsVersion"];
        adids.DisableCPAds = node["DisableCPAds"];

#endif
        return adids;

    }
    public class AdIdsData
    {

#if UNITY_ANDROID
        // Assigning Admob values to Android AdIdsData
        public string LowBanner = "";
        public string BannerID = "";
        public string CollapsibleBannerID = "";
        public string AppOpenID = "";
        public string LowAppOpenID = "";
        public string LowInterstitialID = "";
        public string InterstitialID = "";
        public string RewardedID = "";
        public string RewardedInterstitial = "";
        public string NativeBanner = "";
        public string LowNativeBanner = "";
        public string AdmobAppID = "";

        // Assigning Max values to Android AdIdsData
        public string AppLovinSDKKey = "";
        public string AppLovinBannerID = "";
        public string AppLovinHighAppOpenID = "";
        public string AppLovinLowAppOpenID = "";
        public string AppLovinInterstitialID = "";
        public string AppLovinRewardedID = "";
        public string AppLovinNativeBannerID = "";

        // Assigning LevelPlay values to Android AdIdsData
        public string LevelPlayKey = "";
        public string LevelPlayBannerID = "";
        public string LevelPlayInterstitialID = "";
        public string LevelPlayRewardedID = "";



        // Assigning DisableAdsVersion and DisableCPAds values to Android AdIdsData
        public string DisableAdsVersion = "";
        public string DisableCPAds = "";
#endif

#if UNITY_IOS

        // Assigning Admob values to iOS AdIdsData
        public string LowBanner = "";
        public string BannerID = "";
        public string CollapsibleBannerID = "";
        public string AppOpenID = "";
        public string InterstitialID = "";
        public string RewardedID = "";
        public string RewardedInterstitial = "";
        public string NativeBanner = "";
        public string LowNativeBanner = "";
        public string AdmobAppID = "";

      
        // Assigning Max values to iOS AdIdsData
        public string AppLovinSDKKey = "";
       public string AppLovinBannerID = "";
        public string AppLovinHighAppOpenID = "";
        public string AppLovinLowAppOpenID = "";
        public string AppLovinInterstitialID = "";
        public string AppLovinRewardedID = "";
        public string AppLovinNativeBannerID = "";


        // Assigning LevelPlay values to iOS AdIdsData
        public string LevelPlayKey = "";
        public string LevelPlayBannerID = "";
        public string LevelPlayInterstitialID = "";
        public string LevelPlayRewardedID = "";
        
        // Assigning DisableAdsVersion and DisableCPAds values to iOS AdIdsData
        public string DisableAdsVersion = "";
        public string DisableCPAds = "";

#endif



        public AdIdsData()
        {

        }
    }

    #endregion
}