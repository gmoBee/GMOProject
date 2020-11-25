using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

public class PlayerEntity : MonoBehaviour, IEntityInterface
{
    // Datas
    [Header("Data")]
    [SerializeField] private WeaponInputData weaponInputData = null;

    // All Properties are here
    [Header("Player Properties", order = 0)]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxOxygenLvl = 100;
    [SerializeField] protected Weapon holdingOnHand = null;

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
}
