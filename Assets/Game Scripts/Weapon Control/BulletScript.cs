using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using VHS;

public class BulletScript : MonoBehaviour
{
    #region Private Variables
    private Rigidbody bulletRigid = null;
    private RangedWeapon referedWeapon = null;
    private Vector3 shootDir;
    private int shootForce;
    private IEnumerator m_shootRoutine;

    // Bullet has a damage property to be hold
    // TODO: Properties 
    #endregion

    #region Properties
    public RangedWeapon ReferedWeapon 
    { 
        set => referedWeapon = value;
    }
    #endregion

    #region Unity Built-In Method
    void OnEnable()
    {
        if (bulletRigid == null)
            bulletRigid = GetComponent<Rigidbody>();

        bulletRigid.AddForce(shootDir * shootForce);
        m_shootRoutine = ShootRoutine();
        StartCoroutine(m_shootRoutine);
    }

    private void Update()
    {
        // Bullet move forward
        Ray ray = new Ray(transform.position, shootDir);
        RaycastHit hit;
        Debug.DrawRay(transform.position, shootDir);
        if (Physics.Raycast(ray, out hit, shootForce * Time.deltaTime))
        {
            Debug.Log($"Bullet Hit {hit.collider.name}");
            StopCoroutine(m_shootRoutine);
            gameObject.SetActive(false);
            referedWeapon.SendAmmoBack(gameObject);
        }
    }
    #endregion

    public void Shoot(Vector3 dir, int shootForce)
    {
        shootDir = dir;
        this.shootForce = shootForce;
        gameObject.SetActive(true);
    }

    private IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        referedWeapon.SendAmmoBack(gameObject);
    }
}
