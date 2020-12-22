using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class AbstractAbility
{
    // Each ability has this properties
    private float secondsCooldown;
    private LivingEntity userReference;
    
    // Variable Handler
    private float m_secondsCooldownHolder;
    private IEnumerator m_usingAbilityRoutine;

    // Properties
    public float SecondsUntilCooldown { get => m_secondsCooldownHolder; }
    public IEnumerator AbilityRoutine 
    { 
        get => m_usingAbilityRoutine; 
        protected set => m_usingAbilityRoutine = value; 
    }
    public bool CanUseAbility { get => AbilityRoutine == null && m_secondsCooldownHolder <= 0f; }
    protected LivingEntity UserReference { get => userReference; }

    // Constructor
    protected AbstractAbility(float secondsCooldown, LivingEntity userReference)
    {
        this.secondsCooldown = secondsCooldown;
        this.userReference = userReference;
        m_secondsCooldownHolder = 0f;
    }

    // Custom Methods
    public void ReduceCooldown(float reduceSeconds)
    {
        m_secondsCooldownHolder -= reduceSeconds;
    }

    public void SetCooldown(float seconds)
    {
        secondsCooldown = seconds;
    }

    public virtual void UseAbility()
    {
        if (!CanUseAbility)
            return;

        AbilityRoutine = UsingAbility();
        userReference.StartCoroutine(AbilityRoutine);
    }

    // Generic Methods
    protected abstract IEnumerator UsingAbility();
}
