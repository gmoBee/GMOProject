using System;
using UnityEngine;
using NaughtyAttributes;

public class LivingEntity : MonoBehaviour
{
    // All Properties are here
    [Space, Header("Entity Properties", order = 0)]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int maxOxygenLvl = 100;
    [SerializeField] protected Animator entityAnim = null;

    // Object property variables
    [Space, Header("Inventory & Skills")]
    [SerializeField] protected OwnedWeapons ownedWeapons = new OwnedWeapons();

    private AbstractAbility genericAbility = null;
    protected Action useAbilityAction = null;

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int m_currentOxygenLvl;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int m_currentHealth;

    public int Health { get => m_currentHealth; }
    public int Oxygen { get => m_currentOxygenLvl; }
    public bool HasAbility { get => useAbilityAction != null && genericAbility != null; }
    public Animator EntityAnimator { get => entityAnim; }

    #region Unity BuiltIn Methods
    /// <summary>
    /// Generally, it will initialize generic entity properties like health and oxygen level.
    /// </summary>
    protected virtual void Start()
    {
        m_currentHealth = maxHealth;
        m_currentOxygenLvl = maxOxygenLvl;
    }

    protected virtual void OnDestroy()
    {
        if (useAbilityAction != null)
            useAbilityAction = null;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Heal by amount.
    /// </summary>
    public virtual void Heal(int amount)
    {
        m_currentHealth += amount;
        if (m_currentHealth > maxHealth)
            m_currentHealth = maxHealth;
    }

    /// <summary>
    /// Hit by damage.
    /// </summary>
    public virtual void Hit(int damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth < 0)
            m_currentHealth = 0;
    }

    public void SetAbility(AbstractAbility ability)
    {
        // Ignore null ability
        if (ability == null)
            return;

        // Set up ability
        genericAbility = ability;
        useAbilityAction = genericAbility.UseAbility;
    }
    #endregion
}