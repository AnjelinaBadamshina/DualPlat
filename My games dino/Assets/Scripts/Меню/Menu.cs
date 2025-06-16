using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int singlePlayerMenuSceneIndex = 1; // Индекс сцены меню одиночной игры
    public int twoPlayersMenuSceneIndex = 2;  // Индекс сцены меню для двух игроков

    public void LoadSinglePlayerMenu()
    {
        SceneManager.LoadScene(singlePlayerMenuSceneIndex);
    }

    public void LoadTwoPlayersMenu()
    {
        SceneManager.LoadScene(twoPlayersMenuSceneIndex);
    }

    // Метод для выхода из игры (вызывается при нажатии кнопки "Выход")
    public void ExitGame()
    {
        // Завершение работы приложения
        Application.Quit();
        // Application - класс для управления приложением
        // Quit() - завершает работу приложения (работает только в собранной игре)
    }
}
