using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeAdsManager : MonoBehaviour
{
    #region Singleton
    private static NativeAdsManager _Instance;
    public static NativeAdsManager Instance
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
    public bool CanShowBanner = true;
    public bool CanShowNative = true;
}
