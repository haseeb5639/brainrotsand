#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppTracking : MonoBehaviour
{
    #region Singleton
    private static AppTracking _Instance;
    public static AppTracking Instance
    {
        get
        {

            return _Instance;
        }
    }

    private void Awake()
    {
        if (!_Instance) _Instance = this;

    }
    #endregion

    public float DelayInTracking = 4f;
    public bool Manual = true;

    IEnumerator Start()
    {

        if (!Manual)
        {
            yield return new WaitForSeconds(DelayInTracking);

            ShowTracking();

        }

    }




    public void ShowTracking()
    {

#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == 
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }

#endif
    }





}
