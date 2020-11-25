using UnityEngine;
using NaughtyAttributes;

namespace VHS
{    
    public class InputHandler : MonoBehaviour
    {
        // Input mapping
        [SerializeField] private InputMap inputMap = new InputMap() {
            // All default buttons
            reloadButton = KeyCode.R, crouchButton = KeyCode.C, interactButton = KeyCode.E,
            jumpButton = KeyCode.Space, runButton = KeyCode.LeftShift, slideButton = KeyCode.LeftControl,
            shootButton = KeyCode.Mouse0, scopeButton = KeyCode.Mouse1
        };

        #region Data
        [Space, Header("Input Data")]
        [SerializeField] private WeaponInputData weaponInputData = null;
        [SerializeField] private CameraInputData cameraInputData = null;
        [SerializeField] private MovementInputData movementInputData = null;
        [SerializeField] private InteractionInputData interactionInputData = null;
        #endregion

        #region Unity BuiltIn Methods
        void Start()
        {
            weaponInputData.ResetInput();
            cameraInputData.ResetInput();
            movementInputData.ResetInput();
            interactionInputData.ResetInput();
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
            interactionInputData.InteractedClicked = Input.GetKeyDown(inputMap.interactButton);
            interactionInputData.InteractedReleased = Input.GetKeyUp(inputMap.interactButton);
        }

        void GetCameraInput()
        {
            cameraInputData.InputVectorX = Input.GetAxis("Mouse X");
            cameraInputData.InputVectorY = Input.GetAxis("Mouse Y");

            cameraInputData.ZoomClicked = Input.GetKeyDown(inputMap.scopeButton);
            cameraInputData.ZoomReleased = Input.GetKeyUp(inputMap.scopeButton);

            if (cameraInputData.ZoomClicked)
                cameraInputData.IsZooming = true;

            if (cameraInputData.ZoomReleased)
                cameraInputData.IsZooming = false;
            //Debug.Log($"Is Zooming: {cameraInputData.IsZooming}; Is Scoping: {weaponInputData.IsScoping}");
        }

        void GetMovementInputData()
        {
            movementInputData.InputVectorX = Input.GetAxisRaw("Horizontal");
            movementInputData.InputVectorY = Input.GetAxisRaw("Vertical");

            movementInputData.RunClicked = Input.GetKeyDown(inputMap.runButton);
            movementInputData.RunReleased = Input.GetKeyUp(inputMap.runButton);

            if(movementInputData.RunClicked)
                movementInputData.IsRunning = true;

            if(movementInputData.RunReleased)
                movementInputData.IsRunning = false;

            movementInputData.JumpClicked = Input.GetKeyDown(inputMap.jumpButton);
            movementInputData.CrouchClicked = Input.GetKeyDown(inputMap.crouchButton);

            movementInputData.SlideClicked = Input.GetKeyDown(inputMap.slideButton);
            movementInputData.SlideReleased = Input.GetKeyUp(inputMap.slideButton);

            if (movementInputData.SlideClicked)
                movementInputData.IsSliding = true;

            if (movementInputData.SlideReleased)
                movementInputData.IsSliding = false;
        }

        void GetWeaponInputData()
        {
            // Crosshair target position
            weaponInputData.CrosshairScreenPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray m_crosshairRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            weaponInputData.CrosshairTargetPos = m_crosshairRay.GetPoint(100f);

            weaponInputData.IsReloading = Input.GetKeyDown(inputMap.reloadButton);
            weaponInputData.ShootClicked = Input.GetKeyDown(inputMap.shootButton);
            weaponInputData.ShootReleased = Input.GetKeyUp(inputMap.shootButton);

            if (weaponInputData.ShootClicked)
                weaponInputData.IsShooting = true;

            if (weaponInputData.ShootReleased)
                weaponInputData.IsShooting = false;
        }
        #endregion
    }
}