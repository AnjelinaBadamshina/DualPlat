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
        GameStateManager.Instance.FreezeGame();   // ������������� ����� ��� ����������� ������
    }

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.UnfreezeGame();  // ������������ �����
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

        // ���������, ��������� �� � Level0 (������ 3) �� Level1 (������ 4)
        if (currentSceneIndex == 3 && PlayerPrefs.GetInt("IntroCutsceneWatched", 0) == 0)
        {
            SceneManager.LoadScene("���������"); // ��������� ��������
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex); // ��������� ��������� �������
        }

        GameStateManager.Instance.UnfreezeGame();
    }

    public void EnableNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = true; // �������� ������ "��������� �������"
        }
    }



    public void DisableNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.interactable = false; // ��������� ������ "��������� �������"
            nextLevelButton.GetComponentInChildren<Text>().text = "�����������\n�������"; // �������� ����� ������
        }
    }
}
