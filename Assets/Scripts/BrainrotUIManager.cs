







//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections;

//public class BrainrotUIManager : MonoBehaviour
//{
//    public static BrainrotUIManager instance;

//    [Header("UI References")]
//    public GameObject messagePanel;
//    public TextMeshProUGUI messageText;
//    public Button actionButton;
//    public TextMeshProUGUI actionButtonText;
//    public GameObject cashBar;

//    private bool isHolding = false;
//    private Coroutine typingCoroutine;

//    void Awake()
//    {
//        instance = this;

//        if (messagePanel != null)
//            messagePanel.SetActive(false);

//        if (cashBar != null)
//            cashBar.SetActive(false);
//    }

//    void Start()
//    {
//        if (actionButton != null)
//            actionButton.onClick.AddListener(OnActionPressed);

//        if (cashBar != null)
//            cashBar.SetActive(true);
//    }

//    void OnDestroy()
//    {
//        if (actionButton != null)
//            actionButton.onClick.RemoveListener(OnActionPressed);
//    }

//    // 🧩 Show Message with typewriter animation
//    public void ShowMessage(string msg)
//    {
//        if (messagePanel == null || messageText == null) return;

//        if (typingCoroutine != null)
//            StopCoroutine(typingCoroutine);

//        messagePanel.SetActive(true);
//        typingCoroutine = StartCoroutine(TypeText(msg));
//    }

//    public void HideMessage()
//    {
//        if (messagePanel == null) return;

//        if (typingCoroutine != null)
//            StopCoroutine(typingCoroutine);

//        messagePanel.SetActive(false);
//    }

//    // 🎬 Typewriter effect
//    private IEnumerator TypeText(string fullText)
//    {
//        messageText.text = "";
//        yield return new WaitForSeconds(0.05f);

//        foreach (char c in fullText)
//        {
//            messageText.text += c;
//            yield return new WaitForSeconds(0.02f);
//        }
//    }

//    // ✅ Cleaned Action Press
//    void OnActionPressed()
//    {
//        if (isHolding)
//            BrainrotManager.instance?.Drop();  // BrainrotManager will hide text
//        else
//            BrainrotManager.instance?.Grab();
//    }  // BrainrotManager will show "Drop on your base"






//    public void UpdateActionButton(bool holding)
//    {
//        isHolding = holding;
//    }

//    // 🔹 Helper prompts
//    public void ShowPickupPrompt(string brainrotName)
//    {
//        ShowMessage($"Tap the hand icon to pick up {brainrotName}");
//    }

//    public void ShowDropPrompt()
//    {
//        ShowMessage("Drop the Brainrot on your base");
//    }
//}


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BrainrotUIManager : MonoBehaviour
{
    public static BrainrotUIManager instance;

    [Header("UI References")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public Button actionButton;
    public TextMeshProUGUI actionButtonText;
    public GameObject cashBar;

    private bool isHolding = false;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        instance = this;

        if (messagePanel != null)
            messagePanel.SetActive(false);

        if (cashBar != null)
            cashBar.SetActive(false);
    }

    private void Start()
    {
        if (actionButton != null)
            actionButton.onClick.AddListener(OnActionPressed);

        if (cashBar != null)
            cashBar.SetActive(true);
    }

    private void OnDestroy()
    {
        if (actionButton != null)
            actionButton.onClick.RemoveListener(OnActionPressed);
    }

    // =====================
    // MESSAGE SYSTEM
    // =====================
    public void ShowMessage(string msg)
    {
        if (messagePanel == null || messageText == null) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        messagePanel.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(msg));
    }

    public void HideMessage()
    {
        if (messagePanel == null) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        messagePanel.SetActive(false);
    }

    private IEnumerator TypeText(string fullText)
    {
        messageText.text = "";
        yield return new WaitForSeconds(0.05f);

        foreach (char c in fullText)
        {
            messageText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // =====================
    // ACTION BUTTON
    // =====================
    void OnActionPressed()
    {
        // 🔥 Hide Step-1 image ONLY ONCE when grabbing
        if (!isHolding && TutorialManager.Instance != null)
        {
            TutorialManager.Instance.HideStep1ImageOnGrab();
        }

        if (isHolding)
            BrainrotManager.instance?.Drop();
        else
            BrainrotManager.instance?.Grab();
    }

    public void UpdateActionButton(bool holding)
    {
        isHolding = holding;
    }

    // =====================
    // PROMPTS
    // =====================
    public void ShowPickupPrompt(string brainrotName)
    {
        ShowMessage($"Tap the hand icon to pick up {brainrotName}");
    }

    public void ShowDropPrompt()
    {
        ShowMessage("Drop the Brainrot on your base");
    }
}
