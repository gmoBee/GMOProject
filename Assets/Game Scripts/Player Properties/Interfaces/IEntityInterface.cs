using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEntityInterface
{
    void Hit(int damage);
    void Heal(int amount);
}
