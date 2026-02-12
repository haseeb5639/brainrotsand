
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class WeaponUnlockPanel : MonoBehaviour
//{
//    public GameObject panel;
//    public Image icon;
//    public Button watchAdBtn;

//    public TextMeshProUGUI statusText;

//    WeaponPickupCircle activeCircle;
//    PlayerWeaponHolder weaponHolder;

//    // 👉 Last bought weapon ko track karo
//    WeaponPickupCircle lastUnlockedCircle;

//    void Start()
//    {
//        panel.SetActive(false);
//        weaponHolder = FindObjectOfType<PlayerWeaponHolder>();

//        watchAdBtn.onClick.AddListener(() =>
//        {
//            UnlockWeapon();
//        });
//    }

//    public void Show(WeaponPickupCircle circle)
//    {
//        activeCircle = circle;
//        icon.sprite = circle.weaponIcon;

//        if (circle.isUnlocked)
//        {
//            statusText.text = "Already Buy";
//            watchAdBtn.interactable = false;
//        }
//        else
//        {
//            statusText.text = "Watch Ad to Unlock";
//            watchAdBtn.interactable = true;
//        }

//        panel.SetActive(true);
//    }

//    public void Hide(WeaponPickupCircle circle)
//    {
//        if (activeCircle == circle)
//            panel.SetActive(false);
//    }

//    void UnlockWeapon()
//    {
//        AdsManager.Instance.ShowRewardedAds();
//        // Already unlocked?
//        if (activeCircle.isUnlocked)
//        {
//            statusText.text = "Already Buy";
//            watchAdBtn.interactable = false;
//            return;
//        }

//        // ------ AUTO LOCK PREVIOUS WEAPON ------
//        if (lastUnlockedCircle != null && lastUnlockedCircle != activeCircle)
//        {
//            lastUnlockedCircle.isUnlocked = false;      // previous lock
//        }

//        // Unlock new weapon
//        activeCircle.isUnlocked = true;
//        lastUnlockedCircle = activeCircle;             // save last bought

//        weaponHolder.EquipWeapon(activeCircle.weaponPrefab);

//        statusText.text = "Already Buy";
//        watchAdBtn.interactable = false;

//        panel.SetActive(false);
//    }
//}
