using UnityEngine;

public class ButtonHeartbeat : MonoBehaviour
{
    public float speed = 3f;
    public float scaleAmount = 0.06f; // How much pulse

    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; // 0-1 wave
        float scale = 1f + pulse * scaleAmount;

        transform.localScale = originalScale * scale;
    }
}
