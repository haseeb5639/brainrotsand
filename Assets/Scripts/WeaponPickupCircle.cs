
//////////////////// old scr.. with panel show hide...
////////using UnityEngine;

////////public class WeaponPickupCircle : MonoBehaviour
////////{
////////    [Header("Weapon Info")]
////////    public string weaponName;
////////    public Sprite weaponIcon;
////////    public GameObject weaponPrefab;       // Weapon prefab for hand
////////    public GameObject weaponObjectInCircle; // Scene weapon model to delete

////////    [Header("Trigger Settings")]
////////    public float triggerDistance = 3f;

////////    Transform player;
////////    public WeaponUnlockPanel panel;
////////    public bool isUnlocked = false;


////////    void Start()
////////    {
////////        player = GameObject.FindWithTag("Player")?.transform;
////////        //panel = FindObjectOfType<WeaponUnlockPanel>();

////////        if (player == null)
////////            Debug.LogError("❌ Player with tag 'Player' not found!");

////////        if (panel == null)
////////            Debug.LogError("❌ WeaponUnlockPanel not found! Make sure Weaponpanel object is enabled!");
////////    }

////////    void Update()
////////    {
////////        if (player == null || panel == null)
////////            return;

////////        float dist = Vector3.Distance(player.position, transform.position);

////////        if (dist <= triggerDistance)
////////        {
////////            panel.Show(this);
////////        }
////////        else
////////        {
////////            panel.Hide(this);
////////        }
////////    }
////////}




///////////////// remove object sc............



//using UnityEngine;
//using UnityEngine.UI;

//public class WeaponPickupCircle : MonoBehaviour
//{
//    [Header("Weapon Info")]
//    public string weaponName;
//    public Sprite weaponIcon;
//    public GameObject weaponPrefab;
//    public GameObject weaponObjectInCircle;

//    [Header("Auto Unlock Settings")]
//    public Image loadingCircleUI;
//    public float unlockTime = 1.5f;

//    Transform player;
//    PlayerWeaponHolder weaponHolder;

//    float timer = 0f;
//    bool isPlayerInside = false;
//    public bool isUnlocked = false;
//    bool isLoading = false;

//    public static WeaponPickupCircle lastUnlockedCircle;

//    void Start()
//    {
//        player = GameObject.FindWithTag("Player")?.transform;
//        weaponHolder = FindObjectOfType<PlayerWeaponHolder>();

//        loadingCircleUI.fillAmount = 0f;
//        loadingCircleUI.gameObject.SetActive(false);
//    }

//    // ⭐ Trigger Enter
//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;
//        if (isUnlocked) return;

//        isPlayerInside = true;
//        StartLoading();
//    }

//    // ⭐ Trigger Exit
//    private void OnTriggerExit(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        isPlayerInside = false;
//        CancelLoading();
//    }

//    void StartLoading()
//    {
//        timer = 0f;
//        isLoading = true;

//        loadingCircleUI.fillAmount = 0f;
//        loadingCircleUI.gameObject.SetActive(true);
//    }

//    void Update()
//    {
//        if (!isPlayerInside || !isLoading || isUnlocked)
//            return;

//        timer += Time.deltaTime;
//        loadingCircleUI.fillAmount = timer / unlockTime;

//        if (timer >= unlockTime)
//        {
//            CompleteLoading();
//        }
//    }

//    void CompleteLoading()
//    {
//        isLoading = false;
//        loadingCircleUI.gameObject.SetActive(false);
//        UnlockWeaponInstant();
//    }

//    void CancelLoading()
//    {
//        isLoading = false;
//        timer = 0f;

//        loadingCircleUI.fillAmount = 0f;
//        loadingCircleUI.gameObject.SetActive(false);
//    }

//    void UnlockWeaponInstant()
//    {
//        if (isUnlocked) return;

//        // Lock previous weapon circle
//        if (lastUnlockedCircle != null && lastUnlockedCircle != this)
//        {
//            lastUnlockedCircle.isUnlocked = false;
//            lastUnlockedCircle.RestoreCircleObject();
//        }

//        // Unlock this one
//        isUnlocked = true;
//        lastUnlockedCircle = this;

//        // Hide circle model
//        weaponObjectInCircle.SetActive(false);

//        // Equip prefab clone
//        weaponHolder.EquipWeapon(weaponPrefab);
//    }

//    public void RestoreCircleObject()
//    {
//        weaponObjectInCircle.SetActive(true);
//    }
//}



using UnityEngine;
using UnityEngine.UI;

public class WeaponPickupCircle : MonoBehaviour
{
    [Header("Weapon Info")]
    public GameObject weaponPrefab;
    public GameObject weaponObjectInCircle;

    [Header("Auto Unlock Settings")]
    public Image loadingCircleUI;
    public float unlockTime = 0.5f;

    Transform player;
    PlayerWeaponHolder weaponHolder;

    float timer = 0f;
    bool isPlayerInside = false;
    bool isLoading = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        weaponHolder = FindObjectOfType<PlayerWeaponHolder>();

        loadingCircleUI.fillAmount = 0f;
        loadingCircleUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = true;
        StartLoading();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInside = false;
        CancelLoading();
    }

    void StartLoading()
    {
        timer = 0f;
        isLoading = true;

        loadingCircleUI.fillAmount = 0f;
        loadingCircleUI.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isPlayerInside || !isLoading)
            return;

        timer += Time.deltaTime;
        loadingCircleUI.fillAmount = timer / unlockTime;

        if (timer >= unlockTime)
        {
            CompleteLoading();
        }
    }

    void CompleteLoading()
    {
        AdsManager.Instance.ShowRewardedAds();
        isLoading = false;
        loadingCircleUI.gameObject.SetActive(false);

        EquipWeapon();
    }

    void CancelLoading()
    {
        isLoading = false;
        timer = 0f;

        loadingCircleUI.fillAmount = 0f;
        loadingCircleUI.gameObject.SetActive(false);
    }

    void EquipWeapon()
    {
        // Just equip — circle stays reusable!
        weaponHolder.EquipWeapon(weaponPrefab);
    }
}
