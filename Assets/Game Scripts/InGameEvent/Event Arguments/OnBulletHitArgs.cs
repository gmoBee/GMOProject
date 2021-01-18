using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBulletHitArgs
{
    private BulletScript bullet;
    private Transform hit;

    public BulletScript Bullet => bullet;
    public Transform Hit => hit;

    public OnBulletHitArgs(BulletScript bullet, Transform hit)
    {
        this.bullet = bullet;
        this.hit = hit;
    }
}
