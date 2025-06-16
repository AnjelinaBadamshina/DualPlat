using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettings1p : MonoBehaviour
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

    // Проверяем, заморожена ли игра другими окнами (например, меню, диалоги и т.д.)
    private bool IsGameFrozenByOtherWindows()
    {
        // Предполагаем, что GameStateManager имеет метод или свойство, указывающее на заморозку
        // Например, проверяем, не равно ли время игры нулю и не активна ли пауза
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
        SceneManager.LoadScene(1);
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