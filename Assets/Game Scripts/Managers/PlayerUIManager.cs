using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum CrosshairState { NoCrosshair, Default, Scope, Detect }

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance = null;

    [SerializeField] private Camera activeCamera = null;

    [Header("UI in Game")]
    [SerializeField] private GameRulesUI gameRulesUI = null;
    [SerializeField] private MainPlayerUI mainPlayerUI = null;

    [Header("Data References")]
    [SerializeField] private CrosshairTemplate crosshairTemplate = null;
    [SerializeField] [ReadOnly] private PlayerInputHandler inputHandler = null;
    [SerializeField] [ReadOnly] private PlayerEntity mainPlayer = null;

    private WeaponFlags m_currentHoldWeaponFlag;
    private CrosshairState m_currentCrosshairState;

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

    public void LoadWeaponUI(Weapon wp)
    {
        crosshairTemplate.DestroyCrosshair();
        crosshairTemplate.InitCrosshair(wp.CrosshairPacket);
        m_currentHoldWeaponFlag = wp.Flag;
    }
    #endregion

    public void SetActiveCamera(Camera cam)
    {
        // Deactivate old camera
        if (activeCamera != null)
            activeCamera.enabled = false;

        // Activate new camera
        activeCamera = cam;
        activeCamera.enabled = true;
    }

    public void ChangeCrosshairState(CrosshairState state)
    {
        if (state == m_currentCrosshairState)
            return;

        m_currentCrosshairState = state;
        if (state == CrosshairState.Default)
        {
            crosshairTemplate.SetCrosshairDefault(true);
            crosshairTemplate.SetCrosshairDetect(false);
            crosshairTemplate.SetCrosshairScoping(false);
        }
        else if (state == CrosshairState.Detect)
        {
            crosshairTemplate.SetCrosshairDefault(false);
            crosshairTemplate.SetCrosshairDetect(true);
            crosshairTemplate.SetCrosshairScoping(false);
        }
        else if (state == CrosshairState.Scope)
        {
            crosshairTemplate.SetCrosshairDefault(false);
            crosshairTemplate.SetCrosshairDetect(false);
            crosshairTemplate.SetCrosshairScoping(true);
        }
        else // No Crosshair
        {
            crosshairTemplate.SetCrosshairDefault(false);
            crosshairTemplate.SetCrosshairDetect(false);
            crosshairTemplate.SetCrosshairScoping(false);
        }
    }
}
