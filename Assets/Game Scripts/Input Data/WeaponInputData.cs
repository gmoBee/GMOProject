using System.Collections;
using UnityEngine;

public class WeaponInputData : MonoBehaviour
{
    #region Data
    private Vector3 m_midTargetPosition;

    private bool m_changePrimary;
    private bool m_changeSecondary;

    private bool m_shootClicked;
    private bool m_isShooting;
    private bool m_shootReleased;

    private bool m_isReloading;

    private bool m_scopeClicked;
    private bool m_isScoping;
    private bool m_scopeReleased;
    #endregion

    #region Properties
    public Vector3 MidTargetPosition
    {
        get => m_midTargetPosition;
        set => m_midTargetPosition = value;
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

    public bool IsReloading
    {
        get => m_isReloading;
        set => m_isReloading = value;
    }

    public bool ScopeClicked
    {
        get => m_scopeClicked;
        set => m_scopeClicked = value;
    }

    public bool ScopeReleased
    {
        get => m_scopeReleased;
        set => m_scopeReleased = value;
    }

    public bool IsScoping
    {
        get => m_isScoping;
        set => m_isScoping = value;
    }
    #endregion

    #region Custom Method
    public void ResetInput()
    {
        m_midTargetPosition = Vector3.zero;
        m_changePrimary = false;
        m_changeSecondary = false;
        m_isShooting = false;
        m_isReloading = false;
        m_scopeClicked = false;
        m_isScoping = false;
        m_scopeReleased = false;
    }
    #endregion
}
