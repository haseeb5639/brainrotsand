using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements; // Unity Ads namespace
using System.Collections;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign a UI Text element in Inspector
    public float timeLeft = 30f;
    private Coroutine countdownCoroutine;
    public GameObject adsLoad;

    void Start()
    {
        StartCountdown();
    }

    void StartCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine); // Stop previous coroutine if running
        }
        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        timeLeft = 50f;

        while (timeLeft > 0)
        {
           // timerText.text = Mathf.Ceil(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;

            
            if(timeLeft < 4)
            {
                adsLoad.SetActive(true);
                timerText.text = timeLeft.ToString();
            }
        }



        /*timerText.text = "0";
        Debug.Log("Time left 0");*/
        ShowAd();
        adsLoad.SetActive(false);
    }

    void ShowAd()
    {
        HandleAdResult();
        AdsManager.Instance.ShowAds();
    }

    void HandleAdResult()
    {
        Debug.Log("Ad finished. Restarting timer...");
        StartCountdown();
    }
}
