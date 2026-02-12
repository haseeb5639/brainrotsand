
//using UnityEngine;
//using TMPro;
//using System.Collections.Generic;

//public class MoneyDisplay : MonoBehaviour
//{
//    private static readonly List<MoneyDisplay> allDisplays = new List<MoneyDisplay>();
//    [SerializeField] private TextMeshProUGUI moneyText;

//    void Awake()
//    {
//        // Auto-assign if left empty
//        if (moneyText == null)
//            moneyText = GetComponent<TextMeshProUGUI>();
//    }

//    void OnEnable()
//    {
//        if (!allDisplays.Contains(this))
//            allDisplays.Add(this);

//        UpdateText(); // ✅ Update instantly when enabled
//    }

//    void Start()
//    {
//        // ✅ Extra safety — ensures Controller scene updates properly
//        RewardManager.RefreshAll(); // <-- This line is correct


//    }

//    void OnDisable()
//    {
//        allDisplays.Remove(this);
//    }

//    public void UpdateText()
//    {
//        if (moneyText == null) return;
//        moneyText.text = $"${FormatNumber(RewardManager.TotalMoney)}";
//    }

//    public static void UpdateAllMoneyTexts()
//    {
//        foreach (var display in allDisplays)
//        {
//            if (display != null)
//                display.UpdateText();
//        }
//    }

//    private string FormatNumber(float value)
//    {
//        if (value >= 1_000_000_000)
//            return (value / 1_000_000_000f).ToString("0.#") + "B";
//        else if (value >= 1_000_000)
//            return (value / 1_000_000f).ToString("0.#") + "M";
//        else if (value >= 1_000)
//            return (value / 1_000f).ToString("0.#") + "K";
//        else
//            return value.ToString("0");
//    }


//}


//////
///


using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MoneyDisplay : MonoBehaviour
{
    private static readonly List<MoneyDisplay> allDisplays = new List<MoneyDisplay>();
    [SerializeField] private TextMeshProUGUI moneyText;

    void Awake()
    {
        if (moneyText == null)
            moneyText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (!allDisplays.Contains(this))
            allDisplays.Add(this);

        UpdateText(); // show immediately
    }

    void Start()
    {
        // small delayed refresh for controller scene startup
        Invoke(nameof(RefreshAfterDelay), 0.05f);
    }

    void OnDisable()
    {
        allDisplays.Remove(this);
    }

    private void RefreshAfterDelay()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (moneyText == null) return;
        moneyText.text = $"${FormatNumber(RewardManager.TotalMoney)}";
    }

    public static void UpdateAllMoneyTexts()
    {
        foreach (var display in allDisplays)
            if (display != null)
                display.UpdateText();
    }

    // 🔹 Public helper to refresh safely next frame
    public static void RefreshNextFrame()
    {
        var temp = new GameObject("MoneyRefreshTemp").AddComponent<MonoBehaviourTemp>();
        temp.StartCoroutine(temp.RefreshRoutine());
    }

    private string FormatNumber(float value)
    {
        if (value >= 1_000_000_000) return (value / 1_000_000_000f).ToString("0.#") + "B";
        else if (value >= 1_000_000) return (value / 1_000_000f).ToString("0.#") + "M";
        else if (value >= 1_000) return (value / 1_000f).ToString("0.#") + "K";
        else return value.ToString("0");
    }

    // internal helper MonoBehaviour to delay UI update
    private class MonoBehaviourTemp : MonoBehaviour
    {
        public System.Collections.IEnumerator RefreshRoutine()
        {
            yield return new WaitForEndOfFrame();
            MoneyDisplay.UpdateAllMoneyTexts();
            Destroy(gameObject);
        }
    }
}
