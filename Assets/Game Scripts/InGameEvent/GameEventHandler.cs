using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    public static GameEventHandler instance;

    // Delegates
    public delegate void EntityWeaponRoll(object sender, OnWeaponRollArgs args);
    public delegate void BulletHit(object sender, OnBulletHitArgs args);

    // Events
    public event EntityWeaponRoll OnEntityWeaponRollEvent;
    public event BulletHit OnBulletHitEvent;

    #region Unity BuiltIn Methods
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        // Register default events
        OnBulletHitEvent += BulletHitEventCallback;
    }

    private void OnDestroy()
    {
        // Unregister default events
        OnBulletHitEvent -= BulletHitEventCallback;
    }
    #endregion

    public void CallWeaponRollEvent(object sender, OnWeaponRollArgs arg)
    {
        OnEntityWeaponRollEvent?.Invoke(sender, arg);
    }

    public void CallBulletHitEvent(object sender, OnBulletHitArgs arg)
    {
        OnBulletHitEvent?.Invoke(sender, arg);
    }

    #region Handled Events
    private void BulletHitEventCallback(object sender, OnBulletHitArgs arg)
    {
        StartCoroutine(BulletHoleRoutine(arg.Bullet, arg.Bullet.transform.GetChild(0)));
    }
    #endregion

    #region Other and Custom Methods
    private IEnumerator BulletHoleRoutine(BulletScript bullet, Transform bulletHole)
    {
        bulletHole.gameObject.SetActive(true);
        bulletHole.transform.parent = null;
        float fadeTime = 3f;
        while (fadeTime > 0f)
        {
            yield return null;
            fadeTime -= Time.deltaTime;
        }
        bulletHole.transform.parent = bullet.transform;
        bulletHole.gameObject.SetActive(false);
    }
    #endregion
}
