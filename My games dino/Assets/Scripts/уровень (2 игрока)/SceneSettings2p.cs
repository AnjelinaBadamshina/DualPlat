using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettings2p : MonoBehaviour
{
    public GameObject PausePanel;

    void Update()
    {
        // Проверяем, нажата ли Esc и не заморожена ли игра другими окнами
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameFrozenByOtherWindows())
        {
            TogglePause();
        }
    }

    // Проверяем, заморожена ли игра другими окнами
    private bool IsGameFrozenByOtherWindows()
    {
        // Проверяем, что время заморожено (Time.timeScale == 0) и панель паузы не активна
        return Time.timeScale == 0 && !PausePanel.activeSelf;
    }

    public void PauseButtonPressed()
    {
        TogglePause();
    }

    public void ContinueButtonPressed()
    {
        TogglePause();
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.UnfreezeGame();
        GameStateManager.Instance.ResetCooldowns();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(2); // Загрузка сцены с индексом 2
        GameStateManager.Instance.UnfreezeGame();
    }

    void TogglePause()
    {
        bool isPaused = PausePanel.activeSelf;
        PausePanel.SetActive(!isPaused);
        if (isPaused)
        {
            GameStateManager.Instance.UnfreezeGame();
        }
        else
        {
            GameStateManager.Instance.FreezeGame();
        }
    }
}