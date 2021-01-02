using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransformTarget
{
    Position,
    Rotation,
    Both
}

[CreateAssetMenu(fileName = "Perlin Noise", menuName = "PlayerData/PerlinNoiseData", order = 2)]
public class PerlinNoiseCamData : ScriptableObject
{
    #region Variables
    public TransformTarget transformTarget;

    [Space]
    public float amplitude;
    public float frequency;
    #endregion    
}
