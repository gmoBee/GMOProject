using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public abstract class LivingEntity : MonoBehaviour
{
    // All Properties are here
    [Space, Header("Entity Attributes", order = 0)]
    [SerializeField] private uint maxHealth = 100;
    [SerializeField] private uint maxOxygenLvl = 100;
    [SerializeField] private Animator entityAnim = null;

    // Object property variables
    [Space, Header("Inventory & Skills")]
    [SerializeField] protected OwnedWeapons ownedWeapons = new OwnedWeapons();
    [SerializeField] protected GameAbilityList choosenAbility = GameAbilityList.Nothing;

    private AbstractAbility genericAbility = null;

    // Debugger
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private uint currentOxygenLvl;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private uint currentHealth;

    public bool IsDead => currentHealth == 0;
    public bool HasAbility => genericAbility != null;
    public Animator EntityAnimator => entityAnim;
    public uint MaxHealth => maxHealth;
    public uint MaxOxygenLvl => maxOxygenLvl;
    public OwnedWeapons CurrentOwnedWeapons => ownedWeapons;

    public uint Health
    {
        get => currentHealth;
        protected set
        {
            if (value > maxHealth)
                currentHealth = maxHealth;
            else
                currentHealth = value;
        }
    }
    public uint OxygenLevel
    {
        get => currentOxygenLvl;
        protected set
        {
            if (value > maxOxygenLvl)
                currentOxygenLvl = maxHealth;
            else
                currentOxygenLvl = value;
        }
    }

    public AbstractAbility Ability
    {
        get => genericAbility;
        protected set => genericAbility = value;
    }

    #region Generic Unity BuiltIn Methods
    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected abstract void OnDestroy();
    #endregion

    #region Custom Generic Methods
    protected abstract void WeaponHandInit(Weapon handle);
    protected abstract void HandleAbilityUsage();
    protected abstract IEnumerator WeaponRollRoutine();
    public abstract void Heal(uint amount);
    public abstract void Hit(uint damage);
    public abstract void SetAbility(GameAbilityList ability);
    public abstract void ResetEntity();
    #endregion
}