using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int singlePlayerMenuSceneIndex = 1; // ������ ����� ���� ��������� ����
    public int twoPlayersMenuSceneIndex = 2;  // ������ ����� ���� ��� ���� �������

    public void LoadSinglePlayerMenu()
    {
        SceneManager.LoadScene(singlePlayerMenuSceneIndex);
    }

    public void LoadTwoPlayersMenu()
    {
        SceneManager.LoadScene(twoPlayersMenuSceneIndex);
    }

    // ����� ��� ������ �� ���� (���������� ��� ������� ������ "�����")
    public void ExitGame()
    {
        // ���������� ������ ����������
        Application.Quit();
        // Application - ����� ��� ���������� �����������
        // Quit() - ��������� ������ ���������� (�������� ������ � ��������� ����)
    }
}
