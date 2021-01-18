using UnityEngine;

[System.Serializable]
public struct OwnedWeapons
{
    // List of weapon holder
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    [SerializeField] private Weapon specialWeapon;

    private Weapon m_holdingOnHand;

    public Weapon HoldOnHand
    {
        set => m_holdingOnHand = value;
        get => m_holdingOnHand;
    }

    public Weapon PrimaryWeapon
    {
        set => primaryWeapon = value;
        get => primaryWeapon;
    }

    public Weapon SecondaryWeapon
    {
        set => secondaryWeapon = value;
        get => secondaryWeapon;
    }

    public Weapon SpecialWeapon
    {
        set => specialWeapon = value;
        get => specialWeapon;
    }

    public bool HasWeapon { get => primaryWeapon != null || secondaryWeapon != null; }
}