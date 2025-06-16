using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    public Image iconImage;
    public Image cooldownOverlay;

    private string abilityId;
    private Vector3 originalScale;
    private float pulseTimer = 0f;
    private const float pulseScale = 1.15f;
    private const float pulseSpeed = 2f;

    private void Start()
    {
        if (iconImage != null)
            originalScale = iconImage.transform.localScale;
    }

    public void SetAbility(string newAbilityId, Sprite icon)
    {
        abilityId = newAbilityId;

        if (iconImage != null && icon != null)
            iconImage.sprite = icon;

        UpdateVisualImmediately();
    }

    private void UpdateVisualImmediately()
    {
        if (string.IsNullOrEmpty(abilityId)) return;

        float cooldownProgress = AbilityCooldownTracker.Instance.GetCooldownProgress(abilityId);
        float activeProgress = AbilityCooldownTracker.Instance.GetActiveProgress(abilityId);
        bool isActive = AbilityCooldownTracker.Instance.IsActive(abilityId);
        bool isCooldown = AbilityCooldownTracker.Instance.IsOnCooldown(abilityId);

        if (cooldownOverlay != null)
        {
            if (isActive)
            {
                cooldownOverlay.fillAmount = activeProgress;
                cooldownOverlay.color = new Color(0f, 0f, 0f, 0.6f);
            }
            else if (isCooldown)
            {
                cooldownOverlay.fillAmount = 1f - cooldownProgress;
                cooldownOverlay.color = new Color(0f, 0f, 0f, 0.75f);
            }
            else
            {
                cooldownOverlay.fillAmount = 0f;
                cooldownOverlay.color = new Color(0f, 0f, 0f, 0f);
            }
        }
    }

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameFrozen)
            return;

        if (string.IsNullOrEmpty(abilityId)) return;

        float cooldownProgress = AbilityCooldownTracker.Instance.GetCooldownProgress(abilityId);
        float activeProgress = AbilityCooldownTracker.Instance.GetActiveProgress(abilityId);
        bool isActive = AbilityCooldownTracker.Instance.IsActive(abilityId);
        bool isCooldown = AbilityCooldownTracker.Instance.IsOnCooldown(abilityId);

        if (cooldownOverlay != null)
        {
            if (isActive)
            {
                cooldownOverlay.fillAmount = activeProgress;
                cooldownOverlay.color = new Color(0f, 0f, 0f, 0.6f);
            }
            else if (isCooldown)
            {
                cooldownOverlay.fillAmount = 1f - cooldownProgress;
                cooldownOverlay.color = new Color(0f, 0f, 0f, 0.75f);
            }
            else
            {
                cooldownOverlay.fillAmount = 0f;
                cooldownOverlay.color = new Color(0f, 0f, 0f, 0f);
            }
        }

        if (iconImage != null)
        {
            if (!isActive && !isCooldown)
            {
                pulseTimer += Time.unscaledDeltaTime * pulseSpeed;
                float scale = Mathf.Lerp(1f, pulseScale, (Mathf.Sin(pulseTimer) + 1f) / 2f);
                iconImage.transform.localScale = originalScale * scale;
            }
            else
            {
                iconImage.transform.localScale = originalScale;
                pulseTimer = 0f;
            }
        }
    }
}
