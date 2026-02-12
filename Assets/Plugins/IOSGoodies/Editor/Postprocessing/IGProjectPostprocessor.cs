using UnityEngine;

namespace DeadMosquito.IosGoodies.Editor
{
	using UnityEditor;
	using UnityEditor.Callbacks;
	using UnityEditor.iOS.Xcode;

	public static class IGProjectPostprocessor
	{
		[PostProcessBuild(2000)]
		public static void OnPostProcessBuild(BuildTarget target, string path)
		{
			if (target == BuildTarget.iOS)
			{
				IGPostprocessUtils.ModifyPlist(path, AddFeatureUsageDescriptions);
			}
		}

		static void AddFeatureUsageDescriptions(PlistDocument plist)
		{
			Debug.Log(typeof(IGProjectPostprocessor).FullName + " is postprocessing the iOS project Info.plist to add feature descriptions. Please disable the parts for features you don't use in your project");

			SetFeatureUsageDescription(plist, "NSCameraUsageDescription");
			SetFeatureUsageDescription(plist, "NSPhotoLibraryUsageDescription");
			SetFeatureUsageDescription(plist, "NSPhotoLibraryAddUsageDescription");
			SetFeatureUsageDescription(plist, "NSFaceIDUsageDescription");
		}

		static void SetFeatureUsageDescription(PlistDocument plist, string usageDescriptionKey)
		{
			if (!plist.HasRootElement(usageDescriptionKey))
			{
				plist.root.AsDict().SetString(usageDescriptionKey, "Plist entry Added by IGProjectPostprocessor.cs script");
			}
		}
	}
}