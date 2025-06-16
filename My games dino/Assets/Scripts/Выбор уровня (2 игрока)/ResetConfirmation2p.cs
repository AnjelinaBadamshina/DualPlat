using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetConfirmation2p : MonoBehaviour
{
    public MainMenu2p mainMenu2p;
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
        if (mainMenu2p != null)
        {
            PlayerPrefs.DeleteKey("lastUnlockedLevel2p");
            PlayerPrefs.DeleteKey("IntroCutsceneWatched2p"); // ���������� ���� ��������

            for (int i = 0; i < mainMenu2p.levelButtons.Length; i++)
            {
                PlayerPrefs.DeleteKey("CrystalRecordLevel2p" + i);
            }

            PlayerPrefs.Save();
            SceneManager.LoadScene(2); // ������� ���� ��� 2p
        }
        else
        {
            Debug.LogError("MainMenu2p ������ �� ����������� � ResetConfirmation2p �������!");
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