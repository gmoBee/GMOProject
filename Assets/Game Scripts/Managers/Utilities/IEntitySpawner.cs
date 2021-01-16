using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntitySpawner
{
    /// <summary>
    /// Change position of its current spawner point.
    /// </summary>
    /// <param name="spawner">Spawner Reference</param>
    /// <param name="newPosition">Position set</param>
    void ChangePositionSpawner(Transform spawner, Vector3 newPosition);
    void SpawnEntity(LivingEntity entity, Vector3 position, bool reset);
}
