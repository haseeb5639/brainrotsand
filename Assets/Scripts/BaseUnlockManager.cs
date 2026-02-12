
////////// dummy 
///////
////using UnityEngine;
////using TMPro;
////using System.Collections.Generic;

////public class BaseUnlockManager : MonoBehaviour
////{
////    public static BaseUnlockManager Instance;

////    [Header("UI References")]
////    public GameObject unlockPanel;
////    public TextMeshProUGUI messageText;
////    public GameObject watchButton;
////    public GameObject buyButton;

////    private BaseManager baseManager;
////    private BaseManager.BaseData currentBase;

////    // Prices stored
////    private Dictionary<string, float> baseUnlockCosts = new Dictionary<string, float>();

////    private void Awake()
////    {
////        Instance = this;
////        if (unlockPanel != null)
////            unlockPanel.SetActive(false);
////    }

////    private void Start()
////    {
////        baseManager = BaseManager.instance;
////        AssignBaseCosts();
////    }

////    private void AssignBaseCosts()
////    {
////        if (baseManager == null || baseManager.bases.Count == 0)
////        {
////            Debug.LogWarning("⚠️ No bases found!");
////            return;
////        }

////        for (int i = 0; i < baseManager.bases.Count; i++)
////        {
////            var b = baseManager.bases[i];
////            if (b == null) continue;

////            // PRICE SYSTEM: base1=FREE, next bases +10
////            float unlockCost = (i == 0) ? 0f : i * 10f;

////            baseUnlockCosts[b.baseName] = unlockCost;

////            // DO NOT show "LOCKED" on base 1  
////            if (b.buttonText != null)
////            {
////                if (i == 0)
////                {
////                    // keep original text
////                    b.buttonText.text = b.buttonText.text;
////                }
////                else
////                {
////                    b.buttonText.text = b.isUnlocked ? "$0" : $"{unlockCost}";
////                }
////            }
////        }
////    }

////    public void ShowUnlockPanel(BaseManager.BaseData baseData)
////    {
////        if (baseData == null || baseData.isUnlocked) return;

////        currentBase = baseData;
////        unlockPanel.SetActive(true);

////        float cost = baseUnlockCosts.ContainsKey(baseData.baseName)
////            ? baseUnlockCosts[baseData.baseName]
////            : 0f;

////        messageText.text = $"Watch Ad or Pay ${cost} to Unlock {baseData.baseName}";
////    }

////    public void HideUnlockPanel()
////    {
////        unlockPanel.SetActive(false);
////        currentBase = null;
////    }

////    public void TryBuyBase()
////    {
////        if (currentBase == null) return;

////        float cost = baseUnlockCosts[currentBase.baseName];

////        if (RewardManager.TotalMoney >= cost)
////        {
////            RewardManager.SpendMoney(cost);
////            UnlockBase(currentBase);
////        }
////        else
////        {
////            messageText.text = "💸 Not enough money!";
////        }
////    }

////    public void WatchAdToUnlock()
////    {
////        if (currentBase == null) return;

////        messageText.text = "🎬 Watching Ad...";
////        Invoke(nameof(OnAdWatchedComplete), 2f);
////    }

////    private void OnAdWatchedComplete()
////    {
////        if (currentBase != null)
////            UnlockBase(currentBase);
////    }

////    private void UnlockBase(BaseManager.BaseData baseData)
////    {
////        baseData.isUnlocked = true;

////        PlayerPrefs.SetInt($"BaseUnlocked_{baseData.baseName}", 1);
////        PlayerPrefs.Save();

////        if (baseData.buttonText != null)
////            baseData.buttonText.text = "$0";

////        unlockPanel.SetActive(false);

////        AudioManager.PlayDropSound();
////        MoneyDisplay.UpdateAllMoneyTexts();

////        BrainrotUIManager.instance.ShowMessage($"{baseData.baseName} Unlocked!");
////    }
////}




///////// today dummy 
/////
//using UnityEngine;
//using TMPro;
//using System.Collections.Generic;

//public class BaseUnlockManager : MonoBehaviour
//{
//    public static BaseUnlockManager Instance;

//    [Header("UI References")]
//    public GameObject unlockPanel;
//    public TextMeshProUGUI messageText;
//    public GameObject watchButton;
//    public GameObject buyButton;

//    private BaseManager baseManager;
//    private BaseManager.BaseData currentBase;

//    // Prices stored
//    private Dictionary<string, float> baseUnlockCosts = new Dictionary<string, float>();

//    // NEW: Store original button text so we can restore it after unlocking
//    private Dictionary<string, string> originalButtonTexts = new Dictionary<string, string>();

//    private void Awake()
//    {
//        Instance = this;
//        if (unlockPanel != null)
//            unlockPanel.SetActive(false);
//    }

//    private void Start()
//    {
//        baseManager = BaseManager.instance;
//        AssignBaseCosts();
//    }

//    private void AssignBaseCosts()
//    {
//        if (baseManager == null || baseManager.bases.Count == 0)
//        {
//            Debug.LogWarning("⚠️ No bases found!");
//            return;
//        }

//        for (int i = 0; i < baseManager.bases.Count; i++)
//        {
//            var b = baseManager.bases[i];
//            if (b == null) continue;

//            // Store original text one time
//            if (b.buttonText != null && !originalButtonTexts.ContainsKey(b.baseName))
//            {
//                originalButtonTexts.Add(b.baseName, b.buttonText.text);
//            }

//            // PRICE SYSTEM: base1 = free, others price = i * 10
//            float unlockCost = (i == 0) ? 0f : i * 10f;
//            baseUnlockCosts[b.baseName] = unlockCost;

//            // UI LOGIC  
//            if (b.buttonText != null)
//            {
//                // Base 1 → Keep original text always
//                if (i == 0)
//                {
//                    b.buttonText.text = originalButtonTexts[b.baseName];
//                }
//                else
//                {
//                    // Other bases:
//                    if (b.isUnlocked)
//                        b.buttonText.text = originalButtonTexts[b.baseName]; // Show original text when unlocked
//                    else
//                        b.buttonText.text = "Locked"; // Show Locked when locked
//                }
//            }
//        }
//    }

//    public void ShowUnlockPanel(BaseManager.BaseData baseData)
//    {
//        if (baseData == null || baseData.isUnlocked) return;

//        currentBase = baseData;
//        unlockPanel.SetActive(true);

//        float cost = baseUnlockCosts.ContainsKey(baseData.baseName)
//            ? baseUnlockCosts[baseData.baseName]
//            : 0f;

//        messageText.text = $"Watch Ad or Pay ${cost} to Unlock {baseData.baseName}";
//    }

//    public void HideUnlockPanel()
//    {
//        unlockPanel.SetActive(false);
//        currentBase = null;
//    }

//    public void TryBuyBase()
//    {
//        if (currentBase == null) return;

//        float cost = baseUnlockCosts[currentBase.baseName];

//        if (RewardManager.TotalMoney >= cost)
//        {
//            RewardManager.SpendMoney(cost);
//            UnlockBase(currentBase);
//        }
//        else
//        {
//            messageText.text = "💸 Not enough money!";
//        }
//    }

//    public void WatchAdToUnlock()
//    {
//        if (currentBase == null) return;

//        messageText.text = "🎬 Watching Ad...";
//        AdsManager.Instance.ShowRewardedAds(() =>
//        {
//            Invoke(nameof(OnAdWatchedComplete), 2f);
//        });
//    }

//    private void OnAdWatchedComplete()
//    {
//        if (currentBase != null)
//            UnlockBase(currentBase);
//    }

//    private void UnlockBase(BaseManager.BaseData baseData)
//    {
//        baseData.isUnlocked = true;

//        PlayerPrefs.SetInt($"BaseUnlocked_{baseData.baseName}", 1);
//        PlayerPrefs.Save();

//        // Set original button text after unlock
//        if (baseData.buttonText != null && originalButtonTexts.ContainsKey(baseData.baseName))
//        {
//            baseData.buttonText.text = originalButtonTexts[baseData.baseName];
//        }

//        unlockPanel.SetActive(false);

//        AudioManager.PlayDropSound();
//        MoneyDisplay.UpdateAllMoneyTexts();

//        BrainrotUIManager.instance.ShowMessage($"{baseData.baseName} Unlocked!");
//    }
//}

//final

//using UnityEngine;
//using TMPro;
//using System.Collections.Generic;

//public class BaseUnlockManager : MonoBehaviour
//{
//    public static BaseUnlockManager Instance;

//    [Header("UI References")]
//    public GameObject unlockPanel;
//    public TextMeshProUGUI messageText;
//    public GameObject watchButton;
//    public GameObject buyButton;

//    private BaseManager baseManager;
//    private BaseManager.BaseData currentBase;

//    // Store costs
//    private Dictionary<string, float> baseUnlockCosts = new Dictionary<string, float>();

//    // Store original button text
//    private Dictionary<string, string> originalButtonTexts = new Dictionary<string, string>();

//    private void Awake()
//    {
//        Instance = this;
//        if (unlockPanel != null)
//            unlockPanel.SetActive(false);
//    }

//    private void Start()
//    {
//        baseManager = BaseManager.instance;
//        AssignBaseCosts();
//    }

//    private void AssignBaseCosts()
//    {
//        if (baseManager == null || baseManager.bases.Count == 0)
//        {
//            Debug.LogWarning("⚠️ No bases found!");
//            return;
//        }

//        // EXACT PRICE LIST YOU REQUESTED
//        float[] prices = new float[]
//        {
//            0f,        // Base 1
//            10000f,    // Base 2
//            50000f,    // Base 3
//            100000f,   // Base 4
//            500000f,   // Base 5
//            1000000f,  // Base 6
//            2000000f,  // Base 7
//            5000000f,  // Base 8
//            7000000f,  // Base 9
//            8000000f   // Base 10
//        };

//        for (int i = 0; i < baseManager.bases.Count; i++)
//        {
//            var b = baseManager.bases[i];
//            if (b == null) continue;

//            // Save original UI text
//            if (b.buttonText != null && !originalButtonTexts.ContainsKey(b.baseName))
//                originalButtonTexts.Add(b.baseName, b.buttonText.text);

//            // Assign price from price list
//            float unlockCost = (i < prices.Length) ? prices[i] : 0f;

//            baseUnlockCosts[b.baseName] = unlockCost;

//            // Update UI button text
//            if (b.buttonText != null)
//            {
//                if (b.isUnlocked)
//                {
//                    b.buttonText.text = originalButtonTexts[b.baseName];
//                }
//                else
//                {
//                    if (i == 0)
//                        b.buttonText.text = originalButtonTexts[b.baseName];
//                    else
//                        b.buttonText.text = "Locked";
//                }
//            }
//        }
//    }

//    public void ShowUnlockPanel(BaseManager.BaseData baseData)
//    {
//        if (baseData == null || baseData.isUnlocked) return;

//        currentBase = baseData;
//        unlockPanel.SetActive(true);

//        float cost = baseUnlockCosts[baseData.baseName];

//        messageText.text = $"Watch Ad or Pay ${FormatMoney(cost)} to Unlock {baseData.baseName}";
//    }

//    public void HideUnlockPanel()
//    {
//        unlockPanel.SetActive(false);
//        currentBase = null;
//    }

//    public void TryBuyBase()
//    {
//        if (currentBase == null) return;

//        float cost = baseUnlockCosts[currentBase.baseName];

//        if (RewardManager.TotalMoney >= cost)
//        {
//            RewardManager.SpendMoney(cost);
//            UnlockBase(currentBase);
//        }
//        else
//        {
//            messageText.text = "💸 Not enough money!";
//        }
//    }

//    public void WatchAdToUnlock()
//    {
//        if (currentBase == null) return;

//        messageText.text = "🎬 Watching Ad...";
//        AdsManager.Instance.ShowRewardedAds(() =>
//        {
//            Invoke(nameof(OnAdWatchedComplete), 2f);
//        });
//    }

//    private void OnAdWatchedComplete()
//    {
//        if (currentBase != null)
//            UnlockBase(currentBase);
//    }

//    private void UnlockBase(BaseManager.BaseData baseData)
//    {
//        baseData.isUnlocked = true;

//        PlayerPrefs.SetInt($"BaseUnlocked_{baseData.baseName}", 1);
//        PlayerPrefs.Save();

//        // Restore original button text
//        if (baseData.buttonText != null && originalButtonTexts.ContainsKey(baseData.baseName))
//            baseData.buttonText.text = originalButtonTexts[baseData.baseName];

//        unlockPanel.SetActive(false);

//        AudioManager.PlayDropSound();
//        MoneyDisplay.UpdateAllMoneyTexts();

//        BrainrotUIManager.instance.ShowMessage($"{baseData.baseName} Unlocked!");
//    }

//    // Format money to K/M/B
//    private string FormatMoney(float value)
//    {
//        if (value >= 1_000_000_000)
//            return (value / 1_000_000_000f).ToString("0.#") + "B";

//        if (value >= 1_000_000)
//            return (value / 1_000_000f).ToString("0.#") + "M";

//        if (value >= 1000)
//            return (value / 1000f).ToString("0.#") + "K";

//        return value.ToString("0");
//    }
//}


///////////// with lock icon 
///

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BaseUnlockManager : MonoBehaviour
{
    public static BaseUnlockManager Instance;

    [Header("UI References")]
    public GameObject unlockPanel;
    public TextMeshProUGUI messageText;
    public GameObject watchButton;
    public GameObject buyButton;

    private BaseManager baseManager;
    private BaseManager.BaseData currentBase;

    // Store costs
    private Dictionary<string, float> baseUnlockCosts = new Dictionary<string, float>();

    // Store original button text
    private Dictionary<string, string> originalButtonTexts = new Dictionary<string, string>();

    private void Awake()
    {
        Instance = this;
        if (unlockPanel != null)
            unlockPanel.SetActive(false);
    }

    private void Start()
    {
        baseManager = BaseManager.instance;
        AssignBaseCosts();
        RestoreLockIcons();
    }

    private void AssignBaseCosts()
    {
        if (baseManager == null || baseManager.bases.Count == 0)
        {
            Debug.LogWarning("⚠️ No bases found!");
            return;
        }

        // EXACT PRICE LIST
        float[] prices = new float[]
        {
            0f,        // Base 1
            10000f,    // Base 2
            50000f,    // Base 3
            100000f,   // Base 4
            500000f,   // Base 5
            1000000f,  // Base 6
            2000000f,  // Base 7
            5000000f,  // Base 8
            7000000f,  // Base 9
            8000000f   // Base 10
        };

        for (int i = 0; i < baseManager.bases.Count; i++)
        {
            var b = baseManager.bases[i];
            if (b == null) continue;

            // Save original UI label
            if (b.buttonText != null && !originalButtonTexts.ContainsKey(b.baseName))
                originalButtonTexts.Add(b.baseName, b.buttonText.text);

            // Assign price
            float unlockCost = (i < prices.Length) ? prices[i] : 0f;
            baseUnlockCosts[b.baseName] = unlockCost;

            // Update button text
            if (b.buttonText != null)
            {
                if (b.isUnlocked)
                {
                    b.buttonText.text = originalButtonTexts[b.baseName];
                }
                else
                {
                    if (i == 0)
                        b.buttonText.text = originalButtonTexts[b.baseName];
                    else
                        b.buttonText.text = "Locked";
                }
            }
        }
    }

    private void RestoreLockIcons()
    {
        foreach (var b in baseManager.bases)
        {
            if (b == null) continue;

            if (b.lockIcon != null)
            {
                // Hide icon if unlocked
                b.lockIcon.SetActive(!b.isUnlocked);
            }
        }
    }

    // ------------------------------
    // SHOW PANEL
    // ------------------------------

    public void ShowUnlockPanel(BaseManager.BaseData baseData)
    {
        if (baseData == null || baseData.isUnlocked) return;

        currentBase = baseData;
        unlockPanel.SetActive(true);

        float cost = baseUnlockCosts[baseData.baseName];
        messageText.text = $"Watch Ad or Pay ${FormatMoney(cost)} to Unlock {baseData.baseName}";
    }

    public void HideUnlockPanel()
    {
        unlockPanel.SetActive(false);
        currentBase = null;
    }

    // ------------------------------
    // BUY WITH MONEY
    // ------------------------------

    public void TryBuyBase()
    {
        if (currentBase == null) return;

        float cost = baseUnlockCosts[currentBase.baseName];

        if (RewardManager.TotalMoney >= cost)
        {
            RewardManager.SpendMoney(cost);
            UnlockBase(currentBase);
        }
        else
        {
            messageText.text = " Not enough money!";
        }
    }

    // ------------------------------
    // WATCH AD
    // ------------------------------

    public void WatchAdToUnlock()
    {
        if (currentBase == null) return;

        messageText.text = " Watching Ad...";
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            Invoke(nameof(OnAdWatchedComplete), 2f);
        });
    }

    private void OnAdWatchedComplete()
    {
        if (currentBase != null)
            UnlockBase(currentBase);
    }

    // ------------------------------
    // UNLOCK BASE
    // ------------------------------

    //private void UnlockBase(BaseManager.BaseData baseData)
    //{
    //    baseData.isUnlocked = true;

    //    // Save unlock
    //    PlayerPrefs.SetInt($"BaseUnlocked_{baseData.baseName}", 1);
    //    PlayerPrefs.Save();

    //    // 🔥 HIDE LOCK ICON
    //    if (baseData.lockIcon != null)
    //        baseData.lockIcon.SetActive(false);

    //    // Restore UI label
    //    if (baseData.buttonText != null && originalButtonTexts.ContainsKey(baseData.baseName))
    //        baseData.buttonText.text = originalButtonTexts[baseData.baseName];

    //    unlockPanel.SetActive(false);

    //    // FX and UI updates
    //    AudioManager.PlayDropSound();
    //    MoneyDisplay.UpdateAllMoneyTexts();

    //    BrainrotUIManager.instance.ShowMessage($"{baseData.baseName} Unlocked!");
    //}

    private void UnlockBase(BaseManager.BaseData baseData)
    {
        baseData.isUnlocked = true;

        // Save to PlayerPrefs
        PlayerPrefs.SetInt($"BaseUnlocked_{baseData.baseName}", 1);
        PlayerPrefs.Save();
        // baseData.lockIcon.SetActive(false);

        // 🔥 HIDE LOCK ICON
          if (baseData.lockIcon != null)
           {
               baseData.lockIcon.SetActive(false);
               Debug.Log("HIDING LOCK ICON: " + baseData.baseName);
           }
           else
           {
               Debug.Log("⚠ NO LOCK ICON ASSIGNED: " + baseData.baseName);
           }

        // Restore button text
        if (baseData.buttonText != null && originalButtonTexts.ContainsKey(baseData.baseName))
            baseData.buttonText.text = originalButtonTexts[baseData.baseName];

        unlockPanel.SetActive(false);

        AudioManager.PlayDropSound();
        MoneyDisplay.UpdateAllMoneyTexts();

        BrainrotUIManager.instance.ShowMessage($"{baseData.baseName} Unlocked!");
        LabAnalytics.Instance.LogEvent("UNLOCKBASE");
    }

    // ------------------------------
    // FORMAT MONEY
    // ------------------------------

    private string FormatMoney(float value)
    {
        if (value >= 1_000_000_000)
            return (value / 1_000_000_000f).ToString("0.#") + "B";

        if (value >= 1_000_000)
            return (value / 1_000_000f).ToString("0.#") + "M";

        if (value >= 1000)
            return (value / 1000f).ToString("0.#") + "K";

        return value.ToString("0");
    }
}
