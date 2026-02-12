#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using System.IO;

class MyCustomBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {


        SessionState.SetBool("SelfDeclaredAndroidDependenciesDisabled:com.unity.purchasing", true);


        if (Directory.Exists("Assets/Firebase"))
        {
#if UNITY_IOS
      
          if(!File.Exists("Assets/GoogleService-Info.plist"))
            {
                EditorUtility.DisplayDialog("Oops", "It looks like something's missing! You forgot to add the Firebase File (.infoplist) to the Mani Assets Folder", "Ok");
                throw new BuildFailedException("Kindly include the Firebase File (.json) in the Assets Folder");
            }
#endif

#if UNITY_ANDROID

            if (!File.Exists("Assets/google-services.json"))
            {
                EditorUtility.DisplayDialog("Oops", "It looks like something's missing! You forgot to add the Firebase File (.json) to the Main Assets Folder", "Ok");
                throw new BuildFailedException("Kindly include the Firebase File (.json) in the Assets Folder");
            }

#endif

            return;
        }


    }

}


#endif

