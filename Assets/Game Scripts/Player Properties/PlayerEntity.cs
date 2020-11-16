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

    private Transform m_personControlTransform;

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int currentOxygenLvl;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int currentHealth;

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    void Start()
    {
        // Get self components by initializing player
        InitPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    private void InitPlayer()
    {
        m_personControlTransform = GetComponent<FirstPersonController>().transform;
        currentHealth = maxHealth;
        currentOxygenLvl = maxOxygenLvl;
    }

    public void Heal(int amount)
    {
        // Heal player health
    }

    public void Hit(int damage)
    {
        // Hit player with damage
    }
}
