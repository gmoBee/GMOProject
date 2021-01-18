using System.Collections;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(PlayerInputData))]
public class PlayerEntity : LivingEntity, IEntityAbility, IEntityDeath
{
    // Data Requirements
    [Header("Player Attributes")]
    [SerializeField] private int playerId = 0;
    [SerializeField] private PlayerInputData playerInputData = null;
    [SerializeField] private Animator playerAnim = null;

    // Object property variables
    [Space, Header("Inventory & Skills")]
    [SerializeField] private OwnedWeapons ownedWeapons = new OwnedWeapons();
    [SerializeField] private GameAbilityList choosenAbility = GameAbilityList.Nothing;

    private AbstractAbility m_genericAbility = null;
    private IEnumerator m_weaponRollRoutine;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isChangingWeapon = false;

    public PlayerInputData Inputs => playerInputData;
    public bool HasAbility => m_genericAbility != null;
    public Animator PlayerAnimator => playerAnim;
    public OwnedWeapons CurrentOwnedWeapons => ownedWeapons;
    public AbstractAbility Ability => m_genericAbility;

    #region Unity BuiltIn Methods
    private void Start()
    {
        ResetEntity();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsDead)
        {
            // Handle player wants to change weapon
            HandleRollingWeapon();
            // Handle player wants to use ability
            HandleAbilityUsage();
            // Send weapon input packet
            SendWeaponInputPacket();
        }
        else
        {
            if (!IsDying)
            {
                IsDying = true;
                StartCoroutine(DyingRoutine());
            }
        }
    }

    private void OnDestroy()
    {
        ResetEntity();
    }
    #endregion

    #region Custom Methods
    private void HandleRollingWeapon()
    {
        // Check if currently rolling weapon
        if (m_isChangingWeapon)
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

    private void HandleAbilityUsage()
    {
        if (playerInputData.AbilityPressed && Ability != null)
            Ability.UseAbility(playerAnim);
    }

    private void WeaponHandInit(Weapon handle)
    {
        // Close previous weapon
        if (CurrentOwnedWeapons.HoldOnHand != null)
            CurrentOwnedWeapons.HoldOnHand.gameObject.SetActive(false);

        // Handle on hand with new one
        // TODO: Connect with animation
        ownedWeapons.HoldOnHand = handle;
        if (CurrentOwnedWeapons.HoldOnHand != null)
        {
            CurrentOwnedWeapons.HoldOnHand.gameObject.SetActive(true);
            CurrentOwnedWeapons.HoldOnHand.OnHandOwner = this;

            if (gameObject.tag == "Main Player")
                PlayerUIManager.instance.LoadWeaponUI(CurrentOwnedWeapons.HoldOnHand);
        }
    }

    public void SetAbility(GameAbilityList ability)
    {
        choosenAbility = ability;
        m_genericAbility = AbilityFactory.ChooseAbility(ability, this);
    }

    public void InstantDeath()
    {
        Hit(MaxHealth);
    }

    private IEnumerator DyingRoutine()
    {
        // Default dying time
        float dyingTime = 10f;

        while (dyingTime > 0f)
        {
            yield return null;
            dyingTime -= Time.deltaTime;
        }
    }

    private IEnumerator WeaponRollRoutine()
    {
        m_isChangingWeapon = true;

        // TODO: Change Weapon animation
        yield return null;

        m_isChangingWeapon = false;
    }
    #endregion

    #region Overriden Methods from Parent Class
    public override void Heal(uint amount)
    {
        if (Health + amount > MaxHealth)
            Health = MaxHealth;
        else
            Health += amount;
    }

    public override void Hit(uint damage)
    {
        if (damage > Health)
            Health = 0;
        else
            Health -= damage;
    }

    public override void ResetEntity()
    {
        // Set attributes
        Health = MaxHealth;
        OxygenLevel = MaxOxygenLvl;
        SetAbility(choosenAbility);
        IsDying = false;

        // Initialize weapon
        if (CurrentOwnedWeapons.HasWeapon)
        {
            if (CurrentOwnedWeapons.PrimaryWeapon != null)
                WeaponHandInit(CurrentOwnedWeapons.PrimaryWeapon);
            else
                WeaponHandInit(CurrentOwnedWeapons.SecondaryWeapon);
        }
    }

    public override bool CheckRelation(LivingEntity entity)
    {
        return entity.RelationID == RelationID;
    }
    #endregion
}
