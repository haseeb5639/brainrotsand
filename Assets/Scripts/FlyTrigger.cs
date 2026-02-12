using UnityEngine;

public class FlyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.PlayDamageSound();

        if (other.CompareTag("Player"))
        {
            PlayerFlyToBase fly = other.GetComponent<PlayerFlyToBase>();
            if (fly != null)
            {
                AudioManager.PlayDamageSound();
                fly.LaunchToBase();
                LabAnalytics.Instance.LogEvent("HitEnemy");

            }
        }
    }
}
