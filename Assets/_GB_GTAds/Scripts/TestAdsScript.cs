using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestAdsScript : MonoBehaviour
{
    public GameObject NativeAdsController;
    public Image NativeButton;

    public TextMeshProUGUI debugText;


    private void Start()
    {
        debugText.text = "CanShowAppOpen: " + GoogleAdsManager.Instance.CanShowAppOpen.ToString();
    }

    public void ShowAds()
    {
        AdsManager.Instance.ShowAds();
    }

    public void ShowCollapsibleBanner()
    {
        AdsManager.Instance.ShowCollapsibleBanner();
    }


    public void ShowRewardedAds()
    {
        AdsManager.Instance.ShowRewardedAds(() =>
        {
            Debug.Log("Reward Granted");
        });
    }

    public void RemoveAds()
    {
        AdsManager.Instance.RemoveAds();
    }

    public void RevertRemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 0);
    }

    public void RemoveCP()
    {
        AdsManager.Instance.RemoveCrossPromo();
    }

    public void RevertRemoveCP()
    {
        PlayerPrefs.SetInt("RemoveCP", 0);
    }

    public void RemoveAllAds()
    {
        RemoveAds();
        RemoveCP();
    }

    public void RevertRemoveAllAds()
    {
        RevertRemoveAds();
        RevertRemoveCP();
    }

    public void NativeState()
    {
        if (NativeAdsController.activeInHierarchy)
        {
            NativeAdsController.SetActive(false);
            NativeButton.color = Color.red;
        }
        else if (!NativeAdsController.activeInHierarchy)
        {
            NativeAdsController.SetActive(true);
            NativeButton.color = Color.green;
        }
    }
}
