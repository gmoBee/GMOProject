using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDeathMatchSystem : GameManager
{
    private const GameModes MODE = GameModes.TeamDeathMatch;

    [Header("Required Attributes")]
    [SerializeField] private TeamContainer firstTeam = null;
    [SerializeField] private TeamContainer secondTeam = null;

    #region Unity BuiltIn Methods
    // Start is called before the first frame update
    private void Start()
    {
        SetupGameMode(MODE);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        
    }
    #endregion
}
