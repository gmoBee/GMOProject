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

    // Temporary Attributes
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Vector3 dir;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private Rigidbody bulletRigid;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private List<string> targetTagsHolder;
    [BoxGroup("Temporary Attributes")] [SerializeField] [ReadOnly] private float speedHolder;

    // Only for returning bullet to pool
    public Queue<BulletScript> ReturnToPoolReference { set; get; }

    #region Unity Built-In Methods
    private void Start()
    {
        if (bulletRigid == null)
            bulletRigid = GetComponent<Rigidbody>();

        targetTagsHolder = targetTags;
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
        targetTagsHolder = targetTags;
        speedHolder = moveSpeed;
        StartCoroutine(BulletDisappear());
    }

    public void StartShoot(Vector3 dir, List<string> targetTags, float speed)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        this.dir = dir;
        targetTagsHolder = targetTags;
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
