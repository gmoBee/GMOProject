using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

public class PlayerEntity : MonoBehaviour, IEntityInterface
{
    [System.Serializable]
    public struct OwnedWeapons
    {
        // List of weapon holder
        [SerializeField] private Weapon holdingOnHand;
        [SerializeField] private Weapon primaryWeapon;
        [SerializeField] private Weapon secondaryWeapon;

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
    }

    // Datas
    [Header("Data")]
    [SerializeField] private WeaponInputData weaponInputData = null;
    [SerializeField] private PlayerControl playerController = null;


    // All Properties are here
    [Space, Header("Player Properties", order = 0)]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxOxygenLvl = 100;

    // Weapon Holder
    [Space]
    [SerializeField] protected OwnedWeapons ownedWeapons = new OwnedWeapons();

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int m_currentOxygenLvl;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int m_currentHealth;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private FirstPersonController m_personControlTransform;


    // Properties
    public WeaponInputData WInputData { get => weaponInputData; }


    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get self components by initializing player
        m_personControlTransform = gameObject.GetComponent<FirstPersonController>();
        m_currentHealth = maxHealth;
        m_currentOxygenLvl = maxOxygenLvl;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRollingWeapon();
    }
    #endregion

    public void Heal(int amount)
    {
        // Heal player health
    }

    public void Hit(int damage)
    {
        // Hit player with damage
    }

    public void HandleRollingWeapon()
    {
        // Change weapon to primary
        if (weaponInputData.ChangePrimary)
        {
            Debug.Log("Weapon change to primary");
            if (ownedWeapons.PrimaryWeapon == null)
                return;

            if (ownedWeapons.HoldOnHand.Equals(ownedWeapons.PrimaryWeapon))
                return;

            if (ownedWeapons.HoldOnHand != null)
                ownedWeapons.HoldOnHand.gameObject.SetActive(false);
            ownedWeapons.HoldOnHand = ownedWeapons.PrimaryWeapon;
            ownedWeapons.HoldOnHand.gameObject.SetActive(true);
            Debug.Log("Weapon changed Successfully");
        }

        // Change weapon to secondary
        if (weaponInputData.ChangeSecondary)
        {
            Debug.Log("Weapon change to secondary");
            if (ownedWeapons.SecondaryWeapon == null)
                return;

            if (ownedWeapons.HoldOnHand.Equals(ownedWeapons.SecondaryWeapon))
                return;

            if (ownedWeapons.HoldOnHand != null)
                ownedWeapons.HoldOnHand.gameObject.SetActive(false);
            ownedWeapons.HoldOnHand = ownedWeapons.SecondaryWeapon;
            ownedWeapons.HoldOnHand.gameObject.SetActive(true);
            Debug.Log($"Weapon changed Successfully to {ownedWeapons.HoldOnHand.gameObject.name}");
        }
    }
}
