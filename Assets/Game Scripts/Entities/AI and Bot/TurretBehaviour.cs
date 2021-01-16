using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TurretBehaviour : BotEntity, IGunsInterface, IEntityDeath
{
    [Header("Turret Attributes")]
    [SerializeField] private GunBarrel barrel = null;
    [SerializeField] private uint damagePerBullet = 3;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] [Slider(0.1f, 2f)] private float fireRate = 1f;
    [SerializeField] [Slider(0.8f, 1f)] private float accuracy = 1f;
    [SerializeField] private Animator turretAnim = null;
    [SerializeField] private List<string> targetTags = new List<string>();

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isAutoShooting = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_shootSecondsIn = 0f;

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    private void Start()
    {
        ResetEntity();
        barrel.BarrelReset();
        DecisionMaker.OnDecisionMake += TurretAutoDecision;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsDead)
        {
            HandleAutoDetect();
            HandleAutoShoot();
        }
        else
        {
            if (!IsDying)
            {
                IsDying = true;
                AutoController.RotationEnabled = false;
                StartCoroutine(DyingRoutine());
            }
        }
    }

    private void OnDestroy()
    {
        DecisionMaker.OnDecisionMake -= TurretAutoDecision;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AutoController.CoverRadius);
    }
    #endregion

    #region Handler Methods
    private void HandleAutoDetect()
    {
        if (AutoController.SeesTarget == null)
        {
            // Guarding area
            Collider[] areaDetect = Physics.OverlapSphere(transform.position, AutoController.CoverRadius, DetectLayer.value);
            float distance = Mathf.Infinity;
            Transform nearestTarget = null;
            foreach (Collider c in areaDetect)
            {
                if (c.gameObject.Equals(gameObject))
                    continue;

                LivingEntity entity = c.GetComponent<LivingEntity>();
                if (entity != null && Vector3.Distance(c.transform.position, transform.position) < distance)
                {
                    distance = Vector3.Distance(entity.transform.position, transform.position);
                    nearestTarget = c.transform;
                }
            }

            // If found the target
            if (nearestTarget != null)
                AutoController.SetTargetLook(Quaternion.LookRotation(nearestTarget.position - transform.position).eulerAngles);
        }
    }

    private void HandleAutoShoot()
    {
        if (AutoController.SeesTarget != null)
        {
            m_shootSecondsIn -= Time.deltaTime;
            if (m_shootSecondsIn <= 0f)
            {
                Shoot();
                m_isAutoShooting = true;
                m_shootSecondsIn = fireRate;
            }
        }
        else
        {
            if (m_isAutoShooting)
                m_isAutoShooting = false;
        }
    }
    #endregion

    #region Custom Methods
    public void Shoot()
    {
        // Determine direction with accuracy
        Vector3 randomSpherePoint = Random.insideUnitSphere * Vector3.Distance(barrel.BarrelTransform.position, 
            AutoController.SeesTarget.position) * (1f - accuracy);
        Vector3 shootDir = (AutoController.SeesTarget.position + randomSpherePoint - 
            barrel.BarrelTransform.position).normalized;

        // Start Bullet shoot
        if (barrel.WeaponStock == 0)
            Reload();
        BulletScript bullet = barrel.ReleaseBullet();
        if (bullet.BaseDamage != damagePerBullet)
            bullet.BaseDamage = damagePerBullet;
        bullet.StartShoot(shootDir, 0, RelationID, targetTags, projectileSpeed);
    }

    public void Reload()
    {
        barrel.ReloadWeapon();
        barrel.Restock(barrel.MaxCapacityWeapon);
    }

    public void InstantDeath()
    {
        Hit(MaxHealth);
    }

    private void TurretAutoDecision(BotStatusState state)
    {

    }

    private IEnumerator DyingRoutine()
    {
        // Default dying time
        float dyingTime = 10f;

        while (dyingTime > 0f)
        {
            yield return null;
            dyingTime -= Time.deltaTime;
        }

        // Deactivate object
        gameObject.SetActive(false);
    }
    #endregion

    #region Override Methods from Parent Class
    protected override void ResetChildEntity()
    {
        m_isAutoShooting = false;
    }
    #endregion
}
