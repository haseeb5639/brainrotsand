

using UnityEngine;

public class HammerHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Hammer ka collision debug ke liye rakhna hai, kaam nahi karega:
        Debug.Log("Hammer touched: " + other.name);

        
    }
}
