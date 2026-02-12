using UnityEngine;
using System.Collections;

public class NativeRateUS
{

        #region PUBLIC_VARIABLES

        string title;
        string message;
        string yesButton;
        string noButton;


    public string appLink;

    #endregion

    #region PUBLIC_FUNCTIONS

    public NativeRateUS(string title, string message)
        {
            this.title = title;
            this.message = message;
            this.yesButton = "Rate";
            this.noButton = "Later";
        }

        public NativeRateUS(string title, string message, string yesButtonText, string noButtonText)
        {
            this.title = title;
            this.message = message;
            this.yesButton = yesButtonText;
            this.noButton = noButtonText;
        }

        public void SetUrlString(string urlString)
        {
        appLink = urlString;
        }

        public void init()
        {
#if UNITY_ANDROID
            AndroidDialog dialog = AndroidDialog.Create(title, message, yesButton, noButton);
             dialog.urlString = appLink;
#endif
        }

        #endregion
    }
