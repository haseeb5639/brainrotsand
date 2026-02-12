#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.Services.LevelPlay;
using Unity.Services.LevelPlay.Editor;
using System.Reflection;

#pragma warning disable 0618

public class IronSourceMenu : Editor
{
    const string IntegrationManagerMenuTitle = "Network Manager";

    [MenuItem("Ads Mediation/Documentation", false, 0)]
    public static void Documentation()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenDocumentation);

        Application.OpenURL("https://developers.is.com/ironsource-mobile/unity/unity-plugin/");
    }

    [MenuItem("Ads Mediation/SDK Change Log", false, 1)]
    public static void ChangeLog()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenChangelog);

        Application.OpenURL("https://developers.is.com/ironsource-mobile/unity/sdk-change-log/");
    }

    [MenuItem("Ads Mediation/" + IntegrationManagerMenuTitle, false, 2)]
    public static void SdkManagerProd()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenNetworkManager);

        IronSourceDependenciesManager.ShowISDependenciesManager();
    }

    [MenuItem("Ads Mediation/Developer Settings/LevelPlay Mediation Settings", false, 3)]
    public static void mediationSettings()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuDeveloperSettings,
            EditorAnalyticsService.LevelPlayAction.OpenLevelPlayMediationSettings);

        var path = "Assets/LevelPlay/Resources";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }


        var ironSourceMediationSettings = Resources.Load<IronSourceMediationSettings>(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME);
        if (ironSourceMediationSettings == null)
        {
            EditorServices.Instance.LevelPlayLogger.LogWarning(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME + " can't be found, creating a new one...");
            ironSourceMediationSettings = CreateInstance<IronSourceMediationSettings>();
            AssetDatabase.CreateAsset(ironSourceMediationSettings, IronSourceMediationSettings.IRONSOURCE_SETTINGS_ASSET_PATH);
            ironSourceMediationSettings = Resources.Load<IronSourceMediationSettings>(IronSourceConstants.IRONSOURCE_MEDIATION_SETTING_NAME);
        }

        Selection.activeObject = ironSourceMediationSettings;
    }

    [MenuItem("Ads Mediation/Developer Settings/Mediated Network Settings", false, 4)]
    public static void mediatedNetworkSettings()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuDeveloperSettings,
            EditorAnalyticsService.LevelPlayAction.OpenMediatedNetworkSettings);

        var path = IronSourceConstants.IRONSOURCE_RESOURCES_PATH;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var ironSourceMediatedNetworkSettings = Resources.Load<IronSourceMediatedNetworkSettings>(IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME);
        if (ironSourceMediatedNetworkSettings == null)
        {
            EditorServices.Instance.LevelPlayLogger.LogWarning(IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME + " can't be found, creating a new one...");
            ironSourceMediatedNetworkSettings = CreateInstance<IronSourceMediatedNetworkSettings>();
            AssetDatabase.CreateAsset(ironSourceMediatedNetworkSettings, IronSourceMediatedNetworkSettings.MEDIATION_SETTINGS_ASSET_PATH);
            ironSourceMediatedNetworkSettings = Resources.Load<IronSourceMediatedNetworkSettings>(IronSourceConstants.IRONSOURCE_MEDIATED_NETWORK_SETTING_NAME);
        }

        ironSourceMediatedNetworkSettings.EnableAdmob = true; // Ensure AdMob is enabled by default

        // Use reflection to access AdsConfigLoader
        // Load the 9FoxAds prefab from the Resources folder
        GameObject prefab = Resources.Load<GameObject>("AdsManager");
        if (prefab != null)
        {
            // Get the AdsConfigLoader component using reflection
            var adsConfigLoader = prefab.GetComponent("AdsConfigLoader");
            if (adsConfigLoader != null)
            {
#if UNITY_IOS
            var adsConfigIOSField = adsConfigLoader.GetType().GetField("adsConfigIOS", BindingFlags.Instance | BindingFlags.Public);
            var adsConfigIOS = adsConfigIOSField?.GetValue(adsConfigLoader);
            if (adsConfigIOS != null)
            {
                var admobAppIDField = adsConfigIOS.GetType().GetField("AdmobAppID");
                if (admobAppIDField != null)
                {
                    var admobAppID = (string)admobAppIDField.GetValue(adsConfigIOS);
                    ironSourceMediatedNetworkSettings.AdmobIOSAppId = admobAppID;
                }
            }
#endif
#if UNITY_ANDROID
                var adsConfigAndroidField = adsConfigLoader.GetType().GetField("adsConfigAndroid", BindingFlags.Instance | BindingFlags.Public);
                var adsConfigAndroid = adsConfigAndroidField?.GetValue(adsConfigLoader);
                if (adsConfigAndroid != null)
                {
                    var admobAppIDField = adsConfigAndroid.GetType().GetField("AdmobAppID");
                    if (admobAppIDField != null)
                    {
                        var admobAppID = (string)admobAppIDField.GetValue(adsConfigAndroid);
                        ironSourceMediatedNetworkSettings.AdmobAndroidAppId = admobAppID;
                    }
                }
#endif
            }
            else
            {
                Debug.LogWarning("AdsConfigLoader component could not be found on the  prefab.");
            }
        }
        else
        {
            Debug.LogWarning("9FoxAds prefab could not be found in the Resources folder.");
        }

        EditorUtility.SetDirty(ironSourceMediatedNetworkSettings);


        Selection.activeObject = ironSourceMediatedNetworkSettings;
    }

    [MenuItem("Ads Mediation/Troubleshooting", false, 5)]
    public static void TroubleShootingGuide()
    {
        EditorServices.Instance.EditorAnalyticsService.SendEventEditorClick(
            EditorAnalyticsService.LevelPlayComponent.TopMenuAdsMediation,
            EditorAnalyticsService.LevelPlayAction.OpenTroubleShootingGuide);

        Application.OpenURL("https://docs.unity.com/monetization-dashboard/en-us/manual/editor-levelplay-integration#Troubleshooting");
    }
}
#endif
