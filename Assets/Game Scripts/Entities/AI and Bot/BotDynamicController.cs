using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

[RequireComponent(typeof(NavMeshAgent))]
public class BotDynamicController : BotStaticController
{
    [Header("Dynamic Controller Attributes")]
    [SerializeField] private float radiusIdleArea = 1f;
    [SerializeField] private float secondsMoveStopper = 3f;
    [SerializeField] private float targetStopDistance = 3f;
    [SerializeField] private float lostTargetDistance = 15f;
    [SerializeField] private float lostTargetDuration = 3f;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private NavMeshAgent m_agent;

    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private Transform m_followTarget = null;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private Vector3 m_lastTargetPosition = Vector3.zero;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float m_stopMovingIn = 0f;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private bool m_isStopMoving = false;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private bool m_keepFollowTarget = false;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float m_lookingForLostTarget = 0f;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float m_stopDistanceHolder;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private bool m_movementLocked = false;

    public bool UseAgentRotation
    {
        set
        {
            if (m_agent.updateRotation == value)
                return;

            RotationEnabled = !value;
            InstantLookAt(transform.eulerAngles);
            m_agent.updateRotation = value;
        }
    }

    public bool MovementLocked
    {
        get => m_movementLocked;
        set
        {
            m_movementLocked = value;
            UseAgentRotation = false;
        }
    }

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (m_agent == null)
            m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;

        ControlEntity.DecisionMaker.OnDecisionMake += MoveDecision;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!m_movementLocked)
            HandleMovement();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ControlEntity.DecisionMaker.OnDecisionMake -= MoveDecision;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusIdleArea);
        Gizmos.DrawWireSphere(transform.position, lostTargetDistance);
    }
    #endregion

    #region Handle Methods
    private void HandleMovement()
    {
        // Handle bot follow target movement
        if (m_followTarget != null)
        {
            // Check if target to follow is too far
            if (m_isStopMoving && SeesTarget == null && !m_keepFollowTarget)
                m_followTarget = null;
        }

        // Handle looking for lost target
        if (m_lookingForLostTarget > 0f)
        {
            m_lookingForLostTarget -= Time.deltaTime;
            UseAgentRotation = false;
            return;
        }

        // Handle bot walk or follow
        if (!m_isStopMoving)
        {
            if (SeesTarget == null)
            {
                m_stopMovingIn -= Time.deltaTime;
                UseAgentRotation = true;
            }
            else
            {
                UseAgentRotation = false;
            }

            if (m_followTarget != null)
                m_lastTargetPosition = m_followTarget.position;
            m_agent.destination = m_lastTargetPosition;
        }
        // Handle stop movement
        if (m_stopMovingIn <= 0f)
        {
            if (m_keepFollowTarget)
            {
                m_stopMovingIn = Vector3.Distance(transform.position, m_lastTargetPosition) / m_agent.speed;
                if (m_followTarget != null)
                {
                    if (Vector3.Distance(transform.position, m_followTarget.position) > lostTargetDistance)
                    {
                        m_followTarget = null;
                        m_agent.stoppingDistance = 0f;
                    }
                }
                else
                {
                    ReleaseTarget();
                    m_lookingForLostTarget = lostTargetDuration;
                }
            }
            else
            {
                m_stopMovingIn = secondsMoveStopper;
                if (!transform.position.Equals(m_lastTargetPosition))
                    m_lastTargetPosition = transform.position;
                m_isStopMoving = true;
                m_agent.stoppingDistance = 0f;
            }
        }
    }
    #endregion

    private void MoveDecision(BotStatusState state)
    {
        // Check target follow
        if (SeesTarget != null)
            SetFollowTarget(SeesTarget, targetStopDistance);
        else
            if (!m_keepFollowTarget)
                RandomPathWondering();
        m_isStopMoving = false;
    }

    /// <summary>
    /// Only if the bot is doing idle.
    /// </summary>
    private void RandomPathWondering()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radiusIdleArea + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, radiusIdleArea, LayerMask.GetMask("Default", "Ground"));
        m_lastTargetPosition = navHit.position;
    }

    public void SetFollowTarget(Transform target, float distance)
    {
        m_followTarget = target;
        m_keepFollowTarget = true;
        m_stopDistanceHolder = distance;
        m_agent.stoppingDistance = m_stopDistanceHolder;
    }

    public void ReleaseTarget()
    {
        m_followTarget = null;
        m_keepFollowTarget = false;
    }

    public void GotoPosition(Vector3 pos)
    {
        m_lastTargetPosition = pos;
        m_stopMovingIn = Vector3.Distance(transform.position, m_lastTargetPosition) / m_agent.speed;
    }

    public override void ResetBotControl()
    {
        base.ResetBotControl();
        m_followTarget = null;
        m_stopMovingIn = secondsMoveStopper;
        m_lastTargetPosition = transform.position;
        m_stopDistanceHolder = targetStopDistance;
    }
}
