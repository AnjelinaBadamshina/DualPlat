using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalPicker : MonoBehaviour
{
    private CrystalCounter crystalCounter; // Ссылка на экземпляр класса CrystalCounter

    public AudioSource Audio;

    private void Start()
    {
        // Находим объект с скриптом CrystalCounter и получаем ссылку на его компонент CrystalCounter
        crystalCounter = FindObjectOfType<CrystalCounter>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, вошел ли объект в триггер, имеющий указанный тег
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // Увеличиваем счетчик кристаллов в классе CrystalCounter
            crystalCounter.IncreaseCrystalCount();

            // Воспроизводим звук
            Audio.Play();

            // Уничтожаем объект после использования
            Destroy(gameObject);
        }
    }
}
