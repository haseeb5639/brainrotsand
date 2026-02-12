#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.IO;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;

public class MediationEditorWindow : OdinEditorWindow
{

    static AddRequest Request;
    static RemoveRequest RemoveRequest;
    // private const string LogoPath = "Assets/_GB_GTMediation/Editor/Logo.png"; // Update this with the path to your logo image

    private string currentVersion = "1.0.4.4"; // Update this with your actual current version


    private GUIStyle addButtonStyle;
    private GUIStyle removeButtonStyle;



    private string statusMessage = "";
    private double statusMessageClearTime = 0;

  //  [MenuItem("GB_GT Ads/Ads")]
    private static void OpenWindow()
    {
        GetWindow<MediationEditorWindow>().Show();
    }


    private void InitializeStyles()
    {
        addButtonStyle = new GUIStyle(GUI.skin.button)
        {
            normal = { textColor = Color.white },
            hover = { background = MakeTex(2, 2, new Color(0.2f, 0.8f, 0.2f)) }
        };

        removeButtonStyle = new GUIStyle(GUI.skin.button)
        {
            normal = { textColor = Color.white },
            hover = { background = MakeTex(2, 2, new Color(0.8f, 0.2f, 0.2f)) }
        };

        // Set minimum and maximum window size
        minSize = new Vector2(400, 600);
        maxSize = new Vector2(800, 600);
        titleContent = new GUIContent("GameBus_GameTruck");
        EditorApplication.update += UpdateStatusMessage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EditorApplication.update -= UpdateStatusMessage;
    }
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
    protected override void OnGUI()
    {
        InitializeStyles();
        GUILayout.FlexibleSpace(); // Push everything to the top
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Push the logo to the center
        //Texture2D logo = AssetDatabase.LoadAssetAtPath<Texture2D>(LogoPath);
        //if (logo != null)
        //{
        //    GUILayout.Label(logo, GUILayout.Height(100), GUILayout.Width(100));
        //}
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // Header
        GUILayout.Space(5);
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("GameBus/GameTruck", headerStyle);
        GUILayout.Label("Studio", headerStyle);
        GUILayout.Label($"Version: {currentVersion}", EditorStyles.centeredGreyMiniLabel);

        GUILayout.Space(20);


        // Buttons
        GUILayout.BeginVertical();
        GUILayout.Space(5);
//-----------------------------------------------------------------------------------------------------------------------
        DrawSection("Mediation");

        GUILayout.BeginHorizontal();

        GUIContent MediationBtnContent = new GUIContent("Add Mediation", "Add The Mediation");
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(MediationBtnContent, addButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for adding mediation here
            AddMediation();
            ShowStatusMessage("Added");
        }

        GUIContent RemoveMediationBtnContent = new GUIContent("Remove Mediation", "Remove The Mediation");
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(RemoveMediationBtnContent, removeButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for removing mediation here
            RemoveMediation(true);
            ShowStatusMessage("Removed");
        }

        GUILayout.EndHorizontal();
        DrawSeparator();

//-----------------------------------------------------------------------------------------------------------------------        
        DrawSection("FireBase");

        GUILayout.BeginHorizontal();


        GUIContent FireBaseBtnContent = new GUIContent("Add FireBase", "Add Firebase");
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(FireBaseBtnContent, addButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for adding mediation here
            AddFirebase();
            ShowStatusMessage("Added");
        }

        GUIContent RemoveFireBaseBtnContent = new GUIContent("Remove FireBase", "Remove Firebase");
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(RemoveFireBaseBtnContent, removeButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for removing mediation here
            RemoveFirebase(true);
            ShowStatusMessage("Removed");
        }

        GUILayout.EndHorizontal();
        DrawSeparator();
//-----------------------------------------------------------------------------------------------------------------------
        DrawSection("InApps");

        GUILayout.BeginHorizontal();

        GUIContent InAppsBtnContent = new GUIContent("Add InApps", "Add In App Purchasing");
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(InAppsBtnContent, addButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for adding mediation here
            AddInApps();
            ShowStatusMessage("Added");
        }

        GUIContent RemoveInAppsBtnContent = new GUIContent("Remove InApps", "Remove In App Purchasing");
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(RemoveInAppsBtnContent, removeButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for removing mediation here
            RemoveInApps();
            ShowStatusMessage("Removed");
        }

        GUILayout.EndHorizontal();

        DrawSeparator();

//-----------------------------------------------------------------------------------------------------------------------
        DrawSection("AppTracking");

        GUILayout.BeginHorizontal();

        GUIContent AppTrackingBtnContent = new GUIContent("Add AppTracking", "Add AppTracking Transparency");
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(AppTrackingBtnContent, addButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for adding mediation here

            ShowStatusMessage("Added");
        }

        GUIContent RemoveAppTrackingBtnContent = new GUIContent("Remove AppTracking", "Remove AppTracking Transparency");
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(RemoveAppTrackingBtnContent, removeButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for removing mediation here

            ShowStatusMessage("Removed");
        }

        GUILayout.EndHorizontal();
        DrawSeparator();
        //-----------------------------------------------------------------------------------------------------------------------

        DrawSection("SolarEngine");

        GUILayout.BeginHorizontal();

        GUIContent SolarEngineBtnContent = new GUIContent("Add SolarEngine", "Add SolarEngine Transparency");
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(SolarEngineBtnContent, addButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for adding mediation here

            ShowStatusMessage("Added");
        }

        GUIContent RemoveSolarEngineBtnContent = new GUIContent("Remove SolarEngine", "Remove SolarEngine Transparency");
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(RemoveSolarEngineBtnContent, removeButtonStyle, GUILayout.Height(30)))
        {
            // Implement your logic for removing mediation here

            ShowStatusMessage("Removed");
        }

        GUILayout.EndHorizontal();

        //-----------------------------------------------------------------------------------------------------------------------
        GUILayout.EndVertical();
        GUILayout.Space(5);


        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace(); // Push everything to the bottom



        // Status Message
        if (!string.IsNullOrEmpty(statusMessage))
        {
            GUILayout.Space(5);
            GUIStyle statusStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField(statusMessage, statusStyle);
            GUILayout.Space(5);
        }


        GUIContent websiteLinkContent = new GUIContent("GameBus_GameTruck");
        GUIStyle websiteLinkStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white },
            hover = { textColor = Color.cyan },
        };

        Vector2 linkSize = websiteLinkStyle.CalcSize(websiteLinkContent);

        Rect linkRect = new Rect(position.width - linkSize.x - 10, position.height - linkSize.y - 10, linkSize.x, linkSize.y);

        if (GUI.Button(linkRect, websiteLinkContent, websiteLinkStyle))
        {
            Application.OpenURL("#");
        }
    }
    private void DrawSection(string sectionTitle)
    {
        GUIStyle menuHeaderStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 15,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft
        };

        GUILayout.Label(sectionTitle, menuHeaderStyle);


        GUILayout.Space(5);

    }


    private void DrawSeparator()
    {
        GUILayout.Space(5);
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
        GUILayout.Space(5);
    }

    private void UpdateStatusMessage()
    {
        if (!string.IsNullOrEmpty(statusMessage) && EditorApplication.timeSinceStartup >= statusMessageClearTime)
        {
            ClearStatusMessage();
        }
    }

    private void ShowStatusMessage(string message, double displayTime = 3)
    {
        statusMessage = message;
        statusMessageClearTime = EditorApplication.timeSinceStartup + displayTime;
        Repaint(); // Forces the window to update and display the message
    }

    private void ClearStatusMessage()
    {
        statusMessage = "";
        Repaint();
    }





    #region HelperFunctions

    public static void AddMediation()
    {
        if (Directory.Exists(Application.dataPath + "/MaxSdk") ||
            Directory.Exists(Application.dataPath + "/GoogleMobileAds") ||
            Directory.Exists(Application.dataPath + "/IronSource"))
        {
            bool Output = EditorUtility.DisplayDialog("GameBus Studio", "Heads up! 9 Fox Mediation is already integrated into the project. Are you absolutely sure about removing and reimporting it?", "Yes", "No");
            if (Output)
            {
                ImportPackage();

            }
            return;

        }



        ImportPackage();

    }
    static void ImportPackage()
    {
        RemoveMediation();
        AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/Mediation.unitypackage", true);

    }
    public static void AddFirebase()
    {

        if (Directory.Exists(Application.dataPath + "/Firebase"))
        {
            bool Output = EditorUtility.DisplayDialog("GameBus Studio", "Heads up! Firebase is already integrated into the project. Are you absolutely sure about removing and reimporting it?", "Yes", "No");
            if (Output)
            {

                AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/Firebase.unitypackage", false);

            }
            return;
        }
        else
        {

            AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/Firebase.unitypackage", false);
        }

    }

    public static void RemoveFirebase(bool value = false)
    {
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Editor Default Resources");
        FileUtil.DeleteFileOrDirectory("Assets/Editor Default Resources.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Firebase");
        FileUtil.DeleteFileOrDirectory("Assets/Firebase.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/FirebaseApp.androidlib");
        FileUtil.DeleteFileOrDirectory("Assets//Plugins/Android/FirebaseApp.androidlib.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/FirebaseCrashlytics.androidlib");
        FileUtil.DeleteFileOrDirectory("Assets//Plugins/Android/FirebaseCrashlytics.androidlib.meta");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/Android/MessagingUnityPlayerActivity.java");
        FileUtil.DeleteFileOrDirectory("Assets//Plugins/Android/MessagingUnityPlayerActivity.java.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/iOS/Firebase");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/iOS/Firebase.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/tvOS");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/tvOS.meta");
        if (value)
        {
            AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/RemoveFirebase.unitypackage", false);
        }
        EditorUtility.DisplayDialog("GameBus Studio", "Firebase successfully removed", "Ok");
    }






    public static void RemoveMediation(bool showpop = false)
    {


        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/_GB_GTAds");
        FileUtil.DeleteFileOrDirectory("Assets/_GB_GTAds.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Editor Default Resources");
        FileUtil.DeleteFileOrDirectory("Assets/Editor Default Resources.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/ExternalDependencyManager");
        FileUtil.DeleteFileOrDirectory("Assets/ExternalDependencyManager.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/GoogleMobileAds");
        FileUtil.DeleteFileOrDirectory("Assets/GoogleMobileAds.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/GoogleMobileAdsNative");
        FileUtil.DeleteFileOrDirectory("Assets/GoogleMobileAdsNative.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/IronSource");
        FileUtil.DeleteFileOrDirectory("Assets/IronSource.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/MaxSdk");
        FileUtil.DeleteFileOrDirectory("Assets/MaxSdk.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/Android.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/iOS");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/iOS.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/IOSGoodies");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/IOSGoodies.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Native");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/Native.meta");
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/UnityAds");
        FileUtil.DeleteFileOrDirectory("Assets/Plugins/UnityAds.meta");

        FileUtil.DeleteFileOrDirectory("Assets/_GB_GTMediation/Scripts/AdsManager.cs");
        FileUtil.DeleteFileOrDirectory("Assets/_GB_GTMediation/Scripts/AdsManager.cs.meta");
        FileUtil.DeleteFileOrDirectory("Assets/_GB_GTMediation/Scripts/LabAnalytics.cs");
        FileUtil.DeleteFileOrDirectory("Assets/_GB_GTMediation/Scripts/LabAnalytics.cs.meta");
        if (IsPackageInstalled("com.unity.mobile.notifications"))
        {
            RemoveRequest = Client.Remove("com.unity.mobile.notifications");
            EditorApplication.update += RemoveProgress;
        }
        if (showpop)
        {
            EditorUtility.DisplayDialog("Alert", "Ads Removed Succesfully", "OK");

            if (Directory.Exists(Application.dataPath + "/Firebase"))
            {
                bool Output = EditorUtility.DisplayDialog("GameBus Studio", "Would you like to remove Firebase too", "Yes", "No");
                if (Output)
                {
                    RemoveFirebase();
                }

            }
            if (Directory.Exists(Application.dataPath + "/_GB_GTInAppPurchasing"))
            {
                bool Output = EditorUtility.DisplayDialog("GameBus Studio", "Would you like to uninstall InApps too", "Yes", "No");
                if (Output)
                {
                    RemoveInApps();
                }

            }
            AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/RemoveAds.unitypackage", false);

        }


        AssetDatabase.Refresh();
        EditorSceneManager.SaveOpenScenes();
    }

    
    public static void AddInApps()
    {


            if (!IsPackageInstalled("com.unity.purchasing"))
            {
                Request = Client.Add("com.unity.purchasing");
                EditorUtility.DisplayProgressBar("GameBus Studio", "Importing InApps", 0.7f);

                EditorApplication.update += Progress;
            }
            else
            {
                if (Directory.Exists(Application.dataPath + "/_GB_GTInAppPurchasing"))
                {
                    bool Output = EditorUtility.DisplayDialog("GameBus Studio", "Heads up! InApps are already integrated into the project. Are you absolutely sure about removing and reimporting it?", "Yes", "No");
                    if (Output)
                    {
                        AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/InApps.unitypackage", false);
                    }
                    return;
                }
                else
                {
                    AssetDatabase.ImportPackage(Application.dataPath + "/_GB_GTMediation/Packages/InApps.unitypackage", false);
                }
            }
    }

    public static void RemoveInApps()
    {
        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/_GB_GTInAppPurchasing");
        FileUtil.DeleteFileOrDirectory("Assets/_GB_GTInAppPurchasing.meta");
        if (IsPackageInstalled("com.unity.purchasing"))
        {
            RemoveRequest = Client.Remove("com.unity.purchasing");
            EditorApplication.update += RemoveProgress;
        }
        EditorUtility.DisplayDialog("GameBus Studio", "InApps successfully removed", "Ok");
    }

    static void Progress()
    {






        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                AssetDatabase.ImportPackage(Application.dataPath + "/DOMediation/Packages/DevOpsIAP.unitypackage", false);
                EditorSceneManager.SaveOpenScenes();
                EditorUtility.ClearProgressBar();
            }
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);

            EditorApplication.update -= Progress;

        }

    }
    static void RemoveProgress()
    {

        if (RemoveRequest.IsCompleted)
        {
            EditorApplication.update -= RemoveProgress;
        }
    }
    public static bool IsPackageInstalled(string packageId)
    {
        if (!File.Exists("Packages/manifest.json"))
            return false;

        string jsonText = File.ReadAllText("Packages/manifest.json");
        return jsonText.Contains(packageId);
    }
    #endregion

}
public class PackageExporter
{
    [MenuItem("Window/GB_GT/Export")]
    static void ExportSelectedFolders()
    {
        string[] folderPaths = new string[]
        {
            "Assets/_GB_GTAds/AppTracking",
            "Assets/_GB_GTAds/CrossPromotion",
            "Assets/_GB_GTAds/Editor",
            "Assets/_GB_GTAds/GDPR",
            "Assets/_GB_GTAds/NativePopups",
            "Assets/_GB_GTAds/Prefab",
            "Assets/_GB_GTAds/Resources",
            "Assets/_GB_GTAds/Scenes",
            "Assets/_GB_GTAds/Scripts",
            "Assets/ExternalDependencyManager",
            "Assets/GoogleMobileAds",
            "Assets/IronSource",
            "Assets/Keystore",
            "Assets/MaxSdk",
            "Assets/Plugins/Android",
            "Assets/Plugins/IOS",
            "Assets/Plugins/IOSGoodies",
            "Assets/Plugins/Native",
            "Assets/Resources",
            "Assets/StreamingAssets",
            // Add more folder paths here as needed
          
        };

        string exportPath = Application.dataPath + "/_GB_GTMediation/Packages/Mediation.unitypackage";

        ExportPackageOptions options = ExportPackageOptions.Recurse;
        
        AssetDatabase.ExportPackage(folderPaths, exportPath, options);
        Debug.Log(exportPath);
    }
}
#endif