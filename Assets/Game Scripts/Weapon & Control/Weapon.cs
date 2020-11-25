using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Template and Data
    [Header("Template and Data")]
    [SerializeField] private CrosshairTemplate crosshairTemplate = null;
    [SerializeField] [ReadOnly] protected WeaponInputData weaponInputData = null;

    // Weapon Attributes
    [Header("Generic Weapon Attributes")]
    [Slider(0.1f, 6f)] [SerializeField] private float fireRate;
    [SerializeField] private float damageRate;
    [SerializeField] protected LayerMask targetHitLayer;
    [SerializeField] protected AnimationCurve normalFireRate;

    // Properties
    public float FireRate { set { fireRate = value; } get { return fireRate; } }
    public float DamageRate { set { damageRate = value; } get { return damageRate; } }
    public WeaponInputData InputData { set { weaponInputData = value; } }

    #region Unity Built-In Methods
    protected virtual void OnEnable()
    {
        weaponInputData = GetComponentInParent<PlayerEntity>().WInputData;
    }

    protected abstract void Update();

    protected virtual void OnDisable()
    {
        weaponInputData = null;
    }
    #endregion
}
