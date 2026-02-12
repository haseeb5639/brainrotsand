using UnityEngine;

public class LockIconAnimation : MonoBehaviour
{
    [Header("Floating")]
    public float floatSpeed = 1.5f;
    public float floatAmount = 0.12f;

    [Header("Pulse Scale")]
    public float scaleSpeed = 1.3f;
    public float scaleAmount = 0.08f;

    private Vector3 startPos;
    private Vector3 baseScale;

    void Start()
    {
        startPos = transform.localPosition;
        baseScale = transform.localScale;
    }

    void LateUpdate()
    {
        // ALWAYS face camera (no sideways rotation)
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.Euler(
                0,
                transform.rotation.eulerAngles.y,
                0
            );
        }

        // Floating animation
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);

        // Smooth pulse scale
        float scalePulse = 1f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = baseScale * scalePulse;
    }
}
