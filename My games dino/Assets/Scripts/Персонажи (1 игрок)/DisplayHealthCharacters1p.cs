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

        // ������������� �� ������� CharacterSwitched
        CharacterSwitcher.OnCharacterSwitched += OnCharacterSwitched;
    }

    // ������������ �� ������� ��� ����������� �������
    void OnDestroy()
    {
        CharacterSwitcher.OnCharacterSwitched -= OnCharacterSwitched;
    }

    // ���� ����� ����� ������ ��� ����� ���������
    void OnCharacterSwitched(Transform newCharacterTransform)
    {
        // ���� HealthHero1p �� ����� ���������
        currentHealthHero = newCharacterTransform.GetComponent<HealthHero1p>();

        // ��������� ����������� ��������
        if (currentHealthHero != null)
        {
            SetMaxHealth(currentHealthHero.maxHealth);
            SetHealth(currentHealthHero.TotalHealth);
        }
        else
        {
            Debug.LogError("HealthHero1p �� ������ �� ����� ���������!");
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