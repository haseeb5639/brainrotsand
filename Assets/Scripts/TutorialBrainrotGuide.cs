



using UnityEngine;

public class TutorialBrainrotGuide : MonoBehaviour
{
    public static TutorialBrainrotGuide Instance;

    [Header("Assign Tutorial Brainrot")]
    public BrainrotInteraction tutorialBrainrot;

    [Header("Player Reference")]
    public Transform player;

    private bool tutorialActive = false;

    void Awake()
    {
        Instance = this;

        // Already completed? Disable tutorial forever
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            tutorialActive = false;

            // 💥 Very important — CLEAR any leftover path
            if (ShowGoldenPath.goldenPath != null)
            {
                ShowGoldenPath.goldenPath.IsPathOn = false;
                ShowGoldenPath.goldenPath.SetPathTarget(null);
            }

            gameObject.SetActive(false); // Disable tutorial script
            return;
        }

        // First time only
        tutorialActive = true;
    }

    void Start()
    {
        if (!tutorialActive)
            return;

        // Show tutorial path only ONCE
        ShowGoldenPath.goldenPath.IsPathOn = true;
        ShowGoldenPath.goldenPath.SetPathTarget(tutorialBrainrot.gameObject);
    }

    public void OnTutorialBrainrotPicked()
    {
        if (!tutorialActive) return;

        // Show path to base now
        ShowGoldenPath.goldenPath.IsPathOn = true;
        ShowGoldenPath.goldenPath.SetPathTarget(BaseManager.instance.defaultBaseTarget.gameObject);
    }

    public void OnTutorialBrainrotDropped()
    {
        if (!tutorialActive) return;

        // Clear the path permanently
        ShowGoldenPath.goldenPath.IsPathOn = false;
        ShowGoldenPath.goldenPath.SetPathTarget(null);

        // Save tutorial completed forever
        PlayerPrefs.SetInt("TutorialDone", 1);
        PlayerPrefs.Save();

        // Disable tutorial script in current session too
        gameObject.SetActive(false);
    }

    public void ResetTutorialToStart()
    {
        if (!tutorialActive) return;

        // Show path back to original tutorial brainrot
        ShowGoldenPath.goldenPath.IsPathOn = true;
        ShowGoldenPath.goldenPath.SetPathTarget(tutorialBrainrot.gameObject);
    }
}
