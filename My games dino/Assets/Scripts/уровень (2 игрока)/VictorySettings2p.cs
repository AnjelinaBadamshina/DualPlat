using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictorySettings2p : MonoBehaviour
{
    public GameObject VictoryPanel;
    public Button nextLevelButton;

    public void VictoryPressed()
    {
        VictoryPanel.SetActive(true);
        GameStateManager.Instance.FreezeGame();
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.UnfreezeGame();
        GameStateManager.Instance.ResetCooldowns();
    }

    public void ChangeScrene()
    {
        SceneManager.LoadScene(2);
        GameStateManager.Instance.UnfreezeGame();
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Проверяем, переходим ли с Level0 (индекс 8) на Level1 (индекс 9)
        if (currentSceneIndex == 8 && PlayerPrefs.GetInt("IntroCutsceneWatched2p", 0) == 0)
        {
            SceneManager.LoadScene("Кастсцена 1"); // Замените на имя вашей катсцены для 2p
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }

        GameStateManager.Instance.UnfreezeGame();
    }

    public void EnableNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = true;
        }
    }

    public void DisableNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = false;
            nextLevelButton.GetComponentInChildren<Text>().text = "Продолжение\nследует";
        }
    }
}