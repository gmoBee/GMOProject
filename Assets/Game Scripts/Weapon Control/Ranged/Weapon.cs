using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VHS
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Private Serialized
        #region Requirements
        [Space, Header("Data")]
        [SerializeField] private RectTransform crosshairTemplate = null;
        [SerializeField] protected WeaponInputData weaponInputData = null;
        #endregion

        #region Weapon Attributes
        [Space, Header("Weapon Attributes")]
        [SerializeField] protected LayerMask targetHitLayer = new LayerMask();
        [SerializeField] protected AnimationCurve normalFireRate;
        [SerializeField] private float fireRate = 1f; // This means it shoot or hit for 1 time per second
        [SerializeField] private float damageRate = 5f; // Everytime it hits enemy, enemy will take 5 damage per hit
        #endregion
        #endregion

        private RectTransform placeholderTemplate; // Where to put the crosshair as parent

        #region Properties
        public float FireRate 
        {
            get => fireRate;
            set => fireRate = value; 
        }
        public float DamageRate 
        { 
            get => damageRate;
            set => damageRate = value; 
        }
        #endregion

        #region Custom Methods
        protected void InitCrosshair()
        {
            // TODO: Set Crosshair, ammo baggage, and other stuff
        }

        protected void DestroyCrosshair()
        {
            // TODO: Unset this weapon GUI Template
        }
        #endregion
    }
}
