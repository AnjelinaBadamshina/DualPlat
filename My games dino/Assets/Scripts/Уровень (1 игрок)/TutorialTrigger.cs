using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialPopup tutorialPopup;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            tutorialPopup.Show();
            hasTriggered = true;

            // ”ничтожить триггер сразу после срабатывани€
            Destroy(gameObject);
        }
    }
}
