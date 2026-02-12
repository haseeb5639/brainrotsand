namespace GleyPushNotifications
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class SettingsWindow : EditorWindow
    {
        private Vector2 scrollPosition = Vector2.zero;
        NotificationSettings notificationSettings;

        string info = "This asset requires Mobile Notifications by Unity \n" +
            "Go to Window -> Package Manager and install Mobile Notifications";
        private bool useForAndroid;
        private bool useForIos;
        private string additionalSettings = "To setup notification images open:\n" +
            "Edit -> Project Settings -> Mobile Notifications";

        [MenuItem("Window/GB_GT/Notifications")]
        private static void Init()
        {
           

            SettingsWindow window = (SettingsWindow)GetWindow(typeof(SettingsWindow), true, "Mobile Push Notifications Settings Window" );
            window.minSize = new Vector2(520, 520);
            window.Show();
            SettingsService.OpenProjectSettings("Project/Mobile Notifications");
            window.SaveSettings();
        }

        private void OnEnable()
        {
            notificationSettings = Resources.Load<NotificationSettings>("NotificationSettingsData");
           // notificationSettings = AssetDatabase.LoadAssetAtPath<NotificationSettings>("Assets/DOAds/Notifications/Resources/NotificationSettingsData");
            if (notificationSettings == null)
            {
                CreateNotificationSettings();
                 notificationSettings = Resources.Load<NotificationSettings>("NotificationSettingsData");
                //notificationSettings = AssetDatabase.LoadAssetAtPath<NotificationSettings>("Assets/DOAds/Notifications/Resources/NotificationSettingsData");
            }


            //    useForAndroid = notificationSettings.useForAndroid;
            // useForIos = notificationSettings.useForIos;
            useForAndroid = true;
            useForIos = true;
        }

        private void SaveSettings()
        {
            SetPreprocessorDirectives();
            if (useForAndroid)
            {
                AddPreprocessorDirective("EnableNotificationsAndroid", false, BuildTargetGroup.Android);
            }
            else
            {
                AddPreprocessorDirective("EnableNotificationsAndroid", true, BuildTargetGroup.Android);
            }
            if (useForIos)
            {
                AddPreprocessorDirective("EnableNotificationsIos", false, BuildTargetGroup.iOS);
            }
            else
            {
                AddPreprocessorDirective("EnableNotificationsIos", true, BuildTargetGroup.iOS);
            }
            
               
            

            notificationSettings.useForAndroid = useForAndroid;
            notificationSettings.useForIos = useForIos;
            

            EditorUtility.SetDirty(notificationSettings);
        }

        private void OnGUI()
        {
            EditorStyles.label.wordWrap = true;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));

            EditorGUILayout.Space();

            GUILayout.Label("Select your platforms:", EditorStyles.boldLabel);
            useForAndroid = EditorGUILayout.Toggle("Android", useForAndroid);
            useForIos = EditorGUILayout.Toggle("iOS", useForIos);
            EditorGUILayout.Space();


            EditorGUILayout.LabelField(info);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(additionalSettings);
#if UNITY_2018_3_OR_NEWER
            if (GUILayout.Button("Open Mobile Notification Settings"))
            {
                SettingsService.OpenProjectSettings("Project/Mobile Notifications");
            }
#endif
            EditorGUILayout.Space();
            if (GUILayout.Button("Save"))
            {
                SaveSettings();
            }


            GUILayout.EndScrollView();
        }

        private void SetPreprocessorDirectives()
        {
            //if (usePlaymaker)
            //{
            //    AddPreprocessorDirective(Constants.USE_PLAYMAKER_SUPPORT, false, BuildTargetGroup.Android);
            //    AddPreprocessorDirective(Constants.USE_PLAYMAKER_SUPPORT, false, BuildTargetGroup.iOS);
            //}
            //else
            //{
            //    AddPreprocessorDirective(Constants.USE_PLAYMAKER_SUPPORT, true, BuildTargetGroup.Android);
            //    AddPreprocessorDirective(Constants.USE_PLAYMAKER_SUPPORT, true, BuildTargetGroup.iOS);
            //}

            //if (useBolt)
            //{
            //    AddPreprocessorDirective(Constants.USE_BOLT_SUPPORT, false, BuildTargetGroup.Android);
            //    AddPreprocessorDirective(Constants.USE_BOLT_SUPPORT, false, BuildTargetGroup.iOS);
            //}
            //else
            //{
            //    AddPreprocessorDirective(Constants.USE_BOLT_SUPPORT, true, BuildTargetGroup.Android);
            //    AddPreprocessorDirective(Constants.USE_BOLT_SUPPORT, true, BuildTargetGroup.iOS);
            //}

            //if (useGameflow)
            //{
            //    AddPreprocessorDirective(Constants.USE_GAMEFLOW_SUPPORT, false, BuildTargetGroup.Android);
            //    AddPreprocessorDirective(Constants.USE_GAMEFLOW_SUPPORT, false, BuildTargetGroup.iOS);
            //}
            //else
            //{
            //    AddPreprocessorDirective(Constants.USE_GAMEFLOW_SUPPORT, true, BuildTargetGroup.Android);
            //    AddPreprocessorDirective(Constants.USE_GAMEFLOW_SUPPORT, true, BuildTargetGroup.iOS);
            //}
        }

        private void CreateNotificationSettings()
        {
            NotificationSettings asset = CreateInstance<NotificationSettings>();
            if (!AssetDatabase.IsValidFolder("Assets/DOAds/Notifications/Resources"))
            {
                AssetDatabase.CreateFolder("Assets/DOAds/Notifications", "Resources");
                AssetDatabase.Refresh();
            }

            AssetDatabase.CreateAsset(asset, "Assets/DOAds/Notifications/Resources/NotificationSettingsData.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void AddPreprocessorDirective(string directive, bool remove, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

            if (remove)
            {
                if (textToWrite.Contains(directive))
                {
                    Debug.Log(textToWrite);
                    textToWrite = textToWrite.Replace(directive, "");
                }
            }
            else
            {
                if (!textToWrite.Contains(directive))
                {
                    if (textToWrite == "")
                    {
                        textToWrite += directive;
                    }
                    else
                    {
                        textToWrite += "," + directive;
                    }
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
        }
    }
}