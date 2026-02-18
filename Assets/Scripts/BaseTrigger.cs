












using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BaseTrigger : MonoBehaviour
{
    [Header("Base Settings")]
    public string baseName = "Base";
    public BaseManager.BaseData myBase;

    [Header("Interaction Settings")]
    public float  interactionDistance = 4f;

    private void Start()
    {
        // Setup sphere trigger
        SphereCollider col = GetComponent<SphereCollider>();
        //SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
       // col.radius = interactionDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("isTrigger");

        // Player entered the base range
        if (myBase.isUnlocked)
        {
            BrainrotManager.instance.SetNearbyBase(myBase);

            var held = BrainrotManager.instance.heldBrainrot;

            //if (held != null)
            //{
            //    BrainrotUIManager.instance.ShowMessage(
            //        $"Press Hand Icon to Drop {held.brainrotName}"
            //    );


            //}

            if (held != null)
            {
                BrainrotManager.instance.Drop();

                Debug.Log("✅ Brainrot auto-dropped on base");
            }


            else if (myBase.brainrot != null)
            {
                BrainrotUIManager.instance.ShowMessage(
                    $"Press Hand Icon to Pick {myBase.brainrot.brainrotName}"
                );
                Debug.LogError("isHave BrainRoot in Base");
            }
            else
            {
                //BrainrotUIManager.instance.ShowMessage("Base Empty");
                Debug.LogError("Base Empty");
            }
        }
        else
        {
            BrainrotUIManager.instance.ShowMessage("This base is locked!");
            BaseUnlockManager.Instance.ShowUnlockPanel(myBase);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Player left range → hide everything
        if (BrainrotManager.instance.CurrentNearbyBase == myBase)
        {
            BrainrotManager.instance.SetNearbyBase(null);
        }

        BrainrotUIManager.instance.HideMessage();
        BaseUnlockManager.Instance.HideUnlockPanel();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
