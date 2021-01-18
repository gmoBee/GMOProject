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

    private GameModes m_currentMode;

    #region Unity BuiltIn Methods
    protected virtual void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    protected virtual void OnDisable()
    {
        instance = null;
    }
    #endregion

    protected void SetupGameMode(GameModes mode)
    {
        // TODO: create game system in each game mode
        m_currentMode = mode;
    }
}
