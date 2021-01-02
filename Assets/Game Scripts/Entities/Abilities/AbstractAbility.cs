using System.Collections;
using UnityEngine;

public abstract class AbstractAbility
{
    // Each ability has this properties
    private float secondsCooldown;
    private LivingEntity userReference;
    
    // Variable Handler
    private float m_secondsCooldownHolder;
    private IEnumerator m_coolingDownRoutine;
    protected IEnumerator m_usingAbilityRoutine;

    // Properties
    public bool IsUsingAbility { get => m_usingAbilityRoutine != null; }
    public float CooldownInPercent { get => m_secondsCooldownHolder / secondsCooldown; }
    public bool CanUseAbility { get => !IsUsingAbility && CooldownInPercent <= 0f; }
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

    public void SetCooldown(bool active)
    {
        if (m_coolingDownRoutine != null)
            userReference.StopCoroutine(m_coolingDownRoutine);

        if (active)
        {
            m_coolingDownRoutine = CooldownRoutine();
            userReference.StartCoroutine(m_coolingDownRoutine);
        }
        else
        {
            m_secondsCooldownHolder = 0f;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        m_secondsCooldownHolder = secondsCooldown;
        while (m_secondsCooldownHolder > 0f)
        {
            yield return null;
            m_secondsCooldownHolder -= Time.deltaTime;
        }
        if (m_secondsCooldownHolder < 0f)
            m_secondsCooldownHolder = 0f;
    }

    // Generic Methods
    public abstract void UseAbility();
    protected abstract IEnumerator UsingAbility();
}
