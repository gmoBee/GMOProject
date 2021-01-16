using UnityEngine;
using NaughtyAttributes;

public abstract class BotEntity : LivingEntity
{
    [Header("General Bot Attributes")]
    [SerializeField] private BotStaticController autoController = null;
    [SerializeField] private LayerMask detectLayer = ~0;
    [SerializeField] private BotDecisionMaker decisionMaker = null;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private BotStatusState state;

    public BotStaticController AutoController => autoController;
    public LayerMask DetectLayer => detectLayer;
    public BotDecisionMaker DecisionMaker => decisionMaker;
    public BotStatusState CurrentState
    {
        set => state = value;
        get => state;
    }

    #region Overriden Methods from Parent Class
    public override void Heal(uint amount)
    {
        if (Health + amount > MaxHealth)
            Health = MaxHealth;
        else
            Health += amount;
    }

    public override void Hit(uint damage)
    {
        if (damage > Health)
            Health = 0;
        else
            Health -= damage;
    }

    public override void ResetEntity()
    {
        // Set attributes
        Health = MaxHealth;
        OxygenLevel = MaxOxygenLvl;
        autoController.ResetBotControl();
        IsDying = false;
        ResetChildEntity();
    }

    public override bool CheckRelation(LivingEntity entity)
    {
        return entity.RelationID == RelationID;
    }
    #endregion

    #region Generic Custom Methods
    protected abstract void ResetChildEntity();
    #endregion
}
