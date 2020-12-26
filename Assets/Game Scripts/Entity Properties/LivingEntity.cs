using System;
using UnityEngine;
using NaughtyAttributes;

public abstract class LivingEntity : MonoBehaviour
{
    // All Properties are here
    [Space, Header("Entity Properties", order = 0)]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int maxOxygenLvl = 100;
    [SerializeField] protected Animator entityAnim = null;

    // Object property variables
    [Space, Header("Inventory & Skills")]
    [SerializeField] protected OwnedWeapons ownedWeapons = new OwnedWeapons();
    [SerializeField] private GameAbilityList choosenAbility = GameAbilityList.Nothing;

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
        SetAbility(choosenAbility);
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

    public void SetAbility(GameAbilityList ability)
    {
        // Set up ability
        choosenAbility = ability;
        genericAbility = AbilityFactory.ChooseAbility(ability, this);
        if (genericAbility != null)
            useAbilityAction = genericAbility.UseAbility;
    }

    /// <summary>
    /// Get this entity current ability, It may return null if don't have it.
    /// </summary>
    /// <returns>Ability or null</returns>
    public AbstractAbility GetAbility()
    {
        return genericAbility;
    }
    #endregion

    // Generic Methods
    public abstract void HandleAbilityUsage();
}