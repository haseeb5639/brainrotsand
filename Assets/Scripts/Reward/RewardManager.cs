




using UnityEngine;

public static class RewardManager
{
    private const string MONEY_KEY = "TotalMoney";
    private const string LAST_DAY_KEY = "LastRewardDay";
    private const string LAST_DATE_KEY = "LastRewardDate";

    public static float TotalMoney
    {
        get => PlayerPrefs.GetFloat(MONEY_KEY, 0f);
        private set
        {
            PlayerPrefs.SetFloat(MONEY_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static int LastRewardDay
    {
        get => PlayerPrefs.GetInt(LAST_DAY_KEY, 0);
        private set { PlayerPrefs.SetInt(LAST_DAY_KEY, value); PlayerPrefs.Save(); }
    }

    public static string LastRewardDate
    {
        get => PlayerPrefs.GetString(LAST_DATE_KEY, "");
        private set { PlayerPrefs.SetString(LAST_DATE_KEY, value); PlayerPrefs.Save(); }
    }

    // 💰 Add Money (float-safe)
    public static void AddMoney(float amount)
    {
        TotalMoney += amount;
        PlayerPrefs.Save();

        // slight delay to ensure PlayerPrefs commit before UI update
        MoneyDisplay.RefreshNextFrame();
    }

    public static bool SpendMoney(float amount)
    {
        if (TotalMoney >= amount)
        {
            TotalMoney -= amount;
            PlayerPrefs.Save();
            MoneyDisplay.RefreshNextFrame();
            return true;
        }
        return false;
    }

    public static void RefreshAll()
    {
        MoneyDisplay.UpdateAllMoneyTexts();
    }
}
