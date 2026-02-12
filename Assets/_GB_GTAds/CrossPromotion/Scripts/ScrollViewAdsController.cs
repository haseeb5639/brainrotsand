using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScrollViewAdsController : MonoBehaviour
{
    ScrollRect m_ScrollRect;

    InHouseManager m_ScrollViewAdd;

    int childIndex;

    bool SiblingCheck = false;

    private void Awake()
    {
        childIndex = transform.GetSiblingIndex();

        if (AdsManager.Instance.NoCPPurchased())
        {
            Destroy(this.gameObject);
            return;
        }
        m_ScrollRect = transform.GetComponentInParent<ScrollRect>();

        m_ScrollViewAdd = GetComponent<InHouseManager>();

        m_ScrollViewAdd.PromoWindow.SetActive(false);

    }

  IEnumerator Start()
    {
        transform.SetParent(null);
        yield return new WaitForSecondsRealtime(0.1f);
        if (m_ScrollViewAdd.Apps.Count > 0)
        {
            transform.SetParent(m_ScrollRect.content.transform);

            m_ScrollViewAdd.PromoWindow.SetActive(true);
            yield return new WaitForSecondsRealtime(0.3f);
            transform.SetSiblingIndex(childIndex);
        }
        transform.localScale = Vector3.one;



    }





}
