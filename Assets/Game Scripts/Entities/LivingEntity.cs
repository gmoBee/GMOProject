using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public abstract class LivingEntity : MonoBehaviour
{
    // All Properties are here
    [Space, Header("Entity Attributes", order = 0)]
    [SerializeField] private uint maxHealth = 100;
    [SerializeField] private uint maxOxygenLvl = 100;
    [SerializeField] private uint entityRelationID = 0;

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private uint m_currentOxygenLvl;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private uint m_currentHealth;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isDying = false;

    public bool IsDead => m_currentHealth == 0;
    public uint MaxHealth => maxHealth;
    public uint MaxOxygenLvl => maxOxygenLvl;

    protected bool IsDying
    {
        get => m_isDying;
        set => m_isDying = value;
    }

    public uint Health
    {
        get => m_currentHealth;
        protected set
        {
            if (value > maxHealth)
                m_currentHealth = maxHealth;
            else
                m_currentHealth = value;
        }
    }
    public uint OxygenLevel
    {
        get => m_currentOxygenLvl;
        protected set
        {
            if (value > maxOxygenLvl)
                m_currentOxygenLvl = maxHealth;
            else
                m_currentOxygenLvl = value;
        }
    }

    public uint RelationID
    {
        get => entityRelationID;
        set => entityRelationID = value;
    }

    public IEntitySpawner SpawnPoint { get; set; }

    #region Generic Custom Methods
    public abstract void Heal(uint amount);
    public abstract void Hit(uint damage);
    public abstract void ResetEntity();

    /// <summary>
    /// Check relation between 2 entity.
    /// </summary>
    /// <param name="entity">Other entity</param>
    /// <returns>True if they are friends, else then false</returns>
    public abstract bool CheckRelation(LivingEntity entity);
    #endregion
}