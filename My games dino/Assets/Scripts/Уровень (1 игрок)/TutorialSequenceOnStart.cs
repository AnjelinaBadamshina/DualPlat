using UnityEngine;
using UnityEngine.UI;

public class TutorialSequenceOnStart : MonoBehaviour
{
    public GameObject[] tutorialPanels; // ������ � ������� ������
    private int currentIndex = 0;
    private bool isTutorialActive = false;

    private void Start()
    {
        if (tutorialPanels.Length == 0) return;

        GameStateManager.Instance.FreezeGame(); // ������������� ����
        ShowCurrentPanel();
    }

    private void Update()
    {
        if (!isTutorialActive) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            HideCurrentPanel();
            currentIndex++;

            if (currentIndex < tutorialPanels.Length)
            {
                ShowCurrentPanel();
            }
            else
            {
                isTutorialActive = false;
                GameStateManager.Instance.UnfreezeGame(); // ������������ ����
            }
        }
    }

    void ShowCurrentPanel()
    {
        if (currentIndex < tutorialPanels.Length)
        {
            tutorialPanels[currentIndex].SetActive(true);
            isTutorialActive = true;
        }
    }

    void HideCurrentPanel()
    {
        if (currentIndex < tutorialPanels.Length)
        {
            tutorialPanels[currentIndex].SetActive(false);
        }
    }
}
