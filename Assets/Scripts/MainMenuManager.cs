


/////////////////////////////////////////
/////



//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections;

//public class MainMenuManager : MonoBehaviour
//{
//    [Header("Main Menu Buttons")]
//    [SerializeField] private Button playButton;
//    [SerializeField] private Button privacyButton;
//    [SerializeField] private Button moreGamesButton;
//    [SerializeField] private Button rateButton;
//    [SerializeField] private Button exitButton; // 🆕 added for Quit panel

//    [Header("Panels")]
//    [SerializeField] private GameObject mainPanel;       // Main menu
//    [SerializeField] private GameObject subPanelRoot;    // Panel 2
//    [SerializeField] private GameObject privacyPanel;
//    [SerializeField] private GameObject moreGamesPanel;
//    [SerializeField] private GameObject ratingPanel;
//    [SerializeField] private GameObject quitPanel;       // 🆕 added quit panel

//    [Header("Links")]
//    [SerializeField] private string gameplaySceneName = "Controller";
//    [SerializeField] private string privacyLink = "https://docs.google.com/document/d/1pMMFblr5H3rDTj6aVRZTSI3javexZINPTKKP7T3GD1I/edit?pli=1&tab=t.0#heading=h.uoq0rc43yy42";
//    [SerializeField] private string moreGamesLink = "https://play.google.com/store/apps/details?id=com.gvs.punch.annoying.hit.smasher";
//    [SerializeField] private string ratingLink = ""; // you will provide later

//    [Header("Animation Settings")]
//    [SerializeField] private float bounceScale = 1.15f;
//    [SerializeField] private float bounceDuration = 0.15f;

//    private bool isLoading = false;

//    void Start()
//    {
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;

//        ShowMainMenu();

//        // --- Button listeners ---
//        if (playButton) playButton.onClick.AddListener(() => StartCoroutine(PlayButtonBounce()));
//        if (privacyButton) privacyButton.onClick.AddListener(() => StartCoroutine(ButtonBounceAndOpen(privacyPanel)));
//        if (moreGamesButton) moreGamesButton.onClick.AddListener(() => StartCoroutine(ButtonBounceAndOpen(moreGamesPanel)));
//        if (rateButton) rateButton.onClick.AddListener(() => StartCoroutine(ButtonBounceAndOpen(ratingPanel)));
//        if (exitButton) exitButton.onClick.AddListener(() => StartCoroutine(ButtonBounceAndOpen(quitPanel))); // 🆕 quit panel
//    }

//    // ---------------- ANIMATION LOGIC ----------------
//    private IEnumerator ButtonBounceAndOpen(GameObject targetPanel)
//    {
//        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
//        if (btn != null)
//            yield return StartCoroutine(Bounce(btn.GetComponent<RectTransform>()));

//        yield return new WaitForSecondsRealtime(0.05f);
//        ShowPanel(targetPanel);
//    }

//    private IEnumerator PlayButtonBounce()
//    {
//        if (isLoading) yield break;
//        isLoading = true;

//        yield return StartCoroutine(Bounce(playButton.GetComponent<RectTransform>()));
//        yield return new WaitForSecondsRealtime(0.05f);

//        SceneManager.LoadScene(gameplaySceneName);
//    }

//    private IEnumerator Bounce(RectTransform rt)
//    {
//        if (rt == null) yield break;

//        Vector3 startScale = Vector3.one;
//        Vector3 peakScale = Vector3.one * bounceScale;

//        float elapsed = 0f;
//        while (elapsed < bounceDuration)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            float t = Mathf.Clamp01(elapsed / bounceDuration);
//            float eased = Mathf.Sin(t * Mathf.PI * 0.5f);
//            rt.localScale = Vector3.Lerp(startScale, peakScale, eased);
//            yield return null;
//        }

//        elapsed = 0f;
//        while (elapsed < bounceDuration * 0.6f)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            float t = Mathf.Clamp01(elapsed / (bounceDuration * 0.6f));
//            float eased = 1f - Mathf.Pow(1f - t, 3f);
//            rt.localScale = Vector3.Lerp(peakScale, startScale, eased);
//            yield return null;
//        }

//        rt.localScale = startScale;
//    }

//    // ---------------- PANEL MANAGEMENT ----------------
//    private void ShowMainMenu()
//    {
//        if (mainPanel) mainPanel.SetActive(true);
//        if (subPanelRoot) subPanelRoot.SetActive(false);
//        if (privacyPanel) privacyPanel.SetActive(false);
//        if (moreGamesPanel) moreGamesPanel.SetActive(false);
//        if (ratingPanel) ratingPanel.SetActive(false);
//        if (quitPanel) quitPanel.SetActive(false);
//    }

//    private void ShowPanel(GameObject target)
//    {
//        if (mainPanel) mainPanel.SetActive(false);
//        if (subPanelRoot) subPanelRoot.SetActive(true);

//        if (privacyPanel) privacyPanel.SetActive(false);
//        if (moreGamesPanel) moreGamesPanel.SetActive(false);
//        if (ratingPanel) ratingPanel.SetActive(false);
//        if (quitPanel) quitPanel.SetActive(false);

//        if (target) target.SetActive(true);
//    }

//    // ---------------- SUBPANEL BUTTONS ----------------
//    public void OnPrivacyYes() => Application.OpenURL(privacyLink);
//    public void OnMoreGamesYes() => Application.OpenURL(moreGamesLink);
//    public void OnRateNow()
//    {
//        if (!string.IsNullOrEmpty(ratingLink))
//            Application.OpenURL(ratingLink);
//        else
//            Debug.Log("Rating link not yet assigned!");
//    }

//    // 🆕 QUIT PANEL BUTTONS
//    public void OnQuitYes()
//    {
//        Debug.Log("Application Quit Triggered");
//        Application.Quit();
//    }

//    public void OnQuitNo() => ShowMainMenu();

//    public void OnClosePanel() => ShowMainMenu();
//}















////////////////////////////   rerwad panel 
/////

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button privacyButton;
    [SerializeField] private Button moreGamesButton;
    [SerializeField] private Button rateButton;
    [SerializeField] private Button dailyRewardButton;
    
    [SerializeField] private Button exitButton; // 🆕 added for Quit panel

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject subPanelRoot;
    [SerializeField] private GameObject privacyPanel;
    [SerializeField] private GameObject moreGamesPanel;
    [SerializeField] private GameObject ratingPanel;
    [SerializeField] private GameObject rewardsPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject quitPanel;       // 🆕 added quit panel


    [Header("Links")]
    [SerializeField] private string gameplaySceneName = "Controller";
    [SerializeField] private string privacyLink = "https://docs.google.com/document/d/1pMMFblr5H3rDTj6aVRZTSI3javexZINPTKKP7T3GD1I/edit?pli=1&tab=t.0#heading=h.uoq0rc43yy42";
    [SerializeField] private string moreGamesLink = "https://play.google.com/store/apps/developer?id=The+Game+Village+Studios";
    [SerializeField] public string ratingLink = "";

    [Header("Animation Settings")]
    [SerializeField] private float bounceScale = 1.15f;
    [SerializeField] private float bounceDuration = 0.15f;

    private bool isLoading = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ShowMainMenu();

        
    }

    private IEnumerator ButtonBounceAndOpen(GameObject targetPanel)
    {
        Button btn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
        if (btn != null)
        {
            yield return StartCoroutine(Bounce(btn.GetComponent<RectTransform>()));
        }
        yield return new WaitForSecondsRealtime(0.05f);
        ShowPanel(targetPanel);

        
    }

    public void PlayGame()
    {
        StartCoroutine(PlayButtonBounce());
        
    }

    private IEnumerator PlayButtonBounce()
    {
        if (isLoading) yield break;
        isLoading = true;

        yield return StartCoroutine(Bounce(playButton.GetComponent<RectTransform>()));
        yield return new WaitForSecondsRealtime(0.05f);

        SceneManager.LoadScene(gameplaySceneName);

        //AdsManager.Instance.ShowAds();
        //AdsManager.Instance.ShowRewardedAds();
        LabAnalytics.Instance.LogEvent("PlayButton");
    }

    private IEnumerator Bounce(RectTransform rt)
    {
        if (rt == null) yield break;

        Vector3 startScale = Vector3.one;
        Vector3 peakScale = Vector3.one * bounceScale;

        float elapsed = 0f;
        while (elapsed < bounceDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / bounceDuration);
            float eased = Mathf.Sin(t * Mathf.PI * 0.5f);
            rt.localScale = Vector3.Lerp(startScale, peakScale, eased);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < bounceDuration * 0.6f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / (bounceDuration * 0.6f));
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            rt.localScale = Vector3.Lerp(peakScale, startScale, eased);
            yield return null;
        }

        rt.localScale = startScale;
    }

    private void ShowMainMenu()
    {
        if (mainPanel) mainPanel.SetActive(true);
        if (subPanelRoot) subPanelRoot.SetActive(false);
        if (privacyPanel) privacyPanel.SetActive(false);
        if (moreGamesPanel) moreGamesPanel.SetActive(false);
        if (ratingPanel) ratingPanel.SetActive(false);
        if (rewardsPanel) rewardsPanel.SetActive(false);
        if (settingPanel) settingPanel.SetActive(false);
        if (quitPanel) quitPanel.SetActive(false);
    }

    public void ShowPanel(GameObject target)
    {
        if (mainPanel) mainPanel.SetActive(false);
        if (subPanelRoot) subPanelRoot.SetActive(true);

        if (privacyPanel) privacyPanel.SetActive(false);
        if (moreGamesPanel) moreGamesPanel.SetActive(false);
        if (ratingPanel) ratingPanel.SetActive(false);
        if (rewardsPanel) rewardsPanel.SetActive(false);
        if (settingPanel) settingPanel.SetActive(false);
        if (quitPanel) quitPanel.SetActive(false);

        if (target) target.SetActive(true);
    }

    public void OnPrivacyYes() => Application.OpenURL(privacyLink);
    public void OnMoreGamesYes() => Application.OpenURL(moreGamesLink);
    public void OnRateNow()
    {     
         Application.OpenURL("https://play.google.com/store/apps/details?id=com.gvs.rescue.fix.brainrot.game&hl=en" + Application.identifier);
    }

    public void RemoveAD()
    {
        AdsManager.Instance.RemoveAds();
    }
    public void ShowBanner()
    {
        AdsManager.Instance.ShowBanner();
    }
    // 🆕 QUIT PANEL BUTTONS
    public void OnQuitYes()
     {
         Debug.Log("Application Quit Triggered");
         Application.Quit();
      }
    public void OnClosePanel() => ShowMainMenu();


    
}
