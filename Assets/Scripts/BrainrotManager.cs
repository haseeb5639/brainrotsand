






using UnityEngine;

public class BrainrotManager : MonoBehaviour
{
    public static BrainrotManager instance;

    [Header("References")]
    public Transform playerHoldPoint;

    [HideInInspector] public BrainrotInteraction heldBrainrot;
    public BaseManager.BaseData CurrentNearbyBase;
    [HideInInspector] public BrainrotInteraction currentNearbyBrainrot;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // auto find HoldPoint
        if (playerHoldPoint == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Transform found = player.transform.Find("HoldPoint");

                if (found != null)
                {
                    playerHoldPoint = found;
                }
                else
                {
                    playerHoldPoint = new GameObject("HoldPoint").transform;
                    playerHoldPoint.SetParent(player.transform);
                    playerHoldPoint.localPosition = new Vector3(0, 0, 1.5f);
                }
            }
        }
    }

    public void SetNearbyBase(BaseManager.BaseData baseData) => CurrentNearbyBase = baseData;
    public void SetNearbyBrainrot(BrainrotInteraction brainrot) => currentNearbyBrainrot = brainrot;

    public void ClearNearbyBrainrot(BrainrotInteraction brainrot)
    {
        if (currentNearbyBrainrot == brainrot)
            currentNearbyBrainrot = null;
    }

    // -------------------------------------------------------
    // GRAB + DROP LOGIC
    // -------------------------------------------------------
    public void Grab()
    {
        if (CurrentNearbyBase != null && CurrentNearbyBase.brainrot != null)
            GrabFromBase(CurrentNearbyBase.brainrot);
        else if (currentNearbyBrainrot != null)
            GrabFreeBrainrot(currentNearbyBrainrot);
    }

    public void Drop()
    {
        if (heldBrainrot != null && CurrentNearbyBase != null && CurrentNearbyBase.brainrot == null)
            DropBrainrot(CurrentNearbyBase);
    }

    public void ReturnHeldBrainrotToOrigin()
    {
        if (heldBrainrot == null) return;

        heldBrainrot.ReturnToOrigin();
        heldBrainrot = null;

        BrainrotUIManager.instance.UpdateActionButton(false);
        BrainrotUIManager.instance.ShowMessage("Returned to original spot!");
    }

    private void GrabFromBase(BrainrotInteraction brainrot)
    {
        if (heldBrainrot != null) return;

        brainrot.PickUp(playerHoldPoint);
        heldBrainrot = brainrot;
        CurrentNearbyBase.brainrot = null;

        BrainrotUIManager.instance.UpdateActionButton(true);
        BrainrotUIManager.instance.ShowDropPrompt();
    }

    private void GrabFreeBrainrot(BrainrotInteraction brainrot)
    {
        if (heldBrainrot != null) return;

        brainrot.PickUp(playerHoldPoint);
        heldBrainrot = brainrot;
        currentNearbyBrainrot = null;

        BrainrotUIManager.instance.UpdateActionButton(true);
        BrainrotUIManager.instance.ShowDropPrompt();
    }

    private void DropBrainrot(BaseManager.BaseData baseData)
    {
        if (heldBrainrot == null) return;

        heldBrainrot.DropOnBase(baseData);
        baseData.brainrot = heldBrainrot;
        heldBrainrot.currentBase = baseData;

        heldBrainrot = null;

        BrainrotUIManager.instance.UpdateActionButton(false);
        BrainrotUIManager.instance.HideMessage();
    }

    public void ClearHeldBrainrot()
    {
        heldBrainrot = null;
        

        BrainrotUIManager.instance.HideMessage();
    }


    public bool IsHoldingBrainrot()
    {
        return heldBrainrot != null;
    }
}
