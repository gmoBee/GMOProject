using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType { Supplier, Bombardment }

public static class AbilityFactory
{
    private const float DefaultCooldown = 30f;

    public static AbstractAbility ChooseAbility(AbilityType ability, LivingEntity user)
    {
        if (ability == AbilityType.Supplier)
            return new Supplier(DefaultCooldown, user);
        else // Bombardment
            return new Bombardment(DefaultCooldown, user);
    }
}
