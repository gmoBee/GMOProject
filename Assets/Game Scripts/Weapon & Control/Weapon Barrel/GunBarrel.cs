using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GunBarrel : WeaponBarrel
{
    // Pool or Init Bullet Holder
    private Queue<BulletScript> bulletWeaponPool = new Queue<BulletScript>();

    // Weapon Barrel Attributes
    [SerializeField] private BulletScript bulletPrefab = null;
    [SerializeField] private int maxWeaponStock = 10;
    [SerializeField] private int maxReserveStock = 30;
    [SerializeField] private Transform barrelTransform = null;
    [SerializeField] private float shootForce = 30f;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int m_reservedStock;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int m_weaponStock;

    // Properties
    public int MaxCapacityWeapon 
    { 
        set
        {
            if (value < 0)
                maxWeaponStock = 0;
            else
                maxWeaponStock = value;
        }
        get => maxWeaponStock; 
    }

    public int MaxReserve
    {
        set
        {
            if (value < 0)
                maxReserveStock = 0;
            else
                maxReserveStock = value;
        }
        get => maxReserveStock;
    }

    public int Reserved
    {
        set 
        {
            if (value > maxReserveStock)
                m_reservedStock = maxReserveStock;
            else if (value < 0)
                m_reservedStock = 0;
            else
                m_reservedStock = value;
        } 
        get => m_reservedStock;
    }

    public int WeaponStock { get => m_weaponStock; }
    public float ShootForce { get => shootForce; }
    public Transform BarrelTransform { get => barrelTransform; }

    // Custom Methods
    public override void BarrelReset()
    {
        // Reset attributes
        m_weaponStock = maxWeaponStock;
        m_reservedStock = maxReserveStock;

        // Check if there's current usage and bullets in pool
        if (bulletWeaponPool.Count > maxWeaponStock)
            return;

        // Create hundreds of bullets
        for (int i = 0; i < maxWeaponStock + maxReserveStock; i++)
        {
            BulletScript bullet = UnityEngine.Object.Instantiate(bulletPrefab);
            if (bullet.gameObject.activeSelf)
                bullet.gameObject.SetActive(false);
            bulletWeaponPool.Enqueue(bullet);
        }
    }

    /// <summary>
    /// Using a default bullet restock count.
    /// </summary>
    public void ReloadWeapon()
    {
        int rangeUntilMax = maxWeaponStock - m_weaponStock;
        m_reservedStock -= rangeUntilMax;
        m_weaponStock += rangeUntilMax;
    }

    /// <summary>
    /// Restock with custom amount of bullets.
    /// </summary>
    /// <param name="amount">Amount of bullet to be restock</param>
    public void Restock(int amount)
    {
        Reserved += amount;
    }

    /// <summary>
    /// Release a bullet from pool to be used.
    /// </summary>
    public BulletScript ReleaseBullet()
    {
        // Release bullet from pool
        BulletScript bullet = bulletWeaponPool.Dequeue();
        bullet.ReturnToPoolReference = bulletWeaponPool;

        // Set Active and set position start shoot
        if (!bullet.gameObject.activeSelf)
            bullet.gameObject.SetActive(true);
        bullet.transform.position = barrelTransform.position;
        m_weaponStock--;

        return bullet;
    }
}
