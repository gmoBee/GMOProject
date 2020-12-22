using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombardment : AbstractAbility
{
    public Bombardment(float durationCooldown, LivingEntity userReference)
        : base (durationCooldown, userReference)
    {
        
    }

    protected override IEnumerator UsingAbility()
    {
        yield return null;
    }
}
