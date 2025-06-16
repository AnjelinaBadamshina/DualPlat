using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour
{
    private GameObject[] characters; // Массив для хранения персонажей
    private int index; // Текущий индекс выбранного персонажа
    [SerializeField] private string PrefName;

    private void Start()
    {
        index = PlayerPrefs.GetInt(PrefName, 0); // Получаем индекс выбранного персонажа из сохранения
        characters = new GameObject[transform.childCount]; // Инициализируем массив персонажей размером равным количеству дочерних объектов

        // Заполняем массив персонажей
        for (int i = 0; i < transform.childCount; i++)
        {
            characters[i] = transform.GetChild(i).gameObject;
        }

        // Выключаем все персонажи
        foreach (GameObject go in characters)
        {
            go.SetActive(false);
        }

        // Включаем выбранный персонаж
        if (index >= 0 && index < characters.Length)
        {
            characters[index].SetActive(true);
        }
    }

    // Метод для выбора предыдущего персонажа
    public void SelectLeft()
    {
        characters[index].SetActive(false); // Выключаем текущего персонажа
        index--; // Уменьшаем индекс
        if (index < 0) // Если индекс стал меньше нуля, переходим к последнему персонажу
        {
            index = characters.Length - 1;
        }
        characters[index].SetActive(true); // Включаем выбранный персонаж
        SaveCharacter(); // Сохраняем выбранного персонажа
    }

    // Метод для выбора следующего персонажа
    public void SelectRight()
    {
        characters[index].SetActive(false); // Выключаем текущего персонажа
        index++; // Увеличиваем индекс
        if (index == characters.Length) // Если индекс достиг максимального значения, переходим к первому персонажу
        {
            index = 0;
        }
        characters[index].SetActive(true); // Включаем выбранный персонаж
        SaveCharacter(); // Сохраняем выбранного персонажа
    }

    // Метод для сохранения выбранного персонажа
    public void SaveCharacter()
    {
        PlayerPrefs.SetInt(PrefName, index); // Сохраняем выбранный персонаж в PlayerPrefs
    }
}
