using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController1p : MonoBehaviour
{
    public int crystalCount = 0; // Количество собранных кристаллов на уровне
    public int levelIndex;

    public static LevelController1p instance = null;

    int sceneIndex;
    int levelComplete;

    public VictorySettings1p VictorySettings1p;

    int Crystal;

    private CrystalCounter crystalCounter;

    private bool character1Finished = false;

    void Start()
    {
        CrystalPickup.mainCrystalPickedUp = false; // Сбросить флаг

        if (instance == null)
        {
            instance = this;
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelComplete = PlayerPrefs.GetInt("levelComplete", 0);

        crystalCounter = FindObjectOfType<CrystalCounter>();
    }

    private void Awake()
    {
        instance = this;
    }

    public void CharacterFinished(int characterNumber)
    {
        if (characterNumber == 1)
        {
            character1Finished = true;
        }

        if (character1Finished)
        {
            ShowVictoryPanel();
        }
    }

    public void CharacterLeft(int characterNumber)
    {
        if (characterNumber == 1)
        {
            character1Finished = false;
        }
    }

    public void ShowVictoryPanel()
    {
        // 🔥 Проверка: нельзя завершить уровень без главного кристалла
        if (!CrystalPickup.mainCrystalPickedUp)
        {
            Debug.Log("Главный кристалл не собран — победа невозможна.");
            return;
        }

        crystalCount = crystalCounter.crystal;

        PlayerPrefs.SetInt("CrystalRecordLevel" + levelIndex, crystalCount);

        if (levelIndex > PlayerPrefs.GetInt("lastUnlockedLevel", -1))
        {
            PlayerPrefs.SetInt("lastUnlockedLevel", levelIndex);
        }

        PlayerPrefs.Save();

        VictorySettings1p.VictoryPressed();
    }
}
