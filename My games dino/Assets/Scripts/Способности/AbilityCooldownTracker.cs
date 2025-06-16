using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityCooldownTracker : MonoBehaviour
{
    public static AbilityCooldownTracker Instance;

    private class CooldownData
    {
        public bool isActive;
        public bool isOnCooldown;
        public float activeDuration;
        public float cooldownDuration;

        public float localPhaseTime; // Локальное «замороженное» время
    }

    private Dictionary<string, CooldownData> cooldowns = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartAbility(string uniqueId, float activeDuration, float cooldownDuration)
    {
        if (!cooldowns.ContainsKey(uniqueId))
            cooldowns[uniqueId] = new CooldownData();

        var data = cooldowns[uniqueId];
        data.activeDuration = activeDuration;
        data.cooldownDuration = cooldownDuration;
        data.isActive = true;
        data.isOnCooldown = false;
        data.localPhaseTime = 0f;

        StartCoroutine(HandlePhases(uniqueId));
    }

    private IEnumerator HandlePhases(string uniqueId)
    {
        var data = cooldowns[uniqueId];

        // Активная фаза
        data.localPhaseTime = 0f;
        while (data.localPhaseTime < data.activeDuration)
        {
            if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameFrozen)
            {
                yield return null;
                continue;
            }

            data.localPhaseTime += Time.unscaledDeltaTime;
            yield return null;
        }

        data.isActive = false;
        data.isOnCooldown = true;

        // Кулдаун-фаза
        data.localPhaseTime = 0f;
        while (data.localPhaseTime < data.cooldownDuration)
        {
            if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameFrozen)
            {
                yield return null;
                continue;
            }

            data.localPhaseTime += Time.unscaledDeltaTime;
            yield return null;
        }

        data.isOnCooldown = false;
    }

    public void StartCooldownOnly(string uniqueId, float cooldownDuration)
    {
        if (!cooldowns.ContainsKey(uniqueId))
            cooldowns[uniqueId] = new CooldownData();

        var data = cooldowns[uniqueId];
        data.isActive = false;
        data.activeDuration = 0f;

        data.isOnCooldown = true;
        data.cooldownDuration = cooldownDuration;
        data.localPhaseTime = 0f;

        StartCoroutine(CooldownPhaseOnly(uniqueId));
    }

    private IEnumerator CooldownPhaseOnly(string uniqueId)
    {
        var data = cooldowns[uniqueId];

        while (data.localPhaseTime < data.cooldownDuration)
        {
            if (GameStateManager.Instance != null && GameStateManager.Instance.IsGameFrozen)
            {
                yield return null;
                continue;
            }

            data.localPhaseTime += Time.unscaledDeltaTime;
            yield return null;
        }

        data.isOnCooldown = false;
    }

    public bool IsActive(string uniqueId)
    {
        return cooldowns.ContainsKey(uniqueId) && cooldowns[uniqueId].isActive;
    }

    public bool IsOnCooldown(string uniqueId)
    {
        return cooldowns.ContainsKey(uniqueId) && cooldowns[uniqueId].isOnCooldown;
    }

    public float GetActiveProgress(string uniqueId)
    {
        if (!cooldowns.ContainsKey(uniqueId)) return 1f;

        var data = cooldowns[uniqueId];
        if (!data.isActive || data.activeDuration <= 0f) return 1f;

        return Mathf.Clamp01(data.localPhaseTime / data.activeDuration);
    }

    public float GetCooldownProgress(string uniqueId)
    {
        if (!cooldowns.ContainsKey(uniqueId)) return 1f;

        var data = cooldowns[uniqueId];
        if (!data.isOnCooldown || data.cooldownDuration <= 0f) return 1f;

        return Mathf.Clamp01(data.localPhaseTime / data.cooldownDuration);
    }

    public void EnsureAbilityRegistered(string uniqueId, float activeDuration = 0f, float cooldownDuration = 0f)
    {
        if (!cooldowns.ContainsKey(uniqueId))
        {
            cooldowns[uniqueId] = new CooldownData
            {
                isActive = false,
                isOnCooldown = false,
                activeDuration = activeDuration,
                cooldownDuration = cooldownDuration,
                localPhaseTime = 0f
            };
        }
    }

    public void ResetAllCooldowns()
    {
        cooldowns.Clear();
    }
}
