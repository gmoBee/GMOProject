using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRestrictionInput
{
    void RestrictInteraction(bool restrict);
    void RestrictBodyAction(bool restrict);
    void RestrictWalking(bool restrict);
    void RestrictRunning(bool restrict);
    void RestrictUseAbility(bool restrict);
}
