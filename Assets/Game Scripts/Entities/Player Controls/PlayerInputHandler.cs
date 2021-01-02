using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Space, Header("Input Data")]
    [SerializeField] private PlayerInputData playerInputData = null;

    [SerializeField]
    private PlayerInputMap inputMap = new PlayerInputMap()
    {
        // All default buttons
        reloadButton = KeyCode.R,
        crouchButton = KeyCode.C,
        interactButton = KeyCode.E,
        jumpButton = KeyCode.Space,
        runButton = KeyCode.LeftShift,
        slideButton = KeyCode.LeftControl,
        shootButton = KeyCode.Mouse0,
        scopeButton = KeyCode.Mouse1,
        useAbility = KeyCode.X,
        switchPrimaryButton = KeyCode.Alpha1,
        switchSecondaryButton = KeyCode.Alpha2
    };

    public PlayerInputData InputData => playerInputData;

    #region Unity BuiltIn Methods
    void Start()
    {
        playerInputData.ResetInput();
    }

    void Update()
    {
        GetCameraInput();
        GetMovementInputData();
        GetInteractionInputData();
        GetWeaponInputData();
    }
    #endregion

    #region Custom Methods
    void GetInteractionInputData()
    {
        playerInputData.InteractedClicked = Input.GetKeyDown(inputMap.interactButton);
        playerInputData.InteractedReleased = Input.GetKeyUp(inputMap.interactButton);
    }

    void GetCameraInput()
    {
        playerInputData.CameraMoveDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        playerInputData.ZoomClicked = Input.GetKeyDown(inputMap.scopeButton);
        playerInputData.ZoomReleased = Input.GetKeyUp(inputMap.scopeButton);

        if (playerInputData.ZoomClicked)
            playerInputData.IsZooming = true;

        if (playerInputData.ZoomReleased)
            playerInputData.IsZooming = false;
    }

    void GetMovementInputData()
    {
        playerInputData.MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        playerInputData.RunClicked = Input.GetKeyDown(inputMap.runButton);
        playerInputData.RunReleased = Input.GetKeyUp(inputMap.runButton);

        if (playerInputData.RunClicked)
            playerInputData.IsRunning = true;

        if (playerInputData.RunReleased)
            playerInputData.IsRunning = false;

        playerInputData.JumpClicked = Input.GetKeyDown(inputMap.jumpButton);
        playerInputData.CrouchClicked = Input.GetKeyDown(inputMap.crouchButton);

        playerInputData.SlideClicked = Input.GetKeyDown(inputMap.slideButton);
        playerInputData.SlideReleased = Input.GetKeyUp(inputMap.slideButton);

        if (playerInputData.SlideClicked)
            playerInputData.IsSliding = true;

        if (playerInputData.SlideReleased)
            playerInputData.IsSliding = false;

        playerInputData.AbilityPressed = Input.GetKeyDown(inputMap.useAbility);
    }

    void GetWeaponInputData()
    {
        playerInputData.IsReloading = Input.GetKeyDown(inputMap.reloadButton);
        playerInputData.ShootClicked = Input.GetKeyDown(inputMap.shootButton);
        playerInputData.ShootReleased = Input.GetKeyUp(inputMap.shootButton);
        playerInputData.ChangePrimary = Input.GetKeyDown(inputMap.switchPrimaryButton);
        playerInputData.ChangeSecondary = Input.GetKeyDown(inputMap.switchSecondaryButton);

        if (playerInputData.ShootClicked)
            playerInputData.IsShooting = true;

        if (playerInputData.ShootReleased)
            playerInputData.IsShooting = false;
    }
    #endregion
}
