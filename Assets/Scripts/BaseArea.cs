using UnityEngine;

public enum BaseOwner { Player, AI1, AI2, AI3 }

public class BaseArea : MonoBehaviour
{
    public BaseOwner owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AIManager.Instance.PlayerEnteredBase(owner);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AIManager.Instance.PlayerExitedBase(owner);
        }
    }
}
