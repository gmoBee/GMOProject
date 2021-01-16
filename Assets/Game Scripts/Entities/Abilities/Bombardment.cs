using System.Collections;
using UnityEngine;

public class Bombardment : AbstractAbility, ICancelableAbility
{
    private bool m_isAiming = false;

    public Bombardment(float durationCooldown, LivingEntity userReference)
        : base (durationCooldown, userReference)
    {
        // TODO: Setup default
    }

    public override void UseAbility(Animator animReference)
    {
        if (IsUsingAbility && m_isAiming)
        {
            CancelAbility();
            return;
        }

        if (!CanUseAbility)
            return;

        m_usingAbilityRoutine = UsingAbility(animReference);
        UserReference.StartCoroutine(m_usingAbilityRoutine);
    }

    public void CancelAbility()
    {
        UserReference.StopCoroutine(m_usingAbilityRoutine);
        m_isAiming = false;
    }

    protected override IEnumerator UsingAbility(Animator animReference)
    {
        m_isAiming = true;
        yield return null;
        m_isAiming = false;

        // Start cooldown, finished using ability
        SetCooldown(true);
        m_usingAbilityRoutine = null;
    }
}
