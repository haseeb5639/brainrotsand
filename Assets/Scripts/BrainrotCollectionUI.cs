

//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Collections.Generic;

//public class BrainrotCollectionUI : MonoBehaviour
//{
//    [Header("Panels")]
//    [SerializeField] private GameObject indexPanel;
//    [SerializeField] private GameObject bagPanel;

//    [Header("Buttons")]
//    [SerializeField] private Button indexButton;
//    [SerializeField] private Button bagButton;
//    [SerializeField] private Button indexCloseButton;
//    [SerializeField] private Button bagCloseButton;

//    [Header("Parents for Grid Layouts")]
//    [SerializeField] private Transform indexContainer; // your Index grid parent
//    [SerializeField] private Transform bagContainer;   // your Bag grid parent

//    [Header("Prefabs")]
//    [SerializeField] private GameObject brainrotBoxPrefab;

//    // Store all collected brainrots
//    private readonly Dictionary<string, GameObject> collectedBrainrots = new Dictionary<string, GameObject>();
//    public static BrainrotCollectionUI Instance;
//    private void Awake()
//    {
//        Instance = this;
//    }
//    void Start()
//    {
//        // Button listeners
//        if (indexButton) indexButton.onClick.AddListener(OpenIndex);
//        if (bagButton) bagButton.onClick.AddListener(OpenBag);
//        if (indexCloseButton) indexCloseButton.onClick.AddListener(CloseAll);
//        if (bagCloseButton) bagCloseButton.onClick.AddListener(CloseAll);

//        indexPanel.SetActive(false);
//        bagPanel.SetActive(false);
//    }

//    // ---------------- PANEL HANDLERS ----------------
//    private void OpenIndex()
//    {
//        Time.timeScale = 0f;
//        indexPanel.SetActive(true);
//        bagPanel.SetActive(false);
//    }

//    private void OpenBag()
//    {
//        Time.timeScale = 0f;
//        bagPanel.SetActive(true);
//        indexPanel.SetActive(false);
//    }

//    private void CloseAll()
//    {
//        indexPanel.SetActive(false);
//        bagPanel.SetActive(false);
//        Time.timeScale = 1f;
//    }

//    // ---------------- MAIN FUNCTION ----------------
//    public void AddToBag(string brainrotName, string rarity, Sprite icon)
//    {
//        // prevent duplicates
//        if (collectedBrainrots.ContainsKey(brainrotName))
//            return;

//        // instantiate prefab
//        GameObject box = Instantiate(brainrotBoxPrefab);
//        BrainrotBox brainrotBox = box.GetComponent<BrainrotBox>();
//        box.name = $"{brainrotName}_Box";

//        // ensure parent & layout order are correct
//        box.transform.SetParent(bagContainer, false);
//        box.transform.SetSiblingIndex(bagContainer.childCount - 1); // sequential order (oldest first)

//       /* // set texts
//        TextMeshProUGUI[] texts = box.GetComponentsInChildren<TextMeshProUGUI>(true);
//        foreach (var t in texts)
//        {
//            string lower = t.name.ToLower();
//            if (lower.Contains("name"))
//                t.text = brainrotName;
//            else if (lower.Contains("rarity"))
//            {
//                t.text = rarity.ToUpper();
//                t.color = GetRarityColor(rarity);
//            }
//        }*/

//        // assign unique icon (each prefab instance has its own Image component)
//        /*Image iconImg = null;
//        foreach (var img in box.GetComponentsInChildren<Image>(true))
//        {
//            if (img.name.ToLower().Contains("icon") || img.name.ToLower().Contains("img"))
//            {
//                iconImg = img;
//                break;
//            }
//        }
//        if (iconImg != null && icon != null)
//            iconImg.sprite = icon;*/
//        brainrotBox._icon.sprite = icon;
//        brainrotBox._mameText.text = brainrotName;
//        brainrotBox._mameText.text = rarity;
        
//        collectedBrainrots.Add(brainrotName, box);
//    }

//    // optional — remove from bag when picked up again
//    public void RemoveFromBag(string brainrotName)
//    {
//        if (collectedBrainrots.ContainsKey(brainrotName))
//        {
//            Destroy(collectedBrainrots[brainrotName]);
//            collectedBrainrots.Remove(brainrotName);
//        }
//    }

//    private Color GetRarityColor(string rarity)
//    {
//        switch (rarity.ToLower())
//        {
//            case "common": return new Color(0.7f, 0.7f, 0.7f);
//            case "rare": return new Color(0.0f, 0.75f, 1.0f);
//            case "legend":
//            case "legendary": return new Color(1.0f, 0.65f, 0.0f);
//            case "secret": return new Color(0.73f, 0.33f, 0.83f);
//            default: return Color.white;
//        }
//    }
//}


///////
/////


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BrainrotCollectionUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject indexPanel;
    [SerializeField] private GameObject bagPanel;

    [Header("Buttons")]
    [SerializeField] private Button indexButton;
    [SerializeField] private Button bagButton;
    [SerializeField] private Button indexCloseButton;
    [SerializeField] private Button bagCloseButton;

    [Header("Parents for Grid Layouts")]
    [SerializeField] private Transform indexContainer;
    [SerializeField] private Transform bagContainer;

    [Header("Prefabs")]
    [SerializeField] private GameObject brainrotBoxPrefab;

    private readonly Dictionary<string, GameObject> collectedBrainrots = new Dictionary<string, GameObject>();

    public static BrainrotCollectionUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (indexButton) indexButton.onClick.AddListener(OpenIndex);
        if (bagButton) bagButton.onClick.AddListener(OpenBag);
        if (indexCloseButton) indexCloseButton.onClick.AddListener(CloseAll);
        if (bagCloseButton) bagCloseButton.onClick.AddListener(CloseAll);

        indexPanel.SetActive(false);
        bagPanel.SetActive(false);
    }

    // ---------------- PANEL HANDLERS ----------------
    private void OpenIndex()
    {
        Time.timeScale = 0f;
        indexPanel.SetActive(true);
        bagPanel.SetActive(false);
        AdsManager.Instance.ShowAds();
    }

    private void OpenBag()
    {
        Time.timeScale = 0f;
        bagPanel.SetActive(true);
        indexPanel.SetActive(false);
        AdsManager.Instance.ShowAds();
    }

    private void CloseAll()
    {
        indexPanel.SetActive(false);
        bagPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // ---------------- MAIN FUNCTION ----------------
    public void AddToBag(string brainrotName, string rarity, Sprite icon)
    {
        if (collectedBrainrots.ContainsKey(brainrotName))
            return;

        GameObject box = Instantiate(brainrotBoxPrefab, bagContainer);
        box.name = $"{brainrotName}_Box";

        BrainrotBox brainrotBox = box.GetComponent<BrainrotBox>();
        brainrotBox._icon.sprite = icon;
        brainrotBox._mameText.text = brainrotName;
        brainrotBox._rarityText.text = rarity;
        brainrotBox._rarityText.color = GetRarityColor(rarity);

        collectedBrainrots.Add(brainrotName, box);
    }

    public void RemoveFromBag(string brainrotName)
    {
        if (collectedBrainrots.ContainsKey(brainrotName))
        {
            Destroy(collectedBrainrots[brainrotName]);
            collectedBrainrots.Remove(brainrotName);
        }
    }

    // ---------------- NEW SIMPLE FUNCTION ----------------
    // Call this from your pickup code when brainrot is picked from base
    public void OnBrainrotPicked(string brainrotName)
    {
        RemoveFromBag(brainrotName);
    }

    private Color GetRarityColor(string rarity)
    {
        switch (rarity.ToLower())
        {
            case "common": return new Color(0.7f, 0.7f, 0.7f);
            case "rare": return new Color(0.0f, 0.75f, 1.0f);
            case "legend":
            case "legendary": return new Color(1.0f, 0.65f, 0.0f);
            case "secret": return new Color(0.73f, 0.33f, 0.83f);
            default: return Color.white;
        }
    }
}
