using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [Header("UI in Game")]
    [SerializeField] private GameRulesUI gameRulesUI = null;
    [SerializeField] private MainPlayerUI mainPlayerUI = null;

    [Header("Data References")]
    [SerializeField] private CrosshairTemplate crosshairTemplate = null;
    [SerializeField] [ReadOnly] private PlayerInputHandler inputHandler = null;
    [SerializeField] [ReadOnly] private PlayerEntity mainPlayer = null;

    private WeaponFlags currentHoldWeaponFlag;

    #region Unity BuiltIn Methods
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    // Start is called before the first frame update
    private void Start()
    {
        // Initialize attributes
        GetAllComponents();
        crosshairTemplate.InitCrosshair();
    }

    // Update is called once per frame
    private void Update()
    {
        // Crosshair target position
        inputHandler.InputData.MidTargetScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray m_crosshairRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        inputHandler.InputData.MidTargetPosition = m_crosshairRay.GetPoint(Mathf.Sqrt(Camera.main.farClipPlane));
    }

    private void OnDestroy()
    {
        crosshairTemplate.DestroyCrosshair();
    }
    #endregion

    #region Initializer Methods
    private void GetAllComponents()
    {
        mainPlayer = FindObjectOfType<PlayerEntity>();
        inputHandler = FindObjectOfType<PlayerInputHandler>();
    }
    #endregion
}
