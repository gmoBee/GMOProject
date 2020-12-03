using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BulletScript : MonoBehaviour
{
    [Header("Default Bullet Attributes")]
    [SerializeField] private float moveSpeed; // Bullet shoot with speed
    [SerializeField] private float fadingSeconds; // Destroy bullet, prevent infinite movement
    [SerializeField] private LayerMask targetMask;

    // Temporary Attributes
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 dir;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Rigidbody bulletRigid;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private LayerMask targetMaskHolder;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private float speedHolder;

    // Only for returning bullet to pool
    public Queue<BulletScript> ReturnToPoolReference { set; get; }

    #region Unity Built-In Methods
    private void Start()
    {
        if (bulletRigid == null)
            bulletRigid = GetComponent<Rigidbody>();

        targetMaskHolder = targetMask;
        speedHolder = moveSpeed;
    }

    private void Update()
    {
        bulletRigid.AddForce(dir * speedHolder, ForceMode.Impulse);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, speedHolder))
        {
            Debug.Log(hit.collider.gameObject.name);
            gameObject.SetActive(false);
            ReturnToPoolReference.Enqueue(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, dir);
    }
    #endregion

    public void StartShoot(Vector3 dir)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        // Assign temporary attributes
        this.dir = dir;
        targetMaskHolder = targetMask;
        speedHolder = moveSpeed;
        StartCoroutine(BulletDisappear());
    }

    public void StartShoot(Vector3 dir, LayerMask tm, float speed)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        this.dir = dir;
        targetMaskHolder = tm;
        speedHolder = speed;
        StartCoroutine(BulletDisappear());
    }

    private IEnumerator BulletDisappear()
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

    private IEnumerator BulletDisappear(float seconds)
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
