using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEntityHealth
{
    /// <summary>
    /// Heal by amount.
    /// </summary>
    void Heal(uint amount);

    /// <summary>
    /// Hit by damage.
    /// </summary>
    void Hit(uint damage);
}
