

//using UnityEngine;

//public class PlayerWeaponHolder : MonoBehaviour
//{
//    [Header("Where weapon will attach")]
//    public Transform holdPoint;

//    [Header("Current Equipped Weapon")]
//    public GameObject currentWeapon;

//    [Header("Settings")]
//    public float hideDelay = 2f;

//    bool isAttackHeld = false;

//    //-----------------------------------------------------
//    // EQUIP NEW WEAPON (DO NOT RESET POSITION/ROTATION)
//    //-----------------------------------------------------
//    public void EquipWeapon(GameObject weaponPrefab)
//    {
//        // Destroy old weapon
//        if (currentWeapon != null)
//            Destroy(currentWeapon);

//        // Instantiate weapon at holdPoint
//        currentWeapon = Instantiate(weaponPrefab, holdPoint);

//        // VERY IMPORTANT:
//        // DO NOT FORCE ZERO POSITION/ROTATION
//        // Let prefab keep its original pivot offsets!
//        currentWeapon.transform.localPosition = weaponPrefab.transform.localPosition;
//        currentWeapon.transform.localRotation = weaponPrefab.transform.localRotation;
//        currentWeapon.transform.localScale = weaponPrefab.transform.localScale;

//        currentWeapon.SetActive(true);

//        Debug.Log("Equipped => " + currentWeapon.name);
//    }

//    //-----------------------------------------------------
//    public void OnAttackHold()
//    {
//        isAttackHeld = true;

//        if (currentWeapon != null)
//            currentWeapon.SetActive(true);

//        CancelInvoke(nameof(HideWeapon));
//        Invoke(nameof(HideWeapon), hideDelay);
//    }

//    //-----------------------------------------------------
//    public void OnAttackRelease()
//    {
//        isAttackHeld = false;

//        CancelInvoke(nameof(HideWeapon));
//        Invoke(nameof(HideWeapon), hideDelay);
//    }

//    //-----------------------------------------------------
//    void HideWeapon()
//    {
//        if (isAttackHeld) return;

//        if (currentWeapon != null)
//            currentWeapon.SetActive(false);
//    }
//}




using UnityEngine;

public class PlayerWeaponHolder : MonoBehaviour
{
    [Header("Where weapon will attach")]
    public Transform holdPoint;

    [Header("Weapon Prefabs")]
    public GameObject defaultWeaponPrefab;

    [Header("Current Equipped Weapon")]
    public GameObject currentWeapon;

    [Header("Settings")]
    public float hideDelay = 2f;

    bool isAttackHeld = false;

    void Start()
    {
        if (currentWeapon == null && defaultWeaponPrefab != null)
            EquipWeapon(defaultWeaponPrefab);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponPrefab, holdPoint);

        //currentWeapon.transform.localPosition = Vector3.zero;
        //currentWeapon.transform.localRotation = Quaternion.identity;
        //currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.transform.localPosition = weaponPrefab.transform.localPosition;
             currentWeapon.transform.localRotation = weaponPrefab.transform.localRotation;
              currentWeapon.transform.localScale = weaponPrefab.transform.localScale;



        currentWeapon.SetActive(true);
    }

    // ---------------------------------------------------------
    public void OnAttackHold()
    {
        isAttackHeld = true;

        // Always show weapon while attacking/holding
        if (currentWeapon != null)
            currentWeapon.SetActive(true);

        // DO NOT start hide timer here!
    }

    // ---------------------------------------------------------
    public void OnAttackRelease()
    {
        isAttackHeld = false;

        // Start hide timer ONLY on release
        CancelInvoke(nameof(HideWeapon));
        Invoke(nameof(HideWeapon), hideDelay);
    }

    // ---------------------------------------------------------
    void HideWeapon()
    {
        if (isAttackHeld) return;

        if (currentWeapon != null)
            currentWeapon.SetActive(false);
    }
}
