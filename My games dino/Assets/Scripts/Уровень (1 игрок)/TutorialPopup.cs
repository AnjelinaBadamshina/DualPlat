using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialPopup : MonoBehaviour
{
    public GameObject panel;
    private CanvasGroup canvasGroup;
    private bool isActive = false;

    private void Awake()
    {
        panel.SetActive(false);
        canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
        if (isActive) return;

        panel.SetActive(true);
        canvasGroup.alpha = 0;
        isActive = true;
        GameStateManager.Instance.FreezeGame();
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        float duration = 0.3f;
        float time = 0;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOut()
    {
        float duration = 0.2f;
        float time = 0;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        panel.SetActive(false);
        GameStateManager.Instance.UnfreezeGame();
        isActive = false;
    }
}
