using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputData : MonoBehaviour, IRestrictionInput
{
    private Vector2 m_moveDirection;

    private bool m_isCrouching;
    private bool m_crouchClicked;
    private bool m_abilityPressed;

    private bool m_jumpClicked;

    private bool m_slideClicked;
    private bool m_isSliding;
    private bool m_slideReleased;

    private bool m_runClicked;
    private bool m_isRunning;
    private bool m_runReleased;

    private Vector2 m_cameraMoveDir;

    private bool m_isZooming;
    private bool m_zoomClicked;
    private bool m_zoomReleased;

    private bool m_interactedClicked;
    private bool m_interactedRelease;

    private Vector2 m_crosshairTargetScreenPos;
    private Vector3 m_crosshairTargetPos;

    private bool m_changePrimary;
    private bool m_changeSecondary;

    private bool m_shootClicked;
    private bool m_isShooting;
    private bool m_shootReleased;

    private bool m_isReloading;

    public bool IsMoving => m_moveDirection != Vector2.zero;

    public Vector2 MoveDirection 
    { 
        get => m_moveDirection;
        set => m_moveDirection = value;
    }
    
    public Vector2 CameraMoveDirection 
    { 
        get => m_cameraMoveDir;
        set => m_cameraMoveDir = value;
    }

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

    public bool IsReloading
    {
        get => m_isReloading;
        set => m_isReloading = value;
    }

    public bool IsZooming
    {
        get => m_isZooming;
        set => m_isZooming = value;
    }

    public bool ZoomClicked
    {
        get => m_zoomClicked;
        set => m_zoomClicked = value;
    }

    public bool ZoomReleased
    {
        get => m_zoomReleased;
        set => m_zoomReleased = value;
    }

    public bool AbilityPressed
    {
        get => m_abilityPressed;
        set => m_abilityPressed = value;
    }

    public bool IsRunning
    {
        get => m_isRunning;
        set => m_isRunning = value;
    }

    public bool IsCrouching
    {
        get => m_isCrouching;
        set => m_isCrouching = value;
    }

    public bool IsSliding
    {
        get => m_isSliding;
        set => m_isSliding = value;
    }

    public bool CrouchClicked
    {
        get => m_crouchClicked;
        set => m_crouchClicked = value;
    }

    public bool JumpClicked
    {
        get => m_jumpClicked;
        set => m_jumpClicked = value;
    }

    public bool RunClicked
    {
        get => m_runClicked;
        set => m_runClicked = value;
    }

    public bool RunReleased
    {
        get => m_runReleased;
        set => m_runReleased = value;
    }

    public bool SlideClicked
    {
        get => m_slideClicked;
        set => m_slideClicked = value;
    }

    public bool SlideReleased
    {
        get => m_slideReleased;
        set => m_slideReleased = value;
    }

    public bool InteractedClicked
    {
        get => m_interactedClicked;
        set => m_interactedClicked = value;
    }

    public bool InteractedReleased
    {
        get => m_interactedRelease;
        set => m_interactedRelease = value;
    }

    #region Custom Methods
    #region Input Restriction
    public void RestrictInteraction(bool restrict)
    {

    }

    public void RestrictBodyAction(bool restrict)
    {

    }

    public void RestrictWalking(bool restrict)
    {
        
    }

    public void RestrictRunning(bool restrict)
    {
        
    }

    public void RestrictUseAbility(bool restrict)
    {
        
    }
    #endregion

    public void ResetInput()
    {
        m_moveDirection = Vector2.zero;

        m_isRunning = false;
        m_isCrouching = false;
        m_isSliding = false;

        m_crouchClicked = false;
        m_jumpClicked = false;
        m_runClicked = false;
        m_runReleased = false;

        m_cameraMoveDir = Vector2.zero;

        m_isZooming = false;
        m_zoomClicked = false;
        m_zoomReleased = false;

        m_interactedClicked = false;
        m_interactedRelease = false;

        m_crosshairTargetScreenPos = Vector2.zero;
        m_crosshairTargetPos = Vector3.zero;

        m_changePrimary = false;
        m_changeSecondary = false;
        m_isShooting = false;
        m_isReloading = false;
    }
    #endregion

    // if you want to go online you can send information to other player in here
    public void SendPacket()
    {
        // TODO: Send Game Packet
    }
}
