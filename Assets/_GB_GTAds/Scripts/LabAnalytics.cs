using Firebase.Analytics;
using Firebase.Crashlytics;
using System.Text.RegularExpressions;
using UnityEngine;

public class LabAnalytics : MonoBehaviour
{
    #region Singleton
    private static LabAnalytics _Instance;
    public static LabAnalytics Instance
    {
        get
        {
            if (!_Instance) _Instance = FindObjectOfType<LabAnalytics>();

            return _Instance;
        }
    }

    void Awake()
    {
        if (!_Instance) _Instance = this;
    }

    #endregion


    public void Start()
    {

        PlayerPrefs.SetInt("Session", (PlayerPrefs.GetInt("Session") + 1));
        LogEvent("Session " + PlayerPrefs.GetInt("Session"));
        InitCrashlytics();

    }
    public void LogEvent(string str)
    {
        string filterstr = Filter(str);

        FirebaseAnalytics.LogEvent(filterstr);
        Debug.Log(filterstr);
    }
    public void CrashlyticsEvent(string str)
    {
        string filterstr = Filter(str);

        Crashlytics.Log(str);
        Debug.Log(filterstr);
    }
    private string Filter(string str)
    {
        str = str.TrimStart('.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ');

        str = str.Replace(" ", "_");
        str = Regex.Replace(str, "[^a-zA-Z0-9_]+", "_");
        return str;
    }
    public void InitCrashlytics()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Crashlytics will use the DefaultInstance, as well;
                // this ensures that Crashlytics is initialized.
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
    }

    

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

}
