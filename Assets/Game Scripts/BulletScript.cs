using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BulletScript : MonoBehaviour
{
    [Header("Default Bullet Attributes")]
    [SerializeField] private float moveSpeed = 5f; // Bullet shoot with speed
    [SerializeField] private float fadingSeconds = 3f; // Destroy bullet, prevent infinite movement
    [SerializeField] private List<string> targetTags = new List<string>();
    [SerializeField] private uint baseDamage = 8;
    [SerializeField] private GameObject bulletHole = null;

    // Temporary Attributes
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private Vector3 m_dir;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private uint m_entityID = 0;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private float m_speedHolder;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private uint m_weaponDamage = 0;

    // Only for returning bullet to pool
    public uint EntityID => m_entityID;
    public Queue<BulletScript> ReturnToPoolReference { set; get; }

    public uint BaseDamage
    {
        set => baseDamage = value;
        get => baseDamage;
    }

    #region Unity Built-In Methods
    private void Start()
    {
        m_speedHolder = moveSpeed;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, m_dir, out hit, m_speedHolder))
        {
            LivingEntity m_entityGotHit = hit.collider.gameObject.GetComponent<LivingEntity>();
            if (m_entityGotHit != null)
            {
                if (EntityID == m_entityGotHit.RelationID)
                {
                    transform.position += m_dir * m_speedHolder;
                    return;
                }

                m_entityGotHit.Hit(m_weaponDamage + baseDamage);
            }

            transform.position = hit.point;
            GameEventHandler.instance.CallBulletHitEvent(this, new OnBulletHitArgs(this, hit.transform));
            gameObject.SetActive(false);
            ReturnToPoolReference.Enqueue(this);
        }
        else
        {
            transform.position += m_dir * m_speedHolder;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, m_dir * m_speedHolder);
    }
    #endregion

    public void StartShoot(Vector3 dir, uint weaponDamage, uint entityID)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // Assign temporary attributes
        m_dir = dir;
        m_weaponDamage = weaponDamage;
        m_entityID = entityID;
        m_speedHolder = moveSpeed;

        // Make bullet facing to the target
        transform.eulerAngles = Quaternion.LookRotation(m_dir).eulerAngles;

        // Start coroutine to prevent infinite movement
        StartCoroutine(BulletDisappearRoutine());
    }

    public void StartShoot(Vector3 dir, uint weaponDamage, uint entityID, List<string> targetTags, float speed)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // Assign temporary attributes
        m_dir = dir;
        m_weaponDamage = weaponDamage;
        m_entityID = entityID;
        this.targetTags = targetTags;
        m_speedHolder = speed;

        // Make bullet facing to the target
        transform.eulerAngles = Quaternion.LookRotation(m_dir).eulerAngles;

        // Start coroutine to prevent infinite movement
        StartCoroutine(BulletDisappearRoutine());
    }

    private IEnumerator BulletDisappearRoutine()
    {
        float secHolder = fadingSeconds;
        while (secHolder > 0f)
        {
            secHolder -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        ReturnToPoolReference.Enqueue(this);
    }

    private IEnumerator BulletDisappearRoutine(float seconds)
    {
        while (seconds > 0f)
        {
            seconds -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        ReturnToPoolReference.Enqueue(this);
    }
}
