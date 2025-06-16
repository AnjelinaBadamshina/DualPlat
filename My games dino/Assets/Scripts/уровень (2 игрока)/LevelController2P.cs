using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController2P : MonoBehaviour
{
    public int crystalCount = 0;
    public int levelIndex;

    public static LevelController2P instance = null;

    int sceneIndex;
    int levelComplete;

    public VictorySettings2p VictorySettings2p;

    int Crystal;

    private CrystalCounter crystalCounter;

    private bool character1Finished = false;
    private bool character2Finished = false;

    void Start()
    {
        CrystalPickup.mainCrystalPickedUp = false; // Сбросить флаг

        if (instance == null)
        {
            instance = this;
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelComplete = PlayerPrefs.GetInt("levelComplete2p", 0);

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
        else if (characterNumber == 2)
        {
            character2Finished = true;
        }

        if (character1Finished && character2Finished)
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
        else if (characterNumber == 2)
        {
            character2Finished = false;
        }
    }

    public void ShowVictoryPanel()
    {
        // 🔥 Проверка: нельзя завершать уровень без главного кристалла
        if (!CrystalPickup.mainCrystalPickedUp)
        {
            Debug.Log("Главный кристалл не собран — победа не засчитывается.");
            return;
        }

        crystalCount = crystalCounter.crystal;
        PlayerPrefs.SetInt("CrystalRecordLevel2p" + levelIndex, crystalCount);

        if (levelIndex > PlayerPrefs.GetInt("lastUnlockedLevel2p", -1))
        {
            PlayerPrefs.SetInt("lastUnlockedLevel2p", levelIndex);
        }

        PlayerPrefs.Save();

        VictorySettings2p.VictoryPressed();
    }
}
