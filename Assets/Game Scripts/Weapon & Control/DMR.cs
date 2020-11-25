﻿using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHS;

public class DMR : Weapon, IGunsInterface
{
    // Specific Weapon Attributes
    [Header("DMR Attributes")]
    [SerializeField] private WeaponBarrel weaponBarrel = null;
    [SerializeField] private float reloadTime = 0f;
    [SerializeField] [Slider(1f, 100f)] private float accuracyRadius = 100;

    // Routine holder
    private IEnumerator m_shootRoutine = null;
    private IEnumerator m_scopeRoutine = null;
    private IEnumerator m_reloadRoutine = null;

    // Scoping Section
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 scopePhysicPosition;
    [BoxGroup("Scope Reference")] [SerializeField] private Vector3 normalPhysicPosition;
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
        weaponBarrel.BarrelReset();
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
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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
        if (m_isReloading || weaponBarrel.WeaponStock >= weaponBarrel.MaxCapacityWeapon)
            return;

        if (m_scopeRoutine != null)
            StopCoroutine(m_scopeRoutine);

        if (m_shootRoutine != null)
            StopCoroutine(m_shootRoutine);

        m_reloadRoutine = ReloadRoutine();
        StartCoroutine(m_reloadRoutine);
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
                BulletScript bullet = weaponBarrel.ReleaseBullet();
                Vector3 shootDir = (weaponInputData.CrosshairTargetPos - bullet.transform.position).normalized;
                bullet.StartShoot(shootDir, targetHitLayer, weaponBarrel.ShootForce);

                if (weaponBarrel.WeaponStock <= 0)
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

        weaponBarrel.ReloadWeapon();
        m_isReloading = false;
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