using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using VHS;

[RequireComponent(typeof(PlayerControl))]
public class PlayerEntity : LivingEntity
{
    // Datas
    [Header("Player Attributes")]
    [SerializeField] [ReadOnly] private PlayerControl playerController = null;

    private IEnumerator m_weaponRollRoutine;

    public PlayerControl Controller { get => playerController; }
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isChangingWeapon = false;

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        // Run parent function
        base.Start();
        playerController = GetComponent<PlayerControl>();
        OnHandInit(ownedWeapons.PrimaryWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle player wants to change weapon
        HandleRollingWeapon();

        // Handle player wants to use ability
        if (playerController.MInputData.AbilityPressed && useAbilityAction != null)
            useAbilityAction();
    }
    #endregion

    private void HandleRollingWeapon()
    {
        // Check if currently rolling weapon
        if (m_weaponRollRoutine != null)
            return;

        // Change weapon to primary
        if (playerController.WInputData.ChangePrimary)
        {
            if (ownedWeapons.PrimaryWeapon == null || ownedWeapons.HoldOnHand.Equals(ownedWeapons.PrimaryWeapon))
                return;

            OnHandInit(ownedWeapons.PrimaryWeapon);
        }

        // Change weapon to secondary
        if (playerController.WInputData.ChangeSecondary)
        {
            if (ownedWeapons.SecondaryWeapon == null || ownedWeapons.HoldOnHand.Equals(ownedWeapons.SecondaryWeapon))
                return;

            OnHandInit(ownedWeapons.SecondaryWeapon);
        }
    }

    private void OnHandInit(Weapon handle)
    {
        // Close previous weapon
        if (ownedWeapons.HoldOnHand != null)
            ownedWeapons.HoldOnHand.gameObject.SetActive(false);

        // Handle on hand with new one
        // TODO: Connect with animation
        ownedWeapons.HoldOnHand = handle;
        if (ownedWeapons.HoldOnHand != null)
        {
            ownedWeapons.HoldOnHand.setInputData(playerController.WInputData);
            ownedWeapons.HoldOnHand.gameObject.SetActive(true);
        }
    }

    private IEnumerator WeaponRollRoutine()
    {
        isChangingWeapon = true;

        // TODO: Change Weapon animation
        yield return null;

        isChangingWeapon = false;
    }
}
