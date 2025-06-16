using UnityEngine;
using System.Collections;

public class ElementalPlatform : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        SetInactiveImmediate(); // стартовое состояние
    }

    public void Activate()
    {
        StartCoroutine(FadeIn());
    }

    public void SetInactive()
    {
        StartCoroutine(FadeOut());
    }

    private void SetInactiveImmediate()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
        if (col != null) col.enabled = false;
    }

    private IEnumerator FadeIn()
    {
        col.enabled = true;
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(0.3f, 1f, elapsed / duration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    private IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0.3f, elapsed / duration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.3f);
        col.enabled = false;
    }
}
