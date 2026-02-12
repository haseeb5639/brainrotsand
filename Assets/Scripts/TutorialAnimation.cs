using UnityEngine;
using System.Collections;

public class TutorialAnimation : MonoBehaviour
{
    public bool animatePanel = false;
    public bool animateHand = false;
    public bool animateMiddleImage = false;

    // Panel Settings
    public float panelDuration = 0.25f;

    // Hand Settings
    public float handMoveAmount = 15f;
    public float handSpeed = 1f;

    // Middle Image Settings
    public float floatAmount = 10f;
    public float floatSpeed = 1.2f;
    public float pulseScale = 1.05f;
    public float pulseSpeed = 1f;

    private CanvasGroup canvasGroup;
    private Vector3 originalScale;
    private Vector3 startPos;

    private void Awake()
    {
        originalScale = transform.localScale;
        startPos = transform.localPosition;

        if (animatePanel)
        {
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        if (animatePanel)
            StartCoroutine(PanelPopAnimation());

        if (animateHand)
            StartCoroutine(HandBounceAnimation());

        if (animateMiddleImage)
        {
            StartCoroutine(MiddleFloatAnimation());
            StartCoroutine(MiddlePulseAnimation());
        }
    }

    // =============================
    // PANEL FADE + POP
    // =============================
    IEnumerator PanelPopAnimation()
    {
        float t = 0;
        canvasGroup.alpha = 0;
        transform.localScale = originalScale * 0.7f;

        while (t < panelDuration)
        {
            t += Time.deltaTime;
            float progress = t / panelDuration;

            canvasGroup.alpha = progress;
            transform.localScale = Vector3.Lerp(originalScale * 0.7f, originalScale, progress);

            yield return null;
        }
    }

    // =============================
    // HAND ICON BOUNCE
    // =============================
    IEnumerator HandBounceAnimation()
    {
        while (true)
        {
            // Move up
            yield return MoveTo(startPos + new Vector3(0, handMoveAmount, 0), handSpeed);

            // Move down
            yield return MoveTo(startPos, handSpeed);
        }
    }

    // Helper for movement
    IEnumerator MoveTo(Vector3 target, float speed)
    {
        Vector3 initial = transform.localPosition;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(initial, target, t);
            yield return null;
        }
    }

    // =============================
    // MIDDLE IMAGE FLOAT
    // =============================
    IEnumerator MiddleFloatAnimation()
    {
        while (true)
        {
            yield return MoveTo(startPos + new Vector3(0, floatAmount, 0), floatSpeed);
            yield return MoveTo(startPos, floatSpeed);
        }
    }

    // =============================
    // MIDDLE IMAGE PULSE
    // =============================
    IEnumerator MiddlePulseAnimation()
    {
        while (true)
        {
            // Scale up
            yield return ScaleTo(originalScale * pulseScale, pulseSpeed);

            // Scale down
            yield return ScaleTo(originalScale, pulseSpeed);
        }
    }

    IEnumerator ScaleTo(Vector3 target, float speed)
    {
        Vector3 initial = transform.localScale;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localScale = Vector3.Lerp(initial, target, t);
            yield return null;
        }
    }
}
