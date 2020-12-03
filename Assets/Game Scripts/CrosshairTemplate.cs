using System;
using UnityEngine;

[Serializable]
public class CrosshairTemplate
{
    [SerializeField] private RectTransform uiTemplatePrefab = null;
    [SerializeField] private RectTransform uiPlaceholder = null;
    [SerializeField] private RectTransform uiForScoping = null;

    private RectTransform uiTemplateHolder;

    public void InitCrosshair()
    {
        uiTemplateHolder = UnityEngine.Object.Instantiate(uiTemplatePrefab, uiPlaceholder);
        if (!uiTemplateHolder.gameObject.activeSelf)
            uiTemplateHolder.gameObject.SetActive(true);
        //TODO: Set scope UI
    }

    public void DestroyCrosshair()
    {
        UnityEngine.Object.Destroy(uiTemplateHolder);
        //TODO: Destroy scope UI
    }
}
