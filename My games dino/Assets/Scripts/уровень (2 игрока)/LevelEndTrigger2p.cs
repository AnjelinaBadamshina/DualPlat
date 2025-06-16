using UnityEngine;

public class LevelEndTrigger2p : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LevelController2P.instance != null) // Проверка на null, чтобы избежать ошибок
        {
            if (collision.CompareTag("Player1"))
            {
                LevelController2P.instance.CharacterFinished(1); // Player1 пересек триггер
            }
            else if (collision.CompareTag("Player2"))
            {
                LevelController2P.instance.CharacterFinished(2); // Player2 пересек триггер
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LevelController2P.instance != null) // Проверка на null, чтобы избежать ошибок
        {
            if (collision.CompareTag("Player1"))
            {
                LevelController2P.instance.CharacterLeft(1); // Player1 покинул триггер
            }
            else if (collision.CompareTag("Player2"))
            {
                LevelController2P.instance.CharacterLeft(2); // Player2 покинул триггер
            }
        }
    }
}
