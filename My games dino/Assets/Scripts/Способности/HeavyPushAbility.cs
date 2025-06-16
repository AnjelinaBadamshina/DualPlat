using UnityEngine;

public class HeavyPushAbility : MonoBehaviour, IAbilityWithDuration
{
    public float duration = 6f;

    public float Duration => duration;

    public void Activate()
    {
        HeavyObjectInteractor interactor = GetComponent<HeavyObjectInteractor>();
        if (interactor != null)
        {
            interactor.EnableInteraction(duration);
            Debug.Log("Heavy Push Activated!");
        }
        else
        {
            Debug.LogWarning("HeavyObjectInteractor не найден на персонаже.");
        }
    }
}
