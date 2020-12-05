using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CrosshairTemplate
{
    // Placeholder is where to initialize the prefab and set parent with it
    [SerializeField] private RectTransform placeholder = null;

    // Other stuff you need for crosshair templates
    [Header("UIs")]
    [SerializeField] private RectTransform crosshairPrefab = null;
    [SerializeField] private RectTransform crosshairScopingPrefab = null;
    [SerializeField] private RectTransform crosshairDetectPrefab = null;

    private RectTransform m_templateHolder;
    private RectTransform m_detectHolder;
    private RectTransform m_scopeHolder;

    public void InitCrosshair()
    {
        // Check all templates are set
        if (crosshairPrefab != null)
        {
            m_templateHolder = Object.Instantiate(crosshairPrefab, placeholder);
            if (!m_templateHolder.gameObject.activeSelf)
                m_templateHolder.gameObject.SetActive(true);
        }

        if (crosshairScopingPrefab != null)
        {
            m_scopeHolder = Object.Instantiate(crosshairScopingPrefab, placeholder);
            if (m_scopeHolder.gameObject.activeSelf)
                m_scopeHolder.gameObject.SetActive(false);
        }

        if (crosshairDetectPrefab != null)
        {
            m_detectHolder = Object.Instantiate(crosshairDetectPrefab, placeholder);
            if (m_detectHolder.gameObject.activeSelf)
                m_detectHolder.gameObject.SetActive(false);
        }
    }

    public void DestroyCrosshair()
    {
        // Destroy all UI
        if (m_templateHolder != null)
            Object.Destroy(m_templateHolder.gameObject);
        if (m_detectHolder != null)
            Object.Destroy(m_detectHolder.gameObject);
        if (m_scopeHolder != null)
            Object.Destroy(m_scopeHolder.gameObject);
    }

    public void CrosshairDetect(bool isDetected)
    {
        if (m_detectHolder == null || m_templateHolder == null)
            return;

        if (isDetected)
        {
            if (m_templateHolder.gameObject.activeSelf)
                m_templateHolder.gameObject.SetActive(false);
            if (!m_detectHolder.gameObject.activeSelf)
                m_detectHolder.gameObject.SetActive(true);
        }
        else
        {
            if (!m_templateHolder.gameObject.activeSelf)
                m_templateHolder.gameObject.SetActive(true);
            if (m_detectHolder.gameObject.activeSelf)
                m_detectHolder.gameObject.SetActive(false);
        }
    }

    public void CrosshairScoping(bool isScoping)
    {
        if (m_detectHolder == null || m_templateHolder == null)
            return;

        if (isScoping)
        {
            if (m_templateHolder.gameObject.activeSelf)
                m_templateHolder.gameObject.SetActive(false);
            if (!m_scopeHolder.gameObject.activeSelf)
                m_scopeHolder.gameObject.SetActive(true);
        }
        else
        {
            if (!m_templateHolder.gameObject.activeSelf)
                m_templateHolder.gameObject.SetActive(true);
            if (m_scopeHolder.gameObject.activeSelf)
                m_scopeHolder.gameObject.SetActive(false);
        }
    }
}
