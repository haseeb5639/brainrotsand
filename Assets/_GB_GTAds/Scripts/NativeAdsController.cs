
using UnityEngine;

public class NativeAdsController : MonoBehaviour
{

    void OnEnable()
    {
        NativeAdsManager.Instance.CanShowBanner = false;
        NativeAdsManager.Instance.CanShowNative = true;
        GoogleAdsManager.Instance.HideBanner();
        LevelPlayAdsManager.Instance.HideBanner();
        AdsManager.Instance.ShowNativeAd();
    }

    void OnDisable()
    {
        NativeAdsManager.Instance.CanShowBanner = true;
        NativeAdsManager.Instance.CanShowNative = false;
        AdsManager.Instance.ShowBanner();
        GoogleAdsManager.Instance.HideNativeBanner();
        MaxAdsManager.Instance.HideNativeBanner();


    }
}
