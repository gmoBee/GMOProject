using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum WeaponClass
{
    Undefined = 0,
    Primary = 1,
    Secondary = 2
}

[System.Flags]
public enum WeaponFlags
{
    Default = 0,
    NoScope = 1,
    CantDetect = 2
}

public abstract class Weapon : MonoBehaviour
{
    // Template and Data
    [Header("Data Attributes")]
    [SerializeField] private WeaponInputData inputData = null;
    [SerializeField] private uint damageRate = 5;
    [Slider(0.1f, 5f)] [SerializeField] private float fireRate = 1f;

    // Weapon Attributes
    [Header("Physical Attributes")]
    [SerializeField] private WeaponClass weaponClass = WeaponClass.Primary;
    [SerializeField] private List<string> targetTags = new List<string>();
    [SerializeField] private AnimationCurve normalFireRate = new AnimationCurve();
    [SerializeField] private Light flashLight = null;
    [SerializeField] private WeaponFlags flag;

    // Properties
    public WeaponInputData InputDataForWeapon => inputData;
    public WeaponBarrel GenericBarrel { get; protected set; } = null;
    public WeaponClass WeaponClassification => weaponClass;
    public WeaponFlags Flag => flag;
    
    protected List<string> Targets => targetTags;
    protected AnimationCurve EasingFireRate => normalFireRate;
    protected Light FlashLight => flashLight;

    public float FireRate 
    { 
        set => fireRate = value; 
        get => fireRate; 
    } 

    public uint DamageRate 
    { 
        set => damageRate = value; 
        get => damageRate; 
    }

    // Abstract Methods
    protected abstract void OnEnable();
    protected abstract void Update();
    protected abstract void OnDisable();
}
