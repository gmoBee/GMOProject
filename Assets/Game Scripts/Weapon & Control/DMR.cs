using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

public class DMR : Weapon, IGunsInterface
{
    // Specific Weapon Attributes
    [Header("DMR Attributes")]
    [SerializeField] private GunBarrel gunBarrel = null;
    [SerializeField] private float reloadTime = 0f;
    [SerializeField] [Slider(1f, 100f)] private float accuracyRadius = 100;

    // Routine holder
    private IEnumerator m_shootRoutine = null;
    private IEnumerator m_scopeRoutine = null;
    private IEnumerator m_reloadRoutine = null;

    // Scoping Section
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 scopePhysicPosition = new Vector3();
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 normalPhysicPosition = new Vector3();
    [BoxGroup("Scope Reference")] [SerializeField] [ReadOnly] private CameraController cameraControl;

    // Debugging Section
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isShooting = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isScoping = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isReloading = false;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isSingleShot = false;

    #region Unity Built-In Methods
    protected override void OnEnable()
    {
        base.OnEnable();
        gunBarrel.BarrelReset();
        genericBarrel = gunBarrel;
        cameraControl = gameObject.GetComponentInParent<CameraController>();
    }

    protected override void Update()
    {
        // Weapon react by input
        if (!m_isReloading)
        {
            if (weaponInputData.IsShooting)
                Shoot();
            if (cameraControl.Zoom.ZoomClicked || cameraControl.Zoom.ZoomRelease)
                Scope();
        }
        if (weaponInputData.IsReloading)
            Reload();

        // Handle detection on object
        DetectorHandler();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

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
            transform.localPosition = normalPhysicPosition;
            HideMesh(false);
            m_isScoping = false;
        }
    }
    #endregion

    public void Shoot()
    {
        if (m_isReloading || m_isShooting)
            return;

        m_shootRoutine = ShootRoutine();
        StartCoroutine(m_shootRoutine);
    }

    public void Scope()
    {
        if (m_isReloading || m_isScoping)
            return;

        m_scopeRoutine = ScopeRoutine();
        StartCoroutine(m_scopeRoutine);
    }

    public void Reload()
    {
        if (m_isReloading || gunBarrel.WeaponStock >= gunBarrel.MaxCapacityWeapon)
            return;

        if (m_scopeRoutine != null)
            StopCoroutine(m_scopeRoutine);

        if (m_shootRoutine != null)
            StopCoroutine(m_shootRoutine);

        m_reloadRoutine = ReloadRoutine();
        StartCoroutine(m_reloadRoutine);
    }

    /// <summary>
    /// Only used for changing crosshair when detect an enemy.
    /// </summary>
    protected override void DetectorHandler()
    {
        Ray crosshairRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        Debug.DrawLine(crosshairRay.origin, crosshairRay.origin + crosshairRay.direction, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(crosshairRay, out hit))
        {

            if (m_isScoping || !targetTags.Contains(hit.collider.tag))
                crosshairTemplate.CrosshairDetect(false);
            else
                crosshairTemplate.CrosshairDetect(true);
        }
        else
        {
            crosshairTemplate.CrosshairDetect(false);
        }
    }

    private IEnumerator ShootRoutine()
    {
        m_isShooting = true;
        float fireRateHolder = 0f;

        // TODO: Play VFX

        // As long as it's shooting then keep it running
        while (!weaponInputData.ShootReleased)
        {
            fireRateHolder -= Time.deltaTime;

            if (fireRateHolder <= 0f)
            {
                // Shoot Bullet
                BulletScript bullet = gunBarrel.ReleaseBullet();
                Vector3 shootDir = (weaponInputData.CrosshairTargetPos - bullet.transform.position).normalized;
                bullet.StartShoot(shootDir, targetTags, gunBarrel.ShootForce);
                StartCoroutine(ShootlightPass());

                if (gunBarrel.WeaponStock <= 0)
                {
                    Reload();
                    break;
                }

                if (m_isSingleShot)
                    break;

                // Reset fire time holder
                fireRateHolder = FireRate;
            }

            yield return null;
        }

        // TODO: Stop VFX

        m_isShooting = false;
    }

    private IEnumerator ScopeRoutine()
    {
        m_isScoping = true;

        float _percent = 0f;
        float _smoothPercent = 0f;

        float _speed = 1f / cameraControl.Zoom.ZoomTransitionDuration;

        do {
            if (cameraControl.Zoom.IsZooming)
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

            _smoothPercent = cameraControl.Zoom.ZoomCurve.Evaluate(_percent);
            transform.localPosition = Vector3.Lerp(normalPhysicPosition, scopePhysicPosition, _smoothPercent);
            yield return null;
        } while (_percent > 0f);

        m_isScoping = false;
    }

    private IEnumerator ReloadRoutine()
    {
        m_isReloading = true;

        float m_reloadTimeHolder = reloadTime;

        // Reset physic scope position
        transform.localPosition = normalPhysicPosition;
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
        shootLight.enabled = true;
        yield return null;
        shootLight.enabled = false;
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
}
