using UnityEngine;
using NaughtyAttributes;

namespace VHS
{    
    public class InputHandler : MonoBehaviour
    {
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
            interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
            interactionInputData.InteractedReleased = Input.GetKeyUp(KeyCode.E);
        }

        void GetCameraInput()
        {
            cameraInputData.InputVectorX = Input.GetAxis("Mouse X");
            cameraInputData.InputVectorY = Input.GetAxis("Mouse Y");

            cameraInputData.ZoomClicked = Input.GetMouseButtonDown(1);
            cameraInputData.ZoomReleased = Input.GetMouseButtonUp(1);
        }

        void GetMovementInputData()
        {
            movementInputData.InputVectorX = Input.GetAxisRaw("Horizontal");
            movementInputData.InputVectorY = Input.GetAxisRaw("Vertical");

            movementInputData.RunClicked = Input.GetKeyDown(KeyCode.LeftShift);
            movementInputData.RunReleased = Input.GetKeyUp(KeyCode.LeftShift);

            if(movementInputData.RunClicked)
                movementInputData.IsRunning = true;

            if(movementInputData.RunReleased)
                movementInputData.IsRunning = false;

            movementInputData.JumpClicked = Input.GetKeyDown(KeyCode.Space);
            movementInputData.CrouchClicked = Input.GetKeyDown(KeyCode.C);

            movementInputData.SlideClicked = Input.GetKeyDown(KeyCode.LeftControl);
            movementInputData.SlideReleased = Input.GetKeyUp(KeyCode.LeftControl);

            if (movementInputData.SlideClicked)
                movementInputData.IsSliding = true;

            if (movementInputData.SlideReleased)
                movementInputData.IsSliding = false;
        }

        void GetWeaponInputData()
        {
            // Crosshair target position
            Ray m_crosshairRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            weaponInputData.CrossHairPosition = m_crosshairRay.GetPoint(100f);

            weaponInputData.IsReloading = Input.GetKeyDown(KeyCode.R);
            weaponInputData.ShootClicked = Input.GetMouseButtonDown(0);
            weaponInputData.ShootReleased = Input.GetMouseButtonUp(0);

            if (weaponInputData.ShootClicked)
                weaponInputData.IsShooting = true;

            if (weaponInputData.ShootReleased)
                weaponInputData.IsShooting = false;
        }
        #endregion
    }
}