using System;
using UnityEngine;
using NaughtyAttributes;

[Flags]
public enum BotStatusState
{
    DoNothing = 0,
    Idle = 1,
    Search = 2,
    Attack = 4,
    Defend = 8,
    Follow = 16
}

public class BotDecisionMaker : MonoBehaviour
{
    public delegate void BotMakeDecision(BotStatusState state);
    public event BotMakeDecision OnDecisionMake;

    [Header("Attributes")]
    [SerializeField] private float thinkingSpeed = 1f;
    [SerializeField] private BotEntity entity = null;

    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float m_makeDecisionIn;

    public float SecondsUntilMakeDecision
    {
        set => m_makeDecisionIn = value;
        get => m_makeDecisionIn;
    }

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    private void Start()
    {
        ResetMind();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleAction();
    }

    private void OnDestroy()
    {
        ResetMind();
    }
    #endregion

    private void HandleAction()
    {
        m_makeDecisionIn -= Time.deltaTime;
        // Handle decision making
        if (m_makeDecisionIn <= 0f)
        {
            m_makeDecisionIn = thinkingSpeed;
            OnDecisionMake?.Invoke(entity.CurrentState);
        }
    }

    public void ResetMind()
    {
        m_makeDecisionIn = thinkingSpeed;
    }
}
