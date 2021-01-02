using System.Collections;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(PlayerInputData))]
public class PlayerEntity : LivingEntity
{
    // Data Requirements
    [Header("Player Attributes")]
    [SerializeField] private int playerId = 0;
    [SerializeField] private PlayerInputData playerInputData = null;

    private IEnumerator m_weaponRollRoutine;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isChangingWeapon = false;

    public PlayerInputData Inputs { get => playerInputData; }

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        
    }

    private void Start()
    {
        ResetEntity();
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle player wants to change weapon
        HandleRollingWeapon();
        // Handle player wants to use ability
        HandleAbilityUsage();
        // Send weapon input packet
        SendWeaponInputPacket();
    }

    protected override void OnDisable()
    {

    }

    protected override void OnDestroy()
    {

    }
    #endregion

    #region Custom Methods
    private void HandleRollingWeapon()
    {
        // Check if currently rolling weapon
        if (isChangingWeapon)
            return;

        // Change weapon to primary
        if (playerInputData.ChangePrimary)
        {
            if (CurrentOwnedWeapons.PrimaryWeapon == null || CurrentOwnedWeapons.HoldOnHand.Equals(CurrentOwnedWeapons.PrimaryWeapon))
                return;
            WeaponHandInit(CurrentOwnedWeapons.PrimaryWeapon);
        }

        // Change weapon to secondary
        if (playerInputData.ChangeSecondary)
        {
            if (CurrentOwnedWeapons.SecondaryWeapon == null || CurrentOwnedWeapons.HoldOnHand.Equals(CurrentOwnedWeapons.SecondaryWeapon))
                return;
            WeaponHandInit(CurrentOwnedWeapons.SecondaryWeapon);
        }
    }

    private void SendWeaponInputPacket()
    {
        if (CurrentOwnedWeapons.HoldOnHand != null)
        {
            WeaponInputData dat = CurrentOwnedWeapons.HoldOnHand.InputDataForWeapon;
            dat.MidTargetPosition = playerInputData.MidTargetPosition;
            dat.ChangePrimary = playerInputData.ChangePrimary;
            dat.ChangeSecondary = playerInputData.ChangeSecondary;
            dat.ScopeClicked = playerInputData.ZoomClicked;
            dat.IsScoping = playerInputData.IsZooming;
            dat.ScopeReleased = playerInputData.ZoomReleased;
            dat.ShootClicked = playerInputData.ShootClicked;
            dat.IsShooting = playerInputData.IsShooting;
            dat.ShootReleased = playerInputData.ShootReleased;
        }
    }
    #endregion

    #region Overriden Methods from Parent Class
    protected override void HandleAbilityUsage()
    {
        if (playerInputData.AbilityPressed && Ability != null)
            Ability.UseAbility();
    }

    protected override void WeaponHandInit(Weapon handle)
    {
        // Close previous weapon
        if (CurrentOwnedWeapons.HoldOnHand != null)
            CurrentOwnedWeapons.HoldOnHand.gameObject.SetActive(false);

        // Handle on hand with new one
        // TODO: Connect with animation
        ownedWeapons.HoldOnHand = handle;
        if (CurrentOwnedWeapons.HoldOnHand != null)
            CurrentOwnedWeapons.HoldOnHand.gameObject.SetActive(true);
    }

    public override void Heal(uint amount)
    {
        Health += amount;
    }

    public override void Hit(uint damage)
    {
        Health -= damage;
    }

    public override void SetAbility(GameAbilityList ability)
    {
        choosenAbility = ability;
        Ability = AbilityFactory.ChooseAbility(ability, this);
    }

    public override void ResetEntity()
    {
        // Set attributes
        Health = MaxHealth;
        OxygenLevel = MaxOxygenLvl;
        SetAbility(choosenAbility);

        // Initialize weapon
        if (CurrentOwnedWeapons.HasWeapon)
        {
            if (CurrentOwnedWeapons.PrimaryWeapon != null)
                WeaponHandInit(CurrentOwnedWeapons.PrimaryWeapon);
            else
                WeaponHandInit(CurrentOwnedWeapons.SecondaryWeapon);
        }
    }

    protected override IEnumerator WeaponRollRoutine()
    {
        isChangingWeapon = true;

        // TODO: Change Weapon animation
        yield return null;

        isChangingWeapon = false;
    }
    #endregion
}
