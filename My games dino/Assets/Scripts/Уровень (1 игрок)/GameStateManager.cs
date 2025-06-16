using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    public bool IsGameFrozen { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }

    public void FreezeGame()
    {
        IsGameFrozen = true;
        Time.timeScale = 0f;
        SetState(GameState.Pause);
    }

    public void UnfreezeGame()
    {
        IsGameFrozen = false;
        Time.timeScale = 1f;
        SetState(CurrentState == GameState.Pause ? GameState.Gameplay : CurrentState);
    }

    public void ResetCooldowns()
    {
        AbilityCooldownTracker.Instance?.ResetAllCooldowns();
    }
}

public enum GameState
{
    MainMenu,
    LevelSelection,
    Cutscene,
    Gameplay,
    Pause
}