using System.Collections;
using UnityEngine;

public class Supplier : AbstractAbility
{
    public Supplier(float durationCooldown, LivingEntity userReference)
        : base(durationCooldown, userReference)
    {
        // TODO: Setup default
    }

    public override void UseAbility()
    {
        if (!CanUseAbility)
            return;

        m_usingAbilityRoutine = UsingAbility();
        UserReference.StartCoroutine(m_usingAbilityRoutine);
    }

    protected override IEnumerator UsingAbility()
    {
        Debug.Log("Supplier Ability Used!");
        yield return null;

        // Start cooldown, finished using ability
        SetCooldown(true);
        m_usingAbilityRoutine = null;
    }
}
