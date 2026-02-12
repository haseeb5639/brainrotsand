



//using UnityEngine;
//using UnityEngine.UI;

//public class TutorialManager : MonoBehaviour
//{
//    public static TutorialManager Instance;

//    [Header("Main Tutorial Frame")]
//    public GameObject tutorialPanel;

//    [Header("Step Panels")]
//    public GameObject step1Panel;
//    public GameObject step2Panel;
//    public GameObject step3Panel;

//    [Header("Continue Buttons")]
//    public Button step1Btn;
//    public Button step2Btn;
//    public Button step3Btn;

//    public bool step1Done = false;
//    public bool step2Done = false;
//    //public bool step3Done = false;

//    private const string KEY = "tutorialDone";

//    private void Awake()
//    {
//        Instance = this;

//        if (PlayerPrefs.GetInt(KEY, 0) == 1)
//        {
//            HideAll();
//            gameObject.SetActive(false);
//            return;
//        }

//        HideAll();
//    }

//    private void Start()
//    {
//        step1Btn.onClick.AddListener(() =>
//        {
//            step1Done = true;
//            CloseStep(step1Panel);
//        });

//        step2Btn.onClick.AddListener(() =>
//        {
//            step2Done = true;
//            CloseStep(step2Panel);
//        });

//        //step3Btn.onClick.AddListener(() =>
//        //{
//        //    step3Done = true;
//        //    CloseStep(step3Panel);

//        //    PlayerPrefs.SetInt(KEY, 1);
//        //    PlayerPrefs.Save();
//        //});
//    }

//    public void TriggerStep(int step)
//    {
//        switch (step)
//        {
//            case 1:
//                if (!step1Done)
//                    Show(step1Panel);
//                break;

//            case 2:
//                if (step1Done && !step2Done)
//                    Show(step2Panel);
//                break;

//            //case 3:
//            //    if (step2Done && !step3Done)
//            //        Show(step3Panel);
//            //    break;
//        }
//    }

//    public void ExitStep(int step)
//    {
//        HideAll();
//    }

//    private void Show(GameObject panel)
//    {
//        HideAll();
//        tutorialPanel.SetActive(true);
//        panel.SetActive(true);
//    }

//    private void CloseStep(GameObject panel)
//    {
//        panel.SetActive(false);
//        tutorialPanel.SetActive(false);
//    }

//    private void HideAll()
//    {
//        tutorialPanel.SetActive(false);
//        step1Panel.SetActive(false);
//        step2Panel.SetActive(false);
//        //step3Panel.SetActive(false);
//    }

//    public bool IsTutorialCompleted()
//    {
//        return PlayerPrefs.GetInt("tutorialDone", 0) == 1;
//    }

//}





//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;

//public class TutorialManager : MonoBehaviour
//{
//    public static TutorialManager Instance;

//    [Header("Main Tutorial Frame")]
//    public GameObject tutorialPanel;

//    [Header("Step Panels")]
//    public GameObject step1Panel;
//    public GameObject step2Panel;
//    public GameObject step1img;

//    [Header("Continue Buttons")]
//    public Button step1Btn;
//    public Button step2Btn;

//    private bool step1Done = false;
//    private bool step2Done = false;


//    private const string KEY = "tutorialDone";

//    private void Awake()
//    {
//        // Singleton
//        if (Instance == null)
//            Instance = this;
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        // 🔒 If tutorial already completed → disable forever
//        if (PlayerPrefs.GetInt(KEY, 0) == 1)
//        {
//            HideAll();
//            gameObject.SetActive(false);
//            return;
//        }

//        HideAll();
//    }

//    private void Start()
//    {
//        // STEP 1 COMPLETE
//        step1Btn.onClick.AddListener(() =>
//        {
//            step1Done = true;
//            CloseStep(step1Panel);

//            if (step1Done)
//                Show(step1img) ;


//        });

//        // STEP 2 COMPLETE → FINISH TUTORIAL
//        step2Btn.onClick.AddListener(() =>
//        {
//            step2Done = true;
//            CloseStep(step2Panel);

//            // ✅ MARK TUTORIAL COMPLETE FOREVER
//            PlayerPrefs.SetInt(KEY, 1);
//            PlayerPrefs.Save();

//            HideAll();
//            gameObject.SetActive(false);
//        });
//    }

//    // =====================
//    // EXTERNAL TRIGGERS
//    // =====================
//    public void TriggerStep(int step)
//    {
//        if (PlayerPrefs.GetInt(KEY, 0) == 1)
//            return;

//        switch (step)
//        {
//            case 1:
//                if (!step1Done)
//                    Show(step1Panel);
//                break;

//            case 2:
//                if (step1Done && !step2Done)
//                    Show(step2Panel);
//                break;
//        }
//    }

//    public void ExitStep(int step)
//    {
//        HideAll();
//    }

//    // =====================
//    // UI HELPERS
//    // =====================
//    private void Show(GameObject panel)
//    {
//        HideAll();
//        tutorialPanel.SetActive(true);
//        panel.SetActive(true);
//    }

//    private void CloseStep(GameObject panel)
//    {
//        panel.SetActive(false);
//        tutorialPanel.SetActive(false);
//    }

//    private void HideAll()
//    {
//        tutorialPanel.SetActive(false);
//        step1Panel.SetActive(false);
//        step2Panel.SetActive(false);
//    }

//    public bool IsTutorialCompleted()
//    {
//        return PlayerPrefs.GetInt(KEY, 0) == 1;
//    }

//    // =====================
//    // OPTIONAL: RESET (DEV ONLY)
//    // =====================
//    public void ResetTutorial()
//    {
//        PlayerPrefs.DeleteKey(KEY);
//        PlayerPrefs.Save();
//        Debug.Log("🔄 Tutorial Reset");
//    }


//}


using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("Main Tutorial Frame")]
    public GameObject tutorialPanel;

    [Header("Step Panels")]
    public GameObject step1Panel;
    public GameObject step2Panel;

    [Header("Step 1 Hint Image (After Step 1)")]
    public GameObject step1img;

    [Header("Continue Buttons")]
    public Button step1Btn;
    public Button step2Btn;

    private bool step1Done = false;
    private bool step2Done = false;

    // Step-1 image control
    private bool step1ImageShown = false;
    private bool step1ImageHiddenOnce = false;

    private const string KEY = "tutorialDone";

    private void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tutorial already completed → disable forever
        if (PlayerPrefs.GetInt(KEY, 0) == 1)
        {
            HideAll();
            if (step1img != null)
                step1img.SetActive(false);

            gameObject.SetActive(false);
            return;
        }

        HideAll();
        if (step1img != null)
            step1img.SetActive(false);
    }

    private void Start()
    {
        // =====================
        // STEP 1 COMPLETE
        // =====================
        step1Btn.onClick.AddListener(() =>
        {
            step1Done = true;

            // Close tutorial UI completely
            HideAll();

            // Show ONLY the image (no tutorial panel)
            if (step1img != null)
            {
                step1img.SetActive(true);
                step1ImageShown = true;
            }
        });

        // =====================
        // STEP 2 COMPLETE → FINISH TUTORIAL
        // =====================
        step2Btn.onClick.AddListener(() =>
        {
            step2Done = true;

            HideAll();

            // Mark tutorial completed forever
            PlayerPrefs.SetInt(KEY, 1);
            PlayerPrefs.Save();

            if (step1img != null)
                step1img.SetActive(false);

            gameObject.SetActive(false);
        });
    }

    // =====================
    // EXTERNAL TRIGGERS
    // =====================
    public void TriggerStep(int step)
    {
        if (PlayerPrefs.GetInt(KEY, 0) == 1)
            return;

        switch (step)
        {
            case 1:
                if (!step1Done)
                    Show(step1Panel);
                break;

            case 2:
                if (step1Done && !step2Done)
                    Show(step2Panel);
                break;
        }
    }

    public void ExitStep(int step)
    {
        HideAll();
    }

    // =====================
    // IMAGE CONTROL (CALLED ON GRAB)
    // =====================
    public void HideStep1ImageOnGrab()
    {
        if (!step1ImageShown) return;
        if (step1ImageHiddenOnce) return;

        if (step1img != null)
            step1img.SetActive(false);

        step1ImageHiddenOnce = true;
    }

    // =====================
    // UI HELPERS
    // =====================
    private void Show(GameObject panel)
    {
        HideAll();
        tutorialPanel.SetActive(true);
        panel.SetActive(true);
    }

    private void HideAll()
    {
        tutorialPanel.SetActive(false);
        step1Panel.SetActive(false);
        step2Panel.SetActive(false);
        // ❌ step1img intentionally NOT hidden here
    }

    public bool IsTutorialCompleted()
    {
        return PlayerPrefs.GetInt(KEY, 0) == 1;
    }

    // =====================
    // DEV ONLY
    // =====================
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(KEY);
        PlayerPrefs.Save();
        Debug.Log("🔄 Tutorial Reset");
    }
}
