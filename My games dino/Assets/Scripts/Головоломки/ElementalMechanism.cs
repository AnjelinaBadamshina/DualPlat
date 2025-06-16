using System.Collections;
using UnityEngine;

public class ElementalMechanism : MonoBehaviour
{
    public DamageType requiredType;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public GameObject[] targetsToAffect;

    [Header("Auto-Deactivate Settings")]
    public bool autoDeactivate = false;
    public float activeDuration = 5f;

    private bool isActivated = false;

    public void Activate(DamageType type)
    {
        if (isActivated || type != requiredType)
            return;

        isActivated = true;

        // Меняем спрайт на активный
        GetComponent<SpriteRenderer>().sprite = activeSprite;

        // Активируем все объекты
        foreach (GameObject obj in targetsToAffect)
        {
            if (obj.TryGetComponent(out ElementalPlatform platform))
                platform.Activate();

            else if (obj.TryGetComponent(out ElementalWaterfall waterfall))
                waterfall.Deactivate();
        }

        if (autoDeactivate)
            StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(activeDuration);
        Deactivate();
    }

    public void Deactivate()
    {
        isActivated = false;
        GetComponent<SpriteRenderer>().sprite = inactiveSprite;

        // Откат состояния у объектов
        foreach (GameObject obj in targetsToAffect)
        {
            if (obj.TryGetComponent(out ElementalPlatform platform))
                platform.SetInactive();

            else if (obj.TryGetComponent(out ElementalWaterfall waterfall))
                waterfall.ResetWaterfall(); // Это новый метод, описан ниже
        }
    }
}
