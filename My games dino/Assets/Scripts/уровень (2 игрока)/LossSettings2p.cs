using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LossSettings2p : MonoBehaviour
{
    public GameObject VictoryPanel;

    public void LossPressed()
    {
        VictoryPanel.SetActive(true);
        GameStateManager.Instance.FreezeGame();  // Останавливаем время при отображении панели
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.UnfreezeGame(); // Возобновляем время
        GameStateManager.Instance.ResetCooldowns();

    }

    public void ChangeScrene()
    {
        SceneManager.LoadScene(2);
        GameStateManager.Instance.UnfreezeGame();
    }

}
