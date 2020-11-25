using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WeaponBarrel : MonoBehaviour
{
    // Pool or Init Bullet Holder
    private Queue<BulletScript> bulletWeaponPool = new Queue<BulletScript>();

    // Weapon Barrel Attributes
    [SerializeField] private BulletScript bulletPrefab = null;
    [SerializeField] private int maxWeaponStock;
    [SerializeField] private int maxReserveStock;
    [SerializeField] private Transform barrelTransform = null;
    [SerializeField] private float shootForce;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int reservedStock;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private int weaponStock;

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
                reservedStock = maxReserveStock;
            else if (value < 0)
                reservedStock = 0;
            else
                reservedStock = value;
        } 
        get => reservedStock;
    }

    public int WeaponStock { get => weaponStock; }
    public float ShootForce { get => shootForce; }

    // Custom Methods
    public void BarrelReset()
    {
        // Reset attributes
        weaponStock = maxWeaponStock;
        reservedStock = maxReserveStock;

        // Check if there's current usage and bullets in pool
        if (bulletWeaponPool.Count > maxWeaponStock)
            return;

        // Create hundreds of bullets
        for (int i = 0; i < maxWeaponStock + maxReserveStock; i++)
        {
            BulletScript bullet = Instantiate(bulletPrefab);
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
        int rangeUntilMax = maxWeaponStock - weaponStock;
        reservedStock -= rangeUntilMax;
        weaponStock += rangeUntilMax;
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
        weaponStock--;

        return bullet;
    }
}
