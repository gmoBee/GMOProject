using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInputData", menuName = "FirstPersonController/Data/WeaponInputData", order = 0)]
public class WeaponInputData : ScriptableObject
{
    #region Data
    Vector2 m_crosshairTargetScreenPos;
    Vector3 m_crosshairTargetPos;

    bool m_changePrimary;
    bool m_changeSecondary;

    bool m_shootClicked;
    bool m_isShooting;
    bool m_shootReleased;

    bool m_isHolding;
    bool m_isReloading;
    #endregion

    #region Properties
    public Vector3 CrosshairTargetPos
    {
        get => m_crosshairTargetPos;
        set => m_crosshairTargetPos = value;
    }

    public Vector2 CrosshairScreenPos
    {
        get => m_crosshairTargetScreenPos;
        set => m_crosshairTargetScreenPos = value;
    }

    public bool ChangePrimary
    {
        get => m_changePrimary;
        set => m_changePrimary = value;
    }

    public bool ChangeSecondary
    {
        get => m_changeSecondary;
        set => m_changeSecondary = value;
    }

    public bool ShootClicked
    {
        get => m_shootClicked;
        set => m_shootClicked = value;
    }

    public bool IsShooting
    {
        get => m_isShooting;
        set => m_isShooting = value;
    }

    public bool ShootReleased
    {
        get => m_shootReleased;
        set => m_shootReleased = value;
    }

    public bool IsHolding
    {
        get => m_isHolding;
        set => m_isHolding = value;
    }

    public bool IsReloading
    {
        get => m_isReloading;
        set => m_isReloading = value;
    }
    #endregion

    #region Custom Method
    public void ResetInput()
    {
        m_crosshairTargetScreenPos = Vector2.zero;
        m_crosshairTargetPos = Vector3.zero;
        m_changePrimary = false;
        m_changeSecondary = false;
        m_isHolding = false;
        m_isShooting = false;
        m_isReloading = false;
    }
    #endregion
}
