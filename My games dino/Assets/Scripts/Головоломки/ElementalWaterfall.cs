using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementalWaterfall : MonoBehaviour
{
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private Collider2D col;

    [Header("Fade Settings")]
    public float fadeDurationPerLayer = 0.5f;
    public float overlapTime = 0.25f;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        foreach (Transform child in transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) sprites.Add(sr);
        }
    }

    public void Deactivate()
    {
        StartCoroutine(CascadeFadeOut());
    }

    public void ResetWaterfall()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        col.enabled = true;

        foreach (SpriteRenderer sr in sprites)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        }

        StartCoroutine(CascadeFadeIn());
    }

    private IEnumerator CascadeFadeOut()
    {
        col.enabled = false;

        for (int i = 0; i < sprites.Count; i++)
        {
            StartCoroutine(FadeSprite(sprites[i], 1f, 0f, fadeDurationPerLayer));

            if (i < sprites.Count - 1)
                yield return new WaitForSeconds(fadeDurationPerLayer - overlapTime);
        }

        yield return new WaitForSeconds(fadeDurationPerLayer); // дождаться последнего слоя
        gameObject.SetActive(false);
    }

    private IEnumerator CascadeFadeIn()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < sprites.Count; i++)
        {
            StartCoroutine(FadeSprite(sprites[i], 0f, 1f, fadeDurationPerLayer));

            if (i < sprites.Count - 1)
                yield return new WaitForSeconds(fadeDurationPerLayer - overlapTime);
        }

        yield return new WaitForSeconds(fadeDurationPerLayer); // дождаться последнего слоя
        col.enabled = true;
    }

    private IEnumerator FadeSprite(SpriteRenderer sr, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color baseColor = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, to);
    }
}
