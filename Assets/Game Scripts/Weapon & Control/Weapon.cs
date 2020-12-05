using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponClass { Primary, Secondary };

public abstract class Weapon : MonoBehaviour
{
    // Template and Data
    [Header("Template and Data")]
    [SerializeField] protected CrosshairTemplate crosshairTemplate = null;
    [Space]
    [SerializeField] [ReadOnly] protected WeaponInputData weaponInputData = null;

    // Weapon Attributes
    [Header("Generic Weapon Attributes")]
    [Slider(0.1f, 5f)] [SerializeField] private float fireRate = 1f;
    [SerializeField] private WeaponClass weaponClass = WeaponClass.Primary;
    [SerializeField] private float damageRate = 5f;
    [SerializeField] protected List<string> targetTags;
    [SerializeField] protected AnimationCurve normalFireRate = new AnimationCurve();
    [SerializeField] protected Light shootLight = null;

    // Properties
    public float FireRate { set { fireRate = value; } get { return fireRate; } }
    public float DamageRate { set { damageRate = value; } get { return damageRate; } }
    public WeaponInputData InputData { set { weaponInputData = value; } }
    public WeaponClass WeaponClassification { get => weaponClass; }

    // Unity Built-In Methods
    protected virtual void OnEnable()
    {
        weaponInputData = GetComponentInParent<PlayerEntity>().WInputData;
        crosshairTemplate.InitCrosshair();
    }

    protected abstract void Update();

    protected virtual void OnDisable()
    {
        weaponInputData = null;
        crosshairTemplate.DestroyCrosshair();
    }
}
