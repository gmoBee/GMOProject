using System;
using UnityEngine;
using NaughtyAttributes;

public class FuelBarrel : WeaponBarrel
{
    // Fuel barrel attributes
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float overheatCooldown = 2f;
    [SerializeField] private Transform barrelTransform = null;

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_currentFuel;

    // Properties
    public float MaxFuel
    {
        set => maxFuel = value;
        get => maxFuel;
    }

    public float CurrentFuel { get => m_currentFuel; }
    public float OverheatCooldown { get => overheatCooldown; }
    public Transform BarrelTransform { get => barrelTransform; }

    // Custom Methods
    public void FuelUp(float increasePerSeconds)
    {
        m_currentFuel += Time.deltaTime * increasePerSeconds;
    }

    public override void BarrelReset()
    {
        m_currentFuel = maxFuel;
    }
}
