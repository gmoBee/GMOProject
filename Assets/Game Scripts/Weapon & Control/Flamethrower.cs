using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Weapon
{
    [Space, Header("Flamethrower Attributes")]
    [Slider(1f, 10f)] [SerializeField] private float maxBurnRange = 1f;
    [Slider(0.1f, 10f)] [SerializeField] private float intensity = 0.5f;

    #region Unity Built-In Methods
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();   
    }
    #endregion

    private void Burn()
    {
        //Vector3 m_hitDir = (weaponInputData.CrossHairTargetPos - Barrel.position).normalized;
        //RaycastHit[] m_hits = Physics.CapsuleCastAll(Barrel.position, Barrel.position + m_hitDir, intensity, 
        //    m_hitDir, maxBurnRange, targetHitLayer);
        //Debug.DrawRay(Barrel.position, m_hitDir * maxBurnRange, Color.red);
    }
         
    private IEnumerator ShootRoutine()
    {
        //weaponShooting = true;
        //float timePerFire = 1f / FireRate;
        //float timeFireHolder = 0f;

        //// TODO: Play VFX
        //while (weaponInputData.IsShooting)
        //{
        //    timeFireHolder -= Time.deltaTime;

        //    if (timeFireHolder <= 0f)
        //    {
        //        timeFireHolder = timePerFire;
        //        Burn(); // Ray hit function
        //    }

        //    if (Ammo <= 0)
        //    {
        //        StartCoroutine(ReloadRoutine());
        //        break;
        //    }

        //    yield return null;
        //}

        //// TODO: Stop VFX
        //weaponShooting = false;
        yield return null;
    }

    private IEnumerator ReloadRoutine()
    {
        //weaponReloading = true;
        //float m_reloadTimeHolder = ReloadTime;

        //// TODO: Calculate the same time as animation time
        //// TODO: Play Animation

        //while (m_reloadTimeHolder > 0f)
        //{
        //    m_reloadTimeHolder -= Time.deltaTime;
        //    yield return null;
        //}

        //Ammo = MaxAmmo;
        //weaponReloading = false;
        yield return null;
    }

    
}
