using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TeamContainer : MonoBehaviour, IEntitySpawner
{
    [Header("Team Data Attributes")]
    [SerializeField] private List<Transform> spawnerPoints = new List<Transform>();
    [SerializeField] private float secondsRespawn = 5f;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private List<LivingEntity> m_entitiesInTeam = new List<LivingEntity>();
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_respawnSecondsHolder;

    public uint TeamID { set; get; }

    #region Unity BuiltIn Methods
    private void Start()
    {
        m_respawnSecondsHolder = secondsRespawn;
    }
    #endregion

    public void AssignIntoTeam(LivingEntity entity)
    {
        entity.RelationID = TeamID;
        m_entitiesInTeam.Add(entity);
    }

    public bool RemoveFromTeam(LivingEntity entity)
    {
        return m_entitiesInTeam.Remove(entity);
    }

    public void ChangePositionSpawner(Transform spawner, Vector3 newPosition)
    {
        if (spawnerPoints.Contains(spawner))
            spawner.position = newPosition;
    }

    public void SpawnEntity(LivingEntity entity, Vector3 position, bool reset)
    {
        Transform spawnPoint = spawnerPoints[Random.Range(0, spawnerPoints.Count)];
        entity.transform.position = spawnPoint.position;

        if (!entity.gameObject.activeSelf)
            entity.gameObject.SetActive(true);
        if (reset)
            entity.ResetEntity();
    }
}
