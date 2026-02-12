#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using GoogleMobileAds.Editor;
using UnityEngine;
using System;
using UnityEngine.Networking;
using XcodeUnityCapability = UnityEditor.iOS.Xcode.ProjectCapabilityManager;
public class PostBuildStep
{
    /// <summary>
    /// Description for IDFA request notification 
    /// [sets NSUserTrackingUsageDescription]
    /// </summary>
    /// 

    public class SkAdNetworkData
    {
        [SerializeField] public string[] SkAdNetworkIds;
    }

    private const string TargetUnityIphonePodfileLine = "target 'Unity-iPhone' do";
    [PostProcessBuild(0)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToXcode)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            GoogleMobileAdsSettings settings = GoogleMobileAdsSettings.LoadInstance();
            UpdateIOSPlist(pathToXcode, settings);
            UpdateIOSProject(pathToXcode);
        }
    }

    [PostProcessBuild]
    private static void PostBuildActions(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            XcodeUnityCapability projCapability = new XcodeUnityCapability(projPath, "Unity-iPhone/mmk.entitlements", "Unity-iPhone");
            projCapability.AddPushNotifications(true);
            projCapability.WriteToFile();
        }
    }
    private static SkAdNetworkData GetSkAdNetworkData()
    {
        var uriBuilder = new UriBuilder("https://dash.applovin.com/docs/v1/unity_integration_manager/sk_ad_networks_info");
        uriBuilder.Query += "adnetworks=AdColony,ByteDance,Facebook,Fyber,Google,GoogleAdManager,InMobi,IronSource,Mintegral,MyTarget,Tapjoy,TencentGDT,UnityAds,Vungle,Yandex";
        var unityWebRequest = UnityWebRequest.Get(uriBuilder.ToString());

#if UNITY_2017_2_OR_NEWER
        var operation = unityWebRequest.SendWebRequest();
#else
            var operation = unityWebRequest.Send();
#endif
        // Wait for the download to complete or the request to timeout.
        while (!operation.isDone) { }


#if UNITY_2020_1_OR_NEWER
        if (unityWebRequest.result != UnityWebRequest.Result.Success)
#elif UNITY_2017_2_OR_NEWER
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
#else
            if (unityWebRequest.isError)
#endif
        {
            Debug.LogError("Failed to retrieve SKAdNetwork IDs with error: " + unityWebRequest.error);
            return new SkAdNetworkData();
        }
        try
        {
            return JsonUtility.FromJson<SkAdNetworkData>(unityWebRequest.downloadHandler.text);
        }
        catch (Exception exception)
        {
            Debug.LogError("Failed to parse data '" + unityWebRequest.downloadHandler.text + "' with exception: " + exception);
            return new SkAdNetworkData();
        }
    }
    static void UpdateIOSPlist(string path, GoogleMobileAdsSettings settings)
    {
        string[] mSKAdNetworkId = new string[] {
                "275upjj5gd.skadnetwork",
                "294l99pt4k.skadnetwork",
                "2u9pt9hc89.skadnetwork",
                "3rd42ekr43.skadnetwork",
                "4468km3ulz.skadnetwork",
                "44jx6755aq.skadnetwork",
                "4fzdc2evr5.skadnetwork",
                "4pfyvq9l8r.skadnetwork",
                "5lm9lj6jb7.skadnetwork",
                "6g9af3uyq4.skadnetwork",
                "7rz58n8ntl.skadnetwork",
                "7ug5zh24hu.skadnetwork",
                "8s468mfl3y.skadnetwork",
                "9nlqeag3gk.skadnetwork",
                "9rd848q2bz.skadnetwork",
                "9t245vhmpl.skadnetwork",
                "c6k4g5qg8m.skadnetwork",
                "cg4yq2srnc.skadnetwork",
                "ejvt5qm6ak.skadnetwork",
                "g28c52eehv.skadnetwork",
                "hs6bdukanm.skadnetwork",
                "kbmxgpxpgc.skadnetwork",
                "klf5c3l5u5.skadnetwork",
                "m8dbw4sv7c.skadnetwork",
                "mlmmfzh3r3.skadnetwork",
                "mtkv5xtk9e.skadnetwork",
                "ppxm28t8ap.skadnetwork",
                "prcb7njmu6.skadnetwork",
                "qqp299437r.skadnetwork",
                "rx5hdcabgc.skadnetwork",
                "t38b2kh725.skadnetwork",
                "tl55sbb4fm.skadnetwork",
                "u679fj5vs4.skadnetwork",
                "uw77j35x4d.skadnetwork",
                "v72qych5uu.skadnetwork",
                "wg4vff78zm.skadnetwork",
                "yclnxrl5pm.skadnetwork",
                "2fnua5tdw4.skadnetwork",
                "3qcr597p9d.skadnetwork",
                "3qy4746246.skadnetwork",
                "3sh42y64q3.skadnetwork",
                "424m5254lk.skadnetwork",
                "4dzt52r2t5.skadnetwork",
                "578prtvx9j.skadnetwork",
                "5a6flpkh64.skadnetwork",
                "8c4e2ghe7u.skadnetwork",
                "av6w8kgt66.skadnetwork",
                "cstr6suwn9.skadnetwork",
                "e5fvkxwrpn.skadnetwork",
                "f38h382jlk.skadnetwork",
                "kbd757ywx3.skadnetwork",
                "n6fk4nfna4.skadnetwork",
                "p78axxw29g.skadnetwork",
                "s39g8k73mm.skadnetwork",
                "v4nxqhlyqp.skadnetwork",
                "wzmmz9fp6w.skadnetwork",
                "ydx93a7ass.skadnetwork",
                "zq492l623r.skadnetwork",
                "24t9a8vw3c.skadnetwork",
                "32z4fx6l9h.skadnetwork",
                "523jb4fst2.skadnetwork",
                "54nzkqm89y.skadnetwork",
                "5l3tpt7t6e.skadnetwork",
                "6xzpu9s2p8.skadnetwork",
                "79pbpufp6p.skadnetwork",
                "9b89h5y424.skadnetwork",
                "cj5566h2ga.skadnetwork",
                "feyaarzu9v.skadnetwork",
                "ggvn48r87g.skadnetwork",
                "glqzh8vgby.skadnetwork",
                "gta9lk7p23.skadnetwork",
                "k674qkevps.skadnetwork",
                "ludvb6z3bs.skadnetwork",
                "n9x2a789qt.skadnetwork",
                "pwa73g5rt2.skadnetwork",
                "r45fhb6rf7.skadnetwork",
                "rvh3l7un93.skadnetwork",
                "x8jxxk4ff5.skadnetwork",
                "xy9t38ct57.skadnetwork",
                "zmvfpc5aq8.skadnetwork",
                "n38lu8286q.skadnetwork",
                "v9wttpbfk9.skadnetwork",
                "22mmun2rn5.skadnetwork",
                "252b5q8x7y.skadnetwork",
                "9g2aggbj52.skadnetwork",
                "dzg6xy7pwj.skadnetwork",
                "f73kdq92p3.skadnetwork",
                "hdw39hrw9y.skadnetwork",
                "x8uqf25wch.skadnetwork",
                "y45688jllp.skadnetwork",
                "74b6s63p6l.skadnetwork",
                "97r2b46745.skadnetwork",
                "b9bk5wbcq9.skadnetwork",
                "mls7yz5dvl.skadnetwork",
                "w9q455wk68.skadnetwork",
                "su67r6k2v3.skadnetwork",
                "r26jy69rpl.skadnetwork",
                "238da6jt44.skadnetwork",
                "44n7hlldy6.skadnetwork",
                "488r3q3dtq.skadnetwork",
                "52fl2v3hgk.skadnetwork",
                "5tjdwbrq8w.skadnetwork",
                "737z793b9f.skadnetwork",
                "9yg77x724h.skadnetwork",
                "ecpz2srf59.skadnetwork",
                "gvmwg8q7h5.skadnetwork",
                "lr83yxwka7.skadnetwork",
                "n66cz3y3bx.skadnetwork",
                "nzq8sh4pbs.skadnetwork",
                "pu4na253f3.skadnetwork",
                "v79kvwwj4g.skadnetwork",
                "yrqqpx2mcb.skadnetwork",
                "z4gj7hsk7h.skadnetwork",
                "f7s53z58qe.skadnetwork",
                "mp6xlyr22a.skadnetwork",
                "x44k69ngh6.skadnetwork",
                "7953jerfzd.skadnetwork",
            };
        string plistPath = Path.Combine(path, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));

        //Get Root
        PlistElementDict rootDict = plist.root;
        PlistElementDict transportSecurity = rootDict.CreateDict("NSAppTransportSecurity");
        transportSecurity.SetBoolean("NSAllowsArbitraryLoads", true);

        //Set SKAdNetwork
        PlistElementArray skItem = rootDict.CreateArray("SKAdNetworkItems");
        var skAdNetworkData = GetSkAdNetworkData();
        var skAdNetworkIds = skAdNetworkData.SkAdNetworkIds;
        if (skAdNetworkIds == null || skAdNetworkIds.Length < 1)
        {
            skAdNetworkIds = mSKAdNetworkId;
        }
        foreach (string skAdNetworkId in skAdNetworkIds)
        {
            PlistElementDict skDic = skItem.AddDict();
            skDic.SetString("SKAdNetworkIdentifier", skAdNetworkId);
        }
       
       
        //Add Google AdMob App ID
        rootDict.SetString("GADApplicationIdentifier", settings.GoogleMobileAdsIOSAppId);
            //Enable Google Ad Manager
            rootDict.SetBoolean("GADIsAdManagerApp", true);
        

        string version = Application.unityVersion;
        if (!string.IsNullOrEmpty(version))
        {
            rootDict.SetString("engineType", "Unity");
            rootDict.SetString("engineVersion", version);
        }

        PlistElementString locationAlwaysUsagePrivacy = (PlistElementString)rootDict["NSLocationAlwaysUsageDescription"];
        if (locationAlwaysUsagePrivacy == null)
        {
            rootDict.SetString("NSLocationAlwaysUsageDescription", "Some ad content may require access to the location for an interactive ad experience.");
        }

        PlistElementString locationWhenInUseUsagePrivacy = (PlistElementString)rootDict["NSLocationWhenInUseUsageDescription"];
        if (locationWhenInUseUsagePrivacy == null)
        {
            rootDict.SetString("NSLocationWhenInUseUsageDescription", "Some ad content may require access to the location for an interactive ad experience.");
        }

        PlistElementString locationAlwaysAndWhenInUseUsagePrivacy = (PlistElementString)rootDict["NSLocationAlwaysAndWhenInUseUsageDescription"];
        if (locationAlwaysAndWhenInUseUsagePrivacy == null)
        {
            rootDict.SetString("NSLocationAlwaysAndWhenInUseUsageDescription", "Some ad content may require access to the location for an interactive ad experience.");
        }

        PlistElementString attPrivacy = (PlistElementString)rootDict["NSUserTrackingUsageDescription"];
        if (attPrivacy == null)
        {
            rootDict.SetString("NSUserTrackingUsageDescription", "This identifier will be used to deliver personalized ads to you.");
        }

        PlistElementString bluetoothPrivacy = (PlistElementString)rootDict["NSBluetoothAlwaysUsageDescription"];
        if (bluetoothPrivacy == null)
        {
            rootDict.SetString("NSBluetoothAlwaysUsageDescription", "Some ad content may require access to the location for an interactive ad experience.");
        }

        File.WriteAllText(plistPath, plist.WriteToString());
    }
    private static void UpdateIOSProject(string buildPath)
    {
        UnityEditor.iOS.Xcode.PBXProject proj = new UnityEditor.iOS.Xcode.PBXProject();
        string projPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(buildPath);
        proj.ReadFromFile(projPath);

        string unityMainTargetGuid = string.Empty;
        string unityFrameworkTargetGuid = string.Empty;

#if UNITY_2019_3_OR_NEWER
        unityMainTargetGuid = proj.GetUnityMainTargetGuid();
        unityFrameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();
#else
            unityMainTargetGuid = proj.TargetGuidByName("Unity-iPhone");
            unityFrameworkTargetGuid = unityMainTargetGuid;
#endif
        proj.AddBuildProperty(unityMainTargetGuid, "OTHER_LDFLAGS", "-ObjC");
        proj.AddBuildProperty(unityMainTargetGuid, "OTHER_LDFLAGS", "-lxml2");
        proj.SetBuildProperty(unityMainTargetGuid, "ENABLE_BITCODE", "NO");
        proj.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
        proj.SetBuildProperty(unityFrameworkTargetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

        /// <summary>
        /// For Swift 5+ code that uses the standard libraries, the Swift Standard Libraries MUST be embedded for iOS < 12.2
        /// Swift 5 introduced ABI stability, which allowed iOS to start bundling the standard libraries in the OS starting with iOS 12.2
        /// Issue Reference: https://github.com/facebook/facebook-sdk-for-unity/issues/506
        /// </summary>
        proj.SetBuildProperty(unityMainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
        proj.AddBuildProperty(unityFrameworkTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "/usr/lib/swift");


        // rewrite to file
        File.WriteAllText(projPath, proj.WriteToString());


    }

}
#endif
