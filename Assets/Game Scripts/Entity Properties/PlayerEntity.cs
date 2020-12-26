using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using VHS;

[RequireComponent(typeof(PlayerInputData))]
public class PlayerEntity : LivingEntity
{
    // Datas
    [Header("Player Attributes")]
    [SerializeField] private PlayerInputData playerInputData = null;

    private IEnumerator m_weaponRollRoutine;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isChangingWeapon = false;

    public PlayerInputData Controller { get => playerInputData; }

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        // Run parent function
        base.Start();
        OnHandInit(ownedWeapons.PrimaryWeapon);
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle player wants to change weapon
        HandleRollingWeapon();

        // Handle player wants to use ability
        HandleAbilityUsage();
    }
    #endregion

    private void HandleRollingWeapon()
    {
        // Check if currently rolling weapon
        if (m_weaponRollRoutine != null)
            return;

        // Change weapon to primary
        if (playerInputData.ChangePrimary)
        {
            if (ownedWeapons.PrimaryWeapon == null || ownedWeapons.HoldOnHand.Equals(ownedWeapons.PrimaryWeapon))
                return;

            OnHandInit(ownedWeapons.PrimaryWeapon);
        }

        // Change weapon to secondary
        if (playerInputData.ChangeSecondary)
        {
            if (ownedWeapons.SecondaryWeapon == null || ownedWeapons.HoldOnHand.Equals(ownedWeapons.SecondaryWeapon))
                return;

            OnHandInit(ownedWeapons.SecondaryWeapon);
        }
    }

    public override void HandleAbilityUsage()
    {
        if (playerInputData.AbilityPressed && useAbilityAction != null)
            useAbilityAction();
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
            ownedWeapons.HoldOnHand.setInputData(playerInputData);
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
