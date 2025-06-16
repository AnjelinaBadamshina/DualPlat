using UnityEngine;
using UnityEngine.UI; // Импортируем библиотеку UI

public class WaterRiseController : MonoBehaviour
{
    public float initialRiseSpeed = 0.5f; // Начальная скорость подъема воды
    public float riseAcceleration = 0.1f; // Ускорение подъема воды
    public float riseInterval = 20.0f; // Интервал времени в секундах между подъемами воды
    private float nextRiseTime; // Время, когда вода поднимется в следующий раз
    private float currentRiseSpeed; // Текущая скорость подъема воды

    public Text countdownText; // Ссылка на текстовый элемент UI

    private bool isPaused = false; // Флаг паузы
    private bool isGameOver = false; // Флаг конца игры

    private Vector3 initialPosition; // Начальная позиция воды

    void Start()
    {
        // Устанавливаем начальную скорость и время следующего подъема воды
        currentRiseSpeed = initialRiseSpeed;
        nextRiseTime = Time.time + riseInterval;
        initialPosition = transform.position; // Сохраняем начальную позицию воды
    }

    void Update()
    {
        if (isPaused || isGameOver)
        {
            return; // Не обновляем таймер и воду, если игра на паузе или закончена
        }

        // Обновляем обратный отсчет на экране
        UpdateCountdown();

        // Проверяем, пора ли поднимать воду
        if (Time.time >= nextRiseTime)
        {
            // Поднимаем воду
            RiseWater();
            // Обновляем время следующего подъема воды
            nextRiseTime = Time.time + riseInterval;
            // Увеличиваем скорость подъема
            currentRiseSpeed += riseAcceleration;
        }
    }

    void RiseWater()
    {
        // Поднимаем воду по оси Y
        transform.position += Vector3.up * currentRiseSpeed;
    }

    void UpdateCountdown()
    {
        // Вычисляем оставшееся время до следующего подъема воды
        float timeLeft = nextRiseTime - Time.time;

        // Обновляем текстовый элемент обратного отсчета
        countdownText.text = "До следующей волны: " + Mathf.Ceil(timeLeft).ToString() + "с";

        // Изменение цвета текста в зависимости от оставшегося времени
        if (timeLeft <= 1.0f)
        {
            countdownText.color = Color.red; // Меняем цвет текста на красный
        }
        else
        {
            countdownText.color = Color.white; // Меняем цвет текста на белый
        }
    }

    public void StopAndHideTimer()
    {
        isPaused = true;

    }

    public void ResumeAndShowTimer()
    {
        isPaused = false;
        nextRiseTime = Time.time + riseInterval; // Обновляем время следующего подъема воды
    }

    // Добавляем метод для сброса уровня воды
    public void ResetWaterLevel()
    {
        transform.position = initialPosition; // Сбрасываем позицию воды
        currentRiseSpeed = initialRiseSpeed; // Сбрасываем скорость подъема
        nextRiseTime = Time.time + riseInterval; // Сбрасываем время до следующего подъема
    }
}
