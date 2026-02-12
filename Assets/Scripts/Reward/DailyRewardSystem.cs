////using UnityEngine;
////using UnityEngine.UI;
////using TMPro;
////using System;

////public class DailyRewardSystem : MonoBehaviour
////{
////    [Header("UI Elements")]
////    public Button[] rewardButtons;
////    public TextMeshProUGUI[] rewardTexts;
////    public TextMeshProUGUI timerText;
////    public Button claimButton;
////    public GameObject exclamation;
////    [Header("Reward Amounts")]
////    public int[] rewardAmounts;

////    [Header("Reward Settings")]
////    public int rewardCooldownHours = 24;

////    private DateTime lastClaimDateTime;
////    private int currentDay;
////    private TimeSpan remainingTime;
////    [SerializeField] private GameObject panel;
////    [SerializeField] private GameObject panel2;


////    private void Start()
////    {
////        // Load data
////        string savedTime = PlayerPrefs.GetString("LastClaimDateTime", "");
////        currentDay = PlayerPrefs.GetInt("CurrentDays", 0);

////        if (savedTime == "")
////        {
////            lastClaimDateTime = DateTime.MinValue;
////        }
////        else
////        {
////            lastClaimDateTime = DateTime.Parse(savedTime);
////        }

////        claimButton.onClick.RemoveAllListeners();
////        claimButton.onClick.AddListener(ClaimReward);

////        UpdateTimer();
////        UpdateUI();

////    }

////    private void Update()
////    {
////        UpdateTimer();
////        UpdateUI();
////    }

////    //private void UpdateTimer()
////    //{
////    //    if (lastClaimDateTime == DateTime.MinValue)
////    //    {
////    //        claimButton.interactable = true;
////    //        exclamation.SetActive(true);
////    //        timerText.text = "Claim your reward!";
////    //        return;
////    //    }

////    //    DateTime nextClaimTime = lastClaimDateTime.AddHours(rewardCooldownHours);
////    //    remainingTime = nextClaimTime - DateTime.Now;

////    //    if (remainingTime.TotalSeconds <= 0)
////    //    {
////    //        claimButton.interactable = true;
////    //        timerText.text = "Claim your reward!";
////    //        exclamation.SetActive(true);
////    //    }
////    //    else
////    //    {
////    //        claimButton.interactable = false;
////    //        exclamation.SetActive(false);
////    //        timerText.text = $"Next Reward In: {remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
////    //    }
////    //}
////    private void UpdateTimer()
////    {
////        if (lastClaimDateTime == DateTime.MinValue)
////        {
////            claimButton.interactable = true;
////            exclamation.SetActive(true);
////            timerText.text = "Claim your reward!";
////            return;
////        }

////        // ⬇️  HERE — Converted to 10 seconds instead of 24 hours
////        DateTime nextClaimTime = lastClaimDateTime.AddSeconds(10);

////        remainingTime = nextClaimTime - DateTime.Now;

////        if (remainingTime.TotalSeconds <= 0)
////        {
////            claimButton.interactable = true;
////            timerText.text = "Claim your reward!";
////            exclamation.SetActive(true);
////        }
////        else
////        {
////            claimButton.interactable = false;
////            exclamation.SetActive(false);
////            timerText.text = $"Next Reward In: {remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
////        }
////    }


////    private void UpdateUI()
////    {
////        for (int i = 0; i < rewardButtons.Length; i++)
////        {
////            if (i < currentDay)
////            {
////                rewardButtons[i].interactable = false;
////                rewardTexts[i].text = "CLAIMED";
////            }
////            else if (i == currentDay && claimButton.interactable)
////            {
////                rewardButtons[i].interactable = true;
////                rewardTexts[i].text = $"Claim {rewardAmounts[i]}";
////            }
////            else
////            {
////                rewardButtons[i].interactable = false;
////                rewardTexts[i].text = $"Day {i + 1}";
////            }
////        }
////    }

////    public void ClaimReward()
////    {
////        if (currentDay >= rewardAmounts.Length) return;

////        // Give reward
////        float money = PlayerPrefs.GetFloat("TotalMoney", 0f);
////        money += rewardAmounts[currentDay];
////        PlayerPrefs.SetFloat("TotalMoney", money);

////        // Save time using system clock
////        lastClaimDateTime = DateTime.Now;
////        PlayerPrefs.SetString("LastClaimDateTime", lastClaimDateTime.ToString());

////        currentDay++;
////        PlayerPrefs.SetInt("CurrentDays", currentDay);

////        PlayerPrefs.Save();

////        claimButton.interactable = false;
////        exclamation.SetActive(false);
////        UpdateUI();
////    }
////    // ----------------------------------------------------------
////    // CLOSE PANEL
////    // ----------------------------------------------------------
////    public void ClosePanel()
////    {
////        panel2.SetActive(false);
////        panel.SetActive(true);
////    }

////}






//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System;

//public class DailyRewardSystem : MonoBehaviour
//{
//    [Header("UI Elements")]
//    public Button[] rewardButtons;
//    public TextMeshProUGUI[] rewardTexts;
//    public TextMeshProUGUI timerText;
//    public Button claimButton;
//    public GameObject exclamation;

//    [Header("Reward Amounts")]
//    public int[] rewardAmounts;

//    [Header("Reward Settings")]
//    public int testCooldownSeconds = 10;   // ⬅ TEST MODE (10 seconds loop)
//    [Header("Money UI")]
//    [SerializeField] private TextMeshProUGUI moneyUIText;


//    private DateTime lastClaimDateTime;
//    private int currentDay;
//    private TimeSpan remainingTime;

//    [SerializeField] private GameObject panel;
//    [SerializeField] private GameObject panel2;

//    private void Start()
//    {
//        string savedTime = PlayerPrefs.GetString("LastClaimDateTime", "");
//        currentDay = PlayerPrefs.GetInt("CurrentDays", 0);

//        if (savedTime == "")
//            lastClaimDateTime = DateTime.MinValue;
//        else
//            lastClaimDateTime = DateTime.Parse(savedTime);

//        claimButton.onClick.RemoveAllListeners();
//        claimButton.onClick.AddListener(ClaimReward);

//        UpdateTimer();
//        UpdateUI();
//    }

//    private void Update()
//    {
//        UpdateTimer();
//        UpdateUI();
//    }

//    // ====================================================
//    // TIMER SYSTEM (10 SECONDS FOR TESTING)
//    // ====================================================
//    private void UpdateTimer()
//    {
//        if (lastClaimDateTime == DateTime.MinValue)
//        {
//            claimButton.interactable = true;
//            exclamation.SetActive(true);
//            timerText.text = "Claim your reward!";
//            return;
//        }

//        // 🔁 Test timer → 10 seconds instead of 24 hours
//        DateTime nextClaimTime = lastClaimDateTime.AddSeconds(testCooldownSeconds);

//        remainingTime = nextClaimTime - DateTime.Now;

//        if (remainingTime.TotalSeconds <= 0)
//        {
//            claimButton.interactable = true;
//            exclamation.SetActive(true);
//            timerText.text = "Claim your reward!";
//        }
//        else
//        {
//            claimButton.interactable = false;
//            exclamation.SetActive(false);
//            timerText.text =
//                $"Next Reward In: {remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
//        }
//    }

//    // ====================================================
//    // UI UPDATE SYSTEM
//    // ====================================================
//    private void UpdateUI()
//    {
//        for (int i = 0; i < rewardButtons.Length; i++)
//        {
//            // Claimed
//            if (i < currentDay)
//            {
//                rewardButtons[i].interactable = false;
//                rewardTexts[i].text = "CLAIMED";
//            }
//            // Current day && claim available
//            else if (i == currentDay && claimButton.interactable)
//            {
//                rewardButtons[i].interactable = true;
//                rewardTexts[i].text = "Claim " + rewardAmounts[i];
//            }
//            // Locked future days
//            else
//            {
//                rewardButtons[i].interactable = false;
//                rewardTexts[i].text = "Day " + (i + 1);
//            }
//        }
//    }

//    // ====================================================
//    // CLAIM REWARD + AUTO RESET DAY 7 → DAY 1
//    // ====================================================
//    public void ClaimReward()
//    {
//        if (currentDay >= rewardAmounts.Length) return;

//        // Add money
//        float money = PlayerPrefs.GetFloat("TotalMoney", 0f);
//        money += rewardAmounts[currentDay];
//        PlayerPrefs.SetFloat("TotalMoney", money);

//        // Save claim time
//        lastClaimDateTime = DateTime.Now;
//        PlayerPrefs.SetString("LastClaimDateTime", lastClaimDateTime.ToString());

//        // Move to next day
//        currentDay++;

//        // 🔁 LOOP SYSTEM: If Day 7 complete → Reset to Day 1
//        if (currentDay >= rewardAmounts.Length)
//        {
//            currentDay = 0;   // ← RESET DAY
//        }

//        PlayerPrefs.SetInt("CurrentDays", currentDay);
//        PlayerPrefs.Save();



//        if (moneyUIText != null)
//            moneyUIText.text = money.ToString();
//        MoneyDisplay.UpdateAllMoneyTexts();

//        claimButton.interactable = false;
//        exclamation.SetActive(false);

//        UpdateUI();
//    }

//    // ====================================================
//    // CLOSE PANEL SYSTEM
//    // ====================================================
//    public void ClosePanel()
//    {
//        panel2.SetActive(false);
//        panel.SetActive(true);
//    }




//}




////////////////////// 24 hours 



using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyRewardSystem: MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] rewardButtons;
    public TextMeshProUGUI[] rewardTexts;
    public TextMeshProUGUI timerText;
    public Button claimButton;
    public GameObject exclamation;

    [Header("Reward Amounts")]
    public int[] rewardAmounts;

    [Header("Reward Settings")]
    public int rewardCooldownHours = 24;   // ⬅ 24 HOURS COOLDOWN

    [Header("Money UI")]
    [SerializeField] private TextMeshProUGUI moneyUIText;

    private DateTime lastClaimDateTime;
    private int currentDay;
    private TimeSpan remainingTime;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject panel2;

    private void Start()
    {
        string savedTime = PlayerPrefs.GetString("LastClaimDateTime_24H", "");
        currentDay = PlayerPrefs.GetInt("CurrentDays_24H", 0);

        if (savedTime == "")
            lastClaimDateTime = DateTime.MinValue;
        else
            lastClaimDateTime = DateTime.Parse(savedTime);

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimReward);

        UpdateTimer();
        UpdateUI();
    }

    private void Update()
    {
        UpdateTimer();
        UpdateUI();
    }

    // ====================================================
    // TIMER SYSTEM (24 HOURS PRODUCTION)
    // ====================================================
    private void UpdateTimer()
    {
        if (lastClaimDateTime == DateTime.MinValue)
        {
            claimButton.interactable = true;
            exclamation.SetActive(true);
            timerText.text = "Claim your reward!";
            return;
        }

        DateTime nextClaimTime = lastClaimDateTime.AddHours(rewardCooldownHours);
        remainingTime = nextClaimTime - DateTime.Now;

        if (remainingTime.TotalSeconds <= 0)
        {
            claimButton.interactable = true;
            exclamation.SetActive(true);
            timerText.text = "Claim your reward!";
        }
        else
        {
            claimButton.interactable = false;
            exclamation.SetActive(false);
            timerText.text =
                $"Next Reward In: {remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
        }
    }

    // ====================================================
    // UI UPDATE SYSTEM
    // ====================================================
    private void UpdateUI()
    {
        for (int i = 0; i < rewardButtons.Length; i++)
        {
            // Claimed
            if (i < currentDay)
            {
                rewardButtons[i].interactable = false;
                rewardTexts[i].text = "CLAIMED";
            }
            // Current day && claim available
            else if (i == currentDay && claimButton.interactable)
            {
                rewardButtons[i].interactable = true;
                rewardTexts[i].text = "Claim " + rewardAmounts[i];
            }
            // Locked future days
            else
            {
                rewardButtons[i].interactable = false;
                rewardTexts[i].text = "Day " + (i + 1);
            }
        }
    }

    // ====================================================
    // CLAIM REWARD + AUTO RESET DAY 7 → DAY 1
    // ====================================================
    public void ClaimReward()
    {
        if (currentDay >= rewardAmounts.Length) return;

        // Add money
        float money = PlayerPrefs.GetFloat("TotalMoney", 0f);
        money += rewardAmounts[currentDay];
        PlayerPrefs.SetFloat("TotalMoney", money);

        // Save claim time
        lastClaimDateTime = DateTime.Now;
        PlayerPrefs.SetString("LastClaimDateTime_24H", lastClaimDateTime.ToString());

        // Move to next day
        currentDay++;

        // Reset after final day
        if (currentDay >= rewardAmounts.Length)
        {
            currentDay = 0;
        }

        PlayerPrefs.SetInt("CurrentDays_24H", currentDay);
        PlayerPrefs.Save();

        if (moneyUIText != null)
            moneyUIText.text = money.ToString();

        MoneyDisplay.UpdateAllMoneyTexts();

        claimButton.interactable = false;
        exclamation.SetActive(false);

        UpdateUI();
    }

    // ====================================================
    // CLOSE PANEL SYSTEM
    // ====================================================
    public void ClosePanel()
    {
        panel2.SetActive(false);
        panel.SetActive(true);
    }
}
