﻿using UnityEngine;
using NaughtyAttributes;

public class PlayerCameraControl : MonoBehaviour
{
    // Data
    [Space, Header("Data")]
    [SerializeField] private PlayerInputData playerInputData = null;

    [Space, Header("Custom Classes")]
    [SerializeField] private PlayerCameraZoom cameraZoom = null;
    [SerializeField] private PlayerCameraSway cameraSway = null;

    // Settings
    [Space, Header("Look Settings")]
    [SerializeField] private Vector2 sensitivity = Vector2.zero;
    [SerializeField] private Vector2 smoothAmount = Vector2.zero;
    [SerializeField] [MinMaxSlider(-90f, 90f)] private Vector2 lookAngleMinMax = Vector2.zero;

    // Private Variables
    private float m_yaw;
    private float m_pitch;

    private float m_desiredYaw;
    private float m_desiredPitch;

    // Components              
    private Transform m_pitchTranform;
    private Camera m_cam;

    // Properties
    public PlayerCameraZoom Zoom { get => cameraZoom; }
    public PlayerCameraSway Sway { get => cameraSway; }


    #region Unity Built-In Methods  
    void Awake()
    {
        GetComponents();
        InitValues();
        InitComponents();
        ChangeCursorState();
    }

    void LateUpdate()
    {
        CalculateRotation();
        SmoothRotation();
        ApplyRotation();
        HandleZoom();
    }
    #endregion

    #region Custom Methods
    void GetComponents()
    {
        m_pitchTranform = transform.GetChild(0).transform;
        m_cam = GetComponentInChildren<Camera>();
    }

    void InitValues()
    {
        m_yaw = transform.eulerAngles.y;
        m_desiredYaw = m_yaw;
    }

    void InitComponents()
    {
        cameraZoom.Init(m_cam, playerInputData);
        cameraSway.Init(m_cam.transform);
    }

    void CalculateRotation()
    {
        Vector2 desiredCameraLook = playerInputData.CameraMoveDirection;
        m_desiredYaw += desiredCameraLook.x * sensitivity.x * Time.deltaTime;
        m_desiredPitch -= desiredCameraLook.y * sensitivity.y * Time.deltaTime;

        m_desiredPitch = Mathf.Clamp(m_desiredPitch, lookAngleMinMax.x, lookAngleMinMax.y);
    }

    void SmoothRotation()
    {
        m_yaw = Mathf.Lerp(m_yaw, m_desiredYaw, smoothAmount.x * Time.deltaTime);
        m_pitch = Mathf.Lerp(m_pitch, m_desiredPitch, smoothAmount.y * Time.deltaTime);
    }

    void ApplyRotation()
    {
        transform.eulerAngles = new Vector3(0f, m_yaw, 0f);
        m_pitchTranform.localEulerAngles = new Vector3(m_pitch, 0f, 0f);
    }

    public void HandleSway(Vector3 _inputVector, float _rawXInput)
    {
        cameraSway.SwayPlayer(_inputVector, _rawXInput);
    }

    void HandleZoom()
    {
        if (playerInputData.ZoomClicked || playerInputData.ZoomReleased)
            cameraZoom.ChangeFOV(this);
    }

    public void ChangeRunFOV(bool _returning)
    {
        cameraZoom.ChangeRunFOV(_returning, this);
    }

    void ChangeCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion
}
