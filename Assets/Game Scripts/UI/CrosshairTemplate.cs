using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CrosshairTemplate
{
    // Placeholder is where to initialize the prefab and set parent with it
    [SerializeField] private RectTransform placeholder = null;

    // Other stuff you need for crosshair templates
    [Header("UIs")]
    [SerializeField] private RectTransform crosshairDefaultPrefab = null;
    [SerializeField] private RectTransform crosshairScopingPrefab = null;
    [SerializeField] private RectTransform crosshairDetectPrefab = null;

    private GameObject m_templateHolder;
    private GameObject m_detectHolder;
    private GameObject m_scopeHolder;

    public bool HasTemplate { get => placeholder != null && crosshairDefaultPrefab != null; }

    public void InitCrosshair()
    {
        // Check all templates are set
        if (crosshairDefaultPrefab != null)
        {
            m_templateHolder = Object.Instantiate(crosshairDefaultPrefab, placeholder).gameObject;
            if (!m_templateHolder.activeSelf)
                m_templateHolder.SetActive(true);
        }

        if (crosshairScopingPrefab != null)
        {
            m_scopeHolder = Object.Instantiate(crosshairScopingPrefab, placeholder).gameObject;
            if (m_scopeHolder.activeSelf)
                m_scopeHolder.SetActive(false);
        }

        if (crosshairDetectPrefab != null)
        {
            m_detectHolder = Object.Instantiate(crosshairDetectPrefab, placeholder).gameObject;
            if (m_detectHolder.activeSelf)
                m_detectHolder.SetActive(false);
        }
    }

    public void DestroyCrosshair()
    {
        // Destroy all UI
        if (m_templateHolder != null)
            Object.Destroy(m_templateHolder);
        if (m_detectHolder != null)
            Object.Destroy(m_detectHolder);
        if (m_scopeHolder != null)
            Object.Destroy(m_scopeHolder);
    }

    public void CrosshairDetect(bool isDetected)
    {
        if (m_detectHolder == null || m_templateHolder == null)
            return;

        if (isDetected)
        {
            if (m_templateHolder.activeSelf)
                m_templateHolder.SetActive(false);
            if (!m_detectHolder.activeSelf)
                m_detectHolder.SetActive(true);
        }
        else
        {
            if (!m_templateHolder.activeSelf)
                m_templateHolder.SetActive(true);
            if (m_detectHolder.activeSelf)
                m_detectHolder.SetActive(false);
        }
    }

    public void CrosshairScoping(bool isScoping)
    {
        if (m_detectHolder == null || m_templateHolder == null)
            return;

        if (isScoping)
        {
            if (m_templateHolder.activeSelf)
                m_templateHolder.SetActive(false);
            if (!m_scopeHolder.activeSelf)
                m_scopeHolder.SetActive(true);
        }
        else
        {
            if (!m_templateHolder.activeSelf)
                m_templateHolder.SetActive(true);
            if (m_scopeHolder.activeSelf)
                m_scopeHolder.SetActive(false);
        }
    }
}
