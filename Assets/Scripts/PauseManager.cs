////using UnityEngine;
////using UnityEngine.SceneManagement;

////public class PauseManager : MonoBehaviour
////{
////    [Header("Panels")]
////    [SerializeField] private GameObject resumePanel;

////    [Header("Settings")]
////    [SerializeField] private string mainMenuSceneName = "MainMenu";

////    private bool isPaused = false;

////    void Start()
////    {
////        if (resumePanel != null)
////            resumePanel.SetActive(false);
////    }

////    // Called when Pause button clicked
////    public void OnPauseClick()
////    {
////        if (resumePanel != null)
////            resumePanel.SetActive(true);

////        Time.timeScale = 0f;
////        isPaused = true;
////    }

////    // Called when Resume button clicked
////    public void OnResumeClick()
////    {
////        if (resumePanel != null)
////            resumePanel.SetActive(false);

////        Time.timeScale = 1f;
////        isPaused = false;
////    }

////    // Called when Restart button clicked
////    public void OnRestartClick()
////    {
////        Time.timeScale = 1f;
////        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
////    }

////    // Called when Home button clicked
////    public void OnHomeClick()
////    {
////        Time.timeScale = 1f;
////        SceneManager.LoadScene(mainMenuSceneName);
////    }
////}












////////////
/////
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System.Collections;

//public class PauseManager : MonoBehaviour
//{
//    [Header("Panels")]
//    [SerializeField] private GameObject resumePanel;
//    [SerializeField] private CanvasGroup resumePanelGroup; // for fade

//    [Header("Settings")]
//    [SerializeField] private string mainMenuSceneName = "MainMenu";
//    [SerializeField] private float fadeDuration = 0.3f;

//    private bool isPaused = false;
//    private Coroutine fadeRoutine;

//    void Start()
//    {
//        if (resumePanel != null)
//        {
//            resumePanel.SetActive(false);
//            if (resumePanelGroup != null)
//                resumePanelGroup.alpha = 0f;
//        }
//    }

//    public void OnPauseClick()
//    {
//        if (resumePanel == null) return;
//        resumePanel.SetActive(true);
//        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
//        fadeRoutine = StartCoroutine(FadeCanvasGroup(resumePanelGroup, 0f, 1f));
//        Time.timeScale = 0f;
//        isPaused = true;
//    }

//    public void OnResumeClick()
//    {
//        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
//        fadeRoutine = StartCoroutine(FadeOutAndDeactivate());
//        Time.timeScale = 1f;
//        isPaused = false;
//    }

//    //public void OnRestartClick()
//    //{
//    //    Time.timeScale = 1f;
//    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    //}
//    public void OnRestartClick()
//    {
//        StartCoroutine(RestartGameRoutine());
//    }

//    private IEnumerator RestartGameRoutine()
//    {
//        // Unpause first
//        Time.timeScale = 1f;

//        // Optional: small delay to let UI close gracefully
//        yield return new WaitForSeconds(0.05f);

//        // Reload the current active scene
//        SceneManager.LoadScene(2);
//    }

//    public void OnHomeClick()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(mainMenuSceneName);
//    }

//    private IEnumerator FadeOutAndDeactivate()
//    {
//        yield return FadeCanvasGroup(resumePanelGroup, 1f, 0f);
//        resumePanel.SetActive(false);
//    }

//    private IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end)
//    {
//        if (group == null) yield break;

//        float elapsed = 0f;
//        while (elapsed < fadeDuration)
//        {
//            elapsed += Time.unscaledDeltaTime; // unaffected by pause
//            group.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
//            yield return null;
//        }
//        group.alpha = end;
//    }
//}










/////////////////// Animations for pause menu
///

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject resumePanel;
    [SerializeField] private CanvasGroup resumePanelGroup;
    [SerializeField] private RectTransform resumePanelTransform;

    [Header("Buttons")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private float buttonDelay = 0.08f;
    [SerializeField] private float buttonSlideDistance = 60f;
    [SerializeField] private float buttonBounceScale = 1.15f; // pop size
    [SerializeField] private float buttonBounceDuration = 0.15f;

    [Header("Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scalePop = 1.05f;
    [SerializeField] private float scaleNormal = 1.01f;

    private bool isPaused = false;
    private Coroutine animRoutine;

    // store initial button positions
    private Vector2 homeInitPos, restartInitPos, resumeInitPos;

    void Start()
    {
        if (resumePanel != null)
        {
            resumePanel.SetActive(false);
            if (resumePanelGroup != null)
                resumePanelGroup.alpha = 0f;
            if (resumePanelTransform != null)
                resumePanelTransform.localScale = Vector3.zero;
        }

        // store button start positions
        if (homeButton != null) homeInitPos = homeButton.GetComponent<RectTransform>().anchoredPosition;
        if (restartButton != null) restartInitPos = restartButton.GetComponent<RectTransform>().anchoredPosition;
        if (resumeButton != null) resumeInitPos = resumeButton.GetComponent<RectTransform>().anchoredPosition;
    }

    //public void OnPauseClick()
    //{
    //    if (resumePanel == null) return;
    //    resumePanel.SetActive(true);

    //    if (animRoutine != null) StopCoroutine(animRoutine);
    //    animRoutine = StartCoroutine(OpenPanelRoutine());

    //    Time.timeScale = 0f;
    //    isPaused = true;
    //}
    public void OnPauseClick()
    {
        // 🔥 Prevent unlock panel override
        if (BaseUnlockManager.Instance != null)
            BaseUnlockManager.Instance.HideUnlockPanel();

        if (resumePanel == null) return;
        resumePanel.SetActive(true);

        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(OpenPanelRoutine());

        Time.timeScale = 0f;
        isPaused = true;
        AdsManager.Instance.ShowAds();
    }
    //public void OnResumeClick()
    //{
    //    if (animRoutine != null) StopCoroutine(animRoutine);
    //    animRoutine = StartCoroutine(ClosePanelRoutine());

    //    Time.timeScale = 1f;
    //    isPaused = false;





    //}
    public void OnResumeClick()
    {
        // 🔥 Again force-close unlock panel
        if (BaseUnlockManager.Instance != null)
            BaseUnlockManager.Instance.HideUnlockPanel();

        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(ClosePanelRoutine());

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnRestartClick()
    {
        StartCoroutine(RestartGameRoutine());
    }

    private IEnumerator RestartGameRoutine()
    {
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnHomeClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // ==================== PANEL ANIMATION ====================

    private IEnumerator OpenPanelRoutine()
    {
        if (resumePanelGroup == null || resumePanelTransform == null) yield break;

        float elapsed = 0f;
        resumePanelTransform.localScale = Vector3.zero;
        resumePanelGroup.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            resumePanelGroup.alpha = eased;
            resumePanelTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * scalePop, eased);
            yield return null;
        }

        elapsed = 0f;
        Vector3 popScale = Vector3.one * scalePop;
        while (elapsed < 0.15f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / 0.15f;
            resumePanelTransform.localScale = Vector3.Lerp(popScale, Vector3.one * scaleNormal, t);
            yield return null;
        }

        resumePanelTransform.localScale = Vector3.one * scaleNormal;
        yield return StartCoroutine(AnimateButtonsIn());
    }

    private IEnumerator ClosePanelRoutine()
    {
        yield return StartCoroutine(AnimateButtonsOut());

        float elapsed = 0f;
        Vector3 startScale = resumePanelTransform.localScale;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            resumePanelGroup.alpha = 1f - eased;
            resumePanelTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, eased);
            yield return null;
        }

        resumePanelGroup.alpha = 0f;
        resumePanelTransform.localScale = Vector3.zero;
        resumePanel.SetActive(false);
    }

    // ==================== BUTTON ANIMATION (with bounce) ====================

    private IEnumerator AnimateButtonsIn()
    {
        Button[] buttons = { homeButton, restartButton, resumeButton };
        Vector2[] originalPositions = { homeInitPos, restartInitPos, resumeInitPos };

        for (int i = 0; i < buttons.Length; i++)
        {
            Button btn = buttons[i];
            if (btn == null) continue;

            RectTransform rt = btn.GetComponent<RectTransform>();
            CanvasGroup cg = btn.GetComponent<CanvasGroup>();
            if (cg == null) cg = btn.gameObject.AddComponent<CanvasGroup>();

            // start slightly lower and hidden
            rt.anchoredPosition = originalPositions[i] - new Vector2(0, buttonSlideDistance);
            cg.alpha = 0f;
            rt.localScale = Vector3.one * 0.8f;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                float eased = 1f - Mathf.Pow(1f - t, 3f);
                rt.anchoredPosition = Vector2.Lerp(originalPositions[i] - new Vector2(0, buttonSlideDistance), originalPositions[i], eased);
                cg.alpha = eased;
                yield return null;
            }

            // bounce-pop
            yield return StartCoroutine(BounceButton(rt));

            yield return new WaitForSecondsRealtime(buttonDelay);
        }
    }

    private IEnumerator AnimateButtonsOut()
    {
        Button[] buttons = { resumeButton, restartButton, homeButton };
        Vector2[] originalPositions = { resumeInitPos, restartInitPos, homeInitPos };

        for (int i = 0; i < buttons.Length; i++)
        {
            Button btn = buttons[i];
            if (btn == null) continue;

            RectTransform rt = btn.GetComponent<RectTransform>();
            CanvasGroup cg = btn.GetComponent<CanvasGroup>();
            if (cg == null) continue;

            float elapsed = 0f;
            while (elapsed < fadeDuration * 0.8f)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / (fadeDuration * 0.8f));
                float eased = 1f - Mathf.Pow(1f - t, 3f);
                rt.anchoredPosition = Vector2.Lerp(originalPositions[i], originalPositions[i] - new Vector2(0, buttonSlideDistance), eased);
                cg.alpha = 1f - eased;
                yield return null;
            }

            rt.anchoredPosition = originalPositions[i];
            cg.alpha = 0f;
            yield return new WaitForSecondsRealtime(buttonDelay * 0.5f);
        }
    }

    private IEnumerator BounceButton(RectTransform rt)
    {
        // bounce to 1.15x scale, then back to 1.0
        float elapsed = 0f;
        Vector3 start = Vector3.one;
        Vector3 peak = Vector3.one * buttonBounceScale;

        // scale up
        while (elapsed < buttonBounceDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / buttonBounceDuration);
            float eased = Mathf.Sin(t * Mathf.PI * 0.5f);
            rt.localScale = Vector3.Lerp(start, peak, eased);
            yield return null;
        }

        // scale down
        elapsed = 0f;
        while (elapsed < buttonBounceDuration * 0.6f)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / (buttonBounceDuration * 0.6f));
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            rt.localScale = Vector3.Lerp(peak, Vector3.one, eased);
            yield return null;
        }

        rt.localScale = Vector3.one;
    }
}
