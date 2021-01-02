using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class DMR : Weapon, IGunsInterface
{
    // Specific Weapon Attributes
    [Header("DMR Attributes")]
    [SerializeField] private GunBarrel gunBarrel = null;
    [SerializeField] private float reloadTime = 0f;
    [SerializeField] [Slider(0.8f, 1f)] private float accuracyPercentage = 0.5f;
    [SerializeField] private AnimationCurve scopingCurve = new AnimationCurve();
    [SerializeField] private float scopingTransitionDuration = 0.2f;
    [SerializeField] private bool isSingleShot = false;

    // Value Holders
    private IEnumerator m_shootRoutine = null;
    private IEnumerator m_scopeRoutine = null;
    private IEnumerator m_reloadRoutine = null;

    // Scoping Section
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 scopePhysicPosition = new Vector3();
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 normalPhysicPosition = new Vector3();

    // Debugging Section
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float fireRateHolder = 0f;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isShooting = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isScoping = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool isReloading = false;

    public Vector3 PhysicPositionScoping { get => scopePhysicPosition; }
    public Vector3 PhysicPositionNormal { get => normalPhysicPosition; }

    #region Unity Built-In Methods
    protected override void OnEnable()
    {
        // Setup weapon and attributes
        InputDataForWeapon.ResetInput();
        if (FlashLight.enabled)
            FlashLight.enabled = false; 
    }

    private void Start()
    {
        gunBarrel.BarrelReset();
        GenericBarrel = gunBarrel;
    }

    protected override void Update()
    {
        // Weapon react by input
        if (!isReloading)
        {
            if (InputDataForWeapon.ShootClicked)
                Shoot();
            if (InputDataForWeapon.ScopeClicked)
                Scope();
        }
        if (InputDataForWeapon.IsReloading)
            Reload();
        if (fireRateHolder > 0f)
            fireRateHolder -= Time.deltaTime;
    }

    protected override void OnDisable()
    {
        if (m_shootRoutine != null)
        {
            StopCoroutine(m_shootRoutine);
            isShooting = false;
        }

        if (m_reloadRoutine != null)
        {
            StopCoroutine(m_reloadRoutine);
            isReloading = false;
        }

        if (m_scopeRoutine != null)
        {
            StopCoroutine(m_scopeRoutine);
            transform.localPosition = normalPhysicPosition;
            HideMesh(false);
            isScoping = false;
        }

        InputDataForWeapon.ResetInput();
    }
    #endregion

    public void Shoot()
    {
        if (isReloading || isShooting)
            return;

        m_shootRoutine = ShootRoutine();
        StartCoroutine(m_shootRoutine);
    }

    public void Scope()
    {
        if (isReloading || isScoping)
            return;

        m_scopeRoutine = ScopeRoutine();
        StartCoroutine(m_scopeRoutine);
    }

    public void Reload()
    {
        if (isReloading || gunBarrel.WeaponStock >= gunBarrel.MaxCapacityWeapon)
            return;

        if (m_scopeRoutine != null)
            StopCoroutine(m_scopeRoutine);

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

    private Vector3 RandomDirectionPoint(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(gunBarrel.BarrelTransform.position, targetPosition);
        return Random.insideUnitSphere * distance * (1f - accuracyPercentage);
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        // TODO: Play VFX

        // As long as it's shooting then keep it running
        while (InputDataForWeapon.IsShooting)
        {
            if (fireRateHolder <= 0f)
            {
                // Shoot Bullet
                BulletScript bullet = gunBarrel.ReleaseBullet();
                Vector3 shootDir;
                if (!isScoping)
                    shootDir = ((InputDataForWeapon.MidTargetPosition + RandomDirectionPoint(InputDataForWeapon.MidTargetPosition)) -
                        bullet.transform.position).normalized;
                else
                    shootDir = (InputDataForWeapon.MidTargetPosition - bullet.transform.position).normalized;
                bullet.StartShoot(shootDir, DamageRate, Targets, gunBarrel.ShootForce);
                StartCoroutine(ShootlightPass());

                // Automatically reload when barrel is empty
                if (gunBarrel.WeaponStock <= 0)
                {
                    Reload();
                    break;
                }

                // Reset fire time holder
                fireRateHolder = FireRate;

                // Single shot means the routine immediately stop after the first shot
                if (isSingleShot)
                    break;
            }

            yield return null;
        }

        // TODO: Stop VFX

        isShooting = false;
    }

    private IEnumerator ScopeRoutine()
    {
        isScoping = true;
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
                _percent = 1f;
                HideMesh(true);
            }
            else
            {
                HideMesh(false);
            }

            _smoothPercent = scopingCurve.Evaluate(_percent);
            transform.localPosition = Vector3.Lerp(normalPhysicPosition, scopePhysicPosition, _smoothPercent);
            yield return null;
        } while (_percent > 0f);

        isScoping = false;
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        float m_reloadTimeHolder = reloadTime;

        // Reset physic scope position
        transform.localPosition = normalPhysicPosition;
        HideMesh(false);
        isScoping = false;

        //// TODO: Calculate the same time as animation time
        //// TODO: Play Animation

        while (m_reloadTimeHolder > 0f)
        {
            m_reloadTimeHolder -= Time.deltaTime;
            yield return null;
        }

        gunBarrel.ReloadWeapon();
        isReloading = false;
    }

    /// <summary>
    /// Light came out after shoot for 1 frame.
    /// </summary>
    private IEnumerator ShootlightPass()
    {
        FlashLight.enabled = true;
        yield return new WaitForSeconds(0.1f);
        FlashLight.enabled = false;
    }
}
