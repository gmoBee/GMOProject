using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class BotEntity : LivingEntity, IAgentControl
{
    [Header("Bot Intelligent Attributes")]
    [SerializeField] private float thinkingSpeed = 2f;
    [SerializeField] private float turnDamping = 15f;
    [SerializeField] private LayerMask enemyTargetLayer = ~0;

    [Header("Bot Sight and Search Attributes")]
    [SerializeField] private float sightRadius = 8f;
    [SerializeField] private Transform botEyes = null;

    private NavMeshAgent m_agent;

    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private float makeDecisionIn;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private Transform lookTarget;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private Transform followTarget;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private bool hasResetBrain = false;
    [BoxGroup("AI DEBUG")] [SerializeField] [ReadOnly] private bool isChangingWeapon = false;

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    protected override void OnEnable()
    {
        hasResetBrain = false;
    }

    private void Start()
    {
        GetAllComponents();
        ResetEntity();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!IsDead)
        {
            HandleMovement();
            HandleAction();
        }
        else
        {
            if (!hasResetBrain)
                ResetEntity();
        }
    }

    protected override void OnDisable()
    {
        
    }

    protected override void OnDestroy()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
    #endregion

    #region Initialize Methods
    private void GetAllComponents()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }
    #endregion

    #region Update Handler Methods
    private void HandleMovement()
    {
        // Feel and try look on target
        if (lookTarget == null)
        {
            NearestEntitySearch(ref lookTarget);
        }
        else
        {
            // Looking at target
            Vector3 lookPos = lookTarget.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * turnDamping);

            // Check if distance between target and bot is too far
            if (Vector3.Distance(transform.position, lookTarget.position) > sightRadius)
                lookTarget = null;
        }

        // Chase target
        if (followTarget != null)
        {
            m_agent.destination = followTarget.position;

            // Check if target to follow is too far
            if (Vector3.Distance(transform.position, followTarget.position) > sightRadius)
                followTarget = null;
        }
    }

    private void HandleAction()
    {
        makeDecisionIn -= Time.deltaTime;
        if (makeDecisionIn <= 0f)
        {
            makeDecisionIn = thinkingSpeed;
            UsingBotLogic();
        }
    }

    private void UsingBotLogic()
    {
        // Check target follow
        if (followTarget == null)
        {
            if (!NearestEntitySearch(ref followTarget))
                m_agent.destination = RandomPathWondering();
        }
        else
        {
            //TODO: if it is an enemy then bot will attack
        }
    }

    /// <summary>
    /// Searching for target.
    /// </summary>
    /// <returns>true if found, else then false</returns>
    private bool NearestEntitySearch(ref Transform targetRef)
    {
        List<Collider> hits = new List<Collider>(Physics.OverlapSphere(transform.position, sightRadius, enemyTargetLayer.value));
        while (hits.Count != 0)
        {
            int indexTarget = Random.Range(0, hits.Count);
            if (hits[indexTarget].gameObject.Equals(gameObject))
            {
                hits.RemoveAt(indexTarget);
                continue;
            }

            targetRef = hits[indexTarget].transform;
            return true;
        }
        return false;
    }

    protected override void HandleAbilityUsage()
    {
        // TODO: bot has ability
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Only if the bot is doing idle.
    /// </summary>
    /// <returns>Target walk to position</returns>
    private Vector3 RandomPathWondering()
    {
        Vector3 randomDirection = Random.insideUnitSphere * sightRadius + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, sightRadius, LayerMask.GetMask("Default"));
        return navHit.position;
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
    #endregion

    #region Overriden Methods from Parent Class
    protected override void WeaponHandInit(Weapon handle)
    {
        // Close previous weapon
        if (CurrentOwnedWeapons.HoldOnHand != null)
            CurrentOwnedWeapons.HoldOnHand.gameObject.SetActive(false);

        // Handle on hand with new one
        // TODO: Connect with animation
        ownedWeapons.HoldOnHand = handle;
        if (CurrentOwnedWeapons.HoldOnHand != null)
            CurrentOwnedWeapons.HoldOnHand.gameObject.SetActive(true);
    }

    public override void Heal(uint amount)
    {
        Health += amount;
    }

    public override void Hit(uint damage)
    {
        Health -= damage;
    }

    public override void SetAbility(GameAbilityList ability)
    {
        choosenAbility = ability;
        Ability = AbilityFactory.ChooseAbility(ability, this);
    }

    public override void ResetEntity()
    {
        // Set attributes
        Health = MaxHealth;
        OxygenLevel = MaxOxygenLvl;
        SetAbility(choosenAbility);

        // Initialize weapon
        if (CurrentOwnedWeapons.HasWeapon)
        {
            if (CurrentOwnedWeapons.PrimaryWeapon != null)
                WeaponHandInit(CurrentOwnedWeapons.PrimaryWeapon);
            else
                WeaponHandInit(CurrentOwnedWeapons.SecondaryWeapon);
        }

        // Set bot attributes
        makeDecisionIn = thinkingSpeed;
        followTarget = null;
        hasResetBrain = true;
    }

    protected override IEnumerator WeaponRollRoutine()
    {
        isChangingWeapon = true;

        yield return null;

        isChangingWeapon = false;
    }
    #endregion
}
