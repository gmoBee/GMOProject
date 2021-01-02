using System.Collections;
using UnityEngine;

public class OnWeaponRollArgs
{
    public LivingEntity Entity { private set; get; }
    public Weapon SelectedWeapon { private set; get; }

    public OnWeaponRollArgs(LivingEntity entity, Weapon selectedWeapon)
    {
        Entity = entity;
        SelectedWeapon = selectedWeapon;
    }
}
