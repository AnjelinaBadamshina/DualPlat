using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class HealthHero1p : MonoBehaviour
{
    public int maxHealth = 3;
    public float totalHealth;

    public bool isInvincible = false;
    public float invincibilityTime = 1.0f; // ⏱ Длительность неуязвимости

    private Animator animator;
    public LossSettings1p LossSettings1p;
    [SerializeField] private CameraController1p cameraController;

    public event Action OnDamageTaken;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (totalHealth == 0)
        {
            totalHealth = maxHealth;
        }
    }

    public void SetHealth(float bonusHealth)
    {
        if (isInvincible && bonusHealth < 0)
        {
            Debug.Log("🛡 Урон заблокирован: персонаж неуязвим.");
            return;
        }

        totalHealth += bonusHealth;

        if (totalHealth > maxHealth)
            totalHealth = maxHealth;

        if (bonusHealth < 0)
        {
            OnDamageTaken?.Invoke();
            animator.SetTrigger("Pain");

            if (cameraController != null)
                cameraController.Reset();

            ActivateInvincibility(); // ⏱ Включаем неуязвимость
        }

        if (totalHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (LossSettings1p != null)
        {
            LossSettings1p.LossPressed();
        }
    }

    public float TotalHealth => totalHealth;

    // ⏱ Включаем временную неуязвимость
    public void ActivateInvincibility()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}
