using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModes
{
    Tutorial,
    TeamDeathMatch,
    KingOfTheHill,
    KoHChaos,
    ObjectiveDomain
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void SetupGamemode(GameModes mode)
    {
        // TODO: create game system in each game mode
    }
}
