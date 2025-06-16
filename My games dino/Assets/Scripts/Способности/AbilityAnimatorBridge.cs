using UnityEngine;

public class AbilityAnimatorBridge : MonoBehaviour
{
    public MonoBehaviour abilityScript;
    private System.Action abilityAction;

    private void Awake()
    {
        if (abilityScript != null)
        {
            var method = abilityScript.GetType().GetMethod("Activate");
            if (method != null)
                abilityAction = () => method.Invoke(abilityScript, null);
        }
    }

    public void TriggerAbility()
    {
        abilityAction?.Invoke();
    }
}
