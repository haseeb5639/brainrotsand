using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public class MediationInstaller
{
    private const string MediationPath = "Assets/GoogleMobileAds/Mediation";
    private const string BasePackagePath = "Assets/_GB_GTMediation/Packages/";
    /*
    #if UNITY_IOS
        [MenuItem("GB_GT Ads/Click For iOS", false, 1)]
        public static void InstallIOS()
        {
            ExecutePlatformInstallation("AdmobM_IOS.unitypackage");
        }
    #endif

    #if UNITY_ANDROID
        [MenuItem("GB_GT Ads/Click For Android", false, 1)]
        public static void InstallAndroid()
        {
            ExecutePlatformInstallation("AdmobM_Android.unitypackage");
        }
    #endif
    */
        [MenuItem("GB_GT Ads/2. Solar Engine/1. Add Solar Engine", false, 2)]
        public static void AddSolarEnginePackage()
        {
            AddSolarEngine();
        }
        [MenuItem("GB_GT Ads/2. Solar Engine/2. Remove Solar Engine", false, 2)]
        public static void RemoveSolarEnginePackage()
        {
        RemoveSolarEngine();
        }


    private static void ExecutePlatformInstallation(string packageName)
    {
        try
        {
            // 1. Delete existing mediation files
            if (DeleteFolderWithMeta(MediationPath))
            {
                // 2. Force asset database update
                AssetDatabase.Refresh();

                // 3. Wait for cleanup to complete before importing
                EditorApplication.delayCall += () =>
                {
                    ImportPackage(BasePackagePath + packageName);
                };
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Installation failed: {ex.Message}");
        }
    }

    private static bool DeleteFolderWithMeta(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                // Proper Unity file deletion with meta files
                FileUtil.DeleteFileOrDirectory(path);
                FileUtil.DeleteFileOrDirectory(path + ".meta");
                Debug.Log($"Successfully deleted: {path}");
                return true;
            }

            Debug.LogWarning($"Folder not found: {path}");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Deletion error: {ex.Message}");
            return false;
        }
    }

//--------------------------------------------------------------------------------------------------------------------------------
    private static void AddSolarEngine()
    {
        if (Directory.Exists(Application.dataPath + "/SolarEngineSDK") && Directory.Exists(Application.dataPath + "/SolarEngineNet"))
        {
            bool Output = EditorUtility.DisplayDialog("SolarEngine Already Integrated", "Heads up! Solar Engine is already integrated into the project.", "Back");
            if (Output)
            {
                
                AssetDatabase.Refresh();
                // EditorApplication.delayCall += () =>
                // {
                //     ImportPackage(BasePackagePath + "SolarEngin_in.unitypackage");
                // };
            }
            return;
        }
        else
        {
            bool Output = EditorUtility.DisplayDialog("Add SolarEngine", "Are you absolutely sure about and importing it?", "Yes", "No");
            if (Output)
            {
                AssetDatabase.Refresh();
                EditorApplication.delayCall += () =>
                {
                    ImportPackage(BasePackagePath + "SolarEngin_in.unitypackage");
                };
            }
        }
       
    }

     private static void RemoveSolarEngine()
    {
         try
        {
            // 1. Delete existing mediation files
            if (DeleteSolarEngineFolderWithMeta())
            {
                // 2. Force asset database update
                AssetDatabase.Refresh();

                // 3. Wait for cleanup to complete before importing
                EditorApplication.delayCall += () =>
                {
                    ImportPackage(BasePackagePath + "RemoveSolarEngine.unitypackage");
                };
            }
            else
            {
                Debug.Log($"Solar Engine Not Found");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Installation failed: {ex.Message}");
        }
       
    }

     private static bool DeleteSolarEngineFolderWithMeta()
    {
        try
        {
            if (Directory.Exists(Application.dataPath + "/SolarEngineSDK") && Directory.Exists(Application.dataPath + "/SolarEngineNet"))
            {
                // Proper Unity file deletion with meta files
                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/SolarEngineSDK");
                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/SolarEngineSDK" + ".meta");
                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/SolarEngineNet");
                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/SolarEngineNet" + ".meta");

                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/SolarEngine");
                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/SolarEngine" + ".meta");

                FileUtil.DeleteFileOrDirectory(Application.dataPath + "/_GB_GTAds/Scripts/SolarEngineSdkInit.cs");
                FileUtil.DeleteFileOrDirectory(Application.dataPath + "_GB_GTAds/Scripts/SolarEngineSdkInit.cs" + ".meta");

                
                return true;
            }

           
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Deletion error: {ex.Message}");
            return false;
        }
    }


//-------------------------------------------------------------------------------------------------------------------------------------------
    private static void ImportPackage(string packagePath)
    {
        try
        {
            if (!File.Exists(packagePath))
            {
                Debug.LogError($"Package not found: {packagePath}");
                return;
            }

            // Import with completion callback
            AssetDatabase.ImportPackage(packagePath, false);
            AssetDatabase.importPackageCompleted += OnPackageImportComplete;
            AssetDatabase.importPackageFailed += OnPackageImportFailed;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Import failed: {ex.Message}");
        }
    }

    private static void OnPackageImportComplete(string packageName)
    {
        Debug.Log($"Successfully imported: {packageName}");
        AssetDatabase.Refresh();

        // Add post-import setup here
        CleanupEventHandlers();
    }

    private static void OnPackageImportFailed(string packageName, string errorMessage)
    {
        Debug.LogError($"Failed to import {packageName}: {errorMessage}");
        CleanupEventHandlers();
    }

    private static void CleanupEventHandlers()
    {
        AssetDatabase.importPackageCompleted -= OnPackageImportComplete;
        AssetDatabase.importPackageFailed -= OnPackageImportFailed;
    }
}