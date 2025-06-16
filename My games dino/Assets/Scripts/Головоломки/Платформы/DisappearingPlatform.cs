using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearDelay = 3f;
    public float reappearDelay = 5f;
    public bool reset = true;

    private bool isTriggered = false;
    private SpriteRenderer sr;
    private Collider2D col;
    private float fadeSpeed = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2")))
        {
            isTriggered = true;
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearDelay);
        col.enabled = false;
        yield return StartCoroutine(FadeOut());

        if (reset)
        {
            yield return new WaitForSeconds(reappearDelay);
            StartCoroutine(Reappear());
        }
    }

    IEnumerator FadeOut()
    {
        for (float alpha = 1f; alpha > 0f; alpha -= Time.deltaTime * fadeSpeed)
        {
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f);
    }

    IEnumerator Reappear()
    {
        yield return null; // just to ensure coroutine format
        for (float alpha = 0f; alpha < 1f; alpha += Time.deltaTime * fadeSpeed)
        {
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(1f);
        col.enabled = true;
        isTriggered = false;
    }

    void SetAlpha(float alpha)
    {
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}
