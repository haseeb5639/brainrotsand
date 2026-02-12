using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDPRManager : MonoBehaviour
{
    private const string ADS_PERSONALIZATION_CONSENT = "Ads";
    #region Singleton
    private static GDPRManager _Instance;
    public static GDPRManager Instance
    {
        get
        {

            if (!_Instance) _Instance = FindObjectOfType<GDPRManager>();

            return _Instance;
        }
    }
    void Awake()
    {
        if (!_Instance) _Instance = this;
    }

    #endregion
    private void Start()
    {
       // Debug.Log("Terms of Service has been accepted: " + SimpleGDPR.IsTermsOfServiceAccepted);
        //Debug.Log("Ads personalization consent state: " + SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT));
       // Debug.Log("Is user possibly located in the EEA: " + SimpleGDPR.IsGDPRApplicable);

       // SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
             //       SetTermsOfServiceLink("https://my.tos.url").
             //       SetPrivacyPolicyLink("https://my.policy.url"),
              //      TermsOfServiceDialogClosed);

      

    }
    public void ShowGDPR()
    {
        StartCoroutine(ShowGDPRConsentDialogAndWait());
        Time.timeScale = 0f;
    }

    private void TermsOfServiceDialogClosed()
    {
        // We can assume that user has accepted the terms because
        // TermsOfServiceDialog dialog can only be closed via the 'Accept' button
        Debug.Log("Accepted Terms of Service");
    }

    private IEnumerator ShowGDPRConsentDialogAndWait()
    {
        // Show a consent dialog with two sections (and wait for the dialog to be closed):
        // - Ads Personalization: its value can be changed directly from the UI,
        //   result is stored in the ADS_PERSONALIZATION_CONSENT identifier
        // - Unity Analytics: its value can't be changed from the UI since Unity presents its own UI
        //   to toggle Analytics consent. Instead, a button is shown and when the button is clicked,
        //   UnityAnalyticsButtonClicked function is called to present Unity's own UI
        yield return SimpleGDPR.WaitForDialog(new GDPRConsentDialog().
            AddSectionWithToggle(ADS_PERSONALIZATION_CONSENT, "Ads Personalization", "When enabled, you'll see ads that are more relevant to you. Otherwise, you will still receive ads, but they will no longer be tailored toward you.").
          //  AddSectionWithButton(UnityAnalyticsButtonClicked, "Unity Analytics", "The collected data allows us to optimize the gameplay and update the game with new enjoyable content. You can see your collected data or change your settings from the dashboard.", "Open Analytics Dashboard").
            AddPrivacyPolicies("https://policies.google.com/privacy", "https://sites.google.com/view/gamebusstudiosprivacypolicy/home", "https://sites.google.com/view/gametruckstudiosprivacypolicy/home"));

        // Check if user has granted the Ads Personalization permission
        if (SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Yes)
        {
            // You can show personalized ads to the user

            PlayerPrefs.SetInt("NPA", 1);
            Time.timeScale = 1;
            PlayerPrefs.SetInt("GDPR", 1);
            AdsManager.Instance.Init();
            AppTracking.Instance.ShowTracking();
        }
        else
        {
            PlayerPrefs.SetInt("NPA", 0);
            Time.timeScale = 1;
            PlayerPrefs.SetInt("GDPR", 1);
            AdsManager.Instance.Init();
            AppTracking.Instance.ShowTracking();

        }
    }

    private void UnityAnalyticsButtonClicked()
    {
        // Fetch the URL of the page that allows the user to toggle the Unity Analytics consent
        // "Unity Data Privacy Plug-in" is required: https://assetstore.unity.com/packages/add-ons/services/unity-data-privacy-plug-in-118922
#if !UNITY_5_3_OR_NEWER && !UNITY_5_2 // Initialize must be called on Unity 5.1 or earlier
	//UnityEngine.Analytics.DataPrivacy.Initialize();
#endif
        //UnityEngine.Analytics.DataPrivacy.FetchPrivacyUrl( 
        //	( url ) => SimpleGDPR.OpenURL( url ), // On WebGL, this opens the URL in a new tab
        //	( error ) => Debug.LogError( "Couldn't fetch url: " + error ) );
    }
}
