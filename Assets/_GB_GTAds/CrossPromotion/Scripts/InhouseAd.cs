using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InhouseAd : MonoBehaviour
{
    float currentTimeScale = 0f;
    public float delay;
    public UnityEvent OnEnableEvents, DelayEvents;
    private void OnEnable()
    {
        OnEnableEvents?.Invoke();
        StartCoroutine(Delay());
        PauseApp(true);
        GoogleAdsManager.Instance.HideBanner();

        LevelPlayAdsManager.Instance.HideBanner();
    }
    private void OnDisable()
    {
        PauseApp(false);
        AdsManager.Instance.ShowBanner();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(delay);
        DelayEvents?.Invoke();
    }
    public void PauseApp(bool state)
    {
        AudioListener.pause = state;

        if (state)
        {
            currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = currentTimeScale;
        }

    }
}
