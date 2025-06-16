using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictorySettings1p : MonoBehaviour
{
    public GameObject VictoryPanel;
    public Button nextLevelButton;

    public void VictoryPressed()
    {
        VictoryPanel.SetActive(true);
        GameStateManager.Instance.FreezeGame();   // Останавливаем время при отображении панели
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.UnfreezeGame();  // Возобновляем время
        GameStateManager.Instance.ResetCooldowns();

    }

    public void ChangeScrene()
    {
        SceneManager.LoadScene(1);
        GameStateManager.Instance.UnfreezeGame();
    }


    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Проверяем, переходим ли с Level0 (индекс 3) на Level1 (индекс 4)
        if (currentSceneIndex == 3 && PlayerPrefs.GetInt("IntroCutsceneWatched", 0) == 0)
        {
            SceneManager.LoadScene("Кастсцена"); // Загружаем катсцену
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex); // Загружаем следующий уровень
        }

        GameStateManager.Instance.UnfreezeGame();
    }

    public void EnableNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = true; // Включить кнопку "Следующий уровень"
        }
    }



    public void DisableNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = false; // Выключить кнопку "Следующий уровень"
            nextLevelButton.GetComponentInChildren<Text>().text = "Продолжение\nследует"; // Изменить текст кнопки
        }
    }
}
