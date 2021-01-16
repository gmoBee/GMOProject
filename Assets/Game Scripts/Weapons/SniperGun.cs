using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class SniperGun : Weapon, IGunsWithScopeInterface
{
    // Specific Weapon Attributes
    [Header("Sniper Gun Attributes")]
    [SerializeField] private GunBarrel gunBarrel = null;
    [SerializeField] private float reloadTime = 0f;
    [SerializeField] [Slider(0.8f, 1f)] private float accuracyPercentage = 0.5f;
    [SerializeField] private AnimationCurve scopingCurve = new AnimationCurve();
    [SerializeField] private float scopingTransitionDuration = 0.2f;
    [SerializeField] private bool isSingleShot = false;
    [SerializeField] private bool scopeLocked = false;
    [SerializeField] private VisualEffect burstEffect = null;

    // Event
    [SerializeField] private UnityEvent shootEvent;
    private UnityAction shootAction;

    // Value Holders
    private IEnumerator m_shootRoutine = null;
    private IEnumerator m_scopeRoutine = null;
    private IEnumerator m_reloadRoutine = null;

    private float m_lightIntensity;

    // Scoping Section
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 m_scopePhysicPosition = new Vector3();
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 m_normalPhysicPosition = new Vector3();

    // Debugging Section
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_fireRateHolder = 0f;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isShooting = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isScoping = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isReloading = false;

    public Vector3 PhysicPositionScoping { get => m_scopePhysicPosition; }
    public Vector3 PhysicPositionNormal { get => m_normalPhysicPosition; }

    #region Unity Built-In Methods
    protected override void OnEnable()
    {
        // Setup weapon and attributes
        InputDataForWeapon.ResetInput();
        if (FlashLight.enabled)
            FlashLight.enabled = false;
        m_lightIntensity = FlashLight.intensity;
        if (burstEffect != null)
        {
            burstEffect.enabled = false;
            burstEffect.Stop();
        }
    }

    private void Start()
    {
        gunBarrel.BarrelReset();
        GenericBarrel = gunBarrel;
        shootAction = OnShootEventCallback;
        shootEvent.AddListener(shootAction);
    }

    protected override void Update()
    {
        // Handle Method
        HandleDetector();
        HandleGunUsage();
    }

    protected override void OnDisable()
    {
        if (m_shootRoutine != null)
        {
            StopCoroutine(m_shootRoutine);
            m_isShooting = false;
        }

        if (m_reloadRoutine != null)
        {
            StopCoroutine(m_reloadRoutine);
            m_isReloading = false;
        }

        if (m_scopeRoutine != null)
        {
            StopCoroutine(m_scopeRoutine);
            transform.localPosition = m_normalPhysicPosition;
            HideMesh(false);
            m_isScoping = false;
        }

        InputDataForWeapon.ResetInput();
        shootEvent.RemoveListener(shootAction);
    }
    #endregion

    #region Handler Method
    private void HandleGunUsage()
    {
        // Weapon react by input
        if (!m_isReloading)
        {
            if (InputDataForWeapon.ShootClicked)
                Shoot();
            if (InputDataForWeapon.ScopeClicked && !scopeLocked)
                Scope();
        }
        if (InputDataForWeapon.IsReloading)
            Reload();
        if (m_fireRateHolder > 0f)
            m_fireRateHolder -= Time.deltaTime;
    }

    private void HandleDetector()
    {
        // Crosshair management only for main player
        if (OnHandOwner.tag == "Main Player")
        {
            if (m_isScoping)
            {
                PlayerUIManager.instance.ChangeCrosshairState(CrosshairState.Scope);
                return;
            }

            Vector3 origin = gunBarrel.BarrelTransform.position;
            Vector3 shootDir = (InputDataForWeapon.MidTargetPosition - origin).normalized;
            float distance = Vector3.Distance(origin, InputDataForWeapon.MidTargetPosition);
            RaycastHit hit;
            if (Physics.Raycast(origin, shootDir, out hit, distance))
            {
                LivingEntity entity = hit.collider.GetComponent<LivingEntity>();
                if (entity != null)
                {
                    if (entity.RelationID != OnHandOwner.RelationID)
                    {
                        PlayerUIManager.instance.ChangeCrosshairState(CrosshairState.Detect);
                        return;
                    }
                }
            }

            PlayerUIManager.instance.ChangeCrosshairState(CrosshairState.Default);
        }
    }
    #endregion

    #region Custom Methods
    public void Shoot()
    {
        if (m_isReloading || m_isShooting)
            return;

        m_shootRoutine = ShootRoutine();
        StartCoroutine(m_shootRoutine);
    }

    public void Scope()
    {
        if (m_isReloading || m_scopeRoutine != null)
            return;

        m_scopeRoutine = ScopeRoutine();
        StartCoroutine(m_scopeRoutine);
    }

    public void Reload()
    {
        if (m_isReloading || gunBarrel.WeaponStock >= gunBarrel.MaxCapacityWeapon)
            return;

        if (m_scopeRoutine != null)
        {
            StopCoroutine(m_scopeRoutine);
            m_isScoping = false;
            m_scopeRoutine = null;
        }

        if (m_shootRoutine != null)
            StopCoroutine(m_shootRoutine);

        m_reloadRoutine = ReloadRoutine();
        StartCoroutine(m_reloadRoutine);
    }

    private void HideMesh(bool hide)
    {
        MeshRenderer[] render = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedRender = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (MeshRenderer r in render)
            r.enabled = !hide;
        foreach (SkinnedMeshRenderer sr in skinnedRender)
            sr.enabled = !hide;
    }

    private void OnShootEventCallback()
    {
        StartCoroutine(ShootlightPass());
    }

    private Vector3 RandomDirectionPoint(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(gunBarrel.BarrelTransform.position, targetPosition);
        return Random.insideUnitSphere * distance * (1f - accuracyPercentage);
    }

    private IEnumerator ShootRoutine()
    {
        m_isShooting = true;

        // TODO: Play VFX

        // As long as it's shooting then keep it running
        while (InputDataForWeapon.IsShooting)
        {
            if (m_fireRateHolder <= 0f)
            {
                // Shoot Bullet
                BulletScript bullet = gunBarrel.ReleaseBullet();
                Vector3 shootDir;
                if (!m_isScoping)
                    shootDir = (InputDataForWeapon.MidTargetPosition + RandomDirectionPoint(InputDataForWeapon.MidTargetPosition) -
                        bullet.transform.position).normalized;
                else
                    shootDir = (InputDataForWeapon.MidTargetPosition - bullet.transform.position).normalized;
                bullet.StartShoot(shootDir, DamageRate, OnHandOwner == null ? 0 : OnHandOwner.RelationID, Targets, gunBarrel.ShootForce);
                shootEvent?.Invoke();

                // Automatically reload when barrel is empty
                if (gunBarrel.WeaponStock <= 0)
                {
                    Reload();
                    break;
                }

                // Reset fire time holder
                m_fireRateHolder = FireRate;

                // Single shot means the routine immediately stop after the first shot
                if (isSingleShot)
                    break;
            }

            yield return null;
        }

        // TODO: Stop VFX

        m_isShooting = false;
    }

    private IEnumerator ScopeRoutine()
    {
        
        float _percent = 0f;
        float _smoothPercent = 0f;

        float _speed = 1f / scopingTransitionDuration;

        do {
            if (InputDataForWeapon.IsScoping)
                _percent += Time.deltaTime * _speed;
            else
                _percent -= Time.deltaTime * _speed;

            if (_percent >= 1f)
            {
                m_isScoping = true;
                _percent = 1f;
                HideMesh(true);
            }
            else
            {
                m_isScoping = false;
                HideMesh(false);
            }

            _smoothPercent = scopingCurve.Evaluate(_percent);
            transform.localPosition = Vector3.Lerp(m_normalPhysicPosition, m_scopePhysicPosition, _smoothPercent);
            yield return null;
        } while (_percent > 0f);
        m_scopeRoutine = null;
    }

    private IEnumerator ReloadRoutine()
    {
        m_isReloading = true;

        float m_reloadTimeHolder = reloadTime;

        // Reset physic scope position
        transform.localPosition = m_normalPhysicPosition;
        HideMesh(false);
        m_isScoping = false;

        //// TODO: Calculate the same time as animation time
        //// TODO: Play Animation

        while (m_reloadTimeHolder > 0f)
        {
            m_reloadTimeHolder -= Time.deltaTime;
            yield return null;
        }

        gunBarrel.ReloadWeapon();
        m_isReloading = false;
    }

    /// <summary>
    /// Light came out after shoot for 1 frame.
    /// </summary>
    private IEnumerator ShootlightPass()
    {
        FlashLight.enabled = true;
        FlashLight.intensity = m_lightIntensity;
        float lifetime = Time.deltaTime, intReductionPerFrame = m_lightIntensity / Time.deltaTime;
        if (burstEffect != null)
        {
            burstEffect.enabled = true;
            burstEffect.Play();
            lifetime = burstEffect.GetFloat("Splash Lifetime");
        }

        while (lifetime > 0f)
        {
            yield return null;
            lifetime -= Time.deltaTime;
            FlashLight.intensity -= intReductionPerFrame;
        }

        FlashLight.enabled = false;
        if (burstEffect != null)
        {
            burstEffect.enabled = false;
            burstEffect.Stop();
        }
    }
    #endregion
}
