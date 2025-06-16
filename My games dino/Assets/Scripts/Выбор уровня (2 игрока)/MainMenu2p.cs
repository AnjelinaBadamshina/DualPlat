using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu2p : MonoBehaviour
{
    public ResetConfirmation2p resetConfirmation2p;
    public Button[] levelButtons;
    public Button level0Button;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;

    int lastUnlockedLevel = -1;

    void Start()
    {
        lastUnlockedLevel = PlayerPrefs.GetInt("lastUnlockedLevel2p", -1);

        level0Button.interactable = true;
        level1Button.interactable = true;
        level2Button.interactable = lastUnlockedLevel >= 1;
        level3Button.interactable = lastUnlockedLevel >= 2;
        level4Button.interactable = lastUnlockedLevel >= 3;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int crystalCount = PlayerPrefs.GetInt("CrystalRecordLevel2p" + i, 0);
            for (int j = 0; j < 3; j++)
            {
                levelButtons[i].transform.GetChild(j).gameObject.SetActive(j < crystalCount);
            }
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("lastUnlockedLevel2p", -1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Кастсцена2"); // Замените на имя вашей катсцены для 2p
    }

    public void LoadLevel(int levelIndex)
    {
        // Проверяем, загружаем ли Level1 (индекс 9) и не просмотрена ли катсцена
        if (levelIndex == 9 && PlayerPrefs.GetInt("IntroCutsceneWatched2p", 0) == 0)
        {
            SceneManager.LoadScene("Кастсцена2"); // Замените на имя вашей катсцены для 2p
        }
        else
        {
            SceneManager.LoadScene(levelIndex);
        }
    }

    public void ResetSave()
    {
        resetConfirmation2p.ShowConfirmation();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}