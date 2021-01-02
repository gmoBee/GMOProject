using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameAbilityList { Nothing = 0, Supplier, Bombardment }

public static class AbilityFactory
{
    private const float DefaultCooldown = 3f;

    public static AbstractAbility ChooseAbility(GameAbilityList ability, LivingEntity user)
    {
        if (ability == GameAbilityList.Supplier)
            return new Supplier(DefaultCooldown, user);
        else if (ability == GameAbilityList.Bombardment)
            return new Bombardment(DefaultCooldown, user);
        else
            return null;
    }
}
