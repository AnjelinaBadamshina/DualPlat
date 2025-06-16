using UnityEngine;
using System.Collections;

public class FireSurface : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    [Header("Fade Settings")]
    public float fadeDuration = 1f; // длительность затухания

    private bool isExtinguishing = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Extinguish()
    {
        if (!isExtinguishing)
        {
            isExtinguishing = true;
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
