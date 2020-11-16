using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS 
{
    // TODO: I Though I can do this
    //[CreateAssetMenu(fileName = "NewRangedWeapon", menuName = "WeaponScript/RangedWeapon", order = 0)]
    public class SpecificRangedWeaponTemplate : RangedWeapon
    {
        #region This Weapon Attributes
        // Write all variables here.
        #endregion
        #region Other Components
        // What component needed, write here.
        #endregion

        #region Unity Built-In Methods
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        private void OnDrawGizmos()
        {

        }
        #endregion

        /// <summary>
        /// Weapon ray shoot function. This function only for raycasting the bullet.
        /// </summary>
        private void RayShootFunction()
        {
            Ammo--;
        }

        protected override IEnumerator ReloadRoutine()
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator ShootRoutine()
        {
            throw new System.NotImplementedException();
        }
    }

}
