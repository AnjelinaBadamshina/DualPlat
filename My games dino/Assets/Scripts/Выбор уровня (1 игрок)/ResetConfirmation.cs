using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetConfirmation : MonoBehaviour
{
    public MainMenu mainMenu;
    public Button confirmButton;
    public Button cancelButton;

    void Start()
    {
        confirmButton.onClick.AddListener(ConfirmReset);
        cancelButton.onClick.AddListener(CancelReset);
        gameObject.SetActive(false);
    }

    void ConfirmReset()
    {
        if (mainMenu != null)
        {
            // ���������� ����������
            PlayerPrefs.DeleteKey("lastUnlockedLevel");
            PlayerPrefs.DeleteKey("IntroCutsceneWatched"); // ���������� ���� ��������

            for (int i = 0; i < mainMenu.levelButtons.Length; i++)
            {
                PlayerPrefs.DeleteKey("CrystalRecordLevel" + i);
            }

            PlayerPrefs.Save();

            // ������������� ����� ���� �������
            SceneManager.LoadScene(1); // LevelSelection
        }
        else
        {
            Debug.LogError("MainMenu ������ �� ����������� � ResetConfirmation �������!");
        }

        gameObject.SetActive(false);
    }

    void CancelReset()
    {
        gameObject.SetActive(false);
    }

    public void ShowConfirmation()
    {
        gameObject.SetActive(true);
    }
}