using System.Collections.Generic;
using System.IO;
using UnityEditor;
#if UNITY_ANDROID
using UnityEditor.Android;
#endif
using UnityEditor.Build;
#if UNITY_IOS
using UnityEditor.iOS;
#endif
using UnityEngine;



public class SetIcon : Editor
{
  static  Texture2D newTex;
    [MenuItem("Window/GB_GT/SetIcon")]
    public static void SetPlatformIcon()
    {
        newTex = Resources.Load<Texture2D>("Icon");
        //newTex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Keystore/Icon.png", typeof(Texture2D)); // generate your texture, etc.
        //if (newTex==null)
        //{
        //    newTex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Keystore/Icon.jpg", typeof(Texture2D)); // generate your texture, etc.
        //}
        if (newTex==null)
        {
            Debug.LogError("Place the Icon in Assets/Resources");
            EditorUtility.DisplayDialog("No Icon Found", "Place the Icon in Assets/Resources", "Ok");
            return;
        }
        Generate();
    }
    private class MyPlatformIcon
    {
        public MyPlatformIcon(PlatformIcon platformIcon, BuildTargetGroup group)
        {
            this.platformIcon = platformIcon;
            this.group = group;
        }

        public PlatformIcon platformIcon;
        public BuildTargetGroup group;

        public PlatformIconKind kind => platformIcon.kind;

        public void SetTexture(Texture2D tex) => platformIcon.SetTexture(tex);
    }



    public static void Generate()
    {

        var icons = new List<MyPlatformIcon>();

        void AddIcons(BuildTargetGroup group, PlatformIconKind kind)
        {
            var oldIcons = PlayerSettings.GetPlatformIcons(group, kind);
            foreach (var icon in oldIcons)
            {
                icons.Add(new MyPlatformIcon(icon, group));
            }
        }

#if UNITY_IOS
        AddIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Application);
        AddIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Marketing);
        AddIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Settings);
        AddIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Notification);
        AddIcons(BuildTargetGroup.iOS, iOSPlatformIconKind.Spotlight);
#endif

// ...
#if UNITY_ANDROID
        AddIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Legacy);
        SetIconForAdaptive();
        //AddIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Adaptive);
        AddIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Round);

#endif
        // ...

        var iconsPerKinds = new Dictionary<PlatformIconKind, List<MyPlatformIcon>>();
        foreach (var icon in icons)
        {
            List<MyPlatformIcon> iconsWithKind;
            if (!iconsPerKinds.TryGetValue(icon.kind, out iconsWithKind))
            {
                iconsWithKind = new List<MyPlatformIcon>();
                iconsPerKinds.Add(icon.kind, iconsWithKind);
            }
            iconsWithKind.Add(icon);
#if (UNITY_2018_4_21)
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { newTex }, IconKind.Any);
#else
            //PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new Texture2D[] { newTex }, IconKind.Any);
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { newTex }, IconKind.Any);
#endif

            icon.SetTexture(newTex);


        }

        foreach (var kvp in iconsPerKinds)
        {
            var iconsForThisKind = kvp.Value;
            if (kvp.Value.Count == 0) continue;
            PlayerSettings.SetPlatformIcons(iconsForThisKind[0].group, kvp.Key, iconsForThisKind.ConvertAll((i => i.platformIcon)).ToArray());
        }
    }

   static void SetIconForAdaptive()
    {
       
        Texture2D[] Tex = new Texture2D[] { newTex, newTex };
        SetIcons(Tex); 
    }
   static void SetIcons(Texture2D[] Tex)
    {
#if UNITY_ANDROID
        var platform = BuildTargetGroup.Android;
        var kind = UnityEditor.Android.AndroidPlatformIconKind.Adaptive;

        var icons = PlayerSettings.GetPlatformIcons(platform, kind);

        //Assign textures to each available icon slot.
        for (var i = 0; i < icons.Length; i++)
        {
            icons[i].SetTextures(Tex);
        }
        PlayerSettings.SetPlatformIcons(platform, kind, icons);
#endif
    }
}
