








using UnityEngine;
using System.Collections;
using TMPro;

public class ButtonPressDOTween : MonoBehaviour
{
    [Header("Button Motion Settings")]
    public float pressDepth = 0.15f;
    public float pressSpeed = 6f;
    public float holdDuration = 0.15f;
    public float releaseDelay = 0.25f;

    [Header("Base Connection")]
    public Transform baseObject;
    public TextMeshProUGUI buttonText;

    [Header("FX Settings")]
    public GameObject sparklePrefab;
    public Transform fxSpawnPoint;
    public float fxDestroyDelay = 0.8f;

    private Vector3 originalPos;
    private Vector3 targetPos;
    private bool isAnimating = false;
    private BaseManager baseManager;
    private bool sparklePlayed = false;

    void Start()
    {
        originalPos = transform.position;
        targetPos = originalPos;
        baseManager = BaseManager.instance;

        if (baseManager != null && baseObject != null && buttonText != null)
            baseManager.SetBaseButtonText(baseObject, buttonText);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * pressSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        if (!sparklePlayed)
        {
            sparklePlayed = true;
            SpawnSparkleFX();
        }

        if (!isAnimating)
            StartCoroutine(PressAndRelease());
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
            sparklePlayed = false;
    }

    private IEnumerator PressAndRelease()
    {
        isAnimating = true;

        // 🔊 Play sound ONCE here (no overlap)
        //AudioManager.PlayButtonSound();

        // Move down
        targetPos = originalPos - new Vector3(0, pressDepth, 0);

        // Collect money if available
        if (baseManager != null && baseObject != null)
        {
            var baseData = baseManager.GetBaseFromTransform(baseObject);
            Debug.Log($"🔍 Button on {baseObject.name} triggered — BaseManager found: {(baseData != null ? baseData.baseName : "null")}");

            if (baseData != null && baseData.brainrot != null)
                baseData.brainrot.CollectMoneyFromButton();
            else
                Debug.LogWarning($"⚠️ No brainrot found on {baseData?.baseName ?? "unknown base"}");
        }

        yield return new WaitForSeconds(holdDuration);

        // Move up again
        targetPos = originalPos;
        yield return new WaitForSeconds(releaseDelay);

        isAnimating = false;
    }

    private void SpawnSparkleFX()
    {
        if (sparklePrefab == null) return;

        Vector3 spawnPos = fxSpawnPoint != null
            ? fxSpawnPoint.position
            : transform.position + Vector3.up * 0.1f;

        GameObject fx = Instantiate(sparklePrefab, spawnPos, Quaternion.identity);
        fx.transform.SetParent(transform, true);
        fx.transform.localScale = Vector3.one * 0.6f;
        Destroy(fx, fxDestroyDelay);
    }
}



