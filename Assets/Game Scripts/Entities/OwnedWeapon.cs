using UnityEngine;

[System.Serializable]
public struct OwnedWeapons
{
    // List of weapon holder
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;

    private Weapon holdingOnHand;

    public Weapon HoldOnHand
    {
        set => holdingOnHand = value;
        get => holdingOnHand;
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

    public bool HasWeapon { get => primaryWeapon != null || secondaryWeapon != null; }
}