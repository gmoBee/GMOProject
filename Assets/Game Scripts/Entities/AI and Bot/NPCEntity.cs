using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(BotDynamicController))]
public class NPCEntity : BotEntity, IEntityAbility, IEntityDeath
{
    [Header("NPC Attributes")]
    [SerializeField] private Animator npcAnim = null;
    [SerializeField] private BotDynamicController dynamicController = null;

    // Object property variables
    [Space, Header("Inventory & Skills")]
    [SerializeField] protected OwnedWeapons ownedWeapons = new OwnedWeapons();
    [SerializeField] protected GameAbilityList choosenAbility = GameAbilityList.Nothing;

    private AbstractAbility m_genericAbility = null;
    private IEnumerator m_weaponRollRoutine;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isChangingWeapon = false;

    public bool HasAbility => m_genericAbility != null;
    public Animator NPCAnimator => npcAnim;
    public OwnedWeapons CurrentOwnedWeapons => ownedWeapons;
    public AbstractAbility Ability => m_genericAbility;

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    private void Start()
    {
        ResetEntity();
        dynamicController = GetComponent<BotDynamicController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsDead)
        {
            if (!IsDying)
            {
                IsDying = true;
                AutoController.RotationEnabled = false;
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
    private void HandleAbilityUsage()
    {
        
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
        }
    }

    public void SetAbility(GameAbilityList ability)
    {
        choosenAbility = ability;
        m_genericAbility = AbilityFactory.ChooseAbility(ability, this);
    }

    private IEnumerator WeaponRollRoutine()
    {
        yield return null;
    }
    #endregion

    #region Overriden Methods from Parent Class
    protected override void ResetChildEntity()
    {
        // Set attributes
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

        // Deactivate object
        gameObject.SetActive(false);
    }
    #endregion
}
