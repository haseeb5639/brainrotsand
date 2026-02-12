using UnityEditor;
using UnityEngine;

public class AppNotificationEditor : Editor
{
    [MenuItem("Window/GB_GT/Get Notifications")]
    public static void LoadNotificationsFromFile()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Notifications");
        AppNotifications appNotifications = Resources.Load<AppNotifications>("Notifications");
        if (textAsset != null)
        {
            appNotifications.Notifications = textAsset.text.Split('\n'); // Split the text into an array of notifications
        }
        else
        {
            Debug.LogError("Notification file not found in Resources ");
        }
    }
}