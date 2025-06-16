using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public ResetConfirmation resetConfirmation; // ������ �� ������ ResetConfirmation
    public Button[] levelButtons; // ������ ������ ������� (0-4)
    public Button level0Button;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;

    int lastUnlockedLevel = -1;

    void Start()
    {
        lastUnlockedLevel = PlayerPrefs.GetInt("lastUnlockedLevel", -1);

        level0Button.interactable = true;
        level1Button.interactable = true;
        level2Button.interactable = lastUnlockedLevel >= 1;
        level3Button.interactable = lastUnlockedLevel >= 2;
        level4Button.interactable = lastUnlockedLevel >= 3;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int crystalCount = PlayerPrefs.GetInt("CrystalRecordLevel" + i, 0);
            for (int j = 0; j < 3; j++)
            {
                levelButtons[i].transform.GetChild(j).gameObject.SetActive(j < crystalCount);
            }
        }
    }

    public void StartNewGame()
    {
        // ���������� �������� � ��������
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("lastUnlockedLevel", -1); // ������ Level0 ��������
        PlayerPrefs.Save();

        // ��������� ��������
        SceneManager.LoadScene("IntroCutscene");
    }

    public void LoadLevel(int levelIndex)
    {
        // ���������, ��������� �� Level1 (������ 4) � �� ����������� �� ��������
        if (levelIndex == 4 && PlayerPrefs.GetInt("IntroCutsceneWatched", 0) == 0)
        {
            SceneManager.LoadScene("���������");
        }
        else
        {
            SceneManager.LoadScene(levelIndex);
        }
    }

    public void ResetSave()
    {
        resetConfirmation.ShowConfirmation();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}