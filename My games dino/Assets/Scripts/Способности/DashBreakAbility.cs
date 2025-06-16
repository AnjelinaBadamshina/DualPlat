using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class DashBreakAbility : MonoBehaviour, IAbilityWithDuration
{
    public float dashSpeed = 15f;
    public float dashDuration = 0.4f;

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTimer;

    private HealthHero1p health;
    private HealthHero2p health2;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;

    public Animator animator;

    public float cooldownTime = 3f;
    public float Duration => dashDuration;

    private string uniqueAbilityId;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthHero1p>();
        health2 = GetComponent<HealthHero2p>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (health != null) health.OnDamageTaken += OnDamageTaken;
        if (health2 != null) health2.OnDamageTaken += OnDamageTaken;
    }

    private void OnDestroy()
    {
        if (health != null) health.OnDamageTaken -= OnDamageTaken;
        if (health2 != null) health2.OnDamageTaken -= OnDamageTaken;
    }

    public void Activate()
    {
        if (isDashing) return;

        isDashing = true;
        dashTimer = dashDuration;

        if (animator != null)
            animator.SetTrigger("Dash");

        flashCoroutine = StartCoroutine(FlashWhileDashing());

        var heroImage = GetComponent<HeroImage>();
        if (heroImage != null)
        {
            var switcher = GetComponent<CharacterSwitcher>();
            int playerNumber = switcher != null ? switcher.playerNumber : 0;
            uniqueAbilityId = $"P{playerNumber}_{heroImage.abilityId}";
        }
    }

    private void Update()
    {
        if (isDashing)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashSpeed, rb.velocity.y);
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                rb.velocity = Vector2.zero;

                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                    SetAlpha(1f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("🟡 OnTriggerEnter2D сработал с объектом: " + collision.name);

        if (!isDashing) return;

        if (collision.CompareTag("Fragile"))
        {
            Debug.Log("💥 Хрупкий объект разрушен (тег): " + collision.name);

            FragileWall fragile = collision.GetComponent<FragileWall>();
            if (fragile != null)
            {
                fragile.Break();
            }
            else
            {
                Destroy(collision.gameObject); // fallback
            }
        }

    }

    private void OnDamageTaken()
    {
        if (isDashing && !string.IsNullOrEmpty(uniqueAbilityId))
        {
            Debug.Log("⚠️ Получен урон во время рывка — запускаем кулдаун.");

            isDashing = false;
            rb.velocity = Vector2.zero;

            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                SetAlpha(1f);
            }

            AbilityCooldownTracker.Instance.StartCooldownOnly(uniqueAbilityId, cooldownTime);
        }
    }

    private IEnumerator FlashWhileDashing()
    {
        while (isDashing)
        {
            SetAlpha(0.3f);
            yield return new WaitForSeconds(0.1f);
            SetAlpha(1f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
        }
    }
}
