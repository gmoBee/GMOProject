using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEntity : LivingEntity
{
    #region Unity BuiltIn Methods
    protected override void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        ResetEntity();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    protected override void OnDisable()
    {
        
    }

    protected override void OnDestroy()
    {
        
    }
    #endregion

    protected override void HandleAbilityUsage()
    {
        
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
        yield return null;
    }
}
