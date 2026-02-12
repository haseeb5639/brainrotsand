using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;
using XcodeUnityCapability = UnityEditor.iOS.Xcode.ProjectCapabilityManager;

public class EntitlementsPostProcess : ScriptableObject
{
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
}
