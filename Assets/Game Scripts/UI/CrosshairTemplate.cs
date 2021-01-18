using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CrosshairTemplate
{
    // Placeholder is where to initialize the prefab and set parent with it
    [SerializeField] private RectTransform placeholder = null;
    [SerializeField] private Crosshairs crosshairsPacket;

    private GameObject m_defaultHolder;
    private GameObject m_detectHolder;
    private GameObject m_scopeHolder;

    public bool HasTemplate { get => placeholder != null && crosshairsPacket.defaultPrefab != null; }

    public void InitCrosshair()
    {
        // Check all templates are set
        if (crosshairsPacket.defaultPrefab != null)
        {
            m_defaultHolder = Object.Instantiate(crosshairsPacket.defaultPrefab, placeholder).gameObject;
            if (m_defaultHolder.activeSelf)
                m_defaultHolder.SetActive(true);
        }
        if (crosshairsPacket.scopePrefab != null)
        {
            m_scopeHolder = Object.Instantiate(crosshairsPacket.scopePrefab, placeholder).gameObject;
            if (m_scopeHolder.activeSelf)
                m_scopeHolder.SetActive(false);
        }
        if (crosshairsPacket.detectPrefab != null)
        {
            m_detectHolder = Object.Instantiate(crosshairsPacket.detectPrefab, placeholder).gameObject;
            if (m_detectHolder.activeSelf)
                m_detectHolder.SetActive(false);
        }
    }

    public void InitCrosshair(Crosshairs c)
    {
        crosshairsPacket = c;
        InitCrosshair();
    }

    public void DestroyCrosshair()
    {
        // Destroy all UI
        if (m_defaultHolder != null)
            Object.Destroy(m_defaultHolder);
        if (m_detectHolder != null)
            Object.Destroy(m_detectHolder);
        if (m_scopeHolder != null)
            Object.Destroy(m_scopeHolder);
    }

    public void SetCrosshairDefault(bool isDefault)
    {
        if (m_defaultHolder == null)
            return;

        if (isDefault != m_defaultHolder.activeSelf)
            m_defaultHolder.SetActive(isDefault);
    }

    public void SetCrosshairDetect(bool isDetected)
    {
        if (m_detectHolder == null)
            return;

        if (isDetected != m_detectHolder.activeSelf)
            m_detectHolder.SetActive(isDetected);
    }

    public void SetCrosshairScoping(bool isScoping)
    {
        if (m_detectHolder == null)
            return;

        if (isScoping != m_scopeHolder.activeSelf)
            m_scopeHolder.SetActive(isScoping);
    }
}
