using UnityEngine;
using System.Collections;
#if UNITY_IOS
using DeadMosquito.IosGoodies;
using DeadMosquito.IosGoodies.Example;
#endif

public enum MessageState
{
    OK,
    YES,
    NO,
    RATED,
    REMIND,
    DECLINED,
    CLOSED
}


public class PopupView : MonoBehaviour
{
    #region PUBLIC_VARIABLES
    public static bool internet = true;
    #endregion

    #region Singleton
    private static PopupView _Instance;
    public static PopupView Instance
    {
        get
        {

            if (!_Instance) _Instance = FindObjectOfType<PopupView>();

            return _Instance;
        }
    }

    private void Awake()
    {
        if (!_Instance) _Instance = this;



    }
    #endregion
    #region UNITY_DEFAULT_CALLBACKS

    void OnEnable()
    {
        // Register all Delegate event listener
        AndroidRateUsPopUp.onRateUSPopupComplete += OnRateUSPopupComplete;
        AndroidDialog.onDialogPopupComplete += OnDialogPopupComplete;
        AndroidMessage.onMessagePopupComplete += OnMessagePopupComplete;
    }

    void OnDisable()
    {
        // Deregister all Delegate event listener
        AndroidRateUsPopUp.onRateUSPopupComplete -= OnRateUSPopupComplete;
        AndroidDialog.onDialogPopupComplete -= OnDialogPopupComplete;
        AndroidMessage.onMessagePopupComplete -= OnMessagePopupComplete;
    }


    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            if (internet == true) {
#if UNITY_IOS
				IGDialogs.ShowOneBtnDialog("Oops! No Internet Connection Found!", "Please make sure you're connected to a network to play the game", "Retry", () => internet = true);
				internet = false;
#else
                OnMessagePopUp();
                internet = false;
#endif
            }
        }
    }


    #endregion

    #region DELEGATE_EVENT_LISTENER

    // Raise when click on any button of rate popup
    void OnRateUSPopupComplete(MessageState state)
    {
        switch (state)
        {
            case MessageState.RATED:
                Debug.Log("Rate Button pressed");
                break;
            case MessageState.DECLINED:
                Debug.Log("Declined Button pressed");
                break;
        }
    }

    // Raise when click on any button of Dialog popup
    void OnDialogPopupComplete(MessageState state)
    {
        switch (state)
        {
            case MessageState.YES:

#if UNITY_ANDROID
              //  StartCoroutine( AppUpdate.Instance.StartImmediateUpdate());
#endif

                break;
            case MessageState.NO:
              //  Debug.Log("No button pressed");
                break;
        }
    }

    // Raise when click on ok button of message popup
    void OnMessagePopupComplete(MessageState state)
    {
        Debug.Log("Ok button Clicked");
    }

#endregion

#region BUTTON_EVENT_LISTENER



    // Rate Button click event
    public void OnRatePopUp()
    {
        NativeRateUS ratePopUp = new NativeRateUS("Help Make Our Game More Fun!", "Can you give our game a rating? We want to know what you think." +
            " Your feedback helps us make it even more fun for you. Thanks for your help!",
            "Rate Now", "Later");

        ratePopUp.SetUrlString("https://play.google.com/store/apps/details?id=" + Application.identifier);
        ratePopUp.init();


    }


    // Message Button click event
    public void OnMessagePopUp()
    {
        NativeMessage msg = new NativeMessage("Oops! No Internet Connection Found!", "Please make sure you're connected to a network to play the game");
    }
    public void OnMessagePopUp(string title,string message)
    {
        NativeMessage msg = new NativeMessage(title,message,"Ok");
    }

    // Update Button
    public void OnUpdatePopUp()
    {
        NativeDialog updatePopUp = new NativeDialog("New Update Available", "We've made some exciting improvements to our app and would love" +
            " for you to experience them! By updating to the latest version, you'll enjoy new features, bug fixes, and overall enhanced performance." +
            "So, don't wait any longer, Click the 'Update Now' button to take advantage of all the new features and improvements. Thank you for your support!",
            "Update Now", "Later");

        updatePopUp.init();
    }
   
#endregion
}
