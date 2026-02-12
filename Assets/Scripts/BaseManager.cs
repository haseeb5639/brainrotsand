



using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BaseManager : MonoBehaviour
{
    public static BaseManager instance;

    [System.Serializable]
    public class BaseData
    {
        public string baseName;
        public Transform baseObject;
        public bool isUnlocked;
        public BrainrotInteraction brainrot;
        public TextMeshProUGUI buttonText; // NEW: Reference to the green button text
        public GameObject lockIcon;
    }
    public Transform defaultBaseTarget;

    public List<BaseData> bases = new List<BaseData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public BaseData GetBaseFromTransform(Transform baseTransform)
    {
        foreach (BaseData baseData in bases)
        {
            if (baseData.baseObject == baseTransform)
                return baseData;
        }
        return null;
    }

    // NEW METHOD: Set button text for a base
    public void SetBaseButtonText(Transform baseObject, TextMeshProUGUI buttonText)
    {
        BaseData baseData = GetBaseFromTransform(baseObject);
        if (baseData != null)
        {
            baseData.buttonText = buttonText;
            // Initialize button text
            if (baseData.brainrot != null)
            {
                baseData.brainrot.UpdateButtonText();
            }
            //else
            //{
            //    buttonText.text = "Empty";
            //}
        }
    }
}




