#if UNITY_EDITOR
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEditor.PackageManager.Requests;
using GoogleMobileAds.Editor;
#endif
using UnityEditor;
using UnityEngine;
using System.IO;

public class AdsSettings : MonoBehaviour
{
#if UNITY_EDITOR
    static AddRequest Request;

#if UNITY_ANDROID
    [MenuItem("GB_GT Ads /1. Apply Settings", false, 1)]
    public static void ApplyAndroidSettings()
    {
        // GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_GB_GTAds/Resources/AdsManager.prefab");



        GameObject prefab = Resources.Load<GameObject>("AdsManager");
        AdsConfigLoader adsconfig = prefab.GetComponent<AdsConfigLoader>();
        //  AdsConfigLoader adsconfig = AdsConfigLoader.Instance;
        adsconfig.LoadIds();
        PlayerSettings.productName = adsconfig.adsConfigAndroid.AppName;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, adsconfig.adsConfigAndroid.PackageName);
        PlayerSettings.bundleVersion = adsconfig.adsConfigAndroid.Version;
        PlayerSettings.Android.bundleVersionCode = adsconfig.adsConfigAndroid.BundleCode;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.keystorePass = adsconfig.adsConfigAndroid.KeystorePassword;
        PlayerSettings.keyaliasPass = adsconfig.adsConfigAndroid.KeystorePassword;
        PlayerSettings.Android.keystoreName = Application.dataPath + "/Keystore/" + adsconfig.adsConfigAndroid.KeystoreName;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        AndroidArchitecture aac = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        PlayerSettings.Android.targetArchitectures = aac;
        GoogleMobileAdsSettings instance = GoogleMobileAdsSettings.LoadInstance();
        instance.GoogleMobileAdsAndroidAppId = adsconfig.adsConfigAndroid.AdmobAppID;
        EditorUtility.SetDirty(instance);

        AppLovinSettings appLovinSettings = Resources.Load<AppLovinSettings>("AppLovinSettings");
        appLovinSettings.AdMobAndroidAppId = adsconfig.adsConfigAndroid.AdmobAppID;
        appLovinSettings.SdkKey = adsconfig.adsConfigAndroid.AppLovinSDKKey;
        EditorUtility.SetDirty(appLovinSettings);


        IronSourceMediationSettings LevelPlaySettings = Resources.Load<IronSourceMediationSettings>("IronSourceMediationSettings");
        LevelPlaySettings.AndroidAppKey = adsconfig.adsConfigAndroid.LevelPlayKey;
        EditorUtility.SetDirty(LevelPlaySettings);
        //  EditorApplication.ExecuteMenuItem("Assets/LevelPlay/Developer Settings/LevelPlay Mediation Settings");
        EditorApplication.ExecuteMenuItem("Ads Mediation/Developer Settings/LevelPlay Mediation Settings");
        //  EditorApplication.ExecuteMenuItem("Assets/LevelPlay/Developer Settings/Mediated Network Settings");
        EditorApplication.ExecuteMenuItem("Ads Mediation/Developer Settings/Mediated Network Settings");
        EditorApplication.ExecuteMenuItem("Assets/Google Mobile Ads/Settings...");
        EditorApplication.ExecuteMenuItem("Window/GB_GT/SetIcon");
        SettingsService.OpenProjectSettings("Project/Player");
        AddNotifications();
        if (AssetDatabase.IsValidFolder("Assets/Plugins/Android/GoogleMobileAdsPlugin"))
        {
            AssetDatabase.RenameAsset("Assets/Plugins/Android/GoogleMobileAdsPlugin", "GoogleMobileAdsPlugin.androidlib");

        }
        if (AssetDatabase.IsValidFolder("Assets/IronSource/Plugins/Android/IronSource"))
        {
            AssetDatabase.RenameAsset("Assets/IronSource/Plugins/Android/IronSource", "IronSource.androidlib");

        }




        EditorSceneManager.SaveOpenScenes();

    }

#endif
#if UNITY_IOS
    [MenuItem("GB_GT Ads /1. Apply Settings", false, 1)]
    public static void SetiOSSettings()
    {

        //  GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_GB_GTAds/Resources/AdsManager.prefab");
        GameObject prefab = Resources.Load<GameObject>("AdsManager");


        AdsConfigLoader adsconfig = prefab.GetComponent<AdsConfigLoader>();

        // AdsConfigLoader adsconfig = AdsConfigLoader.Instance;
        adsconfig.LoadIds();
        PlayerSettings.productName = adsconfig.adsConfigIOS.AppName;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, adsconfig.adsConfigIOS.PackageName);
        PlayerSettings.bundleVersion = adsconfig.adsConfigIOS.Version;
        PlayerSettings.iOS.buildNumber = adsconfig.adsConfigIOS.BundleCode;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.iOS.hideHomeButton = true;
        GoogleMobileAdsSettings instance = GoogleMobileAdsSettings.LoadInstance();
        instance.GoogleMobileAdsIOSAppId = adsconfig.adsConfigIOS.AdmobAppID;
        EditorUtility.SetDirty(instance);

        AppLovinSettings appLovinSettings = Resources.Load<AppLovinSettings>("AppLovinSettings");
        appLovinSettings.AdMobIosAppId = adsconfig.adsConfigIOS.AdmobAppID;
        appLovinSettings.SdkKey = adsconfig.adsConfigIOS.AppLovinSDKKey;
        EditorUtility.SetDirty(appLovinSettings);

        IronSourceMediationSettings LevelPlaySettings = Resources.Load<IronSourceMediationSettings>("IronSourceMediationSettings");
        LevelPlaySettings.IOSAppKey = adsconfig.adsConfigIOS.LevelPlayKey;
        EditorUtility.SetDirty(LevelPlaySettings);
        //  EditorApplication.ExecuteMenuItem("Assets/LevelPlay/Developer Settings/LevelPlay Mediation Settings");
        EditorApplication.ExecuteMenuItem("Ads Mediation/Developer Settings/LevelPlay Mediation Settings");
        //  EditorApplication.ExecuteMenuItem("Assets/LevelPlay/Developer Settings/Mediated Network Settings");
        EditorApplication.ExecuteMenuItem("Ads Mediation/Developer Settings/Mediated Network Settings");
        EditorApplication.ExecuteMenuItem("Assets/Google Mobile Ads/Settings...");
        EditorApplication.ExecuteMenuItem("Window/GB_GT/SetIcon");
        SettingsService.OpenProjectSettings("Project/Player");
        AddNotifications();
        EditorSceneManager.SaveOpenScenes();
    }
#endif


    #region Helpers
    static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                Object notificationobject = AssetDatabase.LoadAssetAtPath<Object>("Assets/_GB_GTAds/Notifications/Prefab/NotificationsPrefab.prefab");
                if (notificationobject == null)
                {
                    AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/Notification.unitypackage", false);
                }
                EditorApplication.ExecuteMenuItem("Window/GB_GT/Get Notifications");
                EditorApplication.ExecuteMenuItem("Window/GB_GT/Notifications");
                EditorSceneManager.SaveOpenScenes();
                EditorUtility.ClearProgressBar();
            }
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);

            EditorApplication.update -= Progress;

        }

    }

    public static bool IsPackageInstalled(string packageId)
    {
        if (!File.Exists("Packages/manifest.json"))
            return false;

        string jsonText = File.ReadAllText("Packages/manifest.json");
        return jsonText.Contains(packageId);
    }

    public static void AddNotifications()
    {
        Object notificationobject = AssetDatabase.LoadAssetAtPath<Object>("Assets/_GB_GTAds/Notifications/Prefab/NotificationsPrefab.prefab");


        if (!IsPackageInstalled("com.unity.mobile.notifications"))
        {
            Request = Client.Add("com.unity.mobile.notifications");
            EditorUtility.DisplayProgressBar("9 FOX LABS", "Mobile Notifications Importing", 0.8f);
            EditorApplication.update += Progress;
        }
        else
        {
            if (notificationobject == null)
            {
                AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/Notification.unitypackage", false);
            }
            EditorApplication.ExecuteMenuItem("Window/GB_GT/Get Notifications");
            EditorApplication.ExecuteMenuItem("Window/GB_GT/Notifications");
            EditorSceneManager.SaveOpenScenes();
        }
    }


    #endregion

#if UNITY_IOS
   // [MenuItem("GB_GT Ads /2. Import IOS Mediation", false, 1)]

    public static void Install_IOS()
    {
        string mediationPath = "Assets/GoogleMobileAds/Mediation";

        // 1. Delete existing mediation files
        if (DeleteFolder(mediationPath))
        {
            // 2. Ensure full filesystem/AssetDatabase sync
            AssetDatabase.Refresh();
            EditorApplication.delayCall += () =>
            {
                // 3. Import after Unity processes deletions
                ImportUnityPackage_IOS();
            };
        }
        else
        {
            Debug.LogError("Failed to delete mediation folder!");
        }
    }


    private static void ImportUnityPackage_IOS()
    {
        string packagePath = "Assets/_GB_GTMediation/Packages/AdmobM_IOS.unitypackage";

        if (File.Exists(packagePath))
        {
            AssetDatabase.ImportPackage(packagePath, false);
            Debug.Log($"Started importing: {packagePath}");

            // Optional: Add callback for completion
            AssetDatabase.importPackageCompleted += OnImportComplete;
        }
        else
        {
            Debug.LogError($"Package not found: {packagePath}");
        }
    }


#endif

#if UNITY_ANDROID
    //  [MenuItem("GB_GT Ads /2. Import Android Mediation", false, 1)]
    public static void Install_Android()
    {
        string mediationPath = "Assets/GoogleMobileAds/Mediation";

        // 1. Delete existing mediation files
        if (DeleteFolder(mediationPath))
        {
            // 2. Ensure full filesystem/AssetDatabase sync
            AssetDatabase.Refresh();
            EditorApplication.delayCall += () =>
            {
                // 3. Import after Unity processes deletions
                ImportUnityPackage_Android();
            };
        }
        else
        {
            Debug.LogError("Failed to delete mediation folder!");
        }
    }


    private static void ImportUnityPackage_Android()
    {
        string packagePath = "Assets/_GB_GTMediation/Packages/AdmobM_Android.unitypackage";

        if (File.Exists(packagePath))
        {
            AssetDatabase.ImportPackage(packagePath, false);
            Debug.Log($"Started importing: {packagePath}");

            // Optional: Add callback for completion
            AssetDatabase.importPackageCompleted += OnImportComplete;
        }
        else
        {
            Debug.LogError($"Package not found: {packagePath}");
        }
    }
#endif




    // Assets/GoogleMobileAds/Mediation
    private static bool DeleteFolder(string folderPath)
    {
        try
        {
            if (Directory.Exists(folderPath))
            {
                // Use Unity's deletion method for proper meta file handling
                FileUtil.DeleteFileOrDirectory(folderPath);
                FileUtil.DeleteFileOrDirectory(folderPath + ".meta");
                Debug.Log($"Successfully deleted: {folderPath}");
                return true;
            }

            Debug.LogWarning($"Folder not found: {folderPath}");
            return false;
        }
        catch (IOException ex)
        {
            Debug.LogError($"Delete failed: {ex.Message}");
            return false;
        }
    }

    private static void OnImportComplete(string packageName)
    {
        Debug.Log($"Successfully imported: {packageName}");
        AssetDatabase.Refresh();

        // Re-enable any required SDKs
        // Your post-import logic here
    }


    /* [MenuItem("GB_GT Ads /3. Import InApp Purchases", false, 1)]
     public static void Add_InApps()
     {
         string packagePath = "Assets/_GB_GTMediation/Packages/InApps.unitypackage";

         if (File.Exists(packagePath))
         {
             AssetDatabase.ImportPackage(packagePath, false);
             Debug.Log($"Started importing: {packagePath}");

             // Optional: Add callback for completion
             AssetDatabase.importPackageCompleted += OnImportComplete;
         }
         else
         {
             Debug.LogError($"Package not found: {packagePath}");
         }
     }*/
#endif

}
