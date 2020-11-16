using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public class Flamethrower : RangedWeapon
    {
        #region Flamethrower Attributes
        [BoxGroup("Flamethrower")] [Slider(1f, 10f)] [SerializeField] private float maxBurnRange = 1f;
        [BoxGroup("Flamethrower")] [Slider(0.1f, 10f)] [SerializeField] private float intensity = 0.5f;
        #endregion
        #region Other Components

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

        private void Burn()
        {
            Vector3 m_hitDir = (weaponInputData.CrossHairTargetPos - Barrel.position).normalized;
            RaycastHit[] m_hits = Physics.CapsuleCastAll(Barrel.position, Barrel.position + m_hitDir, intensity, 
                m_hitDir, maxBurnRange, targetHitLayer);
            Debug.DrawRay(Barrel.position, m_hitDir * maxBurnRange, Color.red);

            // TODO: Hit Something

            Ammo--;
        }
         
        protected override IEnumerator ShootRoutine()
        {
            weaponShooting = true;
            float timePerFire = 1f / FireRate;
            float timeFireHolder = 0f;

            // TODO: Play VFX
            while (weaponInputData.IsShooting)
            {
                timeFireHolder -= Time.deltaTime;

                if (timeFireHolder <= 0f)
                {
                    timeFireHolder = timePerFire;
                    Burn(); // Ray hit function
                }

                if (Ammo <= 0)
                {
                    StartCoroutine(ReloadRoutine());
                    break;
                }

                yield return null;
            }

            // TODO: Stop VFX
            weaponShooting = false;
        }

        protected override IEnumerator ReloadRoutine()
        {
            weaponReloading = true;
            float m_reloadTimeHolder = ReloadTime;

            // TODO: Calculate the same time as animation time
            // TODO: Play Animation

            while (m_reloadTimeHolder > 0f)
            {
                m_reloadTimeHolder -= Time.deltaTime;
                yield return null;
            }

            Ammo = MaxAmmo;
            weaponReloading = false;
        }
    }
}

