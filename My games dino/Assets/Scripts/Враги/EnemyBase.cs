using UnityEngine;
using System.Collections;

public enum DamageType { Fire, Water }

public abstract class EnemyBase : MonoBehaviour
{
    public int maxHealth = 2;
    protected int currentHealth;

    [Header("Health Display")]
    public GameObject[] heartIcons;

    [Header("Vulnerability")]
    public DamageType[] vulnerabilities;

    
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public virtual void TakeDamage(int amount, DamageType type)
    {
        if (System.Array.Exists(vulnerabilities, v => v == type))
        {
            currentHealth -= amount;
            UpdateHealthDisplay();
            StartCoroutine(HitEffect());

            if (currentHealth <= 0)
                Die();
        }
    }

    protected virtual void UpdateHealthDisplay()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (heartIcons[i] != null)
                heartIcons[i].SetActive(i < currentHealth);
        }
    }

    protected virtual IEnumerator HitEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
        }
    }

    protected virtual void Die()
    {
        StopAllCoroutines();
        StartCoroutine(FlashThenFadeAndDestroy());
    }


    private IEnumerator FlashThenFadeAndDestroy()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Collider2D col = GetComponent<Collider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (col != null) col.enabled = false;
        if (rb != null) rb.velocity = Vector2.zero;

        // Вспышка перед исчезновением
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }

       
        // Растворение
        float duration = 0.5f;
        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    protected abstract int GetContactDamage();
}
