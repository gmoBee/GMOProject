using UnityEngine;
using NaughtyAttributes;

public class BotStaticController : MonoBehaviour
{
    [Header("Static Controller Attributes")]
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private BotFieldOfView fieldOfView;
    [SerializeField] private Transform upperBody = null;
    [SerializeField] [MinMaxSlider(-90f, 90f)] private Vector2 lookAngleMinMax = Vector2.zero;

    [Header("Other Attributes")]
    [SerializeField] private BotEntity controlBot = null;

    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private Transform m_lookTarget = null;
    
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float m_targetYaw = 0f;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float m_targetPitch = 0f;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private bool m_rotationEnabled = true;

    private Vector3 m_desiredAngle;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_sightYaw = 0f;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_sightPitch = 0f;

    public Transform SeesTarget => m_lookTarget;
    public BotEntity ControlEntity => controlBot;
    public float CoverRadius => fieldOfView.FarSight;
    public bool RotationEnabled
    {
        get => m_rotationEnabled;
        set
        {
            if (!value)
                upperBody.localEulerAngles = Vector3.zero;
            m_rotationEnabled = value;
        }
    }

    #region Unity BuiltIn Methods
    protected virtual void Start()
    {
        ResetBotControl();
        ControlEntity.DecisionMaker.OnDecisionMake += SightDecision;
    }

    protected virtual void Update()
    {
        HandleBotTargetSight();
        if (m_rotationEnabled)
            HandleBotRotation();
    }

    protected virtual void OnDestroy()
    {
        ControlEntity.DecisionMaker.OnDecisionMake -= SightDecision;
    }

    #endregion

    #region Handle Methods
    private void HandleBotTargetSight()
    {
        // Scan field of view
        fieldOfView.HandleSight(new Vector3(upperBody.eulerAngles.x, transform.rotation.eulerAngles.y, upperBody.eulerAngles.z));

        // Handle bot sight control when bot sees a target
        if (m_lookTarget != null)
        {
            // Looking at target, locked rotation at y axis
            Vector3 m_lookDir = (m_lookTarget.position - upperBody.position).normalized;
            m_desiredAngle = Quaternion.LookRotation(m_lookDir).eulerAngles;
            m_desiredAngle.z = 0f;

            // Set target on look target
            m_targetYaw = m_desiredAngle.y;
            m_targetPitch = m_desiredAngle.x;

            // Check if distance between target and bot is too far
            if (Vector3.Distance(m_lookTarget.position, transform.position) > fieldOfView.FarSight)
            {
                // Check if bot is still able to see player
                RaycastHit hit;
                if (Physics.Raycast(upperBody.position, m_lookDir, out hit))
                {
                    if (!hit.transform.Equals(m_lookTarget))
                        m_lookTarget = null;
                }
            }
        }
        else
        {
            // Immediately make decision after sees the enemy on bot sight
            if (NearestEntitySearch())
                ControlEntity.DecisionMaker.SecondsUntilMakeDecision = 0f;
        }
    }

    private void HandleBotRotation()
    {
        // Determine rotation direction
        float m_yawDir, m_pitchDir;

        // Handle Yaw
        if (Mathf.Abs(m_sightYaw - m_targetYaw) > 0f)
        {
            float m_rawDegreeLength = (m_targetYaw - m_sightYaw) % 360f;
            // Normalize Rotation
            if (m_rawDegreeLength > 180f)
                m_rawDegreeLength -= 360f;
            else if (m_rawDegreeLength < -180f)
                m_rawDegreeLength += 360f;

            if (Mathf.Abs(m_rawDegreeLength) < rotationSpeed)
            {
                m_sightYaw = m_targetYaw;
                m_yawDir = 0f;
            }
            else
            {
                // Assign direction
                if (m_rawDegreeLength > 0f)
                    m_yawDir = 1f;
                else
                    m_yawDir = -1f;
            }
        }
        else
            m_yawDir = 0f;

        // Handle Pitch
        if (Mathf.Abs(m_sightPitch - m_targetPitch) > 0f)
        {
            float m_rawDegreeLength = (m_targetPitch - m_sightPitch) % 360f;
            // Normalize Rotation
            if (m_rawDegreeLength > 180f)
                m_rawDegreeLength -= 360f;
            else if (m_rawDegreeLength < -180f)
                m_rawDegreeLength += 360f;

            if (Mathf.Abs(m_rawDegreeLength) < rotationSpeed)
            {
                m_sightPitch = m_targetPitch > 180f ? m_targetPitch - 360f : m_targetPitch;
                m_pitchDir = 0f;
            }
            else
            {
                // Assign direction
                if (m_rawDegreeLength > 0f)
                    m_pitchDir = 1f;
                else
                    m_pitchDir = -1f;
            }
        }
        else
            m_pitchDir = 0f;

        // Calculate by direction move
        m_sightYaw += m_yawDir * rotationSpeed;
        m_sightPitch = Mathf.Clamp(m_sightPitch + m_pitchDir * rotationSpeed, lookAngleMinMax.x, lookAngleMinMax.y);

        // Assign current rotation
        transform.eulerAngles = new Vector3(0f, m_sightYaw, 0f);
        upperBody.localEulerAngles = new Vector3(m_sightPitch, 0f, 0f);
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Searching for target.
    /// </summary>
    private bool NearestEntitySearch()
    {
        int indexTarget = 0;
        while (indexTarget < fieldOfView.CaughtOnSight.Count)
        {
            Transform m_target = fieldOfView.CaughtOnSight[indexTarget];
            if (m_target.Equals(transform))
            {
                fieldOfView.CaughtOnSight.RemoveAt(indexTarget);
                continue;
            }

            if (controlBot.DetectLayer != (controlBot.DetectLayer | 1 << m_target.gameObject.layer))
            {
                indexTarget++;
                continue;
            }

            m_lookTarget = m_target;
            return true;
        }
        return false;
    }

    private void RandomLookWondering(bool yawOnly)
    {
        m_targetYaw = Random.Range(0f, 360f);
        if (!yawOnly)
            m_targetPitch = Random.Range(lookAngleMinMax.x, lookAngleMinMax.y);
        else
            m_targetPitch = 0f;
    }

    protected virtual void SightDecision(BotStatusState state)
    {
        if (m_lookTarget == null && m_rotationEnabled)
            RandomLookWondering(true);
    }

    public void SetTargetLook(Vector3 lookEulerAngle)
    {
        m_targetYaw = lookEulerAngle.y;
        m_targetPitch = lookEulerAngle.x;

        ControlEntity.DecisionMaker.SecondsUntilMakeDecision += (m_targetYaw - m_sightYaw) / rotationSpeed;
    }

    public void InstantLookAt(Vector3 lookEulerAngle)
    {
        m_sightYaw = lookEulerAngle.y;
        m_sightPitch = Mathf.Clamp(lookEulerAngle.x, lookAngleMinMax.x, lookAngleMinMax.y);

        // Assign current rotation
        transform.eulerAngles = new Vector3(0f, m_sightYaw, 0f);
        upperBody.localEulerAngles = new Vector3(m_sightPitch, 0f, 0f);
    }

    public virtual void ResetBotControl()
    {
        // Set bot attributes
        m_lookTarget = null;
        m_sightPitch = 0f;
        InstantLookAt(transform.rotation.eulerAngles);
    }
    #endregion
}
