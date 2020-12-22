using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supplier : AbstractAbility
{
    public Supplier(float durationCooldown, LivingEntity userReference)
        : base(durationCooldown, userReference)
    {
        
    }

    protected override IEnumerator UsingAbility()
    {
        yield return null;
    }
}
