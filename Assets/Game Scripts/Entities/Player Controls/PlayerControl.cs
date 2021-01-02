using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using VHS;

public class PlayerControl : MonoBehaviour
{
    // TODO: If it is going to be a multiplayer game, then change this into Packets
    [Header("Player Input Data")]
    [SerializeField] private WeaponInputData weaponInputData = null;
    [SerializeField] private MovementInputData movementInputData = null;
    [SerializeField] private CameraInputData cameraInputData = null;

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private FirstPersonController m_fps;

    #region Properties
    public WeaponInputData WInputData { get => weaponInputData; }
    public MovementInputData MInputData { get => movementInputData; }
    public CameraInputData CInputData { get => cameraInputData; }
    #endregion

    #region Unity BuiltIn Methods
    private void Start()
    {
        // Get self components by initializing player
        m_fps = gameObject.GetComponent<FirstPersonController>();
    }
    #endregion
}
