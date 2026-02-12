using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public enum InHouse
{
    Banner,
    Icon,
    Native,
    InhouseAd,
    AppOpen
}

public enum InHouseImageType
{
    Icon,
    Render
}
public class InHouseManager : MonoBehaviour
{
    #region Public vars

    public bool ShowAdOnStart = false;
    public float RefreshTime = 5f;
    public string DescriptorUrl;
    public GameObject PromoWindow;
    public Button PlayBtn;
    public Button OkBtn;
    public Image PromoImage;
    public Text AppName, AppDescription;
    [SerializeField]
    public List<AppItem> Apps;
    public InHouse inHouse;
    public InHouseImageType ImageType;
    public bool CanShowOverlay = false;
    public bool UseOldFile = false;

    public bool Debugging = false;

    #endregion

    #region Constants
    private const string KEY_VERSION = "Version";
    private const string KEY_APPLIST = "AppsList";
    private const string KEY_IOS = "-iOS";
    private const string KEY_ANDROID = "-Android";
    private string parsedjson = "";
    private Coroutine Coroutine;
    private CrossPromoDescriptor descriptor = null;

    #endregion

    #region Nested structs
    [System.Serializable]
    public class AppItem
    {
        public string AppTitle;
        public string AppDescription;
        public string[] IconUrl;
        public string Id;
        public AdWeight m_Probability;
        public AppItem()
        {

        }

    }

    private class CrossPromoDescriptor
    {

        public int AppsCount { get; set; }
        public int Version { get; set; }
        public List<AppItem> Items { get; set; }

        public CrossPromoDescriptor()
        {
            Items = new List<AppItem>();
        }

    }

    private class CoroutineWithData
    {
        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }
    #endregion




    private void OnEnable()
    {
        if (!AdsManager.Instance.CP_Ads)
            return;

        PromoWindow.SetActive(false);
        Apps.Clear();
        if (UseOldFile)
        {
            StartCoroutine(HandleResponseFromOldFile());

            return;
        }
        StartCoroutine(DownloadJsonDescriptor());
    }


    private IEnumerator DownloadJsonDescriptor()
    {


        UnityWebRequest www = new UnityWebRequest(DescriptorUrl);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.error != null)
        {

            Print("CrossPromo: WWW error: " + www.error);
        }
        else
        {

            Print("CrossPromo: Descriptor downloaded");

            parsedjson = www.downloadHandler.text;

            yield return StartCoroutine(HandleResponse(parsedjson));
        }
    }


    private IEnumerator HandleResponse(string responseJson)
    {
        descriptor = ParseResponse(responseJson);

        // if (UseOldFile)
        // {
        if (Utils.ConfigFileExists(ImageType))
        {
            File.Delete(Utils.GetSettingsFilePath(ImageType));
        }

        File.WriteAllText(Utils.GetSettingsFilePath(ImageType), responseJson);

        // }
        if (descriptor.AppsCount > 0)
        {
            // Try downloading image
            for (int i = 0; i < descriptor.AppsCount; i++)
            {


                CoroutineWithData cd = new CoroutineWithData(this, downloadImage(i));

                yield return cd.coroutine;

                bool success = (bool)cd.result;
                if (!success)
                {

                    Print("CrossPromo: Failded to load image");

                }
                else
                {
                    //#if UNITY_ANDROID
                    if (isAppInstalled(descriptor.Items[i].Id))
                    {

                    }
                    else
                    {
                        Apps.Add(descriptor.Items[i]);
                    }
                    //#endif
                    //#if UNITY_IOS
                    //                       Apps.Add(descriptor.Items[i]);
                    //#endif

                }
            }

            if (ShowAdOnStart)
            {
                ShowAd();
            }
        }
        yield return null;
    }
    private IEnumerator HandleResponseFromOldFile()
    {

        if (Utils.ConfigFileExists(ImageType))
        {
            string oldFile = File.ReadAllText(Utils.GetSettingsFilePath(ImageType));
            descriptor = ParseResponse(oldFile);
        }
        else
        {
            StartCoroutine(DownloadJsonDescriptor());
            yield break;
        }

        if (descriptor.AppsCount > 0)
        {
            for (int i = 0; i < descriptor.AppsCount; i++)
            {




                //#if UNITY_ANDROID
                if (isAppInstalled(descriptor.Items[i].Id))
                {

                }
                else
                {
                    Apps.Add(descriptor.Items[i]);
                }
                //#endif
                //#if UNITY_IOS
                //                  Apps.Add(descriptor.Items[i]);
                //#endif


            }

            if (ShowAdOnStart)
            {
                ShowAd();
            }
        }
        yield return null;
    }
    public void ShowAd()
    {
        if (!AdsManager.Instance.DisableCPAds)
        {
            if (AdsManager.Instance.NoCPPurchased())
                return;
            if (Apps.Count <= 0)
                return;
            // int rand = UnityEngine.Random.Range(0, Apps.Count);
            //  AppItem app = Apps[rand];
            AppItem app = GetRandomAd();
            PromoWindow.SetActive(false);

            UnityEngine.Events.UnityAction btnACtion = getBtnAction(app.Id);

            if (PlayBtn != null)
            {
                PlayBtn.onClick.RemoveAllListeners();
                PlayBtn.onClick.AddListener(btnACtion);
            }
            if (OkBtn != null)
            {
                OkBtn.onClick.RemoveAllListeners();
                OkBtn.onClick.AddListener(btnACtion);
            }


            int randImage = UnityEngine.Random.Range(0, app.IconUrl.Length);
            PromoImage.sprite = Utils.GetPromoSprite(ImageType, app.Id, randImage);
            PromoWindow.SetActive(true);
            if (AppName)
            {

                AppName.text = app.AppTitle.ToUpper();

            }
            if (AppDescription)
            {

                AppDescription.text = app.AppDescription.ToUpper();

            }
#if UNITY_IOS
        if (CanShowOverlay)
            AppstoreHandler.Instance.ShowOverlay(app.Id);
#endif
            if (Coroutine != null)
            {
                StopCoroutine(Coroutine);

            }
            Coroutine = StartCoroutine(RefreshAd());



        }
        else
        {
            print("aftab => No CP Ads Show");
        }

    }


    IEnumerator RefreshAd()
    {
        yield return new WaitForSecondsRealtime(RefreshTime);
        ShowAd();
    }


    public void HideInhouseAd()
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }
        PromoWindow.SetActive(false);
    }

    private CrossPromoDescriptor ParseResponse(string json)
    {

#if UNITY_IOS
            string platformSuf = KEY_IOS;
#else
        string platformSuf = KEY_ANDROID;
#endif

        CrossPromoDescriptor result = new CrossPromoDescriptor();
        JSONNode rootNode = JSON.Parse(json);
        JSONArray appsArray = rootNode[KEY_APPLIST + platformSuf].AsArray;
        result.AppsCount = appsArray.Count;
        for (int i = 0; i < appsArray.Count; ++i)
        {
            AppItem item = parseItem(appsArray[i]);
            result.Items.Add(item);
        }

        return result;
    }

    private static AppItem parseItem(JSONNode node)
    {
        AppItem result = new AppItem();

        result.AppTitle = node["AppTitle"];
        result.AppDescription = node["AppDescription"];
        JSONArray URLArray = node["IconUrl"].AsArray;
        //result.IconUrl = node["IconUrl"];
        result.IconUrl = new string[URLArray.Count];
        for (int i = 0; i < URLArray.Count; i++)
        {

            result.IconUrl[i] = URLArray[i].Value;

        }
        result.Id = node["Id"];
        JSONNode probabilityNode = node["m_Probability"];

        if (probabilityNode != null)
        {
            float chanceWeight = probabilityNode["m_ChanceWeight"].AsFloat;

            result.m_Probability = new AdWeight { m_ChanceWeight = chanceWeight };
        }
        else
        {
            Debug.Log("Null");
        }
        return result;
    }

    private IEnumerator downloadImage(int index)
    {

        for (int i = 0; i < descriptor.Items[index].IconUrl.Length; i++)
        {
            UnityWebRequest www = new UnityWebRequest(descriptor.Items[index].IconUrl[i]);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.error != null)
            {

                Print("CrossPromo: WWW error: " + www.error);
                yield return false;
            }
            else
            {

                File.WriteAllBytes(Utils.GetImagePath(ImageType, descriptor.Items[index].Id, i), www.downloadHandler.data);
                yield return true;
            }
        }

    }
    private UnityEngine.Events.UnityAction getBtnAction(string id)
    {
        return () =>
        {
#if UNITY_EDITOR
            //ShowAd();

#endif


            // Device links (will redirect user to play store or itunes store)
#if UNITY_ANDROID
            // Android link for device
            // link = "market://details?id=" + id;
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + id);
#endif

#if UNITY_IOS
                // iOS link for device
                // link = "itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=" + id;
                AppstoreHandler.Instance.openAppInStore(id);
#endif


            if (inHouse != InHouse.InhouseAd)
                ShowAd();





            //Application.OpenURL(link);

        };
    }



    private bool isAppInstalled(string bundleID)
    {
#if UNITY_EDITOR
        if (Application.identifier == bundleID)
            return true;
        else
            return false;

#endif
#if UNITY_ANDROID
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        Print(" ********LaunchOtherApp " + bundleID);
        AndroidJavaObject launchIntent = null;
        //if the app is installed, no errors. Else, doesn't get past next line
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID);
            //        
            //        ca.Call("startActivity",launchIntent);
        }
        catch (Exception ex)
        {

            Print("exception" + ex.Message);
        }
        if (launchIntent == null)
            return false;


        return true;

#endif
#if UNITY_IOS
        if (AdsConfigLoader.Instance.adsConfigIOS.APPID== bundleID)
            return true;
        else
            return false;

#endif
    }

    public AppItem GetRandomAd()
    {

        float probabilityTotalWeight = 0;

        if (Apps.Count > 0)
        {
            float currentProbabilityWeightMaximum = 0f;

            foreach (AppItem _GameAd in Apps)
            {
                if (_GameAd.m_Probability.m_ChanceWeight < 0f)
                {
                    Print("You can't have negative weight on an item. Reseting item's weight to 0.");
                    _GameAd.m_Probability.m_ChanceWeight = 0f;
                }
                else
                {
                    _GameAd.m_Probability.m_ChanceRangeFrom = currentProbabilityWeightMaximum;
                    currentProbabilityWeightMaximum += _GameAd.m_Probability.m_ChanceWeight;
                    _GameAd.m_Probability.m_ChanceRangeTo = currentProbabilityWeightMaximum;
                }
            }

            probabilityTotalWeight = currentProbabilityWeightMaximum;

            foreach (AppItem _GameAd in Apps)
                _GameAd.m_Probability.m_ChancePercent = ((_GameAd.m_Probability.m_ChanceWeight) / probabilityTotalWeight) * 100;
        }

        float pickedNumber = UnityEngine.Random.Range(0, probabilityTotalWeight);

        foreach (AppItem _GameAd in Apps)
        {
            if (pickedNumber > _GameAd.m_Probability.m_ChanceRangeFrom && pickedNumber < _GameAd.m_Probability.m_ChanceRangeTo)
            {
                int indx = Apps.IndexOf(_GameAd);
                return Apps[indx];
            }
        }

        Print("Item couldn't be picked... Be sure that you have at least one Promo Game");
        return Apps[0];

    }

    [System.Serializable]
    public class AdWeight
    {
        public float m_ChanceWeight;

        internal float m_ChancePercent;
        internal float m_ChanceRangeFrom;
        internal float m_ChanceRangeTo;

    }

    public void Print(string str)
    {
        if (Debugging)
        {
            Debug.Log(str);

        }
    }
}
