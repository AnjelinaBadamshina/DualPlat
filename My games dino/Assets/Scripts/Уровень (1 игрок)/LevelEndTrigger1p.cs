using UnityEngine;

public class LevelEndTrigger1p : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LevelController1p.instance != null) // Проверка на null, чтобы избежать ошибок
        {
            if (collision.CompareTag("Player1"))
            {
                LevelController1p.instance.CharacterFinished(1); // Player1 пересек триггер
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LevelController1p.instance != null) // Проверка на null, чтобы избежать ошибок
        {
            if (collision.CompareTag("Player1"))
            {
                LevelController1p.instance.CharacterLeft(1); // Player1 покинул триггер
            }
        }
    }
}