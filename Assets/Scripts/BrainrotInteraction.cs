//////// BAg
/////

using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class BrainrotInteraction : MonoBehaviour
{
    [Header("Brainrot Config")]
    public string brainrotName = "Brainrot";

    [Tooltip("Earning rate of this Brainrot (e.g., 1 = $1/s)")]
    public float pricePerSecond = 1f;

    [Tooltip("Set category manually: Common / Rare / Legendary / Secret")]
    public string rarityCategory = "Common";

    [Header("Category Colors")]
    public Color commonColor = new Color(0.7f, 0.7f, 0.7f);
    public Color rareColor = new Color(0.0f, 0.75f, 1.0f);
    public Color legendaryColor = new Color(1.0f, 0.65f, 0.0f);
    public Color secretColor = new Color(0.73f, 0.33f, 0.83f);

    [Header("References")]
    public TextMeshProUGUI infoText;
    public MoneyAnimationController moneyAnimator;
    public Sprite iconSprite; // 🆕 used for Bag Panel icon

    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
   

    [HideInInspector] public bool IsPlacedOnBase;
    [HideInInspector] public BaseManager.BaseData currentBase;

    private float totalEarned;
    private float earnTimer;
    public Camera mainCam;
    private Transform player;
    private bool isPlayerNearby;

    private Vector3 originalPosition;
    private Quaternion originalRotation;


    [Header("Sand Sink Settings")]
    public bool enableSandSink = true;
    public float sinkDelay = 2f;         // spawn ke baad kitni der baad sink start ho
    public float sinkSpeed = 0.5f;       // kitni speed se niche jayega
    public float maxSinkDepth = 2f;      // kitna niche ja sakta hai
    public bool destroyOnFullSink = false;

    private float sinkTimer;
    private float sunkAmount;
    private Vector3 startPosition;
    private bool isSinking;

    private Animator anim;
    
    //public UnityEvent onPick;
    void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        anim = GetComponent<Animator>();

    }

    void Start()
    {
        // Auto-detect camera
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Camera[] cams = FindObjectsOfType<Camera>();
            if (cams.Length > 0) mainCam = cams[0];
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (infoText) infoText.gameObject.SetActive(false);
        UpdateInfoUI();

        startPosition = transform.position;
        sinkTimer = sinkDelay;
    }

    void Update()
    {
        HandleEarning();

        //if (!IsPlacedOnBase)
            HandlePlayerProximity();

        HandleBillboardFacing();

        HandleSandSink(); // 👈 ADD THIS
    }

    void HandleSandSink()
    {
        if (!enableSandSink) return;
        if (IsPlacedOnBase) return; // base pe ho to sink na kare
        if (BrainrotManager.instance.IsHoldingBrainrot()) return;

        sinkTimer -= Time.deltaTime;

        if (sinkTimer <= 0f && !isSinking)
        {
            isSinking = true;

            if (anim != null)
            {
                int randomIndex = Random.Range(0, 6); // 0 to 5
                anim.SetInteger("SinkIndex", randomIndex);
                anim.SetBool("IsSinking", true);
            }
        }

        if (isSinking)
        {
            float moveAmount = sinkSpeed * Time.deltaTime;
            transform.position -= new Vector3(0, moveAmount, 0);
            sunkAmount += moveAmount;

            if (sunkAmount >= maxSinkDepth)
            {
                if (destroyOnFullSink)
                {
                    if (BrainrotSpawner.Instance != null)
                        BrainrotSpawner.Instance.OnBrainrotDestroyed();

                    Destroy(gameObject);
                }
                else
                {
                    ReturnToOrigin();
                    ResetSand();
                }
            }
        }
    }

    void ResetSand()
    {
        isSinking = false;
        sunkAmount = 0f;
        sinkTimer = sinkDelay;
        transform.position = startPosition;
    }



    void HandleEarning()
    {
        if (!IsPlacedOnBase || currentBase == null) return;

        earnTimer += Time.deltaTime;
        if (earnTimer >= 1f)
        {
            totalEarned += pricePerSecond;
            earnTimer = 0f;
            UpdateButtonText();
        }
    }

    void HandlePlayerProximity()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool wasNearby = isPlayerNearby;
        isPlayerNearby = distance <= interactionDistance;

        if (isPlayerNearby && !wasNearby)
        {
            BrainrotManager.instance.SetNearbyBrainrot(this);

            if (!BrainrotManager.instance.IsHoldingBrainrot())
                BrainrotUIManager.instance.ShowPickupPrompt(brainrotName);

            ShowInfo(true);
        }
        else if (!isPlayerNearby && wasNearby)
        {
            BrainrotManager.instance.ClearNearbyBrainrot(this);

            if (!BrainrotManager.instance.IsHoldingBrainrot())
                BrainrotUIManager.instance.HideMessage();

            ShowInfo(false);
        }
    }

    public void UpdateInfoUI()
    {
        if (infoText == null) return;

        string colorCode = "#FFFFFF";
        switch (rarityCategory.ToLower())
        {
            case "common": colorCode = ColorUtility.ToHtmlStringRGB(commonColor); break;
            case "rare": colorCode = ColorUtility.ToHtmlStringRGB(rareColor); break;
            case "legendary": colorCode = ColorUtility.ToHtmlStringRGB(legendaryColor); break;
            case "secret": colorCode = ColorUtility.ToHtmlStringRGB(secretColor); break;
        }

        infoText.text = $"{brainrotName}\n" +
                        $"<size=90%><color=#{colorCode}>{rarityCategory}</color></size>\n" +
                        $"<size=85%><color=#00FF7F>${pricePerSecond}/s</color></size>";
    }

    public void UpdateButtonText()
    {
        if (currentBase != null && currentBase.buttonText != null)
        {
            if (totalEarned > 0)
                currentBase.buttonText.text = $"${FormatNumber(totalEarned)}";
            else
                currentBase.buttonText.text = "$0";
        }
    }

    private string FormatNumber(float value)
    {
        if (value >= 1000000000)
            return (value / 1000000000f).ToString("0.#") + "B";
        else if (value >= 1000000)
            return (value / 1000000f).ToString("0.#") + "M";
        else if (value >= 1000)
            return (value / 1000f).ToString("0.#") + "K";
        else
            return value.ToString("0");
    }

    public void ShowInfo(bool show)
    {
        if (infoText)
            infoText.gameObject.SetActive(show);
    }

    public void PickUp(Transform holdPoint)
    {

        if (BrainrotManager.instance.IsHoldingBrainrot())
            return;

        //    if (TutorialBrainrotGuide.Instance != null &&
        //TutorialBrainrotGuide.Instance.tutorialBrainrot == this)
        //    {
        //        TutorialBrainrotGuide.Instance.OnTutorialBrainrotPicked();
        //    }

        //    if (holdPoint == null) return;
        // 🔹 Tutorial: only show base path ONCE
        if (TutorialBrainrotGuide.Instance != null &&
            TutorialBrainrotGuide.Instance.tutorialBrainrot == this)
        {
            TutorialBrainrotGuide.Instance.OnTutorialBrainrotPicked();
        }

        // 🔹 Prevent crash: holdPoint must exist
        if (holdPoint == null) return;

        if (currentBase != null)
        {
            if (currentBase.buttonText != null)
                currentBase.buttonText.text = $"${totalEarned:F0}";
            currentBase.brainrot = null;
            currentBase = null;

            if (BrainrotCollectionUI.Instance != null)
                BrainrotCollectionUI.Instance.OnBrainrotPicked(brainrotName);
        }

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        IsPlacedOnBase = false;
        earnTimer = 0f;
        ShowInfo(true);
        BrainrotUIManager.instance.ShowDropPrompt();
        AudioManager.PlayPickSound();

        isSinking = false;
        sunkAmount = 0f;

        if (anim != null)
        {
            anim.SetBool("IsSinking", false);
            anim.SetInteger("SinkIndex", 0);
        }


        //onPick?.Invoke();

    }

    public void DropOnBase(BaseManager.BaseData baseData)
    {
        AudioManager.PlayDropSound();

        if (TutorialBrainrotGuide.Instance != null &&
    TutorialBrainrotGuide.Instance.tutorialBrainrot == this)
        {
            TutorialBrainrotGuide.Instance.OnTutorialBrainrotDropped();
        }



        if (baseData == null || baseData.baseObject == null) return;

        foreach (var b in BaseManager.instance.bases)
        {
            if (b == null) continue;
            if (b.brainrot == this)
                b.brainrot = null;
        }

        transform.SetParent(baseData.baseObject);


        float baseTopY = baseData.baseObject.position.y;
        Collider baseCol = baseData.baseObject.GetComponent<Collider>();
        if (baseCol != null)
            baseTopY = baseCol.bounds.max.y;
        else
        {
            MeshRenderer rend = baseData.baseObject.GetComponent<MeshRenderer>();
            if (rend != null)
                baseTopY = rend.bounds.max.y;
        }

        // measure your collider height accurately
        float myHalfHeight = 0f;
        Collider myCol = GetComponent<Collider>();
        if (myCol != null)
            myHalfHeight = myCol.bounds.extents.y;

        // ✅ set position right on the base (minus a small offset to avoid floating)
        Vector3 targetPos = baseData.baseObject.position;
        targetPos.y = baseTopY + myHalfHeight - 0.9f; // grounded fix (was too high)
        


        Vector3 faceDir = baseData.baseObject.forward;
        if (baseData.buttonText != null)
            faceDir = (baseData.buttonText.transform.position - baseData.baseObject.position).normalized;

        StartCoroutine(SmoothDropAndBounce(targetPos, faceDir));

        var confirmedBase = BaseManager.instance.GetBaseFromTransform(baseData.baseObject);
        if (confirmedBase != null)
        {
            confirmedBase.brainrot = this;
            currentBase = confirmedBase;
        }
        else
        {
            baseData.brainrot = this;
            currentBase = baseData;
        }

        IsPlacedOnBase = true;
        earnTimer = 0f;

        UpdateButtonText();
        // FindObjectOfType<BrainrotCollectionUI>()?.AddToBag(brainrotName, rarityCategory, iconSprite);
        BrainrotCollectionUI.Instance.AddToBag(brainrotName, rarityCategory, iconSprite);

        UpdateInfoUI();
        ShowInfo(true);
        BrainrotManager.instance.SetNearbyBrainrot(null);





        // 🧠 BAG SYSTEM INTEGRATION ✅
        try
        {
            BrainrotCollectionUI ui = FindObjectOfType<BrainrotCollectionUI>();
            if (ui != null)
            {
                ui.AddToBag(brainrotName, rarityCategory, iconSprite);
                Debug.Log($"📦 {brainrotName} added to Bag panel after placement!");
            }
            else
            {
                Debug.LogWarning("⚠️ No BrainrotCollectionUI found in scene!");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error adding {brainrotName} to Bag: {ex.Message}");
        }

        if (anim != null)
        {
            anim.SetBool("IsSinking", false);
        }

    }

    private IEnumerator SmoothDropAndBounce(Vector3 targetPos, Vector3 faceDir)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(faceDir, Vector3.up);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        Vector3 downPos = targetPos - Vector3.up * 0.05f;
        Vector3 upPos = targetPos + Vector3.up * 0.03f;

        float bounceT = 0f;
        while (bounceT < 1f)
        {
            bounceT += Time.deltaTime * 5f;
            transform.position = Vector3.Lerp(targetPos, downPos, Mathf.Sin(bounceT * Mathf.PI));
            yield return null;
        }

        bounceT = 0f;
        while (bounceT < 1f)
        {
            bounceT += Time.deltaTime * 5f;
            transform.position = Vector3.Lerp(downPos, upPos, Mathf.Sin(bounceT * Mathf.PI));
            yield return null;
        }

        transform.position = targetPos;
    }


    public void CollectMoneyFromButton()
    {
        if (totalEarned <= 0 || moneyAnimator == null || currentBase == null) return;

        moneyAnimator.PlayMoneyAnimation(totalEarned, transform.position + Vector3.up * 2f);

        // 🧠 Add earned money to total
        //RewardManager.TotalMoney += totalEarned;
        RewardManager.AddMoney(totalEarned);


        totalEarned = 0;

        if (currentBase.buttonText != null)
            currentBase.buttonText.text = $"${FormatNumber(pricePerSecond)}";

        // ✅ Force update all money texts (formats correctly as 10K / 1M)
        MoneyDisplay.UpdateAllMoneyTexts();

    }

    public void ReturnToOrigin()
    {
        if (TutorialBrainrotGuide.Instance != null &&
    TutorialBrainrotGuide.Instance.tutorialBrainrot == this)
        {
            TutorialBrainrotGuide.Instance.ResetTutorialToStart();
        }

        IsPlacedOnBase = false;
        currentBase = null;
        transform.SetParent(null);
        StopAllCoroutines();
        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ReturnRoutine()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float t = 0f;
        float duration = 1f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPos, originalPosition, t);
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, t);
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    private void HandleBillboardFacing()
    {
        if (infoText == null || !infoText.gameObject.activeInHierarchy || mainCam == null)
            return;

        RectTransform rect = infoText.rectTransform;
        Vector3 dir = rect.position - mainCam.transform.position;
        dir.y = 0f;

        Quaternion lookRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        rect.rotation = Quaternion.Slerp(rect.rotation, lookRot, Time.deltaTime * 8f);
    }

    private void OnDestroy()
    {
        if (currentBase != null && currentBase.brainrot == this)
            currentBase.brainrot = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!IsPlacedOnBase)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionDistance);
        }
    }
    private Sprite GetIconSprite()
    {
        /*SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            return sr.sprite;
        return null;*/
        print("getting icon sprite: " + iconSprite.name);
        return iconSprite;

    }

    public void RandomizeSinkValues()
    {
        // 🔹 Very small but random delay
        sinkDelay = Random.Range(0.2f, 3f);

        // 🔹 Accurate full depth based on height
        Collider col = GetComponent<Collider>();
        if (col != null)
            maxSinkDepth = col.bounds.size.y + Random.Range(0.3f, 0.8f);
        else
            maxSinkDepth = 3f;

        // 🔹 Big random time gaps (THIS is the key)
        float totalSinkTime = Random.Range(8f, 35f);

        // 🔹 Correct speed calculation
        sinkSpeed = maxSinkDepth / totalSinkTime;

        sinkTimer = sinkDelay;
    }



}


