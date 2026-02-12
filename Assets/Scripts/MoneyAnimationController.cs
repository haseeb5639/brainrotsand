

//using UnityEngine;
//using TMPro;
//using DG.Tweening;
//using UnityEngine.UI;

//public class MoneyAnimationController : MonoBehaviour
//{
//    [Header("References")]
//    public Canvas mainCanvas;
//    public GameObject moneyIconPrefab;
//    public RectTransform moneyTarget;          // ✅ Target: your top-right money icon object
//    public TextMeshProUGUI mainMoneyText;

//    [Header("Animation Settings")]
//    public float startScale = 0.3f;
//    public float moveDuration = 1.2f;
//    public float holdDuration = 0.3f;          // small wait at target before fade
//    public Ease moveEase = Ease.InOutCubic;
//    public Ease scaleEase = Ease.OutBack;

//    [Header("Counter Settings")]
//    public float countDuration = 0.5f;

//    [Header("Save Settings")]
//    public string playerPrefsKey = "TotalMoney";

//    private Camera mainCam;
//    private float totalMoney;
//    private Tweener countTween;

//    void Start()
//    {
//        mainCam = Camera.main;
//        totalMoney = PlayerPrefs.GetFloat(playerPrefsKey, 0f);
//        UpdateGlobalMoneyUI(instant: true);
//    }

//    // Called by BrainrotInteraction when collecting money
//    public void PlayMoneyAnimation(float amount, Vector3 worldStartPos)
//    {
//        if (!moneyIconPrefab || !mainCanvas) return;

//        // Convert world → UI local position
//        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCam, worldStartPos);
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            mainCanvas.transform as RectTransform, screenPos, mainCanvas.worldCamera, out Vector2 uiStartPos);

//        // Create instance under canvas
//        GameObject iconObj = Instantiate(moneyIconPrefab, mainCanvas.transform);
//        RectTransform icon = iconObj.GetComponent<RectTransform>();
//        icon.anchoredPosition = uiStartPos;
//        icon.localScale = Vector3.one * startScale;

//        // TMP text
//        TextMeshProUGUI tmp = iconObj.GetComponentInChildren<TextMeshProUGUI>();
//        if (tmp != null) tmp.text = $"+${amount:F0}";

//        // Fade component
//        CanvasGroup cg = iconObj.GetComponent<CanvasGroup>() ?? iconObj.AddComponent<CanvasGroup>();
//        cg.alpha = 1f;

//        // --- Animation Sequence ---
//        Vector2 targetPos = Vector2.zero;
//        if (moneyTarget != null)
//        {
//            // convert target (UI icon) to local canvas coordinates
//            RectTransformUtility.ScreenPointToLocalPointInRectangle(
//                mainCanvas.transform as RectTransform,
//                RectTransformUtility.WorldToScreenPoint(mainCam, moneyTarget.position),
//                mainCanvas.worldCamera, out targetPos);
//        }

//        Sequence seq = DOTween.Sequence();
//        seq.Append(icon.DOScale(1f, 0.25f).SetEase(scaleEase))
//           .Join(icon.DOAnchorPos(targetPos, moveDuration).SetEase(moveEase))
//           .AppendInterval(holdDuration)
//           .Join(cg.DOFade(0f, 0.4f))
//           .OnComplete(() =>
//           {
//               Destroy(iconObj);
//               AddMoney(amount);
//           });
//    }

//    private void AddMoney(float amount)
//    {
//        float previous = totalMoney;
//        totalMoney += amount;

//        PlayerPrefs.SetFloat(playerPrefsKey, totalMoney);
//        PlayerPrefs.Save();

//        AnimateMoneyCounter(previous, totalMoney);
//    }

//    private void AnimateMoneyCounter(float fromValue, float toValue)
//    {
//        if (countTween != null && countTween.IsActive())
//            countTween.Kill();

//        countTween = DOTween.To(() => fromValue, x =>
//        {
//            fromValue = x;
//            if (mainMoneyText) mainMoneyText.text = $"${fromValue:F0}";
//        },
//        toValue, countDuration).SetEase(Ease.OutCubic);
//    }

//    private void UpdateGlobalMoneyUI(bool instant = false)
//    {
//        if (mainMoneyText == null) return;
//        if (instant) mainMoneyText.text = $"${totalMoney:F0}";
//        else AnimateMoneyCounter(float.Parse(mainMoneyText.text.TrimStart('$')), totalMoney);
//    }

//    public float GetTotalMoney() => totalMoney;
//}


//using UnityEngine;
//using TMPro;
//using DG.Tweening;
//using UnityEngine.UI;

//public class MoneyAnimationController : MonoBehaviour
//{
//    [Header("References")]
//    public Canvas mainCanvas;
//    public GameObject moneyIconPrefab;       // Prefab with your own DOTween animation
//    public TextMeshProUGUI mainMoneyText;

//    [Header("Save Settings")]
//    public string playerPrefsKey = "TotalMoney";

//    private Camera mainCam;
//    private float totalMoney;

//    void Start()
//    {
//        mainCam = Camera.main;
//        totalMoney = PlayerPrefs.GetFloat(playerPrefsKey, 0f);
//        if (mainMoneyText)
//            mainMoneyText.text = $"${totalMoney:F0}";
//    }
//    public void PlayMoneyAnimation(float amount, Vector3 worldStartPos)
//    {
//        if (!moneyIconPrefab || !mainCanvas) return;

//        // --- Create instance ---
//        GameObject iconObj = Instantiate(moneyIconPrefab, mainCanvas.transform);

//        // --- Reset transform properly ---
//        RectTransform icon = iconObj.GetComponent<RectTransform>();
//        icon.localScale = Vector3.one;                 // ensure correct scaling
//        icon.anchorMin = new Vector2(0.5f, 0.5f);      // center anchor
//        icon.anchorMax = new Vector2(0.5f, 0.5f);
//        icon.anchoredPosition = Vector2.zero;          // center of canvas
//        icon.localRotation = Quaternion.identity;

//        // --- Optional: small offset upwards ---
//        // icon.anchoredPosition = new Vector2(0, 50);

//        // --- Text setup ---
//        TextMeshProUGUI tmp = iconObj.GetComponentInChildren<TextMeshProUGUI>();
//        if (tmp != null)
//            tmp.text = $"+${amount:F0}";

//        // --- Play DOTween animation (your prefab animation) ---
//        DOTweenAnimation tweenAnim = iconObj.GetComponent<DOTweenAnimation>();
//        if (tweenAnim != null)
//            tweenAnim.DORestart();

//        // --- Clean-up and update money ---
//        float animDuration = tweenAnim != null ? tweenAnim.duration : 1.5f;
//        Destroy(iconObj, animDuration + 0.1f);

//        // Delay money update slightly after animation
//        DOVirtual.DelayedCall(animDuration - 0.1f, () =>
//        {
//            totalMoney += amount;
//            PlayerPrefs.SetFloat(playerPrefsKey, totalMoney);
//            PlayerPrefs.Save();

//            if (mainMoneyText)
//                mainMoneyText.text = $"${totalMoney:F0}";
//        });
//    }


//}



///////////////////// Rewads panel 
///
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class MoneyAnimationController : MonoBehaviour
{
    [Header("References")]
    public Canvas mainCanvas;
    public GameObject moneyIconPrefab;       // Prefab with your own DOTween animation
    public TextMeshProUGUI mainMoneyText;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        UpdateMainText();
    }

    public void PlayMoneyAnimation(float amount, Vector3 worldStartPos)
    {
        if (!moneyIconPrefab || !mainCanvas) return;

        // --- Create icon ---
        GameObject iconObj = Instantiate(moneyIconPrefab, mainCanvas.transform);
        RectTransform icon = iconObj.GetComponent<RectTransform>();

        icon.localScale = Vector3.one;
        icon.anchorMin = icon.anchorMax = new Vector2(0.5f, 0.5f);
        icon.anchoredPosition = Vector2.zero;
        icon.localRotation = Quaternion.identity;

        // --- Text setup ---
        TextMeshProUGUI tmp = iconObj.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp) tmp.text = $"+${amount:F0}";

        // --- Play prefab DOTween animation ---
        DOTweenAnimation tweenAnim = iconObj.GetComponent<DOTweenAnimation>();
        if (tweenAnim != null)
            tweenAnim.DORestart();

        float animDuration = tweenAnim != null ? tweenAnim.duration : 1.5f;
        Destroy(iconObj, animDuration + 0.1f);

        // --- Update Money after animation ---
        DOVirtual.DelayedCall(animDuration - 0.1f, () =>
        {
            RewardManager.AddMoney((int)amount);
            UpdateMainText();
        });
    }

    private void UpdateMainText()
    {
        if (mainMoneyText)
            mainMoneyText.text = $"${RewardManager.TotalMoney:F0}";
    }
}
