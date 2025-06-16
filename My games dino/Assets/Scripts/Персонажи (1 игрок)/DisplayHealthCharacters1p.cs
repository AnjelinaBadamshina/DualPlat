using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DisplayHealthCharacters1p : MonoBehaviour
{
    private Slider slider;
    private HealthHero1p currentHealthHero;

    void Start()
    {
        slider = GetComponent<Slider>();

        // Подписываемся на событие CharacterSwitched
        CharacterSwitcher.OnCharacterSwitched += OnCharacterSwitched;
    }

    // Отписываемся от события при уничтожении объекта
    void OnDestroy()
    {
        CharacterSwitcher.OnCharacterSwitched -= OnCharacterSwitched;
    }

    // Этот метод будет вызван при смене персонажа
    void OnCharacterSwitched(Transform newCharacterTransform)
    {
        // Ищем HealthHero1p на новом персонаже
        currentHealthHero = newCharacterTransform.GetComponent<HealthHero1p>();

        // Обновляем отображение здоровья
        if (currentHealthHero != null)
        {
            SetMaxHealth(currentHealthHero.maxHealth);
            SetHealth(currentHealthHero.TotalHealth);
        }
        else
        {
            Debug.LogError("HealthHero1p не найден на новом персонаже!");
        }
    }

    void Update()
    {
        if (currentHealthHero != null)
        {
            SetHealth(currentHealthHero.TotalHealth);
        }
    }

    void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    void SetHealth(float health)
    {
        slider.value = health;
    }
}