
using UnityEngine;
using UnityEngine.UI;

public class InHouseAppOpen : MonoBehaviour
{
    public RawImage AppIcon;
    public Text AppName;

    public Animator anim;
    void OnEnable()
    {

        AppName.text = Application.productName;
        // newTex = PlayerSettings.GetIcons(NamedBuildTarget.Unknown, IconKind.Application);

        AppIcon.texture = Resources.Load<Texture2D>("Icon");

        anim.Play("Start");

        NativeAdsManager.Instance.CanShowBanner = false;

        GoogleAdsManager.Instance.HideBanner();

        LevelPlayAdsManager.Instance.HideBanner();

    }

    private void OnDisable()
    {
        NativeAdsManager.Instance.CanShowBanner = true;
        AdsManager.Instance.ShowBanner();
    }


}
