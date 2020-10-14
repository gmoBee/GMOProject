using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public abstract class RangedWeapon : Weapon
    {
        #region Ranged Weapon Attributes
        [Space, Header("Ranged Weapon Settings")]
        [SerializeField] private int maxAmmo;
        [SerializeField] private float reloadTime;
        [SerializeField] private Transform gunBarrel;
        [Slider(1f, 1000f)] [SerializeField] private double shootForce = 10f;
        private bool m_weaponActive;
        private IEnumerator m_shootingRoutine;
        private IEnumerator m_reloadingRoutine;
        #endregion
        #region Debugger (Optional)
        [Space]
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int ammo;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] protected bool weaponShooting;
        [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] protected bool weaponReloading;
        #endregion

        #region Properties
        public int Ammo
        {
            get => ammo;
            set
            {
                if (value > maxAmmo)
                    ammo = maxAmmo;
                else if (value < 0)
                    ammo = 0;
                else
                    ammo = value;
            }
        }

        public int MaxAmmo
        {
            get => maxAmmo;  
            set
            {
                if (ammo > value)
                    ammo = value;
                maxAmmo = value;
            }
        }

        public float ReloadTime
        {
            get => reloadTime;
            set => reloadTime = value;
        }

        public double ShootForce
        {
            get => shootForce;
            set => shootForce = value;
        }

        public Transform Barrel
        {
            get => gunBarrel;
        }
        #endregion

        #region Unity Built-In Methods
        protected virtual void OnEnable()
        {
            m_weaponActive = true;
        }

        private void FixedUpdate()
        {
            // Run ranged weapon methods
            if (m_weaponActive)
            {
                if (weaponInputData.IsShooting)
                    ShootInvoker();

                if (weaponInputData.IsReloading)
                    ReloadInvoker();
            }
        }

        protected virtual void OnDisable()
        {
            m_weaponActive = false;
        }
        #endregion

        #region Shoot Methods
        private void ShootInvoker()
        {
            if (weaponReloading)
                return;

            if (weaponShooting)
                return;

            m_shootingRoutine = ShootRoutine();
            StartCoroutine(m_shootingRoutine);
        }

        protected abstract IEnumerator ShootRoutine();
        #endregion

        #region Reload Methods
        private void ReloadInvoker()
        {
            if (weaponReloading || ammo == maxAmmo)
                return;

            if (m_shootingRoutine != null && weaponShooting)
            {
                StopCoroutine(m_shootingRoutine);
                if (weaponShooting)
                    weaponShooting = false;
            }

            m_reloadingRoutine = ReloadRoutine();
            StartCoroutine(m_reloadingRoutine);
        }

        protected abstract IEnumerator ReloadRoutine();
        #endregion
    }
}
